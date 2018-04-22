using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDProyecto1.Models
{
    public class Audiovisual
    {
        //Atributos de la clase Audiovisual
        public int AudioVisualID { get; set; }
        public string Tipo { get; set; }
        public string Nombre { get; set; }
        public int Anio { get; set; }
        public string Genero { get; set; }

        public string ToFixedSizeString()
        {
            return $"{string.Format("{0,-20}", Tipo)},{string.Format("{0,-20}", Nombre)},{Anio.ToString("000;-000")},{string.Format("{0,-20}", Genero)}";
        }

    }
}