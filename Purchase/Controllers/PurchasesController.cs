using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Purchase.Data;
using Purchase.Models;
using Purchase.Utility;
using Purchase.ViewModels;
using System.Data;

namespace Purchase.Controllers
{
    public class PurchasesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PurchasesController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            ViewData["MeasurementUnitOptions"] = new SelectList(_context.MeasurementUnits, "Id", "Name");
            ViewData["ProductOptions"] = new SelectList(_context.Products, "Id", "Name");

            var purchasesWithDetails = await _context.PurchaseHeaders.Include(p => p.purchaseDetails).ThenInclude(p => p.MeasurementUnit).ToListAsync();

            var products = await _context.Products.ToArrayAsync();

            List<PurchaseHeaderVM> purchaseHeaderVMs = new List<PurchaseHeaderVM>();

            if (purchasesWithDetails.Count > 0)
            {
                foreach (var purchase in purchasesWithDetails)
                {
                    List<PurchaseDetailVM> purchaseDetailVMs = new List<PurchaseDetailVM>();

                    if (purchase.purchaseDetails.Count > 0)
                    {
                        foreach (var pd in purchase.purchaseDetails)
                        {
                            string productName = products.Single(p => p.Id == pd.ProductId).Name;
                            purchaseDetailVMs.Add(new PurchaseDetailVM(pd.Id, pd.ProductId, pd.Quantity, pd.MeasurementUnitId, pd.ProductUnitPrice, pd.ProductTotalPrice, pd.PurchaseHeaderId, productName, pd.MeasurementUnit.Name));
                        }
                    }

                    if (purchaseDetailVMs.Count > 0)
                    {
                        purchaseHeaderVMs.Add(new PurchaseHeaderVM
                        {
                            Id = purchase.Id,
                            CustomerName = purchase.CustomerName,
                            CustomerEmailAddress = purchase.CustomerEmailAddress,
                            CustomerPhoneNumber = purchase.CustomerPhoneNumber,
                            InvoiceNumber = purchase.InvoiceNumber,
                            PurchaseDate = purchase.PurchaseDate,
                            TotalAmount = purchase.TotalBill,
                            PurchaseDetails = purchaseDetailVMs
                        });
                    }
                    else
                    {
                        purchaseHeaderVMs.Add(new PurchaseHeaderVM
                        {
                            Id = purchase.Id,
                            CustomerName = purchase.CustomerName,
                            CustomerEmailAddress = purchase.CustomerEmailAddress,
                            CustomerPhoneNumber = purchase.CustomerPhoneNumber,
                            InvoiceNumber = purchase.InvoiceNumber,
                            PurchaseDate = purchase.PurchaseDate,
                            TotalAmount = purchase.TotalBill,

                        });
                    }

                }
            }
            return View(purchaseHeaderVMs);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PurchaseDetail(PurchaseDetailVM purchaseDetailVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return PartialView("_PruchaseDetail", purchaseDetailVM);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PurchaseHeaderVM purchaseHeaderVM, string token)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    PurchaseHeader purchaseHeader = new PurchaseHeader(purchaseHeaderVM.Id, PurchaseInvoiceNumber.Get(), purchaseHeaderVM.PurchaseDate, purchaseHeaderVM.PurchaseDetails.Sum(p => p.UnitPrice * p.Quantity), purchaseHeaderVM.CustomerName, purchaseHeaderVM.CustomerPhoneNumber, purchaseHeaderVM.CustomerEmailAddress);

                    foreach (PurchaseDetailVM pdvm in purchaseHeaderVM.PurchaseDetails)
                    {
                        purchaseHeader.purchaseDetails.Add(new PurchaseDetail(pdvm.Id, pdvm.ProductId, pdvm.Quantity, pdvm.MeasurementUnitId, pdvm.UnitPrice, pdvm.Quantity * pdvm.UnitPrice, pdvm.PurchaseHeaderId));
                    }

                    await _context.PurchaseHeaders.AddAsync(purchaseHeader);
                    await _context.SaveChangesAsync();

                    var products = await _context.Products.ToListAsync();
                    var measurementUnits = await _context.MeasurementUnits.ToListAsync();

                    PurchaseHeaderVM phvm = new PurchaseHeaderVM(purchaseHeader.Id, purchaseHeader.CustomerName, purchaseHeader.CustomerPhoneNumber, purchaseHeader.CustomerEmailAddress,
                        purchaseHeader.InvoiceNumber, purchaseHeader.PurchaseDate, purchaseHeader.TotalBill);

                    foreach (PurchaseDetail pd in purchaseHeader.purchaseDetails)
                    {
                        string measurementUnitName = measurementUnits.Single(m => m.Id == pd.MeasurementUnitId).Name;
                        string productName = products.Single(p => p.Id == pd.ProductId).Name;

                        phvm.PurchaseDetails.Add(new PurchaseDetailVM(pd.Id, pd.ProductId, pd.Quantity, pd.MeasurementUnitId, pd.ProductUnitPrice, pd.ProductTotalPrice, pd.PurchaseHeaderId, productName, measurementUnitName));
                    }
                    return PartialView("_PurchaseInfo", phvm);
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateExecutingSP(PurchaseHeaderVM purchaseHeaderVM, string token)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var purchaseDetailDataTable = new DataTable();
                    purchaseDetailDataTable.Columns.Add("ProductId", typeof(int));
                    purchaseDetailDataTable.Columns.Add("Quantity", typeof(decimal));
                    purchaseDetailDataTable.Columns.Add("MeasurementUnitId", typeof(int));
                    purchaseDetailDataTable.Columns.Add("ProductUnitPrice", typeof(decimal));
                    purchaseDetailDataTable.Columns.Add("PurchaseHeaderId", typeof(int));

                    foreach (PurchaseDetailVM pdvm in purchaseHeaderVM.PurchaseDetails)
                    {
                        DataRow dataRow = purchaseDetailDataTable.NewRow();
                        dataRow["ProductId"] = pdvm.ProductId;
                        dataRow["Quantity"] = pdvm.Quantity;
                        dataRow["MeasurementUnitId"] = pdvm.MeasurementUnitId;
                        dataRow["ProductUnitPrice"] = pdvm.UnitPrice;
                        dataRow["PurchaseHeaderId"] = pdvm.PurchaseHeaderId;
                        purchaseDetailDataTable.Rows.Add(dataRow);
                    }

                    SqlParameter customerName = new SqlParameter
                    {
                        ParameterName = "@CustomerName",
                        SqlDbType = SqlDbType.NVarChar,
                        Size = 32767,
                        Value = purchaseHeaderVM.CustomerName
                    };

                    SqlParameter customerPhoneNumber = new SqlParameter
                    {
                        ParameterName = "@CustomerPhoneNumber",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = purchaseHeaderVM.CustomerPhoneNumber
                    };

                    SqlParameter customerEmailAddress = new SqlParameter
                    {
                        ParameterName = "@CustomerEmailAddress",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = purchaseHeaderVM.CustomerEmailAddress
                    };

                    SqlParameter invoiceNumber = new SqlParameter
                    {
                        ParameterName = "@InvoiceNumber",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = PurchaseInvoiceNumber.Get()
                    };

                    SqlParameter purchaseDate = new SqlParameter
                    {
                        ParameterName = "@PurchaseDate",
                        SqlDbType = SqlDbType.Date,
                        Value = purchaseHeaderVM.PurchaseDate
                    };

                    SqlParameter tvp = new SqlParameter
                    {
                        ParameterName = "@TVP",
                        SqlDbType = SqlDbType.Structured,
                        TypeName = "dbo.ParamPurchaseDetail",
                        Value = purchaseDetailDataTable
                    };

                    await _context.Database.ExecuteSqlAsync($"EXEC dbo.PurchaseSP {customerName}, {customerPhoneNumber}, {customerEmailAddress}, {invoiceNumber}, {purchaseDate}, {tvp}");
                    
                    return Content("Succeeded", "text/html", System.Text.Encoding.UTF8);
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
