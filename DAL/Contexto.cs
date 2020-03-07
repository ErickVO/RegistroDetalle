using Microsoft.EntityFrameworkCore;
using RegistroConDetalle.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace RegistroConDetalle.DAL
{
    public class Contexto : DbContext
    {
        public DbSet<Personas> Personas { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source = Personas.db");
        }
    }
}
