using System;
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
            BArbol<string, string> arbol = new BArbol<string, string>(4);
            arbol.Insertar("Hola", "123");
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