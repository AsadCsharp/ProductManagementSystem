using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Purchase.Data;
using Purchase.Models;
using Purchase.ViewModels;

namespace Purchase.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IWebHostEnvironment _webHostEnvironment;


        public ProductsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            IQueryable<ProductVM> productVMs = _context.Products.Select(p => new ProductVM(p.Id, p.Name, p.ImageUrl));
            return View(await productVMs.ToListAsync());
        }

        [HttpPost]
        public async Task<string> GetImageUrl(IFormFile image)
        {
            if (image != null && image.Length > 0)
            {
                string filename = image.FileName;
                string filePath = _webHostEnvironment.WebRootPath + $@"\images\{filename}";
                long size = image.Length;

                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    await image.CopyToAsync(fs);
                    await fs.FlushAsync(CancellationToken.None);
                    await fs.DisposeAsync();
                }
                return $@"/images/{filename}";
            }
            else
            {
                return "Opps!!! No image has uploaded. Try again";
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ContentResult> AddUpdate(ProductVM productVM, string actionType, string token)
        {
            if (actionType == "add")
            {
                await _context.Products.AddAsync(new Product(productVM.Name, productVM.ImageUrl));
                await _context.SaveChangesAsync();
            }

            if (actionType == "edit")
            {
                Product product = new Product(productVM.Id, productVM.Name, productVM.ImageUrl);
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
            }

            string trsWithTds = string.Empty;
            List<ProductVM> productVMs = _context.Products.Select(x => new ProductVM(x.Id, x.Name, x.ImageUrl)).ToList();
            if (productVMs.Count > 0)
            {
                foreach (ProductVM aProductVM in productVMs)
                {
                    trsWithTds += "<tr><td class=\"align-middle\">" + aProductVM.Name + "</td><td class=\"align-middle\"><img src=\"" + aProductVM.ImageUrl + "\" class=\"img-thumbnail\" /></td><td><button type=\"button\" class=\"btn btn-sm text-warning fw-bold\" data-id=\"" + aProductVM.Id + "\" data-image=\"" + aProductVM.ImageUrl + "\" data-name=\"" + aProductVM.Name + "\" onclick=\"editProduct(this)\">Edit</button><button type=\"button\" class=\"btn btn-sm text-danger fw-bold\" onclick=\"deleteProduct('" + token + "', " + aProductVM.Id + ")\">Delete</button></td></tr>";
                }
            }
            return Content(trsWithTds, "text/html", System.Text.Encoding.UTF8);
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<ContentResult> Delete(int id, string token)
        {
            Product product = await _context.Products.FindAsync(id);

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            string trsWithTds = string.Empty;
            List<ProductVM> productVMs = _context.Products.Select(x => new ProductVM(x.Id, x.Name, x.ImageUrl)).ToList();
            if (productVMs.Count > 0)
            {
                foreach (ProductVM aProductVM in productVMs)
                {
                    trsWithTds += "<tr><td class=\"align-middle\">" + aProductVM.Name + "</td><td class=\"align-middle\"><img src=\"" + aProductVM.ImageUrl + "\" class=\"img-thumbnail\" /></td><td><button type=\"button\" class=\"btn btn-sm text-warning fw-bold\" data-id=\"" + aProductVM.Id + "\" data-image=\"" + aProductVM.ImageUrl + "\" data-name=\"" + aProductVM.Name + "\" onclick=\"editProduct(this)\">Edit</button><button type=\"button\" class=\"btn btn-sm text-danger fw-bold\" onclick=\"deleteProduct('" + token + "', " + aProductVM.Id + ")\">Delete</button></td></tr>";
                }
            }
            return Content(trsWithTds, "text/html", System.Text.Encoding.UTF8);
        }
    }
}
