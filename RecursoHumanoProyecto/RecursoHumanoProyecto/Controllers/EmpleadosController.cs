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
    public class EmpleadosController : Controller
    {
        private RecursoHumanoEntities db = new RecursoHumanoEntities();

        // GET: Empleados
        public ActionResult Index()
        {
            var empleados = db.Empleados.Include(e => e.Cargo).Include(e => e.Departamento);
            return View(empleados.ToList());
        }


        public ActionResult EmpleadoInactivo(String buscar)
        {



            var estado = from s in db.Empleados.ToList()
                         where (s.Estatus.Equals("inactivo"))
                         select s;


            if (!String.IsNullOrEmpty(buscar))
            {
                
                estado = estado.Where(j => j.Nombre.Contains(buscar));
            }

           

            return View(estado.ToList());
        }




        public ActionResult EmpleadoActivo(String buscar)

        {


            var estado = from s in db.Empleados.ToList()
                         where (s.Estatus.Equals("activo"))
                         select s;

            if (!String.IsNullOrEmpty(buscar)) {


                estado = estado.Where(g => g.Nombre.Contains(buscar));

            }


            return View(estado.ToList());


        }





        public ActionResult Nomina([Bind(Include = "Id,Mes,Anos,Nomina1")]Models.Nomina model)
        {

            ///////////////////////////////////////////////////////////////////////////////////////////////
            if (model.Id == 0)
            {


                var eliminar = db.Nomina.FirstOrDefault(x => x.Id == 0);

                if (eliminar != null)
                {

                    db.Nomina.Remove(eliminar);
                    db.SaveChanges();
                }

            }



            ///////////////////////////////////////////////////////////////////////////////////////////////////////

            var estado = from s in db.Empleados.ToList()
                         where (s.Estatus.Equals("activo"))
                         select s.Salario;


            var estadoo = from s in db.Empleados.ToList()
                          where (s.Estatus.Equals("inactivo"))
                          select s.Salario;

            if (estado != estadoo)
            {


                var Mes = model.Mes;
                var Anos = model.Anos;

                var empleado = from s in db.Empleados.ToList()
                               where s.Anos.Equals(Anos) && s.Mes.Equals(Mes) && s.Estatus.Equals("activo")
                               select s.Salario;


                double guarda = empleado.Sum();
                float nomin = Convert.ToSingle(guarda);

                ViewBag.Suma = empleado.Sum();
               
                model.Nomina1 = nomin;



               


            }

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////  



            if (ModelState.IsValid)
            {
                db.Nomina.Add(model);
                db.SaveChanges();

            }
            else
            {
                ViewBag.Message = "Verifique que haiga puesto el mes Correctamente";
            }



            return View("Nomina");

        }





        public ActionResult EntradaEmpleada(String Nombre_Empleados)
        {

            var proviene = from s in db.Empleados select s;

            if (!String.IsNullOrEmpty(Nombre_Empleados))
            {

                int a = Int32.Parse(Nombre_Empleados);

                proviene = proviene.Where(j => j.Dias.Equals(a));
            }

            return View(proviene);

        }







        public ActionResult Busqueda(String Nombre_Empleados)
        {

            var proviene = from s in db.Empleados select s;

            if (!String.IsNullOrEmpty(Nombre_Empleados))
            {

                proviene = proviene.Where(j => j.Nombre.Contains(Nombre_Empleados));
            }

            return View(proviene);

        }





        // GET: Empleados/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empleados empleados = db.Empleados.Find(id);
            if (empleados == null)
            {
                return HttpNotFound();
            }
            return View(empleados);
        }

        // GET: Empleados/Create
        public ActionResult Create()
        {
            ViewBag.IdCargo = new SelectList(db.Cargo, "Id", "CargoEmpleado");
            ViewBag.IdDepartamento = new SelectList(db.Departamento, "Id", "NombreDepartamento");
            return View();
        }

        // POST: Empleados/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IdDepartamento,IdCargo,Codigo,Nombre,Apellido,Telefono,Dias,Mes,Anos,Salario,Estatus")] Empleados empleados)
        {
            if (ModelState.IsValid)
            {
                db.Empleados.Add(empleados);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdCargo = new SelectList(db.Cargo, "Id", "CargoEmpleado", empleados.IdCargo);
            ViewBag.IdDepartamento = new SelectList(db.Departamento, "Id", "NombreDepartamento", empleados.IdDepartamento);
            return View(empleados);
        }


       



        // GET: Empleados/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empleados empleados = db.Empleados.Find(id);
            if (empleados == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdCargo = new SelectList(db.Cargo, "Id", "CargoEmpleado", empleados.IdCargo);
            ViewBag.IdDepartamento = new SelectList(db.Departamento, "Id", "NombreDepartamento", empleados.IdDepartamento);
            return View(empleados);
        }

        // POST: Empleados/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IdDepartamento,IdCargo,Codigo,Nombre,Apellido,Telefono,Dias,Mes,Anos,Salario,Estatus")] Empleados empleados)
        {
            if (ModelState.IsValid)
            {
                db.Entry(empleados).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdCargo = new SelectList(db.Cargo, "Id", "CargoEmpleado", empleados.IdCargo);
            ViewBag.IdDepartamento = new SelectList(db.Departamento, "Id", "NombreDepartamento", empleados.IdDepartamento);
            return View(empleados);
        }



        // GET: Empleados/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empleados empleados = db.Empleados.Find(id);
            if (empleados == null)
            {
                return HttpNotFound();
            }
            return View(empleados);
        }

        // POST: Empleados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Empleados empleados = db.Empleados.Find(id);
            db.Empleados.Remove(empleados);
            db.SaveChanges();
            return RedirectToAction("Index");
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
