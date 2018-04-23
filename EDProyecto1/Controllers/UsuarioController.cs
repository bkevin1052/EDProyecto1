using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EDProyecto1.Models;
using EDProyecto1.DBContext;
using System.IO;
using LibreriaDeClases.Clases;
using Newtonsoft.Json;

namespace EDProyecto1.Controllers
{
    public class UsuarioController : Controller
    {
        public static Usuario UsuarioIngresado;
        DefaultConnection db = DefaultConnection.getInstance;
        // GET: Usuario
        public ActionResult InicioSesionUsuario()
        {
            return View();
        }
        public ActionResult RegistroUsuario()
        {
            return View();
        }

        public ActionResult InterfazUsuario()
        {
            return View(AdministradorController.modelusuario);
        }

        public ActionResult WatchList(int id)
        {
            Audiovisual temp = AdministradorController.modelusuario.Where(x => x.AudioVisualID == id).FirstOrDefault();
            AgregarWatchlist(DefaultConnection.BArbolUsuarios.Raiz, UsuarioIngresado, temp);
            return View(DefaultConnection.BArbolUsuarios.Search(UsuarioIngresado.Username).Apuntador.WatchList);
        }

        public ActionResult MostrarWatchList()
        {
            return View(DefaultConnection.BArbolUsuarios.Search(UsuarioIngresado.Username).Apuntador.WatchList);
        }
        // POST: Usuario/Create
        [HttpPost]
        public ActionResult RegistroUsuario(Usuario nuevoUsuario)
        {
            try
            {
                Usuario usuarioRegistrado = DefaultConnection.usuarios.Find(x => (x.Username == nuevoUsuario.Username));
                if (usuarioRegistrado == null)
                {
                    nuevoUsuario.IDUsuario = ++db.IDActual;
                    DefaultConnection.BArbolUsuarios.Insertar(nuevoUsuario.Username, nuevoUsuario);
                    DefaultConnection.usuarios.Add(nuevoUsuario);

                    string rutaWatchlistUsuario = @"C:\Users\" + Environment.UserName + @"\" + nuevoUsuario.Username + @".watchlist";
                    IniciarListaUsuario(rutaWatchlistUsuario, false);
                    return RedirectToAction("InicioSesionUsuario");
                }
                else
                {
                    TempData["alertMessage"] = "Username no disponible.";
                    return RedirectToAction("RegistroUsuario");
                }
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult InicioSesionUsuario(string Username, string Password)
        {
            try
            {
                Usuario usuarioRegistrado = DefaultConnection.usuarios.Find(x => (x.Username == Username) && (x.Password == Password));

                if(usuarioRegistrado == null)
                {
                    TempData["alertMessage"] = "Username o contrasena incorrecta.";
                    return View();
                }
                if(DefaultConnection.BArbolUsuarios.Search(Username).Apuntador.WatchList.Count == 0)
                {
                    LeerArchivo(usuarioRegistrado);
                }
                UsuarioIngresado = usuarioRegistrado;
                return RedirectToAction("InterfazUsuario");
            }
            catch
            {
                return View();
            }
        }

        private void LeerArchivo(Usuario usuario)
        {
            string filePath = @"C:\Users\" + Environment.UserName + @"\" + usuario.Username + @".watchlist";
            try
            {

                using (StreamReader r = new StreamReader(filePath))
                {
                    string linea = r.ReadLine();
                    if(linea != "(Vacío)")
                    {
                        while(linea != null)
                        {
                            linea = linea.Replace("  ","");
                            linea = linea.Replace(" ,", ",");
                            var array = linea.Split(',');
                            Audiovisual temp = new Audiovisual();
                            temp.Tipo = array[0];
                            temp.Nombre = array[1];
                            temp.Anio = int.Parse(array[2]);
                            temp.Genero = array[3];
                            if(temp.Tipo == "Documental")
                            {
                                var audiovisualCatalogo = DefaultConnection.BArbolDocumentaryPorNombre.Search(temp.Nombre);
                                if(audiovisualCatalogo != null)
                                {
                                    temp.AudioVisualID = audiovisualCatalogo.Apuntador.AudioVisualID;
                                    AgregarWatchlistArchivo(DefaultConnection.BArbolUsuarios.Raiz, usuario, temp);
                                }
                            }
                            else if (temp.Tipo == "Serie")
                            {
                                var audiovisualCatalogo = DefaultConnection.BArbolShowPorNombre.Search(temp.Nombre);
                                if (audiovisualCatalogo != null)
                                {
                                    temp.AudioVisualID = audiovisualCatalogo.Apuntador.AudioVisualID;
                                    AgregarWatchlistArchivo(DefaultConnection.BArbolUsuarios.Raiz, usuario, temp);
                                }
                            }
                            else if (temp.Tipo == "Película")
                            {
                                var audiovisualCatalogo = DefaultConnection.BArbolMoviePorNombre.Search(temp.Nombre);
                                if (audiovisualCatalogo != null)
                                {
                                    temp.AudioVisualID = audiovisualCatalogo.Apuntador.AudioVisualID;
                                    AgregarWatchlistArchivo(DefaultConnection.BArbolUsuarios.Raiz, usuario, temp);
                                }
                            }
                            linea = r.ReadLine();
                        }
                    }
                }
            }
            catch
            {
                TempData["alertMessage"] = "El archivo no es válido ";
            }
        }

        private string AgregarWatchlistArchivo(BNodo<string, Usuario> nodo, Usuario usuarioIngresado, Audiovisual audiovisual)
        {
            int contador;
            contador = 0;
            foreach (var item in nodo.Hijos)
            {
                AgregarWatchlistArchivo(nodo.Hijos[contador], usuarioIngresado, audiovisual);
                contador++;
            }
            foreach (var item in nodo.Entradas)
            {
                if (item.Apuntador == usuarioIngresado)
                {
                    Audiovisual yaAgregado = item.Apuntador.WatchList.Where(x => x.Nombre == audiovisual.Nombre).FirstOrDefault();
                    if (yaAgregado == null)
                    {
                        item.Apuntador.WatchList.Add(audiovisual);
                    }
                    else
                    {
                        return "Este elemento ya se encuentra en su watchlist";
                    }
                }
            }
            return "El elemento fue agregado exitosamente.";
        }
        private void EscribirUsuariosDisco(string rutaArchivo)
        {
            StreamWriter GradoWriter = new StreamWriter(rutaArchivo, false);
            GradoWriter.WriteLine("Grado: " + DefaultConnection.BArbolUsuarios.Grado.ToString());
            GradoWriter.WriteLine("Raíz: " + 1.ToString());
            GradoWriter.Close();
        }

        private void EscribirArbolUsuario(BNodo<string, Usuario> nodo, int contador, int contadorPadre)
        {
            foreach(var item in nodo.Entradas)
            {
                string linea;
                linea = $"{contador.ToString("000;-000")}|{contadorPadre.ToString("000;-000")}";
            }
        }
        private void IniciarListaUsuario(string rutaArchivo, bool sobrescribir)
        {
            StreamWriter GradoWriter = new StreamWriter(rutaArchivo, sobrescribir);
            GradoWriter.WriteLine("(Vacío)");
            GradoWriter.Close();
        }

        private string AgregarWatchlist(BNodo<string, Usuario> nodo, Usuario usuarioIngresado, Audiovisual audiovisual)
        {
            int contador;
            contador = 0;
            foreach (var item in nodo.Hijos)
            {
                AgregarWatchlist(nodo.Hijos[contador], usuarioIngresado, audiovisual);
                contador++;
            }
            foreach (var item in nodo.Entradas)
            {
                if(item.Apuntador == usuarioIngresado)
                {
                    Audiovisual yaAgregado = item.Apuntador.WatchList.Where(x => x.Nombre == audiovisual.Nombre).FirstOrDefault();
                    if (yaAgregado == null)
                    {
                        item.Apuntador.WatchList.Add(audiovisual);
                        string rutaWatchlistUsuario = @"C:\Users\" + Environment.UserName + @"\" +item.Apuntador.Username + @".watchlist";
                        string audiovisualobject = item.Apuntador.WatchList.Last().ToFixedSizeString();
                        if (item.Apuntador.WatchList.Count() == 1)
                        {
                            AgregarListaUsuario(rutaWatchlistUsuario, false, audiovisualobject);
                        }
                        else
                        {
                            AgregarListaUsuario(rutaWatchlistUsuario, true, audiovisualobject);
                        }
                    }
                    else
                    {
                        return "Este elemento ya se encuentra en su watchlist";
                    }
                }
            }
            return "El elemento fue agregado exitosamente.";
        }
        private void AgregarListaUsuario(string rutaArchivo, bool sobrescribir,string audiovisual)
        {
            StreamWriter GradoWriter = new StreamWriter(rutaArchivo, sobrescribir);
            GradoWriter.WriteLine(audiovisual);
            GradoWriter.Close();
        }

        // GET: Administrador/Delete/5
        /// <summary>
        /// Busca el objeto que se desea eliminar.
        /// </summary>
        /// <param name="id">Id del objeto que se desea eliminar</param>
        /// <returns>Vista</returns>
        public ActionResult Delete(int id)
        {
            
            return View(AdministradorController.modelusuario.Where(x => x.AudioVisualID == id).FirstOrDefault());
        }

        // POST: Administrador/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                Audiovisual temp = AdministradorController.modelusuario.Where(x => x.AudioVisualID == id).FirstOrDefault();
                EliminarWatchlist(DefaultConnection.BArbolUsuarios.Raiz, UsuarioIngresado, temp);
                return RedirectToAction("MostrarWatchList");
            }
            catch
            {
                return View();
            }
        }
        private void EliminarWatchlist(BNodo<string, Usuario> nodo, Usuario usuarioIngresado, Audiovisual audiovisual)
        {
            int contador;
            contador = 0;
            foreach (var item in nodo.Hijos)
            {
                EliminarWatchlist(nodo.Hijos[contador], usuarioIngresado, audiovisual);
                contador++;
            }
            foreach (var item in nodo.Entradas)
            {
                if (item.Apuntador == usuarioIngresado)
                {
                        item.Apuntador.WatchList.Remove(audiovisual);
                        string rutaWatchlistUsuario = @"C:\Users\" + Environment.UserName + @"\" + item.Apuntador.Username + @".watchlist";
                        EliminarListaUsuario(rutaWatchlistUsuario, usuarioIngresado);
                }
            }
        }

        private void EliminarListaUsuario(string rutaArchivo, Usuario usuario)
        {
            StreamWriter GradoWriter = new StreamWriter(rutaArchivo);
            foreach(var item in usuario.WatchList)
            {
                GradoWriter.WriteLine(item.ToFixedSizeString());
            }
            GradoWriter.Close();
        }
    }
}
