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
        public static BArbol<int, Audiovisual> BArbolShowPorAnio = new BArbol<int, Audiovisual>(3);
        public static BArbol<string, Audiovisual> BArbolShowPorGenero = new BArbol<string, Audiovisual>(3);
        public static BArbol<string, Audiovisual> BArbolMoviePorNombre = new BArbol<string, Audiovisual>(3);
        public static BArbol<int, Audiovisual> BArbolMoviePorAnio = new BArbol<int, Audiovisual>(3);
        public static BArbol<string, Audiovisual> BArbolMoviePorGenero = new BArbol<string, Audiovisual>(3);
        public static BArbol<string, Audiovisual> BArbolDocumentaryPorNombre = new BArbol<string, Audiovisual>(3);
        public static BArbol<int, Audiovisual> BArbolDocumentaryPorAnio = new BArbol<int, Audiovisual>(3);
        public static BArbol<string, Audiovisual> BArbolDocumentaryPorGenero = new BArbol<string, Audiovisual>(3);
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