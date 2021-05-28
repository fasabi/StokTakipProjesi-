using StokTakipProjesi.Filters;
using StokTakipProjesi.Models.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace StokTakipProjesi.Controllers
{
    public class UserController : Controller
    {
        StokTakipDbEntities db = new StokTakipDbEntities();

        [AdminAuth]
        public ActionResult List()
        {
            List<Users> users = db.Users.ToList();

            if (TempData["Message"] != null && TempData["Result"] != null)
            {
                ViewBag.Result = TempData["Result"];
                ViewBag.Message = TempData["Message"];
                TempData["Result"] = null;
                TempData["Message"] = null;
            }

            return View(users);
        }

        [AdminAuth]
        public ActionResult DontAdmin(int id)
        {
            Users userUpdated = db.Users.Find(id);
            Users admin = Session["login"] as Users;

            if (userUpdated.Username == admin.Username)
            {

                userUpdated.IsAdmin = false;
                int result = db.SaveChanges();

                if (result > 0)
                {
                    Session.Clear();
                    Session["Login"] = userUpdated;

                    TempData["Result"] = "success";
                    TempData["Message"] = "Kullanıcının artık 'Admin' yetkisi yok.";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["Result"] = "danger";
                    TempData["Message"] = "Kullanıcının yetkisi değiştirilemedi !";
                    return RedirectToAction("List");
                }
            }
            else
            {
                userUpdated.IsAdmin = false;

                int result = db.SaveChanges();

                if (result > 0)
                {
                    TempData["Result"] = "success";
                    TempData["Message"] = "Kullanıcının artık 'Admin' yetkisi yok.";
                }
                else
                {
                    TempData["Result"] = "danger";
                    TempData["Message"] = "Kullanıcının yetkisi değiştirilemedi !";
                }

                return RedirectToAction("List");
            }
        }

        [AdminAuth]
        public ActionResult DoAdmin(int id)
        {
            Users userUpdated = db.Users.Find(id);

            userUpdated.IsAdmin = true;

            int result = db.SaveChanges();

            if (result > 0)
            {
                TempData["Result"] = "success";
                TempData["Message"] = "Kullanıcının artık 'Admin' yetkisi var.";
            }
            else
            {
                TempData["Result"] = "danger";
                TempData["Message"] = "Kullanıcının yetkisi değiştirilemedi !";
            }

            return RedirectToAction("List");
        }

        [AdminAuth]
        public ActionResult UserDelete(int id)
        {
            Users user = db.Users.Find(id);
            Users admin = Session["login"] as Users;

            if (user.Username == admin.Username)
            {
                Session.Clear();
                return RedirectToAction("Login", "User");
            }
            else
            {
                db.Users.Remove(user);
                int result = db.SaveChanges();

                if (result > 0)
                {
                    TempData["Result"] = "success";
                    TempData["Message"] = "Kullanıcı başarıyla silindi.";
                }
                else
                {
                    TempData["Result"] = "danger";
                    TempData["Message"] = "Kullanıcı silme işlemi başarısız !";
                }
                return RedirectToAction("List");
            }
        }

        public ActionResult Login()
        {
            if (TempData["Message"] != null && TempData["Result"] != null)
            {
                ViewBag.Result = TempData["Result"];
                ViewBag.Message = TempData["Message"];
                TempData["Result"] = null;
                TempData["Message"] = null;
            }

            return View();
        }

        [HttpPost]
        public ActionResult Login(Users user)
        {
            Users userLogin = db.Users.Where(u => u.Username == user.Username).FirstOrDefault();


            if (userLogin == null)
            {
                ViewBag.Result = "danger";
                ViewBag.Message = "Bu kullanıcı adına sahip bir kullanıcı bulunamadı !";
                return View(user);
            }
            else if (user.Password == null)
            {
                ViewBag.Result = "danger";
                ViewBag.Message = "Şifrenizi girmelisiniz !";
                return View(user);
            }
            else
            {
                string hashPassword = Crypto.Hash(user.Password, "SHA-256");
                if (userLogin.Password.CompareTo(hashPassword) == 0)
                {
                    Session["login"] = userLogin;
                    return RedirectToAction("Index", "Home");
                }
            }

            ViewBag.Result = "danger";
            ViewBag.Message = "Şifrenizi yanlış girdiniz.  Lütfen tekrar deneyiniz !";
            return View(user);
        }

        [Auth]
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login", "User");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Users user, string RePassword)
        {
            if (user.Username == null)
            {
                Users isUser = db.Users.Find(user.Username);
                if (isUser == null)
                {
                    ViewBag.Result = "danger";
                    ViewBag.Message = "Bu kullanıcı adına kullanılmaktadır. Lütfen başka bir kullanıcı adı giriniz !";
                    return View(user);
                }
                else
                {
                    ViewBag.Result = "danger";
                    ViewBag.Message = "Kullanıcı adınızı giriniz !";
                    return View(user);
                }
            }
            else if (user.Email == null)
            {
                Users isUser = db.Users.Find(user.Username);
                if (isUser == null)
                {
                    ViewBag.Result = "danger";
                    ViewBag.Message = "Bu email adresi kullanılmaktadır. !";
                    return View(user);
                }
                else
                {
                    ViewBag.Result = "danger";
                    ViewBag.Message = "Email adresinizi giriniz !";
                    return View(user);
                }
            }
            else if (user.Password == null)
            {
                ViewBag.Result = "danger";
                ViewBag.Message = "Şifrenizi giriniz !";
                return View(user);
            }
            else if (RePassword == null)
            {
                ViewBag.Result = "danger";
                ViewBag.Message = "Şifre Tekrarını boş geçemezsiniz !";
                return View(user);
            }
            else if (user.Password.CompareTo(RePassword) == 0)
            {
                user.Password = Crypto.Hash(user.Password, "SHA-256");
                user.IsAdmin = false;

                db.Users.Add(user);
                int result = db.SaveChanges();

                if (result > 0)
                {
                    TempData["Result"] = "success";
                    TempData["Message"] = "Kullanıcı kaydı oluşturuldu. Lütfen giriş yapınız.";
                    return RedirectToAction("Login", "User");
                }
                else
                {
                    ViewBag.Result = "danger";
                    ViewBag.Message = "Kullanıcı kaydı oluşturulamadı !";
                    return View(user);
                }
            }
            else
            {
                ViewBag.Result = "danger";
                ViewBag.Message = "Şifre ile Şifre Tekrarı eşleşmiyor !";
                return View(user);
            }
        }
    }
}