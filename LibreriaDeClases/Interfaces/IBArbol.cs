using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibreriaDeClases.Clases;

namespace LibreriaDeClases.Interfaces
{
    public interface IBArbol<TK,TP>
    {
        Entry<TK, TP> Search(TK key);
        void Insertar(TK nuevaLlave, TP nuevoApuntador);

    }
}
