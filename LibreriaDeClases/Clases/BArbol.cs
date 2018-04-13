using LibreriaDeClases.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaDeClases.Clases
{
    public class BArbol <TKey, T> where TKey : IComparable<TKey>
    {

        public BNodo<TKey, T> Raiz { get; private set; }

        public int Grado { get; private set; }

        public int Altura { get; private set; }

        public BArbol(int grado)
        {
            if(grado < 2)
            {
                throw new ArgumentException("Arbol B debe ser de por lo menos grado 2", "grado");
            }
            this.Raiz = new BNodo<TKey, T>(grado);
            this.Grado = grado;
            this.Altura = 1;
        }

        public Entry<TKey, T> Search(TKey key)
        {
            return this.BusquedaInterna(this.Raiz, key);
        }

        public void Insertar(TKey nuevaLlave, T nuevoApuntador)
        {
            // Solo si hay espacio en la raiz
            if (!this.Raiz.AlcanzaMaximaEntrada)
            {
                this.InsertarNoLleno(this.Raiz, nuevaLlave, nuevoApuntador);
                return;
            }
            // nuevo nodo y se necesita dividir
            BNodo<TKey, T> viejaRaiz = this.Raiz;
            this.Raiz = new BNodo<TKey, T>(this.Grado);
            this.Raiz.Hijos.Add(viejaRaiz);
            this.DividirHijo(this.Raiz, 0, viejaRaiz);
            this.InsertarNoLleno(this.Raiz, nuevaLlave, nuevoApuntador);
            this.Altura++;

        }

        private Entry<TKey, T> BusquedaInterna(BNodo<TKey, T> node, TKey key)
        {
            int i = node.Entradas.TakeWhile(entry => key.CompareTo(entry.LLave) > 0).Count();

            if (i < node.Entradas.Count && node.Entradas[i].LLave.CompareTo(key) == 0)
            {
                return node.Entradas[i];
            }
            return node.EsHoja ? null : this.BusquedaInterna(node.Hijos[i], key);
        }

        public void Eliminar(TKey keyToDelete)
        {
            this.EliminarInterno(this.Raiz, keyToDelete);
            // Si la ultima raiz de la entrada fue movida a un nodo hijo la remueve
            if (this.Raiz.Entradas.Count == 0 && !this.Raiz.EsHoja)
            {
                this.Raiz = this.Raiz.Hijos.Single();
                this.Altura--;
            }

        }

        private void EliminarInterno(BNodo<TKey, T> nodo, TKey LlaveEliminar)
        {
            int i = nodo.Entradas.TakeWhile(entrada => LlaveEliminar.CompareTo(entrada.LLave) > 0).Count();
            // Encontro la llave en un nodo, la elimina
            if (i < nodo.Entradas.Count && nodo.Entradas[i].LLave.CompareTo(LlaveEliminar) == 0)

            {

                this.EliminarLlaveNodo(nodo, LlaveEliminar, i);

                return;

            }
            // Eliminar la lleva de un sub arbol
            if (!nodo.EsHoja)
            {
                this.EliminarLlaveSubArbol(nodo, LlaveEliminar, i);
            }
        }

        private void EliminarLlaveSubArbol(BNodo<TKey, T> NodoPadre, TKey LlaveEliminar, int IndiceSubArbol)
        {
            BNodo<TKey, T> NodoHijo = NodoPadre.Hijos[IndiceSubArbol];
            

            if (NodoHijo.AlcanzaMinimaEntrada)
            {
                int indiceIzquierdo = IndiceSubArbol - 1;
                BNodo<TKey, T> HijoIzquierdo = IndiceSubArbol > 0 ? NodoPadre.Hijos[indiceIzquierdo] : null;
                int indiceDerecho = IndiceSubArbol + 1;
                BNodo<TKey, T> HijoDerecho = IndiceSubArbol < NodoPadre.Hijos.Count - 1 ? NodoPadre.Hijos[indiceDerecho] : null;
                if (HijoIzquierdo != null && HijoIzquierdo.Entradas.Count > this.Grado - 1)
                {
                    NodoHijo.Entradas.Insert(0, NodoPadre.Entradas[IndiceSubArbol]);
                    NodoPadre.Entradas[IndiceSubArbol] = HijoIzquierdo.Entradas.Last();
                    HijoIzquierdo.Entradas.RemoveAt(HijoIzquierdo.Entradas.Count - 1);

                    if (!HijoIzquierdo.EsHoja)
                    {
                        NodoHijo.Hijos.Insert(0, HijoIzquierdo.Hijos.Last());
                        HijoIzquierdo.Hijos.RemoveAt(HijoIzquierdo.Hijos.Count - 1);
                    }
                }
                else if (HijoDerecho != null && HijoDerecho.Entradas.Count > this.Grado - 1)
                {)

                    NodoHijo.Entradas.Add(NodoPadre.Entradas[IndiceSubArbol]);
                    NodoPadre.Entradas[IndiceSubArbol] = HijoDerecho.Entradas.First();
                    HijoDerecho.Entradas.RemoveAt(0);

                    if (!HijoDerecho.EsHoja)
                    {
                        NodoHijo.Hijos.Add(HijoDerecho.Hijos.First());
                        HijoDerecho.Hijos.RemoveAt(0);
                    }
                }
                else
                {
                    if (HijoIzquierdo != null)
                    {
                        NodoHijo.Entradas.Insert(0, NodoPadre.Entradas[IndiceSubArbol]);
                        var oldEntries = NodoHijo.Entradas;
                        NodoHijo.Entradas = HijoIzquierdo.Entradas;
                        NodoHijo.Entradas.AddRange(oldEntries);
                        if (!HijoIzquierdo.EsHoja)
                        {
                            var oldChildren = NodoHijo.Hijos;
                            NodoHijo.Hijos = HijoIzquierdo.Hijos;
                            NodoHijo.Hijos.AddRange(oldChildren);
                        }
                        NodoPadre.Hijos.RemoveAt(indiceIzquierdo);
                        NodoPadre.Entradas.RemoveAt(IndiceSubArbol);
                    }
                    else
                    {
                        Debug.Assert(HijoDerecho != null, "Nodo debe tener por lo menos un hermano");

                        NodoHijo.Entradas.Add(NodoPadre.Entradas[IndiceSubArbol]);
                        NodoHijo.Entradas.AddRange(HijoDerecho.Entradas);
                        if (!HijoDerecho.EsHoja)

                        {

                            NodoHijo.Hijos.AddRange(HijoDerecho.Hijos);
                        }
                        NodoPadre.Hijos.RemoveAt(indiceDerecho);
                        NodoPadre.Entradas.RemoveAt(IndiceSubArbol);
                    }
                }
            }
            this.EliminarInterno(NodoHijo, LlaveEliminar);
        }

        private void EliminarLlaveNodo(BNodo<TKey, T> nodo, TKey LlaveEliminar, int indiceLlaveNodo)
        {
            if (nodo.EsHoja)
            {
                nodo.Entradas.RemoveAt(indiceLlaveNodo);
                return;
            }
            BNodo<TKey, T> predecesorHijo= nodo.Hijos[indiceLlaveNodo];
            if (predecesorHijo.Entradas.Count >= this.Grado)
            {
                Entry<TKey, T> predecessor = this.EliminarPredecesor(predecesorHijo);
                nodo.Entradas[indiceLlaveNodo] = predecessor;
            }
            else
            {
                BNodo<TKey, T> succesorHijo = nodo.Hijos[indiceLlaveNodo + 1];
                if (succesorHijo.Entradas.Count >= this.Grado)
                {
                    Entry<TKey, T> successor = this.EliminarSucesor(predecesorHijo);
                    nodo.Entradas[indiceLlaveNodo] = successor;

                }
                else
                {
                    predecesorHijo.Entradas.Add(nodo.Entradas[indiceLlaveNodo]);
                    predecesorHijo.Entradas.AddRange(succesorHijo.Entradas);
                    predecesorHijo.Hijos.AddRange(succesorHijo.Hijos);



                    nodo.Entradas.RemoveAt(indiceLlaveNodo);
                    nodo.Hijos.RemoveAt(indiceLlaveNodo + 1);
                    this.EliminarInterno(predecesorHijo, LlaveEliminar);

                }

            }

        }

        private Entry<TKey, T> EliminarPredecesor(BNodo<TKey, T> nodo)
        {
            if (nodo.EsHoja)
            {
                var result = nodo.Entradas[nodo.Entradas.Count - 1];
                nodo.Entradas.RemoveAt(nodo.Entradas.Count - 1);
                return result;
            }
            return this.EliminarPredecesor(nodo.Hijos.Last());
        }

        private Entry<TKey, T> EliminarSucesor(BNodo<TKey, T> nodo)
        {
            if (nodo.EsHoja)
            {
                var result = nodo.Entradas[0];
                nodo.Entradas.RemoveAt(0);
                return result;
            }
            return this.EliminarPredecesor(nodo.Hijos.First());
        }

        private void DividirHijo(BNodo<TKey, T> padreNodo, int nodoCorrer, BNodo<TKey, T> nodoMover)
        {

            var nuevoNodo = new BNodo<TKey, T>(this.Grado);

            padreNodo.Entradas.Insert(nodoCorrer, nodoMover.Entradas[this.Grado - 1]);

            padreNodo.Hijos.Insert(nodoCorrer + 1, nuevoNodo);

            nuevoNodo.Entradas.AddRange(nodoMover.Entradas.GetRange(this.Grado , this.Grado-1));

            nodoMover.Entradas.RemoveRange(this.Grado - 1, this.Grado);
            if (!nodoMover.EsHoja)
            {
                nuevoNodo.Hijos.AddRange(nodoMover.Hijos.GetRange(this.Grado, this.Grado));

                nodoMover.Hijos.RemoveRange(this.Grado, this.Grado);
            }

        }

        private void InsertarNoLleno(BNodo<TKey, T> nodo, TKey nuevaLlave, T nuevoApuntador)
        {
            int posicionInsertar= nodo.Entradas.TakeWhile(entry => nuevaLlave.CompareTo(entry.LLave) >= 0).Count();
            // Es hoja
            if (nodo.EsHoja)
            {
                nodo.Entradas.Insert(posicionInsertar, new Entry<TKey, T>() {  LLave = nuevaLlave, Apuntador = nuevoApuntador });
                return;
            }
            // No es hoja

            BNodo<TKey, T> hijo = nodo.Hijos[posicionInsertar];
            if (hijo.AlcanzaMaximaEntrada)
            {
                this.DividirHijo(nodo, posicionInsertar, hijo);
                if (nuevaLlave.CompareTo(nodo.Entradas[posicionInsertar].LLave) > 0)
                {
                    posicionInsertar++;
                }
            }
            this.InsertarNoLleno(nodo.Hijos[posicionInsertar], nuevaLlave, nuevoApuntador);
        }
    }
}
