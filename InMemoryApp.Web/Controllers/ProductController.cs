using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InMemoryApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryApp.Web.Controllers
{
    public class ProductController : Controller
    {
        private IMemoryCache _memoryCache;
        public ProductController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public IActionResult Index()
        {
            ////1.Yol
            //if (string.IsNullOrEmpty(_memoryCache.Get<string>("zaman")))
            //{
            //    //eger memoryde yoksa zamanı cacheleme
            //    _memoryCache.Set<string>("zaman", DateTime.Now.ToString());
            //    //zaman keyiyle şuanki zamanın değerine ulaşabilcez.
            //}
            //2.Yol
            //if(!_memoryCache.TryGetValue("zaman",out string zamancache))
            //trygetvalue degeri bool sonuc döner eger memoryde bu deger varsa alır zamancache'e atar ve true döner.
            //yoksa set etme işlemini yapıcaz aşagıda.
            //{

            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
            options.AbsoluteExpiration = DateTime.Now.AddSeconds(10);
           // options.SlidingExpiration = TimeSpan.FromSeconds(10);
            //cachein önem derecesini atıyoruz.Low atarsak bellek doldugunda ilk bu data bellekten silinir.never remove dersek ve eger cache dolarsa exception fırlatır.
            options.Priority = CacheItemPriority.High;

            //asagıdaki metotla silinen datanın neden silindigini ögreniriz.
            options.RegisterPostEvictionCallback((key, value, reason, state) =>
            {
                _memoryCache.Set("callback", $"{key}->{value} => sebep:{reason}");
            });
            
            _memoryCache.Set<string>("zaman", DateTime.Now.ToString(), options);


            Product p = new Product { Id = 1, Name = "Kalem", Price = 200 };
            _memoryCache.Set<Product>("product:1", p);




            return View();
        }
        //okuma işlemi
        public IActionResult Show()
        {
            _memoryCache.TryGetValue("zaman", out string zamancache);
            _memoryCache.TryGetValue("callback", out string callback);

            ViewBag.callback = callback;
            ViewBag.zaman = zamancache;


            ViewBag.product = _memoryCache.Get<Product>("product:1");

            ////bu metot içerisine verilen keye ait value varsa almaya calısır eger yoksa da entry metoduyla olusturur.
            //_memoryCache.GetOrCreate<string>("zaman", entry =>
            //{
            //    return DateTime.Now.ToString();
            //});
            ////memoryden ilgili key degeriyle datayı silmek.
            //_memoryCache.Remove("zaman");
            //ViewBag.zaman=_memoryCache.Get<String>("zaman");
            return View();
        }
    }
}
