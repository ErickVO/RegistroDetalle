using Microsoft.EntityFrameworkCore;
using RegistroConDetalle.DAL;
using RegistroConDetalle.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace RegistroConDetalle.BLL
{
    public class PersonasBll
    {
        public static bool Guardar(Personas personas)
        {
            bool paso = false;
            Contexto db = new Contexto();

            try
            {
                if (db.Personas.Add(personas) != null)
                {
                    paso = (db.SaveChanges() > 0);
                }

            }
            catch
            {
                throw;
            }
            finally
            {
                db.Dispose();
            }
            return paso;
        }

        public static bool Modificar(Personas personas)
        {
            bool paso = false;
            Contexto db = new Contexto();

            try
            {
                var Anterior = db.Personas.Find(personas.PersonaId);
                foreach (var item in Anterior.Telefonos)
                {
                    if (!personas.Telefonos.Exists(d => d.Id == item.Id))
                        db.Entry(item).State = EntityState.Deleted;
                }

                foreach (var item in personas.Telefonos)
                { 
                    var estado = item.Id > 0 ? EntityState.Modified : EntityState.Added;
                    db.Entry(item).State = estado;
                }

                db.Entry(personas).State = EntityState.Modified;
                paso = (db.SaveChanges() > 0);
            }
            catch
            {
                throw;
            }
            finally
            {
                db.Dispose();
            }
            return paso;
        }

        public static bool Eliminar(int id)
        {
            bool paso = false;
            Contexto db = new Contexto();

            try
            {
                var eliminar = db.Personas.Find(id);
                db.Entry(eliminar).State = EntityState.Deleted;
                paso = (db.SaveChanges() > 0);
            }
            catch
            {
                throw;
            }
            finally
            {
                db.Dispose();
            }
            return paso;
        }

        public static Personas Buscar(int id)
        {
            Personas personas = new Personas();
            Contexto db = new Contexto();

            try
            {
                personas = db.Personas.Include(t => t.Telefonos).Where(p => p.PersonaId == id).Single();
            }
            catch
            {
                throw;
            }
            finally
            {
                db.Dispose();
            }
            return personas;
        }


        public static List<Personas> GetList(Expression<Func<Personas, bool>> personas)
        {
            Contexto db = new Contexto();
            List<Personas> listado = new List<Personas>();

            try
            {
                listado = db.Personas.Where(personas).ToList();
            }
            catch
            {
                throw;
            }
            finally
            {
                db.Dispose();
            }
            return listado;
        }


    }
}
