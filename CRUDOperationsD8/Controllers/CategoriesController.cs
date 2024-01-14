using CRUDOperationsD8.Data;
using CRUDOperationsD8.Models;
using CRUDOperationsD8.Models.VM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace CRUDOperationsD8.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
         }
        public async Task<IActionResult> Index()
        {
            List<Category> listCategory = await GetAllCategory();
            if (listCategory.Count < 1)
            {
                await SeedData.CreateCategoryList(_context);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetDataTabelData()
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();

                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnAscDesc = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int resultTotal = 0;

                List<Category> listCategory = await GetAllCategory();
                var _GetGridItem = listCategory.AsQueryable();
                //Sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnAscDesc)))
                {
                    _GetGridItem = _GetGridItem.OrderBy(sortColumn + " " + sortColumnAscDesc);
                }

                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    searchValue = searchValue.ToLower();
                    _GetGridItem = _GetGridItem.Where(obj => obj.Id.ToString().Contains(searchValue)
                    || obj.Name.ToLower().Contains(searchValue)
                    || obj.Description.ToLower().Contains(searchValue)
                    || obj.CreatedDate.ToString().Contains(searchValue)
                    || obj.ModifiedDate.ToString().Contains(searchValue)
                    || obj.CreatedBy.ToLower().Contains(searchValue)
                    || obj.ModifiedBy.ToLower().Contains(searchValue));
                }

                resultTotal = _GetGridItem.Count();

                var result = _GetGridItem.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = resultTotal, recordsTotal = resultTotal, data = result });

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(long? id)
        {
            var _Category = await _context.Category.FirstOrDefaultAsync(m => m.Id == id);
            return PartialView("_Details", _Category);
        }
        [HttpGet]
        public async Task<IActionResult> AddEdit(int id)
        {
            Category _Category = new();
            if (id > 0) _Category = await _context.Category.Where(x => x.Id == id).FirstOrDefaultAsync();
            return PartialView("_AddEdit", _Category);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(Category _Category)
        {
            JsonResultViewModel _JsonResultViewModel = new();
            try
            {
                if (ModelState.IsValid)
                {
                    if (_Category.Id > 0)
                    {
                        var _CategoriesOld = await _context.Category.FindAsync(_Category.Id);
                        _Category.CreatedDate = _CategoriesOld.CreatedDate;
                        _Category.CreatedBy = _CategoriesOld.CreatedBy;
                        _Category.ModifiedDate = DateTime.Now;
                        _Category.ModifiedBy = "Admin";
                        _context.Entry(_CategoriesOld).CurrentValues.SetValues(_Category);
                        await _context.SaveChangesAsync();

                        _JsonResultViewModel.AlertMessage = "Category Updated Successfully. ID: " + _Category.Id;
                        _JsonResultViewModel.IsSuccess = true;
                        return new JsonResult(_JsonResultViewModel);
                    }
                    else
                    {

                        _Category.CreatedDate = DateTime.Now;
                        _Category.ModifiedDate = DateTime.Now;
                        _Category.CreatedBy = "Admin";
                        _Category.ModifiedBy = "Admin";
                        _context.Add(_Category);
                        await _context.SaveChangesAsync();

                        _JsonResultViewModel.AlertMessage = "Category Created Successfully. ID: " + _Category.Id;
                        _JsonResultViewModel.IsSuccess = true;
                        return new JsonResult(_JsonResultViewModel);
                    }
                }
                _JsonResultViewModel.AlertMessage = "Operation failed.";
                _JsonResultViewModel.IsSuccess = false;
                return new JsonResult(_JsonResultViewModel);
            }
            catch (Exception ex)
            {
                _JsonResultViewModel.IsSuccess = false;
                _JsonResultViewModel.AlertMessage = ex.Message;
                return new JsonResult(_JsonResultViewModel);
                throw;
            }
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(Int64 id)
        {
            try
            {
                var _Categories = await _context.Category.FindAsync(id);
                _Categories.ModifiedDate = DateTime.Now;
                _Categories.ModifiedBy = "Admin";
                _Categories.Cancelled = true;

                _context.Update(_Categories);
                await _context.SaveChangesAsync();
                return new JsonResult(_Categories);
            }
            catch (Exception)
            {
                throw;
            }
        }
        private async Task<List<Category>> GetAllCategory()
        {
            try
            {
                List<Category> listCategory = await _context.Category.Where(x => x.Cancelled == false).ToListAsync();
                return listCategory;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
