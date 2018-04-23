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

        /// <summary>
        /// Metodo Constructor del arbol
        /// </summary>
        /// <param name="grado">Grado del arbol</param>
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

        /// <summary>
        /// Metodo para buscar un objeto y/o dato almacenado en la llave
        /// </summary>
        /// <param name="Llave"></param>
        /// <returns>Valor buscado</returns>
        public Entry<TKey, T> Search(TKey Llave)
        {
            return this.BusquedaInterna(this.Raiz, Llave);
        }

        /// <summary>
        /// Metodo para insertar una nueva llave junto con su objeto o tipo de dato
        /// </summary>
        /// <param name="nuevaLlave"> entero o cadena</param>
        /// <param name="nuevoApuntador">objeto</param>
        public void Insertar(TKey nuevaLlave, T nuevoApuntador)
        {
            InsertarRecursivo(this.Raiz, nuevaLlave, nuevoApuntador, null);

        }


        private void InsertarRecursivo(BNodo<TKey, T> nodo, TKey nuevaLlave, T nuevoApuntador, BNodo<TKey, T> nodoPadre)
        {
            int posicionInsertar = nodo.Entradas.TakeWhile(entry => nuevaLlave.CompareTo(entry.LLave) >= 0).Count();
            //Es hoja
            if (nodo.EsHoja)
            {
                if (this.Raiz == nodo)
                {
                    this.Raiz.Entradas.Insert(posicionInsertar, new Entry<TKey, T>() { LLave = nuevaLlave, Apuntador = nuevoApuntador });
                    if(this.Raiz.AlcanzaMaximaEntrada)
                    {
                        // nuevo nodo y se necesita dividir
                        BNodo<TKey, T> viejaRaiz = this.Raiz;
                        this.Raiz = new BNodo<TKey, T>(this.Grado);
                        this.Raiz.Hijos.Add(viejaRaiz);
                        this.DividirHijo(this.Raiz, 0, viejaRaiz);
                        this.Altura++; 
                    }
                    return;
                }
                else
                {
                    nodo.Entradas.Insert(posicionInsertar, new Entry<TKey, T>() { LLave = nuevaLlave, Apuntador = nuevoApuntador });
                    if (nodo.AlcanzaMaximaEntrada)
                    {
                        posicionInsertar = nodoPadre.Entradas.TakeWhile(entry => nuevaLlave.CompareTo(entry.LLave) >= 0).Count();
                        DividirHijo(nodoPadre, posicionInsertar, nodo);    
                    }
                    return;
                }
            }
            //No es Hoja
            else
            {
                this.InsertarRecursivo(nodo.Hijos[posicionInsertar], nuevaLlave, nuevoApuntador, nodo);
                if (nodoPadre == null)
                {
                    if(nodo.AlcanzaMaximaEntrada)
                    {
                        BNodo<TKey, T> viejaRaiz = this.Raiz;
                        this.Raiz = new BNodo<TKey, T>(this.Grado);
                        this.Raiz.Hijos.Add(viejaRaiz);
                        this.DividirHijo(this.Raiz, 0, viejaRaiz);
                        this.Altura++;
                    }
                    return;
                }
                else
                {
                    if (nodo.AlcanzaMaximaEntrada)
                    {
                        DividirHijo(nodoPadre, posicionInsertar, nodo);
                    }
                    return;
                }
            }

        }

        /// <summary>
        /// Metodo para realizar un busqueda interna entre nodos
        /// </summary>
        /// <param name="node">nodo a buscar</param>
        /// <param name="key">llave a buscar</param>
        /// <returns>valor buscado</returns>
        private Entry<TKey, T> BusquedaInterna(BNodo<TKey, T> node, TKey key)
        {
            int i = node.Entradas.TakeWhile(entry => key.CompareTo(entry.LLave) > 0).Count();

            if (i < node.Entradas.Count && node.Entradas[i].LLave.CompareTo(key) == 0)
            {
                return node.Entradas[i];
            }
            return node.EsHoja ? null : this.BusquedaInterna(node.Hijos[i], key);
        }

        /// <summary>
        /// Metodo para realizar una eliminacion en el arbol
        /// </summary>
        /// <param name="LlaveEliminar">valor a eliminar</param>
        public void Eliminar(TKey LlaveEliminar)
        {
            this.EliminarInterno(this.Raiz, LlaveEliminar);
            // Si la ultima raiz de la entrada fue movida a un nodo hijo la remueve
            if (this.Raiz.Entradas.Count == 0 && !this.Raiz.EsHoja)
            {
                this.Raiz = this.Raiz.Hijos.Single();
                this.Altura--;
            }

        }

        /// <summary>
        /// Metodo para eliminar de manera interna una llave
        /// </summary>
        /// <param name="nodo">nodo a buscar</param>
        /// <param name="LlaveEliminar">llave a eliminar</param>
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

        /// <summary>
        /// Metodo para eliminar la llave de un subarbol,es decir, valor raiz 
        /// </summary>
        /// <param name="NodoPadre">raiz</param>
        /// <param name="LlaveEliminar">valor a eliminar</param>
        /// <param name="IndiceSubArbol">ubicacion del valor</param>
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
                {

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
                        NodoHijo.Entradas.Insert(0, NodoPadre.Entradas[IndiceSubArbol-1]);
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
                        NodoPadre.Entradas.RemoveAt(IndiceSubArbol-1);
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

        /// <summary>
        /// Metodo para elminar la llave de un nodo, es decir, un nodo raiz
        /// </summary>
        /// <param name="nodo">nodo </param>
        /// <param name="LlaveEliminar">llave del nodo</param>
        /// <param name="indiceLlaveNodo">ubicacion</param>
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

        /// <summary>
        /// Metodo para eliminar el predecesor de un nodo
        /// </summary>
        /// <param name="nodo">nodo a eliminar</param>
        /// <returns>valor contenido</returns>
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

        /// <summary>
        /// Metodo para eliminar el sucesor de un nodo
        /// </summary>
        /// <param name="nodo">nodo a eliminar</param>
        /// <returns>valor contenido</returns>
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

        /// <summary>
        /// Metodo que permite dividir un nodo, en 2 nodos y obtener los rangos permitidos segun el grado
        /// </summary>
        /// <param name="padreNodo">raiz o padre</param>
        /// <param name="nodoCorrer">Nodo que se va dividir</param>
        /// <param name="nodoMover">Nodo a mover y cambiar de ubicacion</param>
        private void DividirHijo(BNodo<TKey, T> padreNodo, int nodoCorrer, BNodo<TKey, T> nodoMover)
        {

            var nuevoNodo = new BNodo<TKey, T>(this.Grado);
            if (Grado % 2 == 0)
            {
                padreNodo.Entradas.Insert(nodoCorrer, nodoMover.Entradas[(this.Grado/2)-1]);
            }
            else
            {
                padreNodo.Entradas.Insert(nodoCorrer, nodoMover.Entradas[(this.Grado / 2)]);
            }
            
            if (Grado % 2 == 0)
            {
                nuevoNodo.Entradas.AddRange(nodoMover.Entradas.GetRange((this.Grado / 2), (this.Grado / 2)));
                nodoMover.Entradas.RemoveRange((this.Grado / 2) - 1, (this.Grado / 2) + 1);
            }
            else
            {
                nuevoNodo.Entradas.AddRange(nodoMover.Entradas.GetRange((this.Grado / 2) + 1, this.Grado/2));
                nodoMover.Entradas.RemoveRange((this.Grado / 2), (this.Grado/2) + 1);
            }

            

            if (!nodoMover.EsHoja)
            {
                if (Grado % 2 == 0)
                {
                    nuevoNodo.Hijos.AddRange(nodoMover.Hijos.GetRange((this.Grado / 2), (this.Grado / 2)+1));
                    nodoMover.Hijos.RemoveRange((this.Grado / 2), (this.Grado / 2) + 1);
                }
                else
                {
                    nuevoNodo.Hijos.AddRange(nodoMover.Hijos.GetRange((this.Grado / 2)+1, (this.Grado / 2)+1));
                    nodoMover.Hijos.RemoveRange((this.Grado / 2)+1, (this.Grado / 2)+1);
                }
                
            }
            padreNodo.Hijos.Insert(nodoCorrer + 1, nuevoNodo);
        }

    }
}
