using StokTakipProjesi.Filters;
using StokTakipProjesi.Models.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StokTakipProjesi.Controllers
{
    [Auth]
    public class CategoryController : Controller
    {

        StokTakipDbEntities db = new StokTakipDbEntities();

        public ActionResult List()
        {
            List<Categories> categories = db.Categories.ToList();

            if (TempData["Message"] != null && TempData["Result"] != null)
            {
                ViewBag.Result = TempData["Result"];
                ViewBag.Message = TempData["Message"];
                TempData["Result"] = null;
                TempData["Message"] = null;
            }

            return View(categories);
        }

        [AdminAuth]
        public ActionResult CategoryAdd()
        {
            return View();
        }

        [AdminAuth]
        [HttpPost]
        public ActionResult CategoryAdd(Categories category)
        {
            db.Categories.Add(category);
            int result = db.SaveChanges();

            if (result > 0)
            {
                TempData["Result"] = "success";
                TempData["Message"] = "Kategori başarıyla eklendi.";
            }
            else
            {
                TempData["Result"] = "danger";
                TempData["Message"] = "Kategori ekleme işlemi başarısız !";
            }

            return RedirectToAction("List");
        }

        [AdminAuth]
        public ActionResult CategoryUpdate(int id)
        {
            Categories category = db.Categories.Find(id);
            if (category == null)
            {
                TempData["Result"] = "danger";
                TempData["Message"] = "Güncellenmek istenen kategori bulunamadı !";
                return RedirectToAction("List");
            }
            return View(category);
        }

        [AdminAuth]
        [HttpPost]
        public ActionResult CategoryUpdate(Categories category)
        {
            Categories categoryUpdated = db.Categories.Find(category.Id);

            categoryUpdated.CategoryName = category.CategoryName;

            int result = db.SaveChanges();

            if (result > 0)
            {
                TempData["Result"] = "success";
                TempData["Message"] = "Kategori başarıyla güncellendi.";
            }
            else
            {
                TempData["Result"] = "danger";
                TempData["Message"] = "Kategori güncelleme işlemi başarısız !";
            }

            return RedirectToAction("List");
        }

        [AdminAuth]
        public ActionResult CategoryDelete(int id)
        {
            Categories category = db.Categories.Find(id);

            db.Categories.Remove(category);

            int result = db.SaveChanges();

            if (result > 0)
            {
                TempData["Result"] = "success";
                TempData["Message"] = "Kategori başarıyla silindi.";
            }
            else
            {
                TempData["Result"] = "danger";
                TempData["Message"] = "Kategori silme işlemi başarısız !";
            }

            return RedirectToAction("List");
        }
    }
}