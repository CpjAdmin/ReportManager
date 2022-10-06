using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_Entidades
{
    public class EstadoCuenta
    {
        public int cod_cliente { get; set; }
        public string identificacion { get; set; }
        public string nombre { get; set; }
        public int num_contrato { get; set; }
        public string email { get; set; }
        public string fec_corte { get; set; }
        public string ruta_genera { get; set; }
        public string ruta_envio { get; set; }
        public string archivo { get; set; }
        public string tipo_consulta { get; set; }
        public int id_usuario { get; set; }

        public EstadoCuenta()
        {
            this.cod_cliente = 0;
            this.identificacion = "";
            this.nombre = "";
            this.num_contrato = 0;
            this.email = "";
            this.fec_corte = "";
            this.ruta_genera = "";
            this.ruta_envio = "";
            this.archivo = "";
            this.tipo_consulta = "";
            this.id_usuario = 0;
        }

        public EstadoCuenta(int cod_cliente, string identificacion, string nombre, int num_contrato, string email, string fec_corte, string ruta_genera, string ruta_envio, string archivo, string tipo_consulta)
        {
            this.cod_cliente = cod_cliente;
            this.identificacion = identificacion;
            this.nombre = nombre;
            this.num_contrato = num_contrato;
            this.email = email;
            this.fec_corte = fec_corte;
            this.ruta_genera = ruta_genera;
            this.ruta_envio = ruta_envio;
            this.archivo = archivo;
            this.tipo_consulta = tipo_consulta;
        }
    }
}
