using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LibreriaDeClases.Clases;
using System.Web.Mvc;
using EDProyecto1.DBContext;
using EDProyecto1.Models;
using System.IO;

namespace EDProyecto1.Controllers
{
    public class HomeController : Controller
    {
        string rutaUsuarios = @"C:\Users\" + Environment.UserName + @"\users.tree";
        string rutaArbolShowporNombre = @"C:\Users\" + Environment.UserName + @"\name.showtree";
        string rutaArbolShowporGenero = @"C:\Users\" + Environment.UserName + @"\genre.showtree";
        string rutaArbolShowporAnio = @"C:\Users\" + Environment.UserName + @"\year.showtree";
        string rutaArbolMovieporNombre = @"C:\Users\" + Environment.UserName + @"\name.movietree";
        string rutaArbolMovieporGenero = @"C:\Users\" + Environment.UserName + @"\genre.movietree";
        string rutaArbolMovieporAnio = @"C:\Users\" + Environment.UserName + @"\year.movietree";
        string rutaArbolDocumentaryporNombre = @"C:\Users\" + Environment.UserName + @"\name.documentarytree";
        string rutaArbolDocumentaryporGenero = @"C:\Users\" + Environment.UserName + @"\genre.documentarytree";
        string rutaArbolDocumentaryporAnio = @"C:\Users\" + Environment.UserName + @"\year.documentarytree";
        public ActionResult Index()
        {
            if(!System.IO.File.Exists(rutaUsuarios))
            {
                return View("IngresoGrado");
            }
            else
            {
                LeerGrado();
                return View();
            }
            
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult IngresoGrado()
        {
            return View();
        }
        [HttpPost]
        public ActionResult IngresoGrado(string Grado)
        {
            int grado;
            if(int.TryParse(Grado, out grado))
            {
                grado = int.Parse(Grado);
                if (grado > 1)
                {
                    DefaultConnection.BArbolUsuarios = new BArbol<string, Usuario>(grado);
                    EscribirGrado(rutaUsuarios, false, grado);
                    DefaultConnection.BArbolShowPorNombre = new BArbol<string, Audiovisual>(grado);
                    EscribirGrado(rutaArbolShowporNombre, false, grado);
                    DefaultConnection.BArbolShowPorAnio = new BArbol<int, Audiovisual>(grado);
                    EscribirGrado(rutaArbolShowporAnio, false, grado);
                    DefaultConnection.BArbolShowPorGenero = new BArbol<string, Audiovisual>(grado);
                    EscribirGrado(rutaArbolShowporGenero, false, grado);
                    DefaultConnection.BArbolMoviePorNombre = new BArbol<string, Audiovisual>(grado);
                    EscribirGrado(rutaArbolMovieporNombre, false, grado);
                    DefaultConnection.BArbolMoviePorAnio = new BArbol<int, Audiovisual>(grado);
                    EscribirGrado(rutaArbolMovieporAnio, false, grado);
                    DefaultConnection.BArbolMoviePorGenero = new BArbol<string, Audiovisual>(grado);
                    EscribirGrado(rutaArbolMovieporGenero, false, grado);
                    DefaultConnection.BArbolDocumentaryPorNombre = new BArbol<string, Audiovisual>(grado);
                    EscribirGrado(rutaArbolDocumentaryporNombre, false, grado);
                    DefaultConnection.BArbolDocumentaryPorAnio = new BArbol<int, Audiovisual>(grado);
                    EscribirGrado(rutaArbolDocumentaryporAnio, false, grado);
                    DefaultConnection.BArbolDocumentaryPorGenero = new BArbol<string, Audiovisual>(grado);
                    EscribirGrado(rutaArbolDocumentaryporGenero, false, grado);
                    return View("Index");
                }
                else
                {
                    TempData["alertMessage"] = "Ingrese un grado válido. El grado debe ser mayor o igual a 2.";
                    return View();
                }
            }
            else
            {
                TempData["alertMessage"] = "Ingrese un grado válido. El grado debe ser mayor o igual a 2.";
                return View();
            }
            return View();
        }
        private void EscribirGrado(string rutaArchivo, bool sobrescribir, int grado)
        {
            StreamWriter GradoWriter = new StreamWriter(rutaArchivo, sobrescribir);
            GradoWriter.WriteLine("Grado: "+grado.ToString());
            GradoWriter.Close();
        }

        private void LeerGrado()
        {
            string linea;
            int grado;
            StreamReader lector = new StreamReader(rutaUsuarios);
            linea = lector.ReadLine();
            var array = linea.Split(':', ' ');
            grado = int.Parse(array[2]);
            DefaultConnection.BArbolUsuarios = new BArbol<string, Usuario>(grado);
            DefaultConnection.BArbolShowPorNombre = new BArbol<string, Audiovisual>(grado);
            DefaultConnection.BArbolShowPorAnio = new BArbol<int, Audiovisual>(grado);
            DefaultConnection.BArbolShowPorGenero = new BArbol<string, Audiovisual>(grado);
            DefaultConnection.BArbolMoviePorNombre = new BArbol<string, Audiovisual>(grado);
            DefaultConnection.BArbolMoviePorAnio = new BArbol<int, Audiovisual>(grado);
            DefaultConnection.BArbolMoviePorGenero = new BArbol<string, Audiovisual>(grado);
            DefaultConnection.BArbolDocumentaryPorNombre = new BArbol<string, Audiovisual>(grado);
            DefaultConnection.BArbolDocumentaryPorAnio = new BArbol<int, Audiovisual>(grado);
            DefaultConnection.BArbolDocumentaryPorGenero = new BArbol<string, Audiovisual>(grado);

        }
    }
}