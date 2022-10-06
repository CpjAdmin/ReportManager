using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_Entidades
{
    public class Departamentos
    {
        public int cod_departamento { get; set; }
        public String cod_alterno { get; set; }
        public String nombre { get; set; }

        public Departamentos() { }

        public Departamentos(int cod_departamento, String cod_alterno, String nombre)
        {
            this.cod_departamento = cod_departamento;
            this.cod_alterno = cod_alterno;
            this.nombre = nombre;
        }

    }
}
