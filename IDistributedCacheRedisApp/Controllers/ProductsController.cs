using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace IDistributedCacheRedisApp.Web.Controllers
{
    public class ProductsController : Controller
    {

        private IDistributedCache _distributedCache;
        public ProductsController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        public async Task<IActionResult> Index()
        {
            DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();

            cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(1);

            _distributedCache.SetString("surname", "asiye", cacheEntryOptions);
            await _distributedCache.SetStringAsync("nickname", "nur");

            return View();
        }
        public IActionResult Show()
        {
            string surname = _distributedCache.GetString("surname");
            ViewBag.surname = surname;
            return View();
        }

        public IActionResult Remove()
        {
            _distributedCache.Remove("surname");
            return View();
        }
    }
}

