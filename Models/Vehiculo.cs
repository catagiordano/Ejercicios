using System;
using System.Collections.Generic;
using System.Text;

namespace AccesoDatos.Models
{
    public class Vehiculo
    {
        public int Id { get; set; }

        public string Patente { get; set; }

        public string Marca { get; set; }

        public string Modelo { get; set; }

        public decimal PrecioPorDia { get; set; }

        public int CantidadDisponible { get; set; }

        public decimal ValorVehiculo { get; set; }

        public List<AlquilerVehiculo> Alquileres { get; set; } = new List<AlquilerVehiculo>();
    }
}
