using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoogleCloudVision.Models;
using Microsoft.AspNetCore.Mvc;

namespace GoogleCloudVision.Controllers
{
    public class ShowResponseController : Controller
    {
        public IActionResult Index(string id)
        {
            ViewBag.GoogleResponse = id;
            return View();
        }
    }
}
