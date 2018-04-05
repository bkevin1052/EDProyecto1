using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EDProyecto1.Controllers
{
    public class AudiovisualController : Controller
    {
        // GET: Audiovisual
        public ActionResult Index()
        {
            return View();
        }

        // GET: Audiovisual/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Audiovisual/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Audiovisual/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Audiovisual/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Audiovisual/Edit/5
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

        // GET: Audiovisual/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Audiovisual/Delete/5
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
