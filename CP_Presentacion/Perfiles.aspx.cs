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
    public partial class Perfiles : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Usuario"] == null || (bool)Session["Perfiles"] == false)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                if (!Page.IsPostBack)
                {
                    try
                    {
                        string usuarioActivo = Session["Usuario"].ToString();

                        // Codigo de Usuario
                        int id_usuario = (int)Session["ID"];

                        // *** Cargar Controles
                        CargarComponentes(id_usuario);

                        //*** Auditoría
                        Auditoria(id_usuario, 27, "Ingreso - Perfiles");

                    }
                    catch (Exception ex)
                    {
                        string titulo = Path.GetFileName(Request.Url.AbsolutePath);
                        string mensaje = ex.Message;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "mensajeError('" + mensaje + "', '" + titulo + "');", true);
                    }

                }
            }
        }

        public void CargarComponentes(int id_usuario)
        {
            bool agregarPerfil;
            bool editarPerfil;

            try
            {
                // Menú de Perfiles
                agregarPerfil = ProcesoLN.getInstancia().VerificarPermiso(id_usuario, "btn_Perfiles_Agregar", "");
                editarPerfil = ProcesoLN.getInstancia().VerificarPermiso(id_usuario, "btn_Perfiles_Editar", "");

                btn_Perfiles_Agregar.Visible = agregarPerfil;
                btn_Perfiles_Editar.Visible = editarPerfil;

                if (!agregarPerfil)
                {
                    FnAgregar.Visible = false;
                    txtNombrePerfil.Disabled = true;
                }

                if (editarPerfil)
                {
                    btn_Perfiles_Agregar.Visible = true;
                    ddlEstado.Disabled = false;
                }
                else
                {
                    FnEditar.Visible = false;
                    ddlEstado.Value = "P";
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [WebMethod]
        public static List<Perfil> CargarPerfiles()
        {
            List<Perfil> lista = null;

            try
            {
                lista = PerfilLN.getInstancia().GetPerfiles();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lista;
        }

        [WebMethod]
        public static bool CrearPerfil(string id, string nombre, string descripcion, string estado, string[] listaReportes, string[] listaUsuarios, string login, string cod_usuario)
        {
            bool resultado;

            try
            {
                //Usuario
                int id_usuario = Int32.Parse(cod_usuario);

                //Instancia de perfil
                Perfil perfil = new Perfil
                {
                    id = Int32.Parse(id),
                    nombre = nombre,
                    descripcion = descripcion,
                    estado = estado,
                    listaReportes = string.Join(",", listaReportes),
                    listaUsuarios = string.Join(",", listaUsuarios),
                    login_creacion = login
                };

                //Crear perfil  
                resultado = PerfilLN.getInstancia().MantenimientoPerfil(perfil, "C");

                if (resultado)
                {
                    //*** Auditoría
                    Auditoria(id_usuario, 13, "Crear perfil- " + perfil.nombre);
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.StatusCode = 500;

                if (HttpContext.Current.Session["Usuario"] != null)
                {
                    throw new Exception("Se ha producido un error al crear el Perfil ( " + ex.Message + " )", ex);
                }
                else
                {
                    throw new Exception("Su sesion ha expirado, sera redireccionado a la página principal");
                }
            }
            return resultado;
        }

        [WebMethod]
        public static bool ActualizarPerfil(string id, string nombre, string descripcion, string estado, string[] listaReportes, string[] listaUsuarios, string login, string cod_usuario)
        {
            bool resultado;

            try
            {
                //Usuario
                int id_usuario = Int32.Parse(cod_usuario);

                //Instancia de perfil
                Perfil perfil = new Perfil
                {
                    id = Int32.Parse(id),
                    nombre = nombre,
                    descripcion = descripcion,
                    estado = estado,
                    listaReportes = string.Join(",", listaReportes),
                    listaUsuarios = string.Join(",", listaUsuarios),
                    login_modifica = login
                };

                //Crear perfil  
                resultado = PerfilLN.getInstancia().MantenimientoPerfil(perfil, "U");

                if (resultado)
                {
                    //*** Auditoría
                    Auditoria(id_usuario, 14, "Editar perfil- " + perfil.nombre);
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.StatusCode = 500;

                if (HttpContext.Current.Session["Usuario"] != null)
                {
                    throw new Exception("Se ha producido un error al actualizar el Perfil ( " + ex.Message + " )", ex);
                }
                else
                {
                    throw new Exception("Su sesion ha expirado, sera redireccionado a la página principal");
                }
            }
            return resultado;
        }

        [WebMethod]
        public static List<String> ReportesPorPerfil(string codigoPerfil)
        {
            List<String> lista = new List<String>();

            int cod_perfil = Int32.Parse(codigoPerfil);

            try
            {
                lista = ReportesLN.getInstancia().ReportesPorPerfil(cod_perfil, "R");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lista;

        }

        [WebMethod]
        public static List<String> UsuariosPorPerfil(string codigoPerfil)
        {
            List<String> lista = new List<String>();

            int cod_perfil = Int32.Parse(codigoPerfil);

            try
            {
                lista = UsuarioLN.getInstancia().UsuariosPorPerfil(cod_perfil, "R");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lista;

        }

        [WebMethod]
        public static List<CP_Entidades.Reporte> CargarReportes(string codigoPerfil, string listaReportes)
        {
            List<CP_Entidades.Reporte> lista = null;

            int cod_perfil = Int32.Parse(codigoPerfil);

            try
            {
                lista = ReportesLN.getInstancia().CargarReportes(cod_perfil, listaReportes, "R");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lista;

        }

        [WebMethod]
        public static List<Usuario> ListarUsuarios()
        {
            List<Usuario> lista = null;

            try
            {
                lista = UsuarioLN.getInstancia().GetUsuarios();

                //*** Actualización Masiva de Contraseñas ( Temporal )
                //for (int i = 0; i < lista.Count; i++)
                //{
                //    UsuarioLN.getInstancia().MantenimientoUsuario(lista[i], "U");
                //}
            }
            catch (Exception ex)
            {
                lista = null;
                throw ex;
            }
            return lista;
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