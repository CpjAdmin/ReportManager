using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_Entidades
{
    public class Usuario
    {
        public int cod_usuario { get; set; }
        public String nombre { get; set; }
        public Departamento departamento { get; set; }
        public Sucursal sucursal { get; set; }
        public Rol rol { get; set; }
        public String foto { get; set; }
        public String login { get; set; }
        public String clave { get; set; }
        public String i_estado { get; set; }

        public Usuario()
        {
            this.cod_usuario = 0;
            this.nombre = "";
            this.departamento = new Departamento();
            this.sucursal = new Sucursal();
            this.rol = new Rol();
            this.foto = "usuario.jpg";
            this.login = "";
            this.clave = "";
            this.i_estado = "A";
        }

        public Usuario(int cod_usuario, String nombre, Departamento departamento, Sucursal sucursal, Rol rol, String foto, String login,String clave,String i_estado)
        {
            this.cod_usuario = cod_usuario;
            this.nombre = nombre;
            this.departamento = departamento;
            this.sucursal = sucursal;
            this.rol = rol;
            this.foto = foto;
            this.login = login;
            this.clave = clave;
            this.i_estado = i_estado;
        }
    }
}
