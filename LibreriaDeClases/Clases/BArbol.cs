using LibreriaDeClases.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaDeClases.Clases
{
    public class BArbol <TK, TP> where TK : IComparable<TK>
    {

        public BNodo<TK, TP> Raiz { get; private set; }

        public int Grado { get; private set; }

        public int Altura { get; private set; }

        public BArbol(int grado)
        {
            if(grado < 2)
            {
                throw new ArgumentException("Arbol B debe ser de por lo menos grado 2", "grado");
            }
            this.Raiz = new BNodo<TK, TP>(grado);
            this.Grado = grado;
            this.Altura = 1;
        }

        public Entry<TK, TP> Search(TK key)
        {
            return this.BusquedaInterna(this.Raiz, key);
        }

        public void Insertar(TK nuevaLlave, TP nuevoApuntador)
        {
            // Solo si hay espacio en la raiz
            if (!this.Raiz.HasReachedMaxEntries)
            {
                this.InsertarNoLleno(this.Raiz, nuevaLlave, nuevoApuntador);
                return;
            }
            // nuevo nodo y se necesita dividir

            BNodo<TK, TP> viejaRaiz = this.Raiz;
            this.Raiz = new BNodo<TK, TP>(this.Grado);
            this.Raiz.Hijos.Add(viejaRaiz);
            this.DividirHijo(this.Raiz, 0, viejaRaiz);
            this.InsertarNoLleno(this.Raiz, nuevaLlave, nuevoApuntador);
            this.Altura++;

        }

        private Entry<TK, TP> BusquedaInterna(BNodo<TK, TP> node, TK key)
        {
            int i = node.Entradas.TakeWhile(entry => key.CompareTo(entry.LLave) > 0).Count();

            if (i < node.Entradas.Count && node.Entradas[i].LLave.CompareTo(key) == 0)
            {
                return node.Entradas[i];
            }
            return node.IsLeaf ? null : this.BusquedaInterna(node.Hijos[i], key);
        }

        private void DividirHijo(BNodo<TK, TP> padreNodo, int nodoCorrer, BNodo<TK, TP> nodoMover)
        {

            var newNode = new BNodo<TK, TP>(this.Grado);

            padreNodo.Entradas.Insert(nodoCorrer, nodoMover.Entradas[this.Grado - 1]);

            padreNodo.Hijos.Insert(nodoCorrer + 1, newNode);

            newNode.Entradas.AddRange(nodoMover.Entradas.GetRange(this.Grado, this.Grado - 1));

            nodoMover.Entradas.RemoveRange(this.Grado - 1, this.Grado);
            if (!nodoMover.IsLeaf)
            {
                newNode.Hijos.AddRange(nodoMover.Hijos.GetRange(this.Grado, this.Grado));

                nodoMover.Hijos.RemoveRange(this.Grado, this.Grado);
            }

        }

        private void InsertarNoLleno(BNodo<TK, TP> nodo, TK nuevaLlave, TP nuevoApuntador)

        {

            int posicionInsertar= nodo.Entradas.TakeWhile(entry => nuevaLlave.CompareTo(entry.LLave) >= 0).Count();



            // Es hoja

            if (nodo.IsLeaf)

            {

                nodo.Entradas.Insert(posicionInsertar, new Entry<TK, TP>() {  LLave = nuevaLlave, Apuntador = nuevoApuntador });

                return;

            }



            // No es hoja

            BNodo<TK, TP> child = nodo.Hijos[posicionInsertar];

            if (child.HasReachedMaxEntries)

            {

                this.DividirHijo(nodo, posicionInsertar, child);

                if (nuevaLlave.CompareTo(nodo.Entradas[posicionInsertar].LLave) > 0)

                {

                    posicionInsertar++;

                }

            }



            this.InsertarNoLleno(nodo.Hijos[posicionInsertar], nuevaLlave, nuevoApuntador);

        }
    }
}
