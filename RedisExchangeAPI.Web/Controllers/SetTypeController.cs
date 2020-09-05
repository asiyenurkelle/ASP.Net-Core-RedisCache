using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class SetTypeController : Controller
    {
        private readonly RedisServices _redisService;
        private readonly IDatabase db;

        private string listKey = "hashnames";

        public SetTypeController(RedisServices redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(2);
        }
        public IActionResult Index()
        {
            HashSet<string> namesList = new HashSet<string>();
            if (db.KeyExists(listKey))
            {
                db.SetMembers(listKey).ToList().ForEach(x =>
                {
                    namesList.Add(x.ToString());
                });
            }
            return View(namesList);
        }

        [HttpPost]
        public IActionResult Add(string name)
        {

            db.KeyExpire(listKey, DateTime.Now.AddMinutes(5));

            db.SetAdd(listKey, name);

            return RedirectToAction("Index");
        }

        public async Task <IActionResult> DeleteItem(string name)
        {
            //EGER Bİ METOT İCERİSİNDE BİRDEN FAZLA ASYNC KOD VARSA METODU ASYNC YAPARIZ.
            await db.SetRemoveAsync(listKey, name);
            return RedirectToAction("Index");
        }
    }
}
