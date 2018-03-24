using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaDeClases.Clases
{
    public class Nodo<T>
    {
        public T[] valores;
        public Nodo<T>[] nodo;
        public static int numValores;
        public bool tengoHijos = false;
        public int ocupados = 0;
        public Nodo<T> padre;
        public Nodo()
        {
            //nodo = new Nodo[Raiz.grado * 2 + 3];
            //valores = new int[Raiz.grado * 2 + 1];
        }
    }
}
