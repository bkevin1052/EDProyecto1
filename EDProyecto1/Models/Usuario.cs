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
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int Edad { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public BArbol<string, Audiovisual> WatchList = new BArbol<string, Audiovisual>(3);

    }
}