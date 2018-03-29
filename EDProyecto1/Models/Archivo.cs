using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDProyecto1.Models
{
    public class Archivo
    {
        public string Confirmacion { get; set; }
        public Exception error { get; set; }
        public void SubirArchivo(string ruta, HttpPostedFileBase file)
        {
            try
            {
                file.SaveAs(ruta);
                this.Confirmacion = "Fichero Guardado";
            }
            catch (Exception ex)
            {
                this.error = ex;
            }

        }
    }
}