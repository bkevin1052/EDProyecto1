using EDProyecto1.DBContext;
using EDProyecto1.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LibreriaDeClases.Clases;

namespace EDProyecto1.Controllers
{
    public class AdministradorController : Controller
    {
        public static List<Audiovisual> modelusuario = new List<Audiovisual>();
        DefaultConnection db = DefaultConnection.getInstance;
        // GET: Administrador
        public ActionResult IniciarSesionAdmin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult IniciarSesionAdmin(string Nombre, string Password)
        {
            if (Nombre == "admin" && Password == "admin")
            {
                return RedirectToAction("InterfazAdmin");
            }
            else
            {
                TempData["alertMessage"] = "Usuario o contrasena incorrecta.";
                return View();
            }

        }

        // GET: 
        public ActionResult InterfazAdmin()
        {
            var model = new List<Audiovisual>();
            ConvertiraLista(ref model);
            modelusuario = model;
           return View(model);
        }

        public static void AgregaraLista(ref List<Audiovisual> model, BNodo<string, Audiovisual> nodo)
        {
            foreach (var item in nodo.Hijos)
            {
                AgregaraLista(ref model, item);
            }
            foreach (var item in nodo.Entradas)
            {
                model.Add(item.Apuntador);
            }
        }

        // GET: Administrador/Create
        public ActionResult AgregarAdmin()
        {
            return View();
        }

        // POST: Administrador/Create
        [HttpPost]
        public ActionResult AgregarAdmin([Bind(Include = "AudioVisualID,Tipo,Nombre,Anio,Genero")] Audiovisual audiovisual)
        {
            try
            {
                audiovisual.AudioVisualID = db.IDActual++;
                // TODO: Add insert logic here
                if (audiovisual.Tipo == "Serie")
                {
                    DefaultConnection.BArbolShowPorNombre.Insertar(audiovisual.Nombre, audiovisual);
                    string llave = audiovisual.Anio.ToString() + "_" + audiovisual.Nombre;
                    DefaultConnection.BArbolShowPorAnio.Insertar(llave, audiovisual);
                    llave = audiovisual.Genero.PadRight(20) + "_" + audiovisual.Nombre;
                    DefaultConnection.BArbolShowPorGenero.Insertar(llave, audiovisual);
                }
                else if (audiovisual.Tipo == "Película")
                {
                    DefaultConnection.BArbolMoviePorNombre.Insertar(audiovisual.Nombre, audiovisual);
                    string llave = audiovisual.Anio.ToString() + "_" + audiovisual.Nombre;
                    DefaultConnection.BArbolMoviePorAnio.Insertar(llave, audiovisual);
                    llave = audiovisual.Genero.PadRight(20) + "_" + audiovisual.Nombre;
                    DefaultConnection.BArbolMoviePorGenero.Insertar(llave, audiovisual);
                }
                else if (audiovisual.Tipo == "Documental")
                {
                    DefaultConnection.BArbolDocumentaryPorNombre.Insertar(audiovisual.Nombre, audiovisual);
                    string llave = audiovisual.Anio.ToString() + "_" + audiovisual.Nombre;
                    DefaultConnection.BArbolDocumentaryPorAnio.Insertar(llave, audiovisual);
                    llave = audiovisual.Genero.PadRight(20) + "_" + audiovisual.Nombre;
                    DefaultConnection.BArbolDocumentaryPorGenero.Insertar(llave, audiovisual);
                }
                else
                {
                    TempData["alertMessage"] = "No es un tipo valido. (Documental, Pelicula, Serie) ";
                    return RedirectToAction("AgregarAdmin");
                }
                return RedirectToAction("InterfazAdmin");
            }
            catch
            {
                return View();
            }
        }

        

        // GET: Administrador/Delete/5
        public ActionResult Delete(int id)
        {
            var model = new List<Audiovisual>();
            ConvertiraLista(ref model);

            return View(model.Where(x => x.AudioVisualID == id).FirstOrDefault());
        }

        // POST: Administrador/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                var model = new List<Audiovisual>();
                ConvertiraLista(ref model);
                Audiovisual temp = model.Where(x => x.AudioVisualID == id).FirstOrDefault();
                if (temp.Tipo == "Documental")
                {
                DefaultConnection.BArbolDocumentaryPorNombre.Eliminar(temp.Nombre);
                string llave = temp.Anio.ToString() + "_" + temp.Nombre;
                DefaultConnection.BArbolDocumentaryPorAnio.Eliminar(llave);
                llave = temp.Genero.PadRight(20) + "_" + temp.Nombre;
                DefaultConnection.BArbolDocumentaryPorGenero.Eliminar(llave);
                }
                else if (temp.Tipo == "Serie")
                {
                    DefaultConnection.BArbolShowPorNombre.Eliminar(temp.Nombre);
                    string llave = temp.Anio.ToString() + "_" + temp.Nombre;
                    DefaultConnection.BArbolShowPorAnio.Eliminar(llave);
                    llave = temp.Genero.PadRight(20) + "_" + temp.Nombre;
                    DefaultConnection.BArbolShowPorGenero.Eliminar(llave);
                }
                else
                {
                    DefaultConnection.BArbolMoviePorNombre.Eliminar(temp.Nombre);
                    string llave = temp.Anio.ToString() + "_" + temp.Nombre;
                    DefaultConnection.BArbolMoviePorAnio.Eliminar(llave);
                    llave = temp.Genero.PadRight(20) + "_" + temp.Nombre;
                    DefaultConnection.BArbolMoviePorGenero.Eliminar(llave);
                }
                return RedirectToAction("InterfazAdmin");
            } catch
            {
                return View();
            }
        }

        public ActionResult CargaArchivoJSON()
        {
            return View();
        }

        //Post SubirArchivoJSON
        [HttpPost]
        public ActionResult CargaArchivoJSON(HttpPostedFileBase file)
        {
            Archivo modelo = new Archivo();
            string filePath = string.Empty;
            try
            {
                if (file != null)
                {
                    string ruta = Server.MapPath("~/Temp/");

                    if (!Directory.Exists(ruta))
                    {
                        Directory.CreateDirectory(ruta);
                    }

                    filePath = ruta + Path.GetFileName(file.FileName);

                    string extension = Path.GetExtension(file.FileName);

                    file.SaveAs(filePath);

                    using (StreamReader r = new StreamReader(filePath))
                    {
                        string json = r.ReadToEnd();
                        dynamic array = JsonConvert.DeserializeObject(json);
                        foreach (var item in array)
                        {


                            dynamic itemtemp = JsonConvert.DeserializeObject(item.Value.ToString());

                            Audiovisual temp = new Audiovisual();
                            temp.Tipo = itemtemp.Tipo.Value;
                            temp.Nombre = itemtemp.Nombre.Value;
                            temp.Anio = Convert.ToInt16(itemtemp.Anio.Value);
                            temp.Genero = itemtemp.Genero.Value;
                            temp.AudioVisualID = ++db.IDActual;
                            if (temp.Tipo == "Serie")
                            {
                                DefaultConnection.BArbolShowPorNombre.Insertar(temp.Nombre, temp);
                                string llave = temp.Anio.ToString().PadRight(4) + "_" + temp.Nombre;
                                DefaultConnection.BArbolShowPorAnio.Insertar(llave, temp);
                                llave = temp.Genero.PadRight(20) + "_" + temp.Nombre;
                                DefaultConnection.BArbolShowPorGenero.Insertar(llave, temp);
                            }
                            else if (temp.Tipo == "Película")
                            {
                                DefaultConnection.BArbolMoviePorNombre.Insertar(temp.Nombre, temp);
                                string llave = temp.Anio.ToString().PadRight(4) + "_" + temp.Nombre;
                                DefaultConnection.BArbolMoviePorAnio.Insertar(llave, temp);
                                llave = temp.Genero.PadRight(20) + "_" + temp.Nombre;
                                DefaultConnection.BArbolMoviePorGenero.Insertar(llave, temp);
                            }
                            else if (temp.Tipo == "Documental")
                            {
                                DefaultConnection.BArbolDocumentaryPorNombre.Insertar(temp.Nombre, temp);
                                string llave = temp.Anio.ToString().PadRight(4) + "_" + temp.Nombre;
                                DefaultConnection.BArbolDocumentaryPorAnio.Insertar(llave, temp);
                                llave = temp.Genero.PadRight(20) + "_" + temp.Nombre;
                                DefaultConnection.BArbolDocumentaryPorGenero.Insertar(llave, temp);
                            }

                        }

                    }


                    modelo.SubirArchivo(ruta, file);

                }
                ViewBag.Error = modelo.error;
                ViewBag.Correcto = modelo.Confirmacion;
                return RedirectToAction("InterfazAdmin");
            }
            catch
            {
                TempData["alertMessage"] = "El archivo no es válido ";
                return RedirectToAction("CargaArchivoJSON");
            }
        }

        public static void ConvertiraLista(ref List<Audiovisual> model)
        {
            AgregaraLista(ref model, DefaultConnection.BArbolDocumentaryPorNombre.Raiz);
            AgregaraLista(ref model, DefaultConnection.BArbolMoviePorNombre.Raiz);
            AgregaraLista(ref model, DefaultConnection.BArbolShowPorNombre.Raiz);
        }
        public ActionResult CargaArchivoJSONUsuarios()
        {
            return View();
        }

        //Post SubirArchivoJSON
        [HttpPost]
        public ActionResult CargaArchivoJSONUsuarios(HttpPostedFileBase file)
        {
            try
            {
                Archivo modelo = new Archivo();
                string filePath = string.Empty;
                if (file != null)
                {
                    string ruta = Server.MapPath("~/Temp/");

                    if (!Directory.Exists(ruta))
                    {
                        Directory.CreateDirectory(ruta);
                    }

                    filePath = ruta + Path.GetFileName(file.FileName);

                    string extension = Path.GetExtension(file.FileName);

                    file.SaveAs(filePath);

                    using (StreamReader r = new StreamReader(filePath))
                    {
                        string json = r.ReadToEnd();
                        dynamic array = JsonConvert.DeserializeObject(json);
                        foreach (var item in array)
                        {


                            dynamic itemtemp = JsonConvert.DeserializeObject(item.Value.ToString());

                            Usuario temp = new Usuario();
                            temp.Nombre = itemtemp.Nombre.Value;
                            temp.Apellido = itemtemp.Apellido.Value;
                            temp.Edad = Convert.ToInt16(itemtemp.Edad.Value);
                            temp.Username = itemtemp.Username.Value;
                            temp.Password = itemtemp.Password.Value;
                            Usuario usuarioRegistrado = DefaultConnection.usuarios.Find(x => (x.Username == temp.Username));
                            if (usuarioRegistrado == null)
                            {
                                temp.IDUsuario = ++db.IDActual;

                                DefaultConnection.usuarios.Add(temp);
                                DefaultConnection.BArbolUsuarios.Insertar(temp.Username, temp);
                            }
                            else
                            {
                                TempData["alertMessage"] = "Username no disponible.";
                            }


                        }

                    }


                    modelo.SubirArchivo(ruta, file);

                }
                ViewBag.Error = modelo.error;
                ViewBag.Correcto = modelo.Confirmacion;
                return RedirectToAction("InterfazAdmin");
            }
            catch
            {
                TempData["alertMessage"] = "El archivo no es válido ";
                return RedirectToAction("CargaArchivoJSONUsuarios");
            }
        }

        public ActionResult CerrarSesion()
        {
            string rutaJSONUsuarios = @"C:\Users\" + Environment.UserName + @"\users.json";
            StreamWriter writer = new StreamWriter(rutaJSONUsuarios);
            writer.WriteLine("{");
            int contador;
            contador = 1;
            foreach (var item in DefaultConnection.usuarios)
            {
                if (DefaultConnection.usuarios.Last() != item)
                {
                    writer.WriteLine("\"nodo" + contador.ToString() + "\":" + JsonConvert.SerializeObject(item) + ",");
                }
                else
                {
                    writer.WriteLine("\"nodo" + contador.ToString() + "\":" + JsonConvert.SerializeObject(item));
                }
                contador++;

            }
            writer.WriteLine("}");
            writer.Close();
            return RedirectToAction("IniciarSesionAdmin");
        }
    }
}
