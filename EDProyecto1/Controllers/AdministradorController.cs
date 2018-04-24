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
        /// <summary>
        /// Inicio de Sesión de Administrador
        /// </summary>
        /// <returns>Vista</returns>
        public ActionResult IniciarSesionAdmin()
        {
            return View();
        }

        /// <summary>
        /// Verifica que sea el usuario y contraseña correcta.
        /// </summary>
        /// <param name="Nombre">Usuario de administrador</param>
        /// <param name="Password">Contraseña de administrados</param>
        /// <returns>Interfaz de Administrador o mensaje de error</returns>
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
        /// <summary>
        /// Crea el modelo de la interfaz de administrador.
        /// </summary>
        /// <returns>Vista</returns>
        public ActionResult InterfazAdmin()
        {
            var model = new List<Audiovisual>();
            ConvertiraLista(ref model);
            return View(model);
        }

        /// <summary>
        /// Recorrer el árbol y lo agrega a la lista deseada.
        /// </summary>
        /// <param name="model">Lista de modelo</param>
        /// <param name="nodo">Nodo</param>
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
        /// <summary>
        /// Agrega un nuevo film a los árboles.
        /// </summary>
        /// <returns>Vista</returns>
        public ActionResult AgregarAdmin()
        {
            return View();
        }

        // POST: Administrador/Create
        /// <summary>
        /// Agrega un nuevo film a los árboles.
        /// </summary>
        /// <param name="audiovisual">Los datos ingresados convertidos a objeto</param>
        /// <returns>Interfaz de Admin</returns>
        [HttpPost]
        public ActionResult AgregarAdmin([Bind(Include = "AudioVisualID,Tipo,Nombre,Anio,Genero")] Audiovisual audiovisual)
        {
            try
            {
                audiovisual.AudioVisualID = db.IDActual++;
                // TODO: Add insert logic here
                if (audiovisual.Tipo == "Serie")
                {
                    if (DefaultConnection.BArbolShowPorNombre.Search(audiovisual.Nombre) == null)
                    {
                        DefaultConnection.BArbolShowPorNombre.Insertar(audiovisual.Nombre, audiovisual);
                        string llave = audiovisual.Anio.ToString() + "_" + audiovisual.Nombre;
                        DefaultConnection.BArbolShowPorAnio.Insertar(llave, audiovisual);
                        llave = audiovisual.Genero.PadRight(20) + "_" + audiovisual.Nombre;
                        DefaultConnection.BArbolShowPorGenero.Insertar(llave, audiovisual);
                    }
                }
                else if (audiovisual.Tipo == "Película")
                {
                    if (DefaultConnection.BArbolMoviePorNombre.Search(audiovisual.Nombre) == null)
                    {
                        DefaultConnection.BArbolMoviePorNombre.Insertar(audiovisual.Nombre, audiovisual);
                        string llave = audiovisual.Anio.ToString() + "_" + audiovisual.Nombre;
                        DefaultConnection.BArbolMoviePorAnio.Insertar(llave, audiovisual);
                        llave = audiovisual.Genero.PadRight(20) + "_" + audiovisual.Nombre;
                        DefaultConnection.BArbolMoviePorGenero.Insertar(llave, audiovisual);
                    }
                }
                else if (audiovisual.Tipo == "Documental")
                {
                    if (DefaultConnection.BArbolDocumentaryPorNombre.Search(audiovisual.Nombre) == null)
                    {
                        DefaultConnection.BArbolDocumentaryPorNombre.Insertar(audiovisual.Nombre, audiovisual);
                        string llave = audiovisual.Anio.ToString() + "_" + audiovisual.Nombre;
                        DefaultConnection.BArbolDocumentaryPorAnio.Insertar(llave, audiovisual);
                        llave = audiovisual.Genero.PadRight(20) + "_" + audiovisual.Nombre;
                        DefaultConnection.BArbolDocumentaryPorGenero.Insertar(llave, audiovisual);
                    }
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
        /// <summary>
        /// Busca el objeto que se desea eliminar.
        /// </summary>
        /// <param name="id">Id del objeto que se desea eliminar</param>
        /// <returns>Vista</returns>
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
                                if (DefaultConnection.BArbolShowPorNombre.Search(temp.Nombre) == null)
                                {
                                    DefaultConnection.BArbolShowPorNombre.Insertar(temp.Nombre, temp);
                                    string llave = temp.Anio.ToString().PadRight(4) + "_" + temp.Nombre;
                                    DefaultConnection.BArbolShowPorAnio.Insertar(llave, temp);
                                    llave = temp.Genero.PadRight(20) + "_" + temp.Nombre;
                                    DefaultConnection.BArbolShowPorGenero.Insertar(llave, temp);
                                }
                            }
                            else if (temp.Tipo == "Película")
                            {
                                if (DefaultConnection.BArbolMoviePorNombre.Search(temp.Nombre) == null)
                                {
                                    DefaultConnection.BArbolMoviePorNombre.Insertar(temp.Nombre, temp);
                                    string llave = temp.Anio.ToString().PadRight(4) + "_" + temp.Nombre;
                                    DefaultConnection.BArbolMoviePorAnio.Insertar(llave, temp);
                                    llave = temp.Genero.PadRight(20) + "_" + temp.Nombre;
                                    DefaultConnection.BArbolMoviePorGenero.Insertar(llave, temp);
                                }
                            }
                            else if (temp.Tipo == "Documental")
                            {
                                if (DefaultConnection.BArbolDocumentaryPorNombre.Search(temp.Nombre) == null)
                                {
                                    DefaultConnection.BArbolDocumentaryPorNombre.Insertar(temp.Nombre, temp);
                                    string llave = temp.Anio.ToString().PadRight(4) + "_" + temp.Nombre;
                                    DefaultConnection.BArbolDocumentaryPorAnio.Insertar(llave, temp);
                                    llave = temp.Genero.PadRight(20) + "_" + temp.Nombre;
                                    DefaultConnection.BArbolDocumentaryPorGenero.Insertar(llave, temp);
                                }
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
                                string rutaWatchlistUsuario = @"C:\Users\" + Environment.UserName + @"\" + temp.Username + @".watchlist";
                                if (!System.IO.File.Exists(rutaWatchlistUsuario))
                                {
                                    IniciarListaUsuario(rutaWatchlistUsuario, false);
                                }
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
            string rutaArbolUsuarios = @"C:\Users\" + Environment.UserName + @"\users.tree";
            writer = new StreamWriter(rutaArbolUsuarios, false);
            writer.WriteLine("Grado: " + DefaultConnection.BArbolUsuarios.Grado.ToString());
            writer.WriteLine("Raíz: 1");
            writer.WriteLine("Próxima posición: " + (DefaultConnection.usuarios.Count() + 1).ToString());
            contador = 1;
            EscribirArbolUsuario(DefaultConnection.BArbolUsuarios.Raiz, DefaultConnection.BArbolUsuarios.Grado, ref contador, 0, ref writer, rutaArbolUsuarios);
            writer.Close();
            EscribirProximaPosicion(contador, rutaArbolUsuarios);
            string ruta = @"C:\Users\" + Environment.UserName + @"\name.showtree";
            writer = new StreamWriter(ruta, false);
            writer.WriteLine("Grado: " + DefaultConnection.BArbolShowPorNombre.Grado.ToString());
            writer.WriteLine("Raíz: 1");
            writer.WriteLine("Próxima posición: ");
            contador = 1;
            EscribirArbolAudiovisual(DefaultConnection.BArbolShowPorNombre.Raiz, DefaultConnection.BArbolShowPorNombre.Grado, ref contador, 0, ref writer, ruta, DefaultConnection.BArbolShowPorNombre.Raiz);
            writer.Close();
            EscribirProximaPosicion(contador, ruta);
            ruta = @"C:\Users\" + Environment.UserName + @"\genre.showtree";
            writer = new StreamWriter(ruta, false);
            writer.WriteLine("Grado: " + DefaultConnection.BArbolShowPorGenero.Grado.ToString());
            writer.WriteLine("Raíz: 1");
            writer.WriteLine("Próxima posición: ");
            contador = 1;
            EscribirArbolAudiovisual(DefaultConnection.BArbolShowPorGenero.Raiz, DefaultConnection.BArbolShowPorGenero.Grado, ref contador, 0, ref writer, ruta, DefaultConnection.BArbolShowPorGenero.Raiz);
            writer.Close();
            EscribirProximaPosicion(contador, ruta);
            ruta = @"C:\Users\" + Environment.UserName + @"\year.showtree";
            writer = new StreamWriter(ruta, false);
            writer.WriteLine("Grado: " + DefaultConnection.BArbolShowPorAnio.Grado.ToString());
            writer.WriteLine("Raíz: 1");
            writer.WriteLine("Próxima posición: ");
            contador = 1;
            EscribirArbolAudiovisual(DefaultConnection.BArbolShowPorAnio.Raiz, DefaultConnection.BArbolShowPorAnio.Grado, ref contador, 0, ref writer, ruta, DefaultConnection.BArbolShowPorAnio.Raiz);
            writer.Close();
            EscribirProximaPosicion(contador, ruta);
            ruta = @"C:\Users\" + Environment.UserName + @"\name.movietree";
            writer = new StreamWriter(ruta, false);
            writer.WriteLine("Grado: " + DefaultConnection.BArbolMoviePorNombre.Grado.ToString());
            writer.WriteLine("Raíz: 1");
            writer.WriteLine("Próxima posición: ");
            contador = 1;
            EscribirArbolAudiovisual(DefaultConnection.BArbolMoviePorNombre.Raiz, DefaultConnection.BArbolMoviePorNombre.Grado, ref contador, 0, ref writer, ruta, DefaultConnection.BArbolMoviePorNombre.Raiz);
            writer.Close();
            EscribirProximaPosicion(contador, ruta);
            ruta = @"C:\Users\" + Environment.UserName + @"\genre.movietree";
            writer = new StreamWriter(ruta, false);
            writer.WriteLine("Grado: " + DefaultConnection.BArbolMoviePorGenero.Grado.ToString());
            writer.WriteLine("Raíz: 1");
            writer.WriteLine("Próxima posición: ");
            contador = 1;
            EscribirArbolAudiovisual(DefaultConnection.BArbolMoviePorGenero.Raiz, DefaultConnection.BArbolMoviePorGenero.Grado, ref contador, 0, ref writer, ruta, DefaultConnection.BArbolMoviePorGenero.Raiz);
            writer.Close();
            EscribirProximaPosicion(contador, ruta);
            ruta = @"C:\Users\" + Environment.UserName + @"\year.movietree";
            writer = new StreamWriter(ruta, false);
            writer.WriteLine("Grado: " + DefaultConnection.BArbolMoviePorAnio.Grado.ToString());
            writer.WriteLine("Raíz: 1");
            writer.WriteLine("Próxima posición: ");
            contador = 1;
            EscribirArbolAudiovisual(DefaultConnection.BArbolMoviePorAnio.Raiz, DefaultConnection.BArbolMoviePorAnio.Grado, ref contador, 0, ref writer, ruta, DefaultConnection.BArbolMoviePorAnio.Raiz);
            writer.Close();
            EscribirProximaPosicion(contador, ruta);
            ruta = @"C:\Users\" + Environment.UserName + @"\name.documentarytree";
            writer = new StreamWriter(ruta, false);
            writer.WriteLine("Grado: " + DefaultConnection.BArbolDocumentaryPorNombre.Grado.ToString());
            writer.WriteLine("Raíz: 1");
            writer.WriteLine("Próxima posición: ");
            contador = 1;
            EscribirArbolAudiovisual(DefaultConnection.BArbolDocumentaryPorNombre.Raiz, DefaultConnection.BArbolDocumentaryPorNombre.Grado, ref contador, 0, ref writer, ruta, DefaultConnection.BArbolDocumentaryPorNombre.Raiz);
            writer.Close();
            EscribirProximaPosicion(contador, ruta);
            ruta = @"C:\Users\" + Environment.UserName + @"\genre.documentarytree";
            writer = new StreamWriter(ruta, false);
            writer.WriteLine("Grado: " + DefaultConnection.BArbolDocumentaryPorGenero.Grado.ToString());
            writer.WriteLine("Raíz: 1");
            writer.WriteLine("Próxima posición: ");
            contador = 1;
            EscribirArbolAudiovisual(DefaultConnection.BArbolDocumentaryPorGenero.Raiz, DefaultConnection.BArbolDocumentaryPorGenero.Grado, ref contador, 0, ref writer, ruta, DefaultConnection.BArbolDocumentaryPorGenero.Raiz);
            writer.Close();
            EscribirProximaPosicion(contador, ruta);
            ruta = @"C:\Users\" + Environment.UserName + @"\year.documentarytree";
            writer = new StreamWriter(ruta, false);
            writer.WriteLine("Grado: " + DefaultConnection.BArbolDocumentaryPorAnio.Grado.ToString());
            writer.WriteLine("Raíz: 1");
            writer.WriteLine("Próxima posición: ");
            contador = 1;
            EscribirArbolAudiovisual(DefaultConnection.BArbolDocumentaryPorAnio.Raiz, DefaultConnection.BArbolDocumentaryPorAnio.Grado, ref contador, 0, ref writer, ruta, DefaultConnection.BArbolDocumentaryPorAnio.Raiz);
            writer.Close();
            EscribirProximaPosicion(contador, ruta);

            return RedirectToAction("IniciarSesionAdmin");
        }
        private void IniciarListaUsuario(string rutaArchivo, bool sobrescribir)
        {
            StreamWriter GradoWriter = new StreamWriter(rutaArchivo, sobrescribir);
            GradoWriter.WriteLine("(Vacío)");
            GradoWriter.Close();
        }

        private void EscribirArbolUsuario(BNodo<string, Usuario> nodo, int grado, ref int contador, int contadorPadre, ref StreamWriter writer, string ruta)
        {
            string linea;
            linea = $"{contador.ToString("000;-000")}|{contadorPadre.ToString("000;-000")}";
            for (int i = 0; i < grado; i++)
            {
                linea += $"|{String.Format("{0, -3}", "")}";
            }
            for (int i = 0; i < nodo.Entradas.Count(); i++)
            {
                Usuario item = nodo.Entradas[i].Apuntador;
                linea += $"|{item.ToFixedSizeString()}";
            }
            for (int i = nodo.Entradas.Count(); i < (grado - 1); i++)
            {
                linea += $"|{String.Format("{0, -87}", "")}";
            }
            writer.WriteLine(linea);
            if (nodo != DefaultConnection.BArbolUsuarios.Raiz)
            {
                writer.Close();
                ReescribirArchivo(contadorPadre, contador, ruta);
                writer = new StreamWriter(ruta, true);
            }

            contador++;
            int contadorHijos;
            contadorHijos = 0;
            contadorPadre = contador - 1;
            foreach (var item in nodo.Hijos)
            {
                EscribirArbolUsuario(nodo.Hijos[contadorHijos], grado, ref contador, contadorPadre, ref writer, ruta);
                contadorHijos++;
            }
        }

        private void ReescribirArchivo(int contador, int Posicion, string ruta)
        {
            string[] lines = System.IO.File.ReadAllLines(ruta);
            string line = lines[contador + 2];
            bool hijoyaIngresado;
            hijoyaIngresado = false;
            var linea = line.Split('|');
            line = "";
            for (int i = 0; i < linea.Length; i++)
            {
                if(i!=0)
                {
                    line += $"|";
                }
                if(i>1 && linea[i] == "   "&&!hijoyaIngresado)
                {
                    line += Posicion.ToString("000;-000");
                    hijoyaIngresado = true;
                }
                else
                {
                    line += linea[i];
                }
            }
            lines[contador + 2] = line;
            StreamWriter writer = new StreamWriter(ruta, false);
            for (int i = 0; i < lines.Length; i++)
            {
                    writer.WriteLine(lines[i]);
            }
            writer.Close();
        }

        private void EscribirProximaPosicion(int Posicion, string ruta)
        {
            string[] lines = System.IO.File.ReadAllLines(ruta);
            lines[2] = "Próxima posición: " + Posicion.ToString();
            StreamWriter writer = new StreamWriter(ruta, false);
            for (int i = 0; i < lines.Length; i++)
            {
                writer.WriteLine(lines[i]);
            }
            writer.Close();
        }


        private void EscribirArbolAudiovisual(BNodo<string, Audiovisual> nodo, int grado, ref int contador, int contadorPadre, ref StreamWriter writer, string ruta, BNodo<string, Audiovisual> raiz)
        {
            string linea;
            linea = $"{contador.ToString("000;-000")}|{contadorPadre.ToString("000;-000")}";
            for (int i = 0; i < grado; i++)
            {
                linea += $"|{String.Format("{0, -3}", "")}";
            }
            for (int i = 0; i < nodo.Entradas.Count(); i++)
            {
                Audiovisual item = nodo.Entradas[i].Apuntador;
                linea += $"|{item.ToFixedSizeString()}";
            }
            for (int i = nodo.Entradas.Count(); i < (grado - 1); i++)
            {
                linea += $"|{String.Format("{0, -66}", "")}";
            }
            writer.WriteLine(linea);
            if (nodo != raiz)
            {
                writer.Close();
                ReescribirArchivo(contadorPadre, contador, ruta);
                writer = new StreamWriter(ruta, true);
            }

            contador++;
            int contadorHijos;
            contadorHijos = 0;
            contadorPadre = contador - 1;
            foreach (var item in nodo.Hijos)
            {
                EscribirArbolAudiovisual(nodo.Hijos[contadorHijos], grado, ref contador, contadorPadre, ref writer, ruta, raiz);
                contadorHijos++;
            }
        }
    }
}
