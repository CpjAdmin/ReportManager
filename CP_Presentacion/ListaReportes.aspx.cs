using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using CP_LogicaNegocio;
using CP_Entidades;
using System.IO;

namespace CP_Presentacion
{
    public partial class ListadoReportes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Usuario"] == null || (bool)Session["ListaReportes"] == false)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                if (!Page.IsPostBack)
                {
                    try
                    {
                        //Cod Usuario
                        int id_usuario = Int32.Parse(Session["ID"].ToString());

                        //Carga los Controles
                        CargarControles();

                        // *** Cargar Controles
                        CargarComponentes(id_usuario);

                        //*** Auditoría
                        Auditoria(id_usuario, 12, "Ingreso - Módulo de Reportes");

                    }
                    catch (Exception ex)
                    {

                        string titulo = Path.GetFileName(Request.Url.AbsolutePath);
                        string mensaje = ex.Message.Replace("'", "\"");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "mensajeError('" + mensaje + "', '" + titulo + "');", true);
                    }
                }
            }
        }

        public void CargarComponentes(int id_usuario)
        {
            bool agregarReporte;
            bool editarReporte;

            try
            {
                // Menú de Perfiles
                agregarReporte = ProcesoLN.getInstancia().VerificarPermiso(id_usuario, "btn_ListaReportes_Agregar", "");
                editarReporte = ProcesoLN.getInstancia().VerificarPermiso(id_usuario, "btn_ListaReportes_Editar", "");

                if (!agregarReporte)
                {
                    FnAgregar.Visible = false;
                }

                if (!editarReporte)
                {
                    FnEditar.Visible = false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void CargarControles()
        {
            //********** DropDownList Sistemas
            ddlSistema.DataSource = ReportesLN.getInstancia().CargarSistemas();
            ddlSistema.DataTextField = "nombre";
            ddlSistema.DataValueField = "codigo";
            ddlSistema.DataBind();

            //********** DropDownList Nodos ( Directorios )
            ddlDirectorio.DataSource = ReportesLN.getInstancia().CargarDirectorios();
            ddlDirectorio.DataTextField = "nombre";
            ddlDirectorio.DataValueField = "codigo";
            ddlDirectorio.DataBind();

            //********** DropDownList Nodos ( Directorios )
            ddlPropietario.DataSource = UsuarioLN.getInstancia().GetUsuariosDDL();
            ddlPropietario.DataTextField = "nombre";
            ddlPropietario.DataValueField = "cod_usuario";
            ddlPropietario.DataBind();
        }

        [WebMethod]
        public static List<CP_Entidades.Reporte> ReportesPorUsuario(string login, string tipo)
        {
            List<CP_Entidades.Reporte> lista = null;

            try
            {
                //Genera la lista de reportes
                lista = ReportesLN.getInstancia().ReportesPorUsuario(login, tipo, 1);
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.StatusCode = 500;

                if (HttpContext.Current.Session["Usuario"] != null)
                {
                    throw new Exception("Se ha producido un error al cargar los reportes ( " + ex.Message + " )", ex);
                }
                else
                {
                    throw new Exception("Su sesion ha expirado, sera redireccionado a la página principal");
                }
            }

            return lista;

        }

        [WebMethod]
        public static List<Subdirectorio> CargarSubdirectorios(string cod_directorio)
        {
            List<Subdirectorio> listaSubdirectorios = new List<Subdirectorio>();

            int codigoDirectorio = Int32.Parse(cod_directorio);

            try
            {
                //********** DropDownList Cantones
                listaSubdirectorios = ReportesLN.getInstancia().CargarSubdirectorios(codigoDirectorio);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return listaSubdirectorios;

        }

        [WebMethod]
        public static bool MantenimientoReportes(string accion, string id, string codigo, string nombre, string archivoRpt, string cod_sistema, string cod_directorio, string cod_subdirectorio, string ubicacion, string cod_estado, string cod_usuario, string cod_propietario)
        {
            bool resultado;
            try
            {
                int id_usuario = Int32.Parse(cod_usuario);

                //Instancia de reporte
                CP_Entidades.Reporte reporte = new CP_Entidades.Reporte()
                {
                    cod_reporte = id,
                    cod_alterno = codigo,
                    nombre = nombre,
                    nombre_archivo = archivoRpt,
                    cod_sistema = cod_sistema,
                    cod_directorio = cod_directorio,
                    cod_subdirectorio = cod_subdirectorio,
                    ubicacion = ubicacion,
                    estado = cod_estado,
                    propietario = cod_propietario
                };

                if (accion == "C")
                {
                    // Crear reporte  
                    resultado = ReportesLN.getInstancia().MantenimientoReportes(reporte, "C");

                    if (resultado)
                    {
                        //*** Auditoría
                        Auditoria(id_usuario, 13, "Crear reporte - " + reporte.cod_alterno);
                    }
                }
                else if (accion == "U")
                {
                    // Actualizar reporte  
                    resultado = ReportesLN.getInstancia().MantenimientoReportes(reporte, "U");
                    if (resultado)
                    {
                        //*** Auditoría
                        Auditoria(id_usuario, 14, "Editar reporte - " + reporte.cod_alterno);
                    }
                }
                else if (accion == "D")
                {
                    //Inactivar reporte  
                    resultado = ReportesLN.getInstancia().MantenimientoReportes(reporte, "D");

                    if (resultado)
                    {
                        //*** Auditoría
                        Auditoria(id_usuario, 14, "Eliminar reporte - " + reporte.cod_alterno);
                    }

                }
                else
                {
                    resultado = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return resultado;
        }

        //Auditoría
        public static void Auditoria(int id_usuario, int id_proceso, string detalle)
        {
            try
            {
                //*** Auditoría
                int id = id_usuario;
                int cod_proceso = id_proceso;
                string descripcion = detalle;

                Proceso proceso = new Proceso
                {
                    id_usuario = id,
                    cod_proceso = cod_proceso,
                    descripcion = descripcion
                };

                ProcesoLN.getInstancia().AuditarProceso(proceso, System.Web.HttpContext.Current);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}