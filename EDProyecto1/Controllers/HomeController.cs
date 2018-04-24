using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LibreriaDeClases.Clases;
using System.Web.Mvc;
using EDProyecto1.DBContext;
using EDProyecto1.Models;
using System.IO;

namespace EDProyecto1.Controllers
{
    public class HomeController : Controller
    {
        string rutaUsuarios = @"C:\Users\" + Environment.UserName + @"\users.tree";
        string rutaArbolShowporNombre = @"C:\Users\" + Environment.UserName + @"\name.showtree";
        string rutaArbolShowporGenero = @"C:\Users\" + Environment.UserName + @"\genre.showtree";
        string rutaArbolShowporAnio = @"C:\Users\" + Environment.UserName + @"\year.showtree";
        string rutaArbolMovieporNombre = @"C:\Users\" + Environment.UserName + @"\name.movietree";
        string rutaArbolMovieporGenero = @"C:\Users\" + Environment.UserName + @"\genre.movietree";
        string rutaArbolMovieporAnio = @"C:\Users\" + Environment.UserName + @"\year.movietree";
        string rutaArbolDocumentaryporNombre = @"C:\Users\" + Environment.UserName + @"\name.documentarytree";
        string rutaArbolDocumentaryporGenero = @"C:\Users\" + Environment.UserName + @"\genre.documentarytree";
        string rutaArbolDocumentaryporAnio = @"C:\Users\" + Environment.UserName + @"\year.documentarytree";
        DefaultConnection db = DefaultConnection.getInstance;

        public ActionResult Index()
        {
            if(!System.IO.File.Exists(rutaUsuarios))
            {
                return View("IngresoGrado");
            }
            else
            {
                if (DefaultConnection.BArbolMoviePorNombre == null)
                {
                    LeerGrado();
                    LeerArchivo();

                }
                return View();
            }
            
        }

        private void LeerArchivo()
        {
            string[] lines = System.IO.File.ReadAllLines(rutaUsuarios);
            int raiz = Convert.ToInt16(lines[1].Remove(0, 6));
            LeerArbolUsuarios(lines, (raiz + 2), DefaultConnection.BArbolUsuarios.Grado, null, DefaultConnection.BArbolUsuarios);
            lines = System.IO.File.ReadAllLines(rutaArbolDocumentaryporAnio);
            raiz = Convert.ToInt16(lines[1].Remove(0, 6));
            LeerArbolAudiovisual(lines, (raiz + 2), DefaultConnection.BArbolDocumentaryPorAnio.Grado, null, DefaultConnection.BArbolDocumentaryPorAnio, 2);
            lines = System.IO.File.ReadAllLines(rutaArbolDocumentaryporGenero);
            raiz = Convert.ToInt16(lines[1].Remove(0, 6));
            LeerArbolAudiovisual(lines, (raiz + 2), DefaultConnection.BArbolDocumentaryPorGenero.Grado, null, DefaultConnection.BArbolDocumentaryPorGenero, 1);
            lines = System.IO.File.ReadAllLines(rutaArbolDocumentaryporNombre);
            raiz = Convert.ToInt16(lines[1].Remove(0, 6));
            LeerArbolAudiovisual(lines, (raiz + 2), DefaultConnection.BArbolDocumentaryPorNombre.Grado, null, DefaultConnection.BArbolDocumentaryPorNombre, 0);
            lines = System.IO.File.ReadAllLines(rutaArbolMovieporAnio);
            raiz = Convert.ToInt16(lines[1].Remove(0, 6));
            LeerArbolAudiovisual(lines, (raiz + 2), DefaultConnection.BArbolMoviePorAnio.Grado, null, DefaultConnection.BArbolMoviePorAnio, 2);
            lines = System.IO.File.ReadAllLines(rutaArbolMovieporGenero);
            raiz = Convert.ToInt16(lines[1].Remove(0, 6));
            LeerArbolAudiovisual(lines, (raiz + 2), DefaultConnection.BArbolMoviePorGenero.Grado, null, DefaultConnection.BArbolMoviePorGenero, 1);
            lines = System.IO.File.ReadAllLines(rutaArbolMovieporNombre);
            raiz = Convert.ToInt16(lines[1].Remove(0, 6));
            LeerArbolAudiovisual(lines, (raiz + 2), DefaultConnection.BArbolMoviePorNombre.Grado, null, DefaultConnection.BArbolMoviePorNombre, 0);
            lines = System.IO.File.ReadAllLines(rutaArbolShowporAnio);
            raiz = Convert.ToInt16(lines[1].Remove(0, 6));
            LeerArbolAudiovisual(lines, (raiz + 2), DefaultConnection.BArbolShowPorAnio.Grado, null, DefaultConnection.BArbolShowPorAnio, 2);
            lines = System.IO.File.ReadAllLines(rutaArbolShowporGenero);
            raiz = Convert.ToInt16(lines[1].Remove(0, 6));
            LeerArbolAudiovisual(lines, (raiz + 2), DefaultConnection.BArbolShowPorGenero.Grado, null, DefaultConnection.BArbolShowPorGenero, 1);
            lines = System.IO.File.ReadAllLines(rutaArbolShowporNombre);
            raiz = Convert.ToInt16(lines[1].Remove(0, 6));
            LeerArbolAudiovisual(lines, (raiz + 2), DefaultConnection.BArbolShowPorNombre.Grado, null, DefaultConnection.BArbolShowPorNombre, 0);
        }

        private void LeerArbolUsuarios(string[] lines, int nlinea, int grado, BNodo<string, Usuario> nodoPadre, BArbol<string, Usuario> arbol)
        {
            string[] linea = lines[nlinea].Split('|');
            BNodo<string, Usuario> Nodo = new BNodo<string, Usuario>(grado);
            for (int i = (2 + grado); i < (linea.Count()); i++)
            {
                if (!(linea[i] == String.Format("{0, -87}", "")))
                {
                    linea[i] = linea[i].Replace("  ", "");
                    linea[i] = linea[i].Replace(" ,", ",");
                    linea[i] = linea[i].Trim();
                    string[] lineaUsuario = linea[i].Split(',');
                    Usuario nuevoUsuario = new Usuario();
                    nuevoUsuario.Nombre = lineaUsuario[0];
                    nuevoUsuario.Apellido = lineaUsuario[1];
                    nuevoUsuario.Edad = Convert.ToInt16(lineaUsuario[2]);
                    nuevoUsuario.Username = lineaUsuario[3];
                    nuevoUsuario.Password = lineaUsuario[4];
                    nuevoUsuario.IDUsuario = db.IDActual++;
                    Entry<string, Usuario> entry = new Entry<string, Usuario>();
                    entry.LLave = nuevoUsuario.Username;
                    entry.Apuntador = nuevoUsuario;
                    Nodo.Entradas.Add(entry);
                    DefaultConnection.usuarios.Add(nuevoUsuario);
                }
            }
            for (int i = 2; i < (2+grado) ; i++)
            {
                if (!(linea[i] == String.Format("{0, -3}", "")))
                {
                    LeerArbolUsuarios(lines, (Convert.ToInt16(linea[i])+2), grado, Nodo, arbol);
                }
            }
            if (nodoPadre == null)
            {
                arbol.Raiz = Nodo;
            }
            else
            {
                nodoPadre.Hijos.Add(Nodo);
            }
        }
        private void LeerArbolAudiovisual(string[] lines, int nlinea, int grado, BNodo<string, Audiovisual> nodoPadre, BArbol<string, Audiovisual> arbol, int llave)
        {
            string[] linea = lines[nlinea].Split('|');
            BNodo<string, Audiovisual> Nodo = new BNodo<string, Audiovisual>(grado);
            for (int i = (2 + grado); i < (linea.Count()); i++)
            {
                if (!(linea[i] == String.Format("{0, -66}", "")))
                {
                    linea[i] = linea[i].Replace("  ", "");
                    linea[i] = linea[i].Replace(" ,", ",");
                    linea[i] = linea[i].Trim();
                    string[] lineaAudiovisual = linea[i].Split(',');
                    Audiovisual nuevoAudiovisual = new Audiovisual();
                    nuevoAudiovisual.Tipo = lineaAudiovisual[0];
                    nuevoAudiovisual.Nombre = lineaAudiovisual[1];
                    nuevoAudiovisual.Anio = Convert.ToInt16(lineaAudiovisual[2]);
                    nuevoAudiovisual.Genero = lineaAudiovisual[3];
                    nuevoAudiovisual.AudioVisualID = db.IDActual++;
                    Entry<string, Audiovisual> entry = new Entry<string,  Audiovisual>();
                    if (llave == 0)
                    {
                        entry.LLave = nuevoAudiovisual.Nombre;
                    }
                    else if(llave == 1)
                    {
                        entry.LLave = nuevoAudiovisual.Genero.PadRight(20) + "_" + nuevoAudiovisual.Nombre;
                    }
                    else
                    {
                        entry.LLave = nuevoAudiovisual.Anio.ToString().PadRight(4) + "_" + nuevoAudiovisual.Nombre;
                    }
                        entry.Apuntador = nuevoAudiovisual;
                        Nodo.Entradas.Add(entry);
                    
                    
                }
            }
            for (int i = 2; i < (2 + grado); i++)
            {
                if (!(linea[i] == String.Format("{0, -3}", "")))
                {
                    LeerArbolAudiovisual(lines, (Convert.ToInt16(linea[i]) + 2), grado, Nodo, arbol, llave);
                }
            }
            if (nodoPadre == null)
            {
                arbol.Raiz = Nodo;
            }
            else
            {
                nodoPadre.Hijos.Add(Nodo);
            }
        }
        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult IngresoGrado()
        {
            return View();
        }
        [HttpPost]
        public ActionResult IngresoGrado(string Grado)
        {
            int grado;
            if(int.TryParse(Grado, out grado))
            {
                grado = int.Parse(Grado);
                if (grado > 2)
                {
                    DefaultConnection.BArbolUsuarios = new BArbol<string, Usuario>(grado);
                    EscribirGrado(rutaUsuarios, false, grado);
                    DefaultConnection.BArbolShowPorNombre = new BArbol<string, Audiovisual>(grado);
                    EscribirGrado(rutaArbolShowporNombre, false, grado);
                    DefaultConnection.BArbolShowPorAnio = new BArbol<string, Audiovisual>(grado);
                    EscribirGrado(rutaArbolShowporAnio, false, grado);
                    DefaultConnection.BArbolShowPorGenero = new BArbol<string, Audiovisual>(grado);
                    EscribirGrado(rutaArbolShowporGenero, false, grado);
                    DefaultConnection.BArbolMoviePorNombre = new BArbol<string, Audiovisual>(grado);
                    EscribirGrado(rutaArbolMovieporNombre, false, grado);
                    DefaultConnection.BArbolMoviePorAnio = new BArbol<string, Audiovisual>(grado);
                    EscribirGrado(rutaArbolMovieporAnio, false, grado);
                    DefaultConnection.BArbolMoviePorGenero = new BArbol<string, Audiovisual>(grado);
                    EscribirGrado(rutaArbolMovieporGenero, false, grado);
                    DefaultConnection.BArbolDocumentaryPorNombre = new BArbol<string, Audiovisual>(grado);
                    EscribirGrado(rutaArbolDocumentaryporNombre, false, grado);
                    DefaultConnection.BArbolDocumentaryPorAnio = new BArbol<string, Audiovisual>(grado);
                    EscribirGrado(rutaArbolDocumentaryporAnio, false, grado);
                    DefaultConnection.BArbolDocumentaryPorGenero = new BArbol<string, Audiovisual>(grado);
                    EscribirGrado(rutaArbolDocumentaryporGenero, false, grado);
                    return View("Index");
                }
                else
                {
                    TempData["alertMessage"] = "Ingrese un grado válido. El grado debe ser mayor o igual a 2.";
                    return View();
                }
            }
            else
            {
                TempData["alertMessage"] = "Ingrese un grado válido. El grado debe ser mayor a 2.";
                return View();
            }
        }
        private void EscribirGrado(string rutaArchivo, bool sobrescribir, int grado)
        {
            StreamWriter GradoWriter = new StreamWriter(rutaArchivo, sobrescribir);
            GradoWriter.WriteLine("Grado: "+grado.ToString());
            GradoWriter.Close();
        }

        private void LeerGrado()
        {
            string linea;
            int grado;
            StreamReader lector = new StreamReader(rutaUsuarios);
            linea = lector.ReadLine();
            var array = linea.Split(':', ' ');
            grado = int.Parse(array[2]);
            DefaultConnection.BArbolUsuarios = new BArbol<string, Usuario>(grado);
            DefaultConnection.BArbolShowPorNombre = new BArbol<string, Audiovisual>(grado);
            DefaultConnection.BArbolShowPorAnio = new BArbol<string, Audiovisual>(grado);
            DefaultConnection.BArbolShowPorGenero = new BArbol<string, Audiovisual>(grado);
            DefaultConnection.BArbolMoviePorNombre = new BArbol<string, Audiovisual>(grado);
            DefaultConnection.BArbolMoviePorAnio = new BArbol<string, Audiovisual>(grado);
            DefaultConnection.BArbolMoviePorGenero = new BArbol<string, Audiovisual>(grado);
            DefaultConnection.BArbolDocumentaryPorNombre = new BArbol<string, Audiovisual>(grado);
            DefaultConnection.BArbolDocumentaryPorAnio = new BArbol<string, Audiovisual>(grado);
            DefaultConnection.BArbolDocumentaryPorGenero = new BArbol<string, Audiovisual>(grado);
            lector.Close();
        }

        public ActionResult BusquedaNombre()
        {
            return View();
        }

        [HttpPost]
        public ActionResult BusquedaNombre(string nombre)
        {
            try
            {
                var resultado = DefaultConnection.BArbolDocumentaryPorNombre.Search(nombre);
                List<Audiovisual> modelo = new List<Audiovisual>();
                if (resultado != null)
                {
                    modelo.Add(resultado.Apuntador);
                    return View(modelo);
                }
                else
                {
                    resultado = DefaultConnection.BArbolMoviePorNombre.Search(nombre);
                    if (resultado != null)
                    {
                        modelo.Add(resultado.Apuntador);
                        return View(modelo);
                    }
                    else
                    {
                        resultado = DefaultConnection.BArbolShowPorNombre.Search(nombre);
                        if (resultado != null)
                        {
                            modelo.Add(resultado.Apuntador);
                            return View(modelo);
                        }
                        else
                        {
                            TempData["alertMessage"] = "No se encontro el elemento buscado.";
                            return View();
                        }
                    }
                }
            }
            catch
            {
                TempData["alertMessage"] = "No hay nada en el catalogo";
                return View();
            }

        }

        public ActionResult BusquedaAnio()
        {
            return View();
        }

        [HttpPost]
        public ActionResult BusquedaAnio(string anio, string nombre)
        {
            try
            {
                int ianio;
                if (int.TryParse(anio, out ianio))
                {


                    string llave = anio.ToString().PadRight(4) + "_" + nombre;
                    var resultado = DefaultConnection.BArbolDocumentaryPorAnio.Search(llave);
                    List<Audiovisual> modelo = new List<Audiovisual>();
                    if (resultado != null)
                    {
                        modelo.Add(resultado.Apuntador);
                        return View(modelo);
                    }
                    else
                    {
                        resultado = DefaultConnection.BArbolMoviePorAnio.Search(llave);
                        if (resultado != null)
                        {
                            modelo.Add(resultado.Apuntador);
                            return View(modelo);
                        }
                        else
                        {
                            resultado = DefaultConnection.BArbolShowPorAnio.Search(llave);
                            if (resultado != null)
                            {
                                modelo.Add(resultado.Apuntador);
                                return View(modelo);
                            }
                            else
                            {
                                TempData["alertMessage"] = "No se encontro el elemento buscado.";
                                return View();
                            }
                        }
                    }
                }
                else
                {
                    TempData["alertMessage"] = "El anio no es valido.";
                    return View();
                }
                
            }
            catch
            {
                TempData["alertMessage"] = "No hay nada en el catalogo";
                return View();
            }

        }

        public ActionResult BusquedaGenero()
        {
            return View();
        }

        [HttpPost]
        public ActionResult BusquedaGenero(string genero, string nombre)
        {
            try
            {


                    string llave = genero.PadRight(20) + "_" + nombre;
                    var resultado = DefaultConnection.BArbolDocumentaryPorGenero.Search(llave);
                    List<Audiovisual> modelo = new List<Audiovisual>();
                    if (resultado != null)
                    {
                        modelo.Add(resultado.Apuntador);
                        return View(modelo);
                    }
                    else
                    {
                        resultado = DefaultConnection.BArbolMoviePorGenero.Search(llave);
                        if (resultado != null)
                        {
                            modelo.Add(resultado.Apuntador);
                            return View(modelo);
                        }
                        else
                        {
                            resultado = DefaultConnection.BArbolShowPorGenero.Search(llave);
                            if (resultado != null)
                            {
                                modelo.Add(resultado.Apuntador);
                                return View(modelo);
                            }
                            else
                            {
                                TempData["alertMessage"] = "No se encontro el elemento buscado.";
                                return View();
                            }
                        }
                    }

            }
            catch
            {
                TempData["alertMessage"] = "No hay nada en el catalogo";
                return View();
            }

        }

        public ActionResult Busquedas()
        {
            return View();
        }
    }
}