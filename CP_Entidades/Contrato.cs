using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_Entidades
{
    public class Contrato
    {
        public int cod_cliente { get; set; }
        public string identificacion { get; set; }
        public int num_contrato { get; set; }

        public Contrato()
        {
            this.cod_cliente = 0;
            this.identificacion = "0";
            this.num_contrato = 0;
        }

        public Contrato(int cod_cliente, string identificacion, int num_contrato)
        {
            this.cod_cliente = cod_cliente;
            this.identificacion = identificacion;
            this.num_contrato = num_contrato;
        }
    }
}
