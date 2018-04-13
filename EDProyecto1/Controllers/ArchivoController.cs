using EDProyecto1.DBContext;
using EDProyecto1.Models;
using LibreriaDeClases.Clases;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EDProyecto1.Controllers
{
    public class ArchivoController : Controller
    {
        // GET: Archivo
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CargaArchivoJSON()
        {
            return View();
        }

        //Post SubirArchivoJSON
        [HttpPost]
        public ActionResult CargaArchivoJSON(HttpPostedFileBase file)
        {
            string filePath = string.Empty;
            Archivo modelo = new Archivo();
            if (file != null)
            {
                string ruta = Server.MapPath("~/Temp/");

                if (!Directory.Exists(ruta))
                {
                    Directory.CreateDirectory(ruta);
                }

                filePath = ruta + Path.GetFileName(file.FileName);

                string extension = Path.GetExtension(file.FileName);

                file.SaveAs(filePath);

                using (StreamReader r = new StreamReader(filePath))
                {
                    string json = r.ReadToEnd();
                    dynamic array = JsonConvert.DeserializeObject(json);
                    foreach (var item in array)
                    {


                        dynamic itemtemp = JsonConvert.DeserializeObject(item.Value.ToString());

                        Audiovisual temp = new Audiovisual();
                        temp.Tipo = itemtemp.Tipo;
                        temp.Nombre = itemtemp.Nombre;
                        temp.Anio = int.Parse(itemtemp.Anio);
                        temp.Genero = itemtemp.Genero;
                        //BNodo<Audiovisual> n = new BNodo<Audiovisual>();
                        if (itemtemp.Tipo == "Show")
                        {

                        }
                        else if (itemtemp.Tipo == "Movie")
                        {

                        }
                        else if (itemtemp.Tipo == "Documentary")
                        {

                        }

                        //DefaultConnection.miAVLFechas.logWriterAsignacion(HomeController.ruta, true);
                        //DBContext.DefaultConnection.miAVLFechas.Insertar(n);

                    }

                }


                modelo.SubirArchivo(ruta, file);

            }
            ViewBag.Error = modelo.error;
            ViewBag.Correcto = modelo.Confirmacion;
            return View();
        }
    }
}