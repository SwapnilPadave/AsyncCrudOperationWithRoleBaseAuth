using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using System.Web.Mvc;
using CurdOperation.Models;

namespace WebApplication3.Controllers
{
    public class ProductController : Controller
    {
        private readonly CompanyDBContext _context;

        public ProductController(CompanyDBContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult> Index(/*string search = "", int PageNo = 1*/ int pg=1)
        {
            #region Paging
            //List<Product> Products = _context.Products.Where(temp => temp.ProductName.Contains(search)).ToList();
            //ViewBag.search = search;

            //int NoOfRecordsPerPage = 5;
            //int NoOfPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(Products.Count) / Convert.ToDouble(NoOfRecordsPerPage)));
            //int NoOfRecordsToSkip = (PageNo - 1) * NoOfRecordsPerPage;
            //ViewBag.PageNo = PageNo;
            //ViewBag.NoOfPages = NoOfPages;
            //Products = Products.Skip(NoOfRecordsToSkip).Take(NoOfRecordsPerPage).ToList();
            #endregion //1
            #region //2
            const int pageSize = 5;
            if (pg < 1)
                pg = 1;
            int recsCount = _context.Products.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = _context.Products.Skip(recSkip).Take(pager.PageSize).ToListAsync();
            ViewBag.Pager = pager;
            #endregion
            ViewBag.Categories = _context.Categories.ToListAsync();
            return View(await data);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {

            ViewBag.Categories = _context.Categories.ToList();

            return View(new Product());
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create(Product p)
        {
            if (ModelState.IsValid)
            {
                
                _context.Products.Add(p);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Product");
            }
            return View(p);
        }
        public ActionResult Details(int id)
        {
            ViewBag.Categories = _context.Categories.ToList();
            Product p = _context.Products.Where(temp => temp.ProductID == id).FirstOrDefault();
            return View(p);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            ViewBag.Categories = _context.Categories.ToList();
            Product p = _context.Products.Where(temp => temp.ProductID == id).FirstOrDefault();
            return View(p);

        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Product p)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(p).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(p);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]

        public ActionResult Delete(int id)
        {
            ViewBag.Categories = _context.Categories.ToList();
            Product exsistingProduct = _context.Products.Where(temp => temp.ProductID == id).FirstOrDefault();
            return View(exsistingProduct);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(long id, Product p)
        {

            Product exsistingProduct = _context.Products.Where(temp => temp.ProductID == id).FirstOrDefault();
            _context.Products.Remove(exsistingProduct);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}

