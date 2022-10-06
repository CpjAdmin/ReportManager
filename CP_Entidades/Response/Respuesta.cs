using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_Entidades.Response
{
    public class Respuesta
    {
        public int Exito { get; set; }

        public string Mensaje { get; set; }

        public object Data { get; set; }

        public Respuesta()
        {
            //*** 0 = Incorrecto | 1 = Correcto
            this.Exito = 0;
        }
    }
}
