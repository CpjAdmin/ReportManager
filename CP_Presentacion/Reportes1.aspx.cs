using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CP_Presentacion
{
    public partial class Reportes : System.Web.UI.Page
    {
        string msgError = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Usuario"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }

            if (!this.IsPostBack)
            {
                String nombreReporte = Request.QueryString["nombreReporte"];
                String path = Request.QueryString["path"];

                reporte_ServerClick(path, nombreReporte);
            }
        }

        //Cargar del Reporte
        protected void reporte_ServerClick(String path, String nombreReporte)
        {
            try
            {
                //string idReporte = (string)Request.QueryString["id"];

                //Reporte ObjReportes = ReportesLN.getInstancia().GetReportes(Session["Usuario"].ToString(), cod_proceso);
                if (path != "")
                {
                    //Configurar Nombre del Reporte
                    txtNombreReporte.Text = nombreReporte;


                    ReportViewer1.ServerReport.ReportServerUrl = new Uri("http://172.28.39.138/SC_ReportServer");
                    ReportViewer1.ServerReport.ReportPath = path;

                    //ReportViewer1.ZoomMode = ZoomMode.FullPage;
                    ReportViewer1.ShowCredentialPrompts = false;
                    IReportServerCredentials credenciales = new CustomReportCredentials("asalazar", "HOUgee3690201703", "COOPECAJA");
                    ReportViewer1.ServerReport.ReportServerCredentials = credenciales;

                    /* 
                    // Parametro Usuario
                    string usuarioSM = Session["Usuario"].ToString();
                    ReportParameter userParam = new ReportParameter("Usuario", usuarioSM, true);
                    ReportViewer1.ServerReport.SetParameters(new ReportParameter[] { userParam });
                    */

                    ReportViewer1.Visible = true;

                    //Estilo del ReportViewer
                    styleReportViewer();

                    //ReportViewer1.DataBind();
                    ReportViewer1.ServerReport.Refresh();

                }
                else
                {
                    Response.Write("<script>alert('El Usuario no tiene reportes asociados!...!')</script>");
                }
            }
            catch (Exception ex)
            {
                msgError = "*** Contacte al administrador de la aplicación. *** \\r\\n ";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showError", "alert('!!!  ATENCIÓN  !!! \\r\\n " + ex.Message + " \\r\\n " + msgError + "');", true);

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
            ReportViewer1.ZoomPercent = 100;
        }

        // Implementación de CustomReportCredentials
        public class CustomReportCredentials : IReportServerCredentials
        {
            private string _UserName;
            private string _PassWord;
            private string _DomainName;

            public CustomReportCredentials(string UserName, string PassWord, string DomainName)
            {
                _UserName = UserName;
                _PassWord = PassWord;
                _DomainName = DomainName;
            }

            public System.Security.Principal.WindowsIdentity ImpersonationUser
            {
                get { return null; }
            }

            public ICredentials NetworkCredentials
            {
                get { return new NetworkCredential(_UserName, _PassWord, _DomainName); }
            }

            public bool GetFormsCredentials(out Cookie authCookie, out string user,
             out string password, out string authority)
            {
                authCookie = null;
                user = password = authority = null;
                return false;
            }
        }


    } // Partial Class END 
} // namespace END
