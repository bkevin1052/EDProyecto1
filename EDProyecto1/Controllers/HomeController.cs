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
            BArbol<int, int> arbol = new BArbol<int, int>(3);
            arbol.Insertar(1,20);
            arbol.Insertar(2,30);
            arbol.Insertar(3,100);
            arbol.Insertar(4,40);
            arbol.Insertar(5,20);
            arbol.Insertar(6,30);
            arbol.Insertar(7,10);



            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}