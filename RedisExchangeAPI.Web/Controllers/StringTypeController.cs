using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class StringTypeController : Controller
    {
        private readonly RedisServices _redisService;

        private readonly IDatabase db;
        public StringTypeController(RedisServices redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(0);
        }
        public IActionResult Index()
        {
            //db0 veritabanını seçtik kaydetmek amaçlı.
            //var db = _redisService.GetDb(0);
            db.StringSet("name", "Asiye Nur Kelle");
            db.StringSet("ziyaretci", 100);
            db.StringSet("calisan", "Zeynep Kelle");
           
            return View();
        }

        public IActionResult Show()
        {
            var value = db.StringGet("name");
            var deneme = db.StringGet("calisan");
            //ziyaretci değerini bir arttırıcaz.
            db.StringIncrement("ziyaretci",10);
            //memoryde bu value'a sahip data varmı diye kontrol ettik varsa datayı aldık.
            if (value.HasValue)
            {
                ViewBag.value = value.ToString();
                ViewBag.deneme = deneme.ToString();
            }

            return View();
           
        }

    }
}
