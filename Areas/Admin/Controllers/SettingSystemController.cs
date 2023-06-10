using ShopOnline.Data;
using ShopOnline.Models;
using ShopOnline.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;

namespace ShopOnline.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SettingSystemController : Controller
    {
        private readonly DataAccessLayer _dal = new DataAccessLayer();

        // GET: Admin/SettingSystem
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Partial_Setting()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult AddSetting(SettingSystemViewModel req)
        {
            // title
            var checkTitle = _dal.GetFiltered<SystemSetting>(x => x.SettingKey.Contains("SettingTitle")).FirstOrDefault();
            if (checkTitle == null)
            {
                var set = new SystemSetting();
                set.SettingKey = "SettingTitle";
                set.SettingValue = req.SettingTitle;
                _dal.Add(set);
            }
            else
            {
                checkTitle.SettingValue = req.SettingTitle;
                _dal.Update(checkTitle);
            }

            // logo
            var checkLogo = _dal.GetFiltered<SystemSetting>(x => x.SettingKey.Contains("SettingLogo")).FirstOrDefault();
            if (checkLogo == null)
            {
                var set = new SystemSetting();
                set.SettingKey = "SettingLogo";
                set.SettingValue = req.SettingLogo;
                _dal.Add(set);
            }
            else
            {
                checkLogo.SettingValue = req.SettingLogo;
                _dal.Update(checkLogo);
            }

            // email
            var checkEmail = _dal.GetFiltered<SystemSetting>(x => x.SettingKey.Contains("SettingEmail")).FirstOrDefault();
            if (checkEmail == null)
            {
                var set = new SystemSetting();
                set.SettingKey = "SettingEmail";
                set.SettingValue = req.SettingEmail;
                _dal.Add(set);
            }
            else
            {
                checkEmail.SettingValue = req.SettingEmail;
                _dal.Update(checkEmail);
            }

            // hotline
            var checkHotline = _dal.GetFiltered<SystemSetting>(x => x.SettingKey.Contains("SettingHotline")).FirstOrDefault();
            if (checkHotline == null)
            {
                var set = new SystemSetting();
                set.SettingKey = "SettingHotline";
                set.SettingValue = req.SettingHotline;
                _dal.Add(set);
            }
            else
            {
                checkHotline.SettingValue = req.SettingHotline;
                _dal.Update(checkHotline);
            }

            // title seo
            var checkTitleSeo = _dal.GetFiltered<SystemSetting>(x => x.SettingKey.Contains("SettingTitleSeo")).FirstOrDefault();
            if (checkTitleSeo == null)
            {
                var set = new SystemSetting();
                set.SettingKey = "SettingTitleSeo";
                set.SettingValue = req.SettingTitleSeo;
                _dal.Add(set);
            }
            else
            {
                checkTitleSeo.SettingValue = req.SettingTitleSeo;
                _dal.Update(checkTitleSeo);
            }

            // des seo
            var checkDesSeo = _dal.GetFiltered<SystemSetting>(x => x.SettingKey.Contains("SettingDesSeo")).FirstOrDefault();
            if (checkDesSeo == null)
            {
                var set = new SystemSetting();
                set.SettingKey = "SettingDesSeo";
                set.SettingValue = req.SettingDesSeo;
                _dal.Add(set);
            }
            else
            {
                checkDesSeo.SettingValue = req.SettingDesSeo;
                _dal.Update(checkDesSeo);
            }

            // key seo
            var checkKeySeo = _dal.GetFiltered<SystemSetting>(x => x.SettingKey.Contains("SettingKeySeo")).FirstOrDefault();
            if (checkKeySeo == null)
            {
                var set = new SystemSetting();
                set.SettingKey = "SettingKeySeo";
                set.SettingValue = req.SettingKeySeo;
                _dal.Add(set);
            }
            else
            {
                checkKeySeo.SettingValue = req.SettingKeySeo;
                _dal.Update(checkKeySeo);
            }

            _dal.SaveChanges();
            return View("Index");
        }
    }
}
