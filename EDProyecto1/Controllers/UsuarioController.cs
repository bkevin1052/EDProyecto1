using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EDProyecto1.Models;
using EDProyecto1.DBContext;

namespace EDProyecto1.Controllers
{
    public class UsuarioController : Controller
    {
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
            return View();
        }

        public ActionResult WatchList()
        {
            return View();
        }

        // GET: Usuario/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // POST: Usuario/Create
        [HttpPost]
        public ActionResult RegistroUsuario(Usuario nuevoUsuario)
        {
            try
            {
                DefaultConnection.BArbolUsuarios.Insertar(nuevoUsuario.Username, nuevoUsuario);
                DefaultConnection.usuarios.Add(nuevoUsuario);
                return View();
            }  catch
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
                    return View();
                }
                return View("InterfazUsuario");
            }
            catch
            {
                return View();
            }
        }

        // GET: Usuario/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Usuario/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Usuario/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Usuario/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
