using CP_Entidades;
using CP_LogicaNegocio;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;

namespace CP_Presentacion
{
    public partial class Estados_de_Cuenta_TD : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Usuario"] == null || (bool)Session["EstadoCuentaTD"] == false)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                if (!Page.IsPostBack)
                {
                    try
                    {
                        //*** Cargar Controles
                        CargarControles();
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

        //********** Cargar Discos
        private ArrayList GetDiscos()
        {
            DriveInfo[] infoDiscos = DriveInfo.GetDrives();
            ArrayList ListaDiscos = new ArrayList();

            foreach (DriveInfo d in infoDiscos)
            {
                if (d.IsReady == true && d.TotalFreeSpace > 0)
                {
                    if (d.DriveType.ToString() == "Fixed" || d.DriveType.ToString() == "Network")
                    {
                        ListaDiscos.Add(d.Name);
                    }
                }
            }

            //** Reversar el Orden
            ListaDiscos.Reverse();

            return ListaDiscos;
        }

        //********** DropDownList Provincias
        private void CargarProvincias()
        {
            ddlProvincia.DataSource = ProvinciaLN.getInstancia().GetProvincias();
            ddlProvincia.DataTextField = "nombre";
            ddlProvincia.DataValueField = "cod_provincia";
            ddlProvincia.DataBind();
        }

        //********** DropDownList Centros
        private void CargarCentros()
        {
            ddlCentro.DataSource = CentroLN.getInstancia().GetCentros();
            ddlCentro.DataTextField = "nombre";
            ddlCentro.DataValueField = "cod_centro";
            ddlCentro.DataBind();
        }

        //********** Cargar Controles
        private void CargarControles()
        {
            //DropDownList Modal Discos
            ddlDisco.DataSource = GetDiscos();
            ddlDisco.DataBind();

            //********** DropDownList Provincias
            CargarProvincias();

            //********** DropDownList Centros
            CargarCentros();
        }

        // WebMethod - Cargar Cantones
        [WebMethod]
        public static List<Canton> CargarCantones(string provinciaId)
        {
            List<Canton> listaCantones = new List<Canton>();

            //********** DropDownList Cantones
            listaCantones = CantonLN.getInstancia().GetCantones(provinciaId);

            return listaCantones;
        }

        // WebMethod - Cargar Distritos
        [WebMethod]
        public static List<Distrito> CargarDistritos(string provinciaId, string cantonId)
        {
            List<Distrito> listaDistritoes = new List<Distrito>();

            //********** DropDownList Distritoes
            listaDistritoes = DistritoLN.getInstancia().GetDistritos(provinciaId, cantonId);

            return listaDistritoes;
        }

        // WebMethod - Cargar Instituciones
        [WebMethod]
        public static List<Institucion> CargarInstituciones(string centroId)
        {
            List<Institucion> listaInstituciones = new List<Institucion>();

            //********** DropDownList Cantones
            listaInstituciones = InstitucionLN.getInstancia().GetInstituciones(centroId);

            return listaInstituciones;
        }

        // WebMethod - Cargar LugaresTrabajo
        [WebMethod]
        public static List<LugarTrabajo> CargarLugaresTrabajo(string centroId, string institucionId)
        {
            List<LugarTrabajo> listaLugarTrabajo = new List<LugarTrabajo>();

            //********** DropDownList Distritoes
            listaLugarTrabajo = LugarTrabajoLN.getInstancia().GetLugarTrabajo(centroId, institucionId);

            return listaLugarTrabajo;
        }

    }
}

