using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Purchase.Data;
using Purchase.Models;
using Purchase.ViewModels;

namespace Purchase.Controllers
{
    public class MeasurementUnitsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MeasurementUnitsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            IQueryable<MeasurementUnitVM> measurementUnitVMs = _context.MeasurementUnits.Select(x => new MeasurementUnitVM(x.Id, x.Name));
            return View(await measurementUnitVMs.ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ContentResult> AddUpdate(MeasurementUnitVM aMeasurementUnitVM, string actionType, string token)
        {

            if (actionType == "add")
            {
                await _context.MeasurementUnits.AddAsync(new MeasurementUnit(aMeasurementUnitVM.Name));
                await _context.SaveChangesAsync();
            }

            if (actionType == "edit")
            {
                MeasurementUnit measurementUnit = new MeasurementUnit(aMeasurementUnitVM.Id, aMeasurementUnitVM.Name);
                _context.MeasurementUnits.Update(measurementUnit);
                await _context.SaveChangesAsync();
            }

            string trsWithTds = string.Empty;
            List<MeasurementUnitVM> measurementUnitVMs = _context.MeasurementUnits.Select(x => new MeasurementUnitVM(x.Id, x.Name)).ToList();
            if(measurementUnitVMs.Count > 0)
            {
                foreach(MeasurementUnitVM measurementUnitVM in  measurementUnitVMs)
                {
                    trsWithTds += "<tr><td class=\"align-middle\">" + measurementUnitVM.Name + "</td><td><button type=\"button\" class=\"btn btn-sm text-warning fw-bold\" data-id=\"" + measurementUnitVM.Id + "\" data-name=\"" + measurementUnitVM.Name + "\" onclick=\"editMeasurementUnit(this)\">Edit</button><button type=\"button\" class=\"btn btn-sm text-danger fw-bold\" onclick=\"deleteMeasurementUnit('" + token + "'," + measurementUnitVM.Id + ")\">Delete</button></td></tr>";
                }
            }
            return Content(trsWithTds, "text/html", System.Text.Encoding.UTF8);
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<ContentResult> Delete(int id, string token)
        {
            MeasurementUnit measurementUnit = await _context.MeasurementUnits.FindAsync(id);

            _context.MeasurementUnits.Remove(measurementUnit);
            await _context.SaveChangesAsync();

            string trsWithTds = string.Empty;
            List<MeasurementUnitVM> measurementUnitVMs = _context.MeasurementUnits.Select(x => new MeasurementUnitVM(x.Id, x.Name)).ToList();
            if (measurementUnitVMs.Count > 0)
            {
                foreach (MeasurementUnitVM measurementUnitVM in measurementUnitVMs)
                {
                    trsWithTds += "<tr><td class=\"align-middle\">" + measurementUnitVM.Name + "</td><td><button type=\"button\" class=\"btn btn-sm text-warning fw-bold\" data-id=\"" + measurementUnitVM.Id + "\" data-name=\"" + measurementUnitVM.Name + "\" onclick=\"editMeasurementUnit(this)\">Edit</button><button type=\"button\" class=\"btn btn-sm text-danger fw-bold\" onclick=\"deleteMeasurementUnit('" + token + "'," + measurementUnitVM.Id + ")\">Delete</button></td></tr>";
                }
            }
            return Content(trsWithTds, "text/html", System.Text.Encoding.UTF8);
        }
    }
}
