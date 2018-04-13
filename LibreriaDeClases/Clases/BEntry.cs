using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaDeClases.Clases
{
    public class Entry<TKey, T> : IEquatable<Entry<TKey, T>>
    {
        public TKey LLave { get; set; }

        public T Apuntador { get; set; }

        public bool Equals(Entry<TKey, T> other)
        {
           return this.LLave.Equals(other.LLave) && this.Apuntador.Equals(other.Apuntador);
        }

    }
}
