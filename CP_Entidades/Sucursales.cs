using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_Entidades
{
    public class Sucursales
    {

        public int cod_sucursal { get; set; }
        public String cod_alterno { get; set; }
        public String nombre { get; set; }
        public String direccion { get; set; }
        public String telefono1 { get; set; }
        public String telefono2 { get; set; }
        public String fax { get; set; }

        public Sucursales() { }

        public Sucursales(int cod_sucursal, String cod_alterno, String nombre, String direccion, String telefono1, String telefono2, String fax)
        {
            this.cod_sucursal = cod_sucursal;
            this.cod_alterno = cod_alterno;
            this.nombre = nombre;
            this.direccion = direccion;
            this.telefono1 = telefono1;
            this.telefono2 = telefono2;
            this.fax = fax;

        }
    }
}
