using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaDeClases.Clases
{
    public class Entry<TK, TP> : IEquatable<Entry<TK, TP>>
    {
        public TK LLave { get; set; }

        public TP Apuntador { get; set; }

        public bool Equals(Entry<TK, TP> other)
        {
           return this.LLave.Equals(other.LLave) && this.Apuntador.Equals(other.Apuntador);
        }

    }
}
