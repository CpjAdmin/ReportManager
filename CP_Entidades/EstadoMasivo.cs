using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_Entidades
{
    public class EstadoMasivo
    {
        public int num_lote { get; set; }
        public string fec_corte { get; set; }
        public Contrato[] lista_contratos { get; set; }
        public string ruta { get; set; }
        public string formato { get; set; }
        public string borrar_directorio { get; set; }
        public int id_usuario { get; set; }
        public string navegador { get; set; }

        public EstadoMasivo()
        {
            this.num_lote = 0;
            this.fec_corte = "";
            this.lista_contratos = Array.Empty<Contrato>();
            this.ruta = "";
            this.formato = "";
            this.borrar_directorio = "";
            this.id_usuario = 0;
            this.navegador = "";
        }

        public EstadoMasivo(int num_lote, string fec_corte, Contrato[] lista_contratos, string ruta, string formato, string borrar_directorio, int id_usuario, string navegador)
        {
            this.num_lote = num_lote;
            this.fec_corte = fec_corte;
            this.lista_contratos = lista_contratos;
            this.ruta = ruta;
            this.formato = formato;
            this.borrar_directorio = borrar_directorio;
            this.id_usuario = id_usuario;
            this.navegador = navegador;
        }
    }
}
