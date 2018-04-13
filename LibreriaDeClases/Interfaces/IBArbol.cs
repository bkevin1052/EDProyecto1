using LibreriaDeClases.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaDeClases.Interfaces
{
    public interface IBArbol<TKey, T>
    {
        Entry<TKey, T> Search(TKey key);
        void Insertar(TKey nuevaLlave, T nuevoApuntador);
        Entry<TKey, T> BusquedaInterna(BNodo<TKey, T> node, TKey key);
        void Eliminar(TKey LlaveEliminar);
        void EliminarInterno(BNodo<TKey, T> nodo, TKey LlaveEliminar);
        void EliminarLlaveSubArbol(BNodo<TKey, T> NodoPadre, TKey LlaveEliminar, int IndiceSubArbol);
        void EliminarLlaveNodo(BNodo<TKey, T> nodo, TKey LlaveEliminar, int indiceLlaveNodo);
        Entry<TKey, T> EliminarPredecesor(BNodo<TKey, T> nodo);
        Entry<TKey, T> EliminarSucesor(BNodo<TKey, T> nodo);
        void DividirHijo(BNodo<TKey, T> padreNodo, int nodoCorrer, BNodo<TKey, T> nodoMover);
        void InsertarNoLleno(BNodo<TKey, T> nodo, TKey nuevaLlave, T nuevoApuntador);


    }
}
