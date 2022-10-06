using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CP_AccesoDatos;
using CP_Entidades;
using System.Data;

namespace CP_LogicaNegocio
{
    public class ReportesLN
    {
        #region "Logica de Negocio Reporte"
        private static ReportesLN ObjReporte = null;
        private ReportesLN() { }
        public static ReportesLN getInstancia()
        {
            if (ObjReporte == null)
            {
                ObjReporte = new ReportesLN();
            }
            return ObjReporte;
        }
        #endregion


        public bool MantenimientoReportes(Reporte reporte, String cod_proceso)
        {
            try
            {

                return ReportesDAO.getInstancia().MantenimientoReportes(reporte, cod_proceso);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Reporte> CargarReportes(int cod_perfil, string listaReportes, String cod_proceso)
        {
            try
            {
                return ReportesDAO.getInstancia().CargarReportes(cod_perfil, listaReportes, cod_proceso);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<String> ReportesPorPerfil(int cod_perfil, String cod_proceso)
        {
            try
            {
                return ReportesDAO.getInstancia().ReportesPorPerfil(cod_perfil, cod_proceso);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Reporte> ReportesPorUsuario(String login,String tipo,  int cod_proceso)
        {
            try
            {
                return ReportesDAO.getInstancia().ReportesPorUsuario(login, tipo, cod_proceso);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable CargarTreeview(int cod_usuario, String tipo_directorio, int codigo)
        {
            String proceso; // Directorio o Reporte
            try
            {
                if (cod_usuario == 0)
                {
                    proceso = "Directorio";
                }
                else
                {
                    proceso = "Reporte";
                }

                return ReportesDAO.getInstancia().CargarTreeview(cod_usuario, tipo_directorio, codigo, proceso);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Cargar Sistemas
        public DataTable CargarSistemas()
        {
            try
            {
                return ReportesDAO.getInstancia().CargarSistemas();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Cargar Directorios
        public DataTable CargarDirectorios()
        {
            try
            {
                return ReportesDAO.getInstancia().CargarDirectorios();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Cargar Subdirectorios
        public List<Subdirectorio> CargarSubdirectorios(int cod_directorio)
        {
            try
            {
                return ReportesDAO.getInstancia().CargarSubdirectorios(cod_directorio);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
