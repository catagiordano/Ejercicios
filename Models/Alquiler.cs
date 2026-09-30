using System;
using System.Collections.Generic;
using System.Text;

namespace AccesoDatos.Models
    {
        public class Alquiler
        {
            public int Id { get; set; }

            public int ClienteId { get; set; }

            public Client Cliente { get; set; }

            public DateTime FechaAlquiler { get; set; }

            public DateTime FechaLimite { get; set; }

            public DateTime? FechaDevolucion { get; set; }

            public decimal MontoBase { get; set; }

            public decimal SeguroPorcentaje { get; set; }

            public decimal MontoSeguro { get; set; }

            public decimal MontoFinal { get; set; }

            public bool Devuelto { get; set; }

            public List<AlquilerVehiculo> Vehiculos { get; set; } = new List<AlquilerVehiculo>();
        }
}

