using CP_Entidades;
using CP_LogicaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CP_Presentacion
{
    public partial class PowerBI_Informe : System.Web.UI.Page
    {
        string msgError = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Usuario"] == null || (bool)Session["PBI"] == false)
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

                        string tagBI = Request.QueryString["t"];
                        bool pbi = ProcesoLN.getInstancia().VerificarPermiso(id_usuario, tagBI, "");

                        if (!pbi)
                        {
                            Response.Redirect("~/Inicio.aspx", false);
                        }
                        else
                        {
                            String codPBI = Request.QueryString["c"];
                            String urlBI = Request.QueryString["r"];

                            if (urlBI != null)
                            {
                                FramePBI.Src = "https://app.powerbi.com/view?r=" + urlBI;

                                //*** Auditoría
                                Auditoria(id_usuario, Int32.Parse(codPBI), "Power BI - " + codPBI);
                            }
                        }

                        
                    }
                    catch (Exception ex)
                    {
                        msgError = "*** Contacte al administrador de la aplicación. *** \\r\\n ";
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showError", "alert('!!!  ATENCIÓN  !!! \\r\\n " + ex.Message + " \\r\\n " + msgError + "');", true);
                    }
                }
            }

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