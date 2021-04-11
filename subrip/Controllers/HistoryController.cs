using Microsoft.AspNetCore.Mvc;
using subrip.Models;
using subrip.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace subrip.Controllers
{
    public class HistoryController : Controller
    {
        private SubtitleRepository _repository;

        public HistoryController(SubtitleRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var result = _repository.GetAll();
            return View(result);
        }

        public FileResult Download([FromRoute] int id)
        {
            Subtitle result = _repository.Get(id);
            byte[] fileBytes = System.IO.File.ReadAllBytes(result.Path);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, result.FileName);
        }
    }
}
