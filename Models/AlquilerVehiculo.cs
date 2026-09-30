using AccesoDatos.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccesoDatos.Models
{
    public class AlquilerVehiculo
    {
        public int Id { get; set; }

        public int AlquilerId { get; set; }

        public Alquiler Alquiler { get; set; }

        public int VehiculoId { get; set; }

        public Vehiculo Vehiculo { get; set; }
    }
}