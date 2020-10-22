using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GoogleCloudVision.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace GoogleCloudVision.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(List<IFormFile> files)
        {
            var file = files.First();
            using (var client = new WebClient())
            {
                Mainrequests Mainrequests = new Mainrequests()
                {

                    requests = new List<requests>()
                    {
                        new requests()
                        {
                            image = new image()
                            {
                                content = ImageToBase64(file)
                            },

                            features = new List<features>()
                            {
                                new features()
                                {
                                    type = "FACE_DETECTION",
                                }
                            }

                        }

                    }

                };

                client.Headers.Add("Content-Type:application/json");
                client.Headers.Add("Accept:application/json");
                var response =
                    client.UploadString(
                        "https://vision.googleapis.com/v1/images:annotate?key=" +
                        "ApiKey", JsonConvert.SerializeObject(Mainrequests));

                //var ret = Json(data: response);
                var jsonReturn = JsonConvert.DeserializeObject<JsonReturn>(response);
                return RedirectToAction("Index", "ShowResponse", new{id=jsonReturn.responses[0].faceAnnotations[0].joyLikelihood});
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public string ImageToBase64(IFormFile file)
        {
            byte[] bytes = null;
            using MemoryStream memoryStream = new MemoryStream();
            file.CopyToAsync(memoryStream).GetAwaiter();
            memoryStream.Position = 0;
            bytes = memoryStream.ToArray();
            var base64String = Convert.ToBase64String(bytes);
            return base64String;
        }
    }
}
