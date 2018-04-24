using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LibreriaDeClases.Clases;

namespace EDProyecto1.Models
{
    public class Usuario
    {
        //Crear atributos de la clase Usuario
        public int IDUsuario { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int Edad { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public List<Audiovisual>WatchList = new List<Audiovisual>();
        public int FixedSize()
        {
            return 87;
        }
        public string ToFixedSizeString()
        {
            return $"{string.Format("{0,-20}", Nombre)},{string.Format("{0,-20}", Apellido)},{Edad.ToString("000;-000")},{string.Format("{0,-20}", Username)},{string.Format("{0,-20}", Password)}";
        }

    }
}