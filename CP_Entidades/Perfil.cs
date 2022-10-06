using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_Entidades
{
    public class Perfil
    {
        public int id { get; set; }
        public String nombre { get; set; }
        public String descripcion { get; set; }
        public String estado { get; set; }
        public String fecha_creacion { get; set; }
        public String fecha_modifica { get; set; }
        public String login_creacion { get; set; }
        public String login_modifica { get; set; }
        public String terminal_creacion { get; set; }
        public String terminal_modifica { get; set; }
        public String listaReportes { get; set; }
        public String listaUsuarios { get; set; }

        public Perfil()
        {
            this.id = 0;
            this.nombre = "";
            this.descripcion = "";
            this.estado = "";
            this.fecha_creacion = "";
            this.fecha_modifica = "";
            this.login_creacion = "";
            this.login_modifica = "";
            this.terminal_creacion = "";
            this.terminal_modifica = "";
            this.listaReportes = "";
            this.listaUsuarios = "";
        }

        public Perfil(int id, String nombre, String descripcion, String estado, String fecha_creacion, String fecha_modifica
                     , String login_creacion, String login_modifica, String terminal_creacion, String terminal_modifica, String listaReportes, String listaUsuarios)
        {
            this.id = id;
            this.nombre = nombre;
            this.descripcion = descripcion;
            this.estado = estado;
            this.fecha_creacion = fecha_creacion;
            this.fecha_modifica = fecha_modifica;
            this.login_creacion = login_creacion;
            this.login_modifica = login_modifica;
            this.terminal_creacion = terminal_creacion;
            this.terminal_modifica = terminal_modifica;
            this.listaReportes = listaReportes;
            this.listaUsuarios = listaUsuarios;
        }

    }
}
