using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.Code;

namespace WebApp.Controllers
{
    public class StatusController : Controller
    {
        private MosaicStoreManager _mosaicStoreManager;

        public StatusController(MosaicStoreManager mosaicStoreManager)
        {
            _mosaicStoreManager = mosaicStoreManager;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Mosaic()
        {
            
            return View(_mosaicStoreManager);
        }
    }
}