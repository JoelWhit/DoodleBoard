using Microsoft.AspNet.SignalR;
using DoodleBoard.Contract.Model;
using DoodleBoard.Contract.Repository;
using DoodleBoard.Model;
using DoodleBoard.Website.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoodleBoard.Website.Controllers
{    
    public class WhiteboardController : Controller
    {
        private readonly IWhiteboardRepository _wbRepository;

        public WhiteboardController(IWhiteboardRepository wbRepository)
        {
            _wbRepository = wbRepository;
        }

        [HttpGet]
        [Route("whiteboard/{id}")]
        public ActionResult Index(string id)
        {          
            return View(new WhiteboardViewModel(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("whiteboard/create")]
        public ActionResult Create(CreateWhiteboardViewModel model)
        {
            IWhiteboard wb = new Whiteboard(model.Password);
            _wbRepository.Save(wb);
            _wbRepository.SaveChanges();
            return RedirectToAction("Index", "Whiteboard", new { id = wb.Id });
        }
    }
}