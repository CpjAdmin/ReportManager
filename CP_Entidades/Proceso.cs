using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_Entidades
{
    public class Proceso
    {
        public int cod_proceso { get; set; }
        public String nombre { get; set; }
        public String descripcion { get; set; }
        public String formulario { get; set; }
        public String etiqueta_id { get; set; }
        public String etiqueta_nombre { get; set; }


        public int id_usuario { get; set; }
        public String navegador { get; set; }
        public String pagina { get; set; }
        public String proceso { get; set; }
        public String terminal_id { get; set; }

        public Proceso()
        {
            this.cod_proceso = 0;
            this.nombre = "";
            this.descripcion = "";
            this.formulario = "";
            this.etiqueta_id = "";
            this.etiqueta_nombre = "";

            this.id_usuario = 0;
            this.navegador = "";
            this.pagina = "";
            this.proceso = "";
            this.terminal_id = "";

        }

        public Proceso(int cod_proceso, string nombre, string descripcion, string formulario, string etiqueta_id, string etiqueta_nombre)
        {
            this.cod_proceso = this.cod_proceso;
            this.nombre = this.nombre;
            this.descripcion = this.descripcion;
            this.formulario = this.formulario;
            this.etiqueta_id = this.etiqueta_id;
            this.etiqueta_nombre = this.etiqueta_nombre;
        }
    }
}
