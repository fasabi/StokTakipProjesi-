
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
    public class HomeController : Controller
    {
        StokTakipDbEntities db = new StokTakipDbEntities();
        public ActionResult Index()
        {
            Users user = Session["login"] as Users;
            List<Products> products = db.Products.ToList();

            if (TempData["Message"] != null && TempData["Result"] != null)
            {
                ViewBag.Result = TempData["Result"];
                ViewBag.Message = TempData["Message"];
                TempData["Result"] = null;
                TempData["Message"] = null;
            }

            return View(products);
        }

        [AdminAuth]
        public ActionResult ProductAdd()
        {
            // Kategorilerin dropdownliste(SelectListItem nesnesi olarak) getirilmesi için veritabanından select sorgusu çekilerek categories listesine atıldı.
            List<SelectListItem> categories = (from cat in db.Categories.ToList()
                                               select new SelectListItem
                                               {
                                                   Text = cat.CategoryName,
                                                   Value = cat.Id.ToString()
                                               }).ToList();

            ViewBag.CategoryList = categories; // Viewde bu veriyi yakalamak için ViewBag özelliği kullanılarak belleğe atıldı.

            return View();
        }

        [AdminAuth]
        [HttpPost]
        public ActionResult ProductAdd(Products product)
        {
            Categories category = db.Categories.Where(c => c.Id == product.Categories.Id).FirstOrDefault(); //Seçilen Kategori nesnesi bulunarak category nesnesine atandı.

            product.Categories = category; // Ürünlerdeki kategori bulunan category nesenesine eşitlendi.
            product.CategoryId = category.Id;
            db.Products.Add(product); // Veritabanında Ürünler tablosuna ekleme yapıldı.
            int result = db.SaveChanges(); //Veritabanında yapılan değişiklikler kaydedildi.

            if (result > 0)
            {
                TempData["Result"] = "success";
                TempData["Message"] = "Ürün başarıyla eklendi."; // Ürünün eklendiğine dair messaj vermek için TempData kullanıldı.
            }
            else
            {
                TempData["Result"] = "danger";
                TempData["Message"] = "Ürün ekleme işlemi başarısız !";
            }

            return RedirectToAction("Index"); // Ürünlerin listelendiği Index sayfasına yönlendirildi.
        }

        [AdminAuth]
        public ActionResult ProductUpdate(int id)
        {
            // Güncellemek istenen ürünün id si Ürünler tablosundan bulunarak product nesnesine atılır.
            Products product = db.Products.Find(id);

            // Güncellenmek istenen kategorinin seçili getirilerek tüm kategorilerin listelenmesi için tekrar kategoriler listesi çekildi.
            List<SelectListItem> categories = (from cat in db.Categories.ToList()
                                               select new SelectListItem
                                               {
                                                   Text = cat.CategoryName,
                                                   Value = cat.Id.ToString()
                                               }).ToList();

            ViewBag.CategoryList = categories;
            return View(product); // Ürün güncelleme sayfasına gönderildi.
        }

        [AdminAuth]
        [HttpPost]
        public ActionResult ProductUpdate(Products product)
        {
            Products productUpdated = db.Products.Find(product.Id); // Güncellemek istenen ürünün id si Ürünler tablosundan bulunarak product nesnesine atılır.

            productUpdated.Name = product.Name;
            productUpdated.CategoryId = product.Categories.Id;
            productUpdated.Brand = product.Brand;
            productUpdated.Model = product.Model;
            productUpdated.UnitPrice = product.UnitPrice;
            productUpdated.StockAmount = product.StockAmount;
            productUpdated.StockCode = product.StockCode;

            int result = db.SaveChanges();

            if (result > 0)
            {
                TempData["Result"] = "success";
                TempData["Message"] = "Ürün başarıyla güncellendi.";
            }
            else
            {
                TempData["Result"] = "danger";
                TempData["Message"] = "Ürün güncelleme işlemi başarısız !";
            }
            return RedirectToAction("Index");
        }

        [AdminAuth]
        public ActionResult ProductDelete(int id)
        {
            Products product = db.Products.Find(id);
            db.Products.Remove(product);
            int result = db.SaveChanges();

            if (result > 0)
            {
                TempData["Result"] = "success";
                TempData["Message"] = "Ürün başarıyla silindi.";
            }
            else
            {
                TempData["Result"] = "danger";
                TempData["Message"] = "Ürün silme işlemi başarısız !";
            }
            return RedirectToAction("Index");
        }
    }
}