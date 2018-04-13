using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaDeClases.Clases
{
    public class BNodo<TKey, T>
    {

        private int grado;

        public BNodo(int grado)
        {
            this.grado = grado;
            this.Hijos = new List<BNodo<TKey, T>>(grado);
            this.Entradas = new List<Entry<TKey, T>>(grado);
        }

        public List<BNodo<TKey, T>> Hijos { get; set; }

        public List<Entry<TKey, T>> Entradas { get; set; }

        public bool EsHoja
        {
            get
            {
                return this.Hijos.Count == 0;
            }
        }
        
        public bool AlcanzaMaximaEntrada
        {
            get
            {
                return this.Entradas.Count == (2*this.grado) - 1;
            }
        }

        public bool AlcanzaMinimaEntrada
        {
            get
            {
                return this.Entradas.Count == this.grado - 1;
            }
        }
    }
}
