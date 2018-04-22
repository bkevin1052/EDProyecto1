using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EDProyecto1.Models;
using EDProyecto1.DBContext;
using System.IO;
using LibreriaDeClases.Clases;

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
            UsuarioIngresado.WatchList.Add(temp);
            return View(UsuarioIngresado.WatchList);
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
                    ListaUsuario(rutaWatchlistUsuario, false);
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
                UsuarioIngresado = usuarioRegistrado;
                return RedirectToAction("InterfazUsuario");
            }
            catch
            {
                return View();
            }
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
        private void ListaUsuario(string rutaArchivo, bool sobrescribir)
        {
            StreamWriter GradoWriter = new StreamWriter(rutaArchivo, sobrescribir);
            GradoWriter.WriteLine("(Vacío)");
            GradoWriter.Close();
        }
    }
}
