using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RecursoHumanoProyecto.Models;

namespace RecursoHumanoProyecto.Controllers
{
    public class NominasController : Controller
    {
        private RecursoHumanoEntities db = new RecursoHumanoEntities();

        // GET: Nominas
        public ActionResult Index()
        {
            return View(db.Nomina.ToList());
        }
        


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
