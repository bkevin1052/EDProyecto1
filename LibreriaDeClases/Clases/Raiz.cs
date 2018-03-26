using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaDeClases.Clases
{
    public class Raiz<T>
    {
        public static int Grado;
        public Nodo<T> PrimerNodo;
        public static bool EsRaiz;
        public static int Nivel = 1;
        public static int Imprimir = 1;
        public static String Arbol = "";
        public Raiz(int grado)
        {
            Grado = grado;
            PrimerNodo = new Nodo<T>();
            List<T> llevarIngresos = new List<T>();
            EsRaiz = true;
        }
    }
}
