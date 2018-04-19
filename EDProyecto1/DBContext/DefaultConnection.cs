using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EDProyecto1.Models;
using LibreriaDeClases.Clases;


namespace EDProyecto1.DBContext
{
    public class DefaultConnection
    {
        private static volatile DefaultConnection Instance;
        private static object syncRoot = new Object();

        public static BArbol<string, Audiovisual> BArbolShowPorNombre;
        public static BArbol<string, Audiovisual> BArbolShowPorAnio;
        public static BArbol<string, Audiovisual> BArbolShowPorGenero;
        public static BArbol<string, Audiovisual> BArbolMoviePorNombre;
        public static BArbol<string, Audiovisual> BArbolMoviePorAnio;
        public static BArbol<string, Audiovisual> BArbolMoviePorGenero;
        public static BArbol<string, Audiovisual> BArbolDocumentaryPorNombre;
        public static BArbol<string, Audiovisual> BArbolDocumentaryPorAnio;
        public static BArbol<string, Audiovisual> BArbolDocumentaryPorGenero;
        public static BArbol<string, Usuario> BArbolUsuarios;
        public static List<Usuario> usuarios = new List<Usuario>();

        public int IDActual { get; set; }

        private DefaultConnection()
        {
            IDActual = 0;
        }

        public static DefaultConnection getInstance
        {
            get
            {
                if (Instance == null)
                {
                    lock (syncRoot)
                    {
                        if (Instance == null)
                        {
                            Instance = new DefaultConnection();
                        }
                    }
                }
                return Instance;
            }
        }
    }
}