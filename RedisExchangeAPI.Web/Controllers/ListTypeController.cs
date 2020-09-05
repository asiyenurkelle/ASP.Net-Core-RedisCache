using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class ListTypeController : Controller
    {
        private readonly RedisServices _redisService;
        private readonly IDatabase db;
        private string listKey = "names";

        public ListTypeController(RedisServices redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(1);
        }
        public IActionResult Index()
        {
            //redisteki dataları okucaz.
            List<string> namesList = new List<string>();
            if (db.KeyExists(listKey))
            {
                //verileri okutturduk.en baştan memorydeki tüm dataları okur.
                db.ListRange(listKey).ToList().ForEach(x =>
                {
                    namesList.Add(x.ToString());
                });
            }
            return View(namesList);
        }
        [HttpPost]
        public IActionResult Add(string name)
        {
            //redise data ekleme.
            db.ListLeftPush(listKey, name);
            return RedirectToAction("Index");
        }

        public IActionResult DeleteItem(string name)
        {
            //kayıtlı dataları silme metodu
            //uygulamalarda async metotlar kullanmak uygulama performansını arttırır.
            db.ListRemoveAsync(listKey, name).Wait();
            return RedirectToAction("Index");
        }
    }
}
