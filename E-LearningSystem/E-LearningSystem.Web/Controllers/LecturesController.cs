﻿namespace E_LearningSystem.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class LecturesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
