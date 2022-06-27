using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using CurdOperation.Models;
using System.Collections.Generic;
using System;

namespace WebApplication3.Controllers
{
    public class CategoryController : Controller
    {
        private readonly CompanyDBContext _context;

        public CategoryController(CompanyDBContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult> Index(/*string search="",int PageNo=1*/ int pg=1)
        {
            Category c = _context.Categories.FirstOrDefault();
            #region Paging
            //ViewBag.search = search;

            //int NoOfRecordsPerPage = 5;
            //int NoOfPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(Categories.Count) / Convert.ToDouble(NoOfRecordsPerPage)));
            //int NoOfRecordsToSkip = (PageNo - 1) * NoOfRecordsPerPage;
            //ViewBag.PageNo = PageNo;
            //ViewBag.NoOfPages = NoOfPages;
            //Categories = Categories.Skip(NoOfRecordsToSkip).Take(NoOfRecordsPerPage).ToList();
            #endregion
            const int pageSize = 5;
            if (pg < 1)
                pg = 1;
            int recsCount = _context.Categories.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = _context.Categories.Skip(recSkip).Take(pager.PageSize).ToListAsync();
            this.ViewBag.Pager = pager;

            //return View(await _context.Categories.ToListAsync());
            return View(await data);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View(new Category());
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create(Category c)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Add(c);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Category");
            }
            return View(c);
        }
        [Authorize(Roles = "Admin,User")]
        public ActionResult Details(int id)
        {

            Category c = _context.Categories.Where(temp => temp.CategoryID == id).FirstOrDefault();
            return View(c);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            Category c = _context.Categories.Where(temp => temp.CategoryID == id).FirstOrDefault();
            return View(c);

        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Category c)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(c).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(c);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]

        public ActionResult Delete(int id)
        {

            Category existingCategory = _context.Categories.Where(temp => temp.CategoryID == id).FirstOrDefault();
            return View(existingCategory);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(long id, Category c)
        {

            Category existingCategory = _context.Categories.Where(temp => temp.CategoryID == id).FirstOrDefault();
            _context.Categories.Remove(existingCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
