using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CP_Entidades;
using CP_LogicaNegocio;
using Microsoft.Reporting.WebForms;
using System.Net;
using System.Security.Cryptography;
//Borrar
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Services;

namespace CP_Presentacion
{
    public partial class Inicio : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Usuario"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                if (!Page.IsPostBack)
                {
                    try
                    {
                        //Navegador Web
                        Session["navegador"] = VerificarNavegador();
                    }
                    catch (Exception ex)
                    {
                        string titulo = "Report Manager";
                        string mensaje = "Contacte al administrador del sistema, " + ex.Message.Replace("'", "\""); ;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "mensajeError('" + mensaje + "', '" + titulo + "');", true);
                    }
                }
            }
        }


        //****************************************************************** Actualizar Claves
        [WebMethod]
        public static bool ActualizarClave(String login, String id, String clave, String sistema)
        {
            bool resultado = false;
            int cod_proceso = 0;

            try
            {
                int id_usuario = Int32.Parse(id);

                string descripcion = "Actualizó clave de " + sistema;

                if (sistema == "CPC")
                {
                    //Auditoría
                    cod_proceso = 4;

                    //Instancia de Usuario
                    CPC usuarioCPC = new CPC
                    {
                        usuario = login,
                        clave = clave
                    };

                    //Actualizar Usuario  CPC 
                    resultado = CPCLN.getInstancia().ActualizarClave(usuarioCPC);
                }
                else if (sistema == "REPORT MANAGER")
                {
                    //Auditoría
                    cod_proceso = 2;

                    Encriptar encriptar = new Encriptar();

                    //Instancia de Usuario
                    Usuario usuario = new Usuario
                    {
                        cod_usuario = id_usuario,
                        clave = encriptar.Crypto(clave)
                    };



                    //Actualizar Usuario   REPORT MANAGER
                    resultado = UsuarioLN.getInstancia().MantenimientoUsuario(usuario, "U");
                }
                else if (sistema == "SIC" || sistema == "CENDEISSS")
                {
                    //Encriptar
                    Encriptar encriptar = new Encriptar();
                    //Instancia de Usuario
                    Usuario usuarioSIC = new Usuario
                    {
                        login = login,
                        clave = encriptar.CryptoSIC(clave)
                    };

                    if (sistema == "SIC")
                    {
                        //Auditoría
                        cod_proceso = 3;

                        //Actualizar Usuario  SIC
                        resultado = UsuarioLN.getInstancia().ActualizarClaveSIC(usuarioSIC);
                    }
                    else
                    {
                        //Auditoría
                        cod_proceso = 6;
                        //Actualizar Usuario  CENDEISSS
                        resultado = UsuarioLN.getInstancia().ActualizarClaveCendeisss(usuarioSIC);

                    }
                }

                if (resultado)
                {
                    //Auditoría
                    Proceso proceso = new Proceso
                    {
                        id_usuario = id_usuario,
                        cod_proceso = cod_proceso,
                        descripcion = descripcion
                    };

                    ProcesoLN.getInstancia().AuditarProceso(proceso, System.Web.HttpContext.Current);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return resultado;
        }

        public string VerificarNavegador()
        {
            System.Web.HttpBrowserCapabilities browser = Request.Browser;

            string navegador = browser.Browser + " " + browser.Version;

            return navegador;
        }

    } // Partial Class END 
} // namespace END