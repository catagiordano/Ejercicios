using System;
using System.Collections.Generic;
using System.Text;

namespace AccesoDatos.Models
{
    public class Client
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public string DNI { get; set; }

        public string LicenciaConducir { get; set; }

        public string Telefono { get; set; }

        public bool TieneDescuento { get; set; }

        public List<Alquiler> Alquileres { get; set; } = new List<Alquiler>();
    }
}
