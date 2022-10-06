using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_Entidades
{
    public class BitacoraEstadoEnc
    {
        public int id { get; set; }
        public int cod_proceso { get; set; }
        public int cod_usuario { get; set; }
        public string pagina { get; set; }
        public int num_lote { get; set; }
        public int num_registros { get; set; }
        public int cod_cliente_inicio { get; set; }
        public int cod_cliente_final { get; set; }
        public string i_pruebas { get; set; }
        public string i_borrar_dir { get; set; }
        public string fecha_corte { get; set; }
        public string servidor_genera { get; set; }
        public string servidor_ssrs { get; set; }
        public string url_ssrs { get; set; }
        public string dir_local { get; set; }
        public string dir_remoto { get; set; }
        public int archivos_bloqueados { get; set; }
        public int archivos_errores { get; set; }
        public string request { get; set; }
        public string response { get; set; }
        public int spid { get; set; }
        public string navegador { get; set; }
        public string terminal_id { get; set; }
        public string fecha_inicio { get; set; }
        public string fecha_fin { get; set; }

        public BitacoraEstadoEnc()
        {
            this.id = 0;
            this.cod_proceso = 0;
            this.cod_usuario = 0;
            this.pagina = "";
            this.num_lote = 0;
            this.num_registros = 0;
            this.cod_cliente_inicio = 0;
            this.cod_cliente_final = 0;
            this.i_pruebas = "";
            this.i_borrar_dir = "";
            this.fecha_corte = "";
            this.servidor_genera = "";
            this.servidor_ssrs = "";
            this.url_ssrs = "";
            this.dir_local = "";
            this.dir_remoto = "";
            this.archivos_bloqueados = 0;
            this.archivos_errores = 0;
            this.request = "";
            this.response = "";
            this.spid = 0;
            this.navegador = "";
            this.terminal_id = "";
            this.fecha_inicio = "";
            this.fecha_fin = "";
        }

        public BitacoraEstadoEnc(int id, int cod_proceso, int cod_usuario, string pagina, int num_lote, int num_registros, int cod_cliente_inicio, int cod_cliente_final, string i_pruebas, string i_borrar_dir, string fecha_corte, string servidor_genera, string servidor_ssrs, string url_ssrs, string dir_local, string dir_remoto, int archivos_bloqueados, int archivos_errores, string request, string response, int spid, string navegador, string terminal_id, string fecha_inicio, string fecha_fin)
        {
            this.id = id;
            this.cod_proceso = cod_proceso;
            this.cod_usuario = cod_usuario;
            this.pagina = pagina;
            this.num_lote = num_lote;
            this.num_registros = num_registros;
            this.cod_cliente_inicio = cod_cliente_inicio;
            this.cod_cliente_final = cod_cliente_final;
            this.i_pruebas = i_pruebas;
            this.i_borrar_dir = i_borrar_dir;
            this.fecha_corte = fecha_corte;
            this.servidor_genera = servidor_genera;
            this.servidor_ssrs = servidor_ssrs;
            this.url_ssrs = url_ssrs;
            this.dir_local = dir_local;
            this.dir_remoto = dir_remoto;
            this.archivos_bloqueados = archivos_bloqueados;
            this.archivos_errores = archivos_errores;
            this.request = request;
            this.response = response;
            this.spid = spid;
            this.navegador = navegador;
            this.terminal_id = terminal_id;
            this.fecha_inicio = fecha_inicio;
            this.fecha_fin = fecha_fin;
        }
    }
}
