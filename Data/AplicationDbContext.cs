using AccesoDatos.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccesoDatos.Data
{
    public class AplicationDbContext : DbContext
    {
        public DbSet<Vehiculo> Vehiculos { get; set; }

        public DbSet<Client> Clientes { get; set; }

        public DbSet<Alquiler> Alquileres { get; set; }

        public DbSet<AlquilerVehiculo> AlquileresVehiculos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(
                "Data Source=C:\\Database\\RentACar.db"
            );
        }
    }
}
