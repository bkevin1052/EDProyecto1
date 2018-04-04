using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaDeClases.Clases
{
    public class BNodo<TK, TP>
    {

        private int grado;

        public BNodo(int grado)
        {
            this.grado = grado;
            this.Hijos = new List<BNodo<TK, TP>>(grado);
            this.Entradas = new List<Entry<TK, TP>>(grado);
        }

        public List<BNodo<TK, TP>> Hijos { get; set; }

        public List<Entry<TK, TP>> Entradas { get; set; }

        public bool IsLeaf
        {
            get
            {
                return this.Hijos.Count == 0;
            }
        }
        
        public bool HasReachedMaxEntries
        {
            get
            {
                return this.Entradas.Count == (2 * this.grado) - 1;
            }
        }



        public bool HasReachedMinEntries
        {
            get
            {
                return this.Entradas.Count == this.grado - 1;
            }
        }
    }
}
