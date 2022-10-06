using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_Entidades
{
    public class BitacoraEstadoDet
    {
        public int id { get; set; }
        public int num_lote { get; set; }
        public int num_registro { get; set; }
        public int cod_cliente { get; set; }
        public string identificacion { get; set; }
        public int num_contrato { get; set; }
        public string estado { get; set; }
        public string response { get; set; }
        public int spid { get; set; }

        public BitacoraEstadoDet()
        {
            this.id = 0;
            this.num_lote = 0;
            this.num_registro = 0;
            this.cod_cliente = 0;
            this.identificacion = "";
            this.num_contrato = 0;
            this.estado = "";
            this.response = "";
            this.spid = 0;
        }

        public BitacoraEstadoDet(int id, int num_lote, int num_registro, int cod_cliente, string identificacion, int num_contrato, string estado, string response, int spid)
        {
            this.id = id;
            this.num_lote = num_lote;
            this.num_registro = num_registro;
            this.cod_cliente = cod_cliente;
            this.identificacion = identificacion;
            this.num_contrato = num_contrato;
            this.estado = estado;
            this.response = response;
            this.spid = spid;
        }
    }
}
