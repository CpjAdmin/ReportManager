using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_Entidades
{
    public class Subdirectorio
    {
        public String cod_subdirectorio { get; set; }
        public String nombre { get; set; }
        public Subdirectorio()
        {
            cod_subdirectorio = "";
            nombre = "";
        }

    }

    public class Reporte
    {
        public String cod_reporte { get; set; }
        public String cod_alterno { get; set; }
        public String nombre { get; set; }
        public String nombre_archivo { get; set; }
        public String cod_directorio { get; set; }
        public String cod_subdirectorio { get; set; }
        public String cod_sistema { get; set; }
        public String descripcion { get; set; }
        public String propietario { get; set; }
        public String ubicacion { get; set; }
        public String directorio { get; set; }
        public String subdirectorio { get; set; }
        public String sistema { get; set; }
        public String estado { get; set; }
        public String url { get; set; }


        public Reporte() {

            this.cod_reporte = "";
            this.cod_alterno = "";
            this.nombre = "";
            this.nombre_archivo = "";
            this.cod_directorio = "";
            this.cod_subdirectorio = "";
            this.cod_sistema = "";
            this.descripcion = "";
            this.propietario = "";
            this.ubicacion = "";
            this.directorio = "";
            this.subdirectorio = "";
            this.sistema = "";
            this.estado = "A";
            this.url = "";
        }

        public Reporte(String cod_reporte, String cod_alterno, String nombre, String cod_directorio, String cod_subdirectorio, String cod_sistema, String nombre_archivo, String descripcion, String propietario,String ubicacion,String directorio,String subdirectorio,String sistema, String estado, String url)
        {
            this.cod_reporte = cod_reporte;
            this.cod_alterno = cod_alterno;
            this.nombre = nombre;
            this.nombre_archivo = nombre_archivo;
            this.cod_directorio = cod_directorio;
            this.cod_subdirectorio = cod_subdirectorio;
            this.cod_sistema = cod_sistema;
            this.descripcion = descripcion;
            this.propietario = propietario;
            this.ubicacion = ubicacion;
            this.directorio = directorio;
            this.subdirectorio = subdirectorio;
            this.sistema = sistema;
            this.estado = estado;
            this.url = url;
        }
    }
}
