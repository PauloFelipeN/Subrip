using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using subrip.Models;
using subrip.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace subrip.Controllers
{
    public class HomeController : Controller
    {
        private SubtitleRepository _repository;
        private IWebHostEnvironment _appEnvironment;

        public HomeController(IWebHostEnvironment env, SubtitleRepository repository)
        {
            _appEnvironment = env;
            _repository = repository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upload([FromForm] Subtitle model)
        {
            if (ModelState.IsValid)
            {
                var subtitle = new Subtitle(model.Offset, model.File);
                _repository.Add(subtitle);
                _repository.ProcessFile(subtitle);

                TempData["$Sucesso$"] = "Updalod realizado com sucesso!";
                return RedirectToAction("Download", new { id = subtitle.Id });
            }

            TempData["$AlertMessage$"] = "Modelo inválido!";
            return RedirectToAction("Index");
        }

        public FileResult Download([FromRoute] int id)
        {
            Subtitle result = _repository.Get(id);
            byte[] fileBytes = System.IO.File.ReadAllBytes(result.Path);

            TempData["$Sucesso$"] = "Updalod realizado com sucesso!";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet,result.FileName);
        }
    }
}
