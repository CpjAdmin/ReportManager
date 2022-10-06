using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CP_Entidades;
using System.IO;
using System.Security.Principal;
using System.Configuration;
using CP_LogicaNegocio;

namespace CP_Presentacion
{
    public partial class Reporte : System.Web.UI.Page
    {
        Encriptar encriptar;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Usuario"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }

            if (!this.IsPostBack)
            {
                encriptar = new Encriptar();

                String usuarioActivo = Session["Usuario"].ToString();
                String usuarioRpt = encriptar.Decrypto(HttpUtility.UrlDecode(Request.QueryString["urpt"]));  

                if (!usuarioActivo.Equals(usuarioRpt))
                {
                    string titulo = Path.GetFileName(Request.Url.AbsolutePath);
                    string mensaje = "El usuario " + usuarioActivo + " no está autorizado para ejecutar este reporte!";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "mensajeError('" + mensaje + "', '" + titulo + "');", true);
                }
                else
                {
                    String nombreReporte = HttpUtility.UrlDecode(Request.QueryString["nrpt"]);
                    String id = HttpUtility.UrlDecode(Request.QueryString["irpt"]);

                    reporte_ServerClick(id, nombreReporte);
                }
            }
        }

        //Cargar del Reporte
        protected void reporte_ServerClick(String path, String nombreReporte)
        {
            try
            {
                if (!String.IsNullOrEmpty(path))
                {
                    //Configurar Nombre del Reporte
                    txtNombreReporte.Text = nombreReporte;

                    // ReportServer 
                    string ReportServer = ConfigurationManager.AppSettings["ReportServer"];

                    if (string.IsNullOrEmpty(ReportServer))
                    {
                        throw new Exception("Falta el ReportServer del archivo web.config");
                    }

                    ReportViewer1.ServerReport.ReportServerUrl = new Uri(ReportServer); 
                    ReportViewer1.ServerReport.ReportPath = path;
                    ReportViewer1.ShowCredentialPrompts = false;
                    IReportServerCredentials credenciales = new CustomReportCredentials();
                    ReportViewer1.ServerReport.ReportServerCredentials = credenciales;

                    try
                    {
                        string usuarioRM = Session["Usuario"].ToString();
                        ReportParameter usuarioParam = new ReportParameter("Usuario", usuarioRM, false);
                        ReportViewer1.ServerReport.SetParameters(new ReportParameter[] { usuarioParam });
                    }
                    catch (Exception)
                    {

                    }
                    //Visible
                    ReportViewer1.Visible = true;
                    //Estilo del ReportViewer
                    styleReportViewer();
                    //ReportViewer1.DataBind();
                    ReportViewer1.ServerReport.Refresh();

                    //Auditoría
                    int id_usuario  = (int)Session["ID"]; 
                    int cod_proceso = 28;
                    string descripcion = nombreReporte;

                    Proceso proceso = new Proceso
                    {
                        id_usuario = id_usuario,
                        cod_proceso = cod_proceso,
                        descripcion = descripcion
                    };

                    ProcesoLN.getInstancia().AuditarProceso(proceso, System.Web.HttpContext.Current);
                }
                else
                {
                    string titulo = Path.GetFileName(Request.Url.AbsolutePath);
                    string mensaje = "Error al cargar el reporte!";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "mensajeError('" + mensaje + "', '" + titulo + "');", true);
                }
            }
            catch (Exception ex)
            {
                string titulo = Path.GetFileName(Request.Url.AbsolutePath);
                string mensaje = ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "mensajeError('" + mensaje + "', '" + titulo + "');", true);
            }
        }

        //Estilo del ReportViewer
        public void styleReportViewer()
        {
            //Procesar el Reporte
            ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
            // Style Color
            ReportViewer1.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
            ReportViewer1.ForeColor = System.Drawing.ColorTranslator.FromHtml("#000000");
            ReportViewer1.SplitterBackColor = System.Drawing.ColorTranslator.FromHtml(htmlColor: "#004680");
            ReportViewer1.Height = Unit.Percentage(100);
            ReportViewer1.Width = Unit.Percentage(100);
            // Opciones
            ReportViewer1.SizeToReportContent = true;
            // Style Zoom
            ReportViewer1.ZoomMode = ZoomMode.Percent;
            ReportViewer1.ZoomPercent = 90;
        }

        // Implementación de CustomReportCredentials (Serializble para convertir el objeto en una secuencia de bytes y conservarlo en memoria)
        [Serializable]
        public class CustomReportCredentials : IReportServerCredentials
        {
            public WindowsIdentity ImpersonationUser
            {
                get { return null; }
            }
            public ICredentials NetworkCredentials
            {

                get {

                    // Usuario
                    string usuario = ConfigurationManager.AppSettings["ReportViewerUsuario"];

                    if (string.IsNullOrEmpty(usuario))
                        throw new Exception(
                            "Falta el usuario del archivo web.config");
                    // Clave
                    string clave   = ConfigurationManager.AppSettings["ReportViewerClave"];

                    if (string.IsNullOrEmpty(clave))
                        throw new Exception(
                            "Falta la contraseña del archivo web.config");
                    // Dominio
                    string dominio = ConfigurationManager.AppSettings["ReportViewerDominio"];

                    if (string.IsNullOrEmpty(dominio))
                        throw new Exception(
                            "Falta el dominio del archivo web.config");

                    return new NetworkCredential(usuario, clave, dominio);
                }
            }
            public bool GetFormsCredentials(out Cookie authCookie, out string user,
             out string password, out string authority)
            {
                authCookie = null;
                user = password = authority = null;

                // No se usan credenciales de formulario
                return false;
            }
        }


    } // Partial Class END 
} // namespace END
