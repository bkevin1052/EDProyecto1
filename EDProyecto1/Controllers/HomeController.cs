﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LibreriaDeClases.Clases;
using System.Web.Mvc;

namespace EDProyecto1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}