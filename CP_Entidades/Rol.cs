using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_Entidades
{
    public class Rol
    {
        public int cod_rol { get; set; }
        public String nombre { get; set; }
        public String descripcion { get; set; }

        public Rol() { }

        public Rol(int cod_rol, String nombre, String descripcion)
        {
            this.cod_rol = cod_rol;
            this.nombre = nombre;
            this.descripcion = descripcion;
          
        }
    }
}
