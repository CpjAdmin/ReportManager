using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_Entidades
{
    public class BusquedaAsociado
    {
        public int cod_proceso { get; set; }
        public string codigo { get; set; }
        public string identificacion { get; set; }
        public string cod_provincia { get; set; }
        public string cod_canton { get; set; }
        public string cod_distrito { get; set; }
        public string cod_centro { get; set; }
        public string cod_institucion { get; set; }
        public string cod_lugar { get; set; }
        public string con_email { get; set; }
        public string tipo_consulta { get; set; }
        public int ult_cod_cliente_gen { get; set; }

        public BusquedaAsociado()
        {
            this.codigo = string.Empty;
            this.identificacion = string.Empty;
            this.cod_provincia = string.Empty; 
            this.cod_canton = string.Empty;
            this.cod_distrito = string.Empty;
            this.cod_centro = string.Empty;
            this.cod_institucion = string.Empty;
            this.cod_lugar = string.Empty;
            this.con_email = string.Empty;
            this.tipo_consulta = string.Empty;
            this.ult_cod_cliente_gen = 0;
        }

        public BusquedaAsociado(string codigo, string identificacion, string cod_provincia, string cod_canton, string cod_distrito, string cod_centro, string cod_institucion, string cod_lugar, string con_email, string tipo_consulta, int ult_cod_cliente_gen)
        {
            this.codigo = codigo;
            this.identificacion = identificacion;
            this.cod_provincia = cod_provincia;
            this.cod_canton = cod_canton;
            this.cod_distrito = cod_distrito;
            this.cod_centro = cod_centro;
            this.cod_institucion = cod_institucion;
            this.cod_lugar = cod_lugar;
            this.con_email = con_email;
            this.tipo_consulta = tipo_consulta;
            this.ult_cod_cliente_gen = ult_cod_cliente_gen;
        }
    }
}
