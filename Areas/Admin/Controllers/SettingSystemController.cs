using ShopLearning.Models;
using ShopLearning.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static ShopLearning.Models.ApplicationUser;

namespace ShopLearning.Areas.Admin.Controllers
{
    public class SettingSystemController : Controller
    {
        // GET: Admin/SettingSystem
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Partial_Setting()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult AddSetting(SettingSystemViewModel rep)
        {
            SystemSetting set = null;
            var checkTitle = db.SystemSettings.FirstOrDefault(x => x.SettingKey.Contains("SettingTitle"));
            if(checkTitle == null ) 
            {
                 set = new SystemSetting();
                set.SettingKey = "SettingTitle";
                set.SettingValue = rep.SettingTitle;
                db.SystemSettings.Add(set);
            }
            else
            {
                checkTitle.SettingValue = rep.SettingTitle;
                db.Entry(checkTitle).State = System.Data.Entity.EntityState.Modified;

            }
            //Logo
            var checkLogo = db.SystemSettings.FirstOrDefault(x => x.SettingKey.Contains("SettingLogo"));
            if (checkLogo == null )
            {
                 set = new SystemSetting();
                set.SettingKey = "SettingLogo";
                set.SettingValue = rep.SettingLogo;
                db.SystemSettings.Add(set);
            }
            else
            {
                checkLogo.SettingValue = rep.SettingLogo;
                db.Entry(checkLogo).State = System.Data.Entity.EntityState.Modified;

            }

            //Email
            var email = db.SystemSettings.FirstOrDefault(x => x.SettingKey.Contains("SettingEmail"));
            if (email == null )
            {
                set = new SystemSetting();
                set.SettingKey = "SettingEmail";
                set.SettingValue = rep.SettingEmail;
                db.SystemSettings.Add(set);
            }
            else
            {
                email.SettingValue = rep.SettingEmail;
                db.Entry(email).State = System.Data.Entity.EntityState.Modified;

            }
            //HotLine
            var Hotline = db.SystemSettings.FirstOrDefault(x => x.SettingKey.Contains("SettingHotLine"));
            if (Hotline == null )
            {
                set = new SystemSetting();
                set.SettingKey = "SettingHotLine";
                set.SettingValue = rep.SettingHotLine;
                db.SystemSettings.Add(set);
            }
            else
            {
                Hotline.SettingValue = rep.SettingHotLine;
                db.Entry(Hotline).State = System.Data.Entity.EntityState.Modified;

            }
            //TitleSeo
            var TitleSeo = db.SystemSettings.FirstOrDefault(x => x.SettingKey.Contains("SettingTitleSeo"));
            if (TitleSeo == null)
            {
                set = new SystemSetting();
                set.SettingKey = "SettingTitleSeo";
                set.SettingValue = rep.SettingHotLine;
                db.SystemSettings.Add(set);
            }
            else
            {
                TitleSeo.SettingValue = rep.SettingTitleSeo;
                db.Entry(TitleSeo).State = System.Data.Entity.EntityState.Modified;

            }
            //DesSeo
            var DesSeo = db.SystemSettings.FirstOrDefault(x => x.SettingKey.Contains("SettingDesSeo"));
            if (DesSeo == null )
            {
                set = new SystemSetting();
                set.SettingKey = "SettingDesSeo";
                set.SettingValue = rep.SettingHotLine;
                db.SystemSettings.Add(set);
            }
            else
            {
                DesSeo.SettingValue = rep.SettingDesSeo;
                db.Entry(DesSeo).State = System.Data.Entity.EntityState.Modified;

            }
            //KeySeo
            var KeySeo = db.SystemSettings.FirstOrDefault(x => x.SettingKey.Contains("SettingKeySeo"));
            if (KeySeo == null )
            {
                set = new SystemSetting();
                set.SettingKey = "SettingKeySeo";
                set.SettingValue = rep.SettingKeySeo;
                db.SystemSettings.Add(set);
            }
            else
            {
                KeySeo.SettingValue = rep.SettingKeySeo;
                db.Entry(KeySeo).State = System.Data.Entity.EntityState.Modified;

            }
            db.SaveChanges();
            return View("Partial_Setting");
        }
    }
}