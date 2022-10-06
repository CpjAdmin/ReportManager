using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CP_Entidades;
using CP_LogicaNegocio;
using System.Web.Services;

namespace CP_Presentacion
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            usuario.Focus();

            if (!string.IsNullOrEmpty((string)Session["Usuario"])) 
            {
                Session.Clear();
            }
        }

        protected void btnIngresar_Click(object sender, EventArgs e)
        {
            try
            {

                // Navegador
                string navegador = VerificarNavegador();
                Session["navegador"] = navegador;

                if (Page.IsValid)
                {
                    Encriptar encriptar = new Encriptar();
                    Usuario ObjUsuario = UsuarioLN.getInstancia().AccesoSistema(usuario.Value, encriptar.Crypto(password.Value)); 

                    if (ObjUsuario != null)
                    {
                        string NombreUsuario = ObjUsuario.nombre;
                        string DepartamentoUsuario = ObjUsuario.departamento.nombre;
                        int Cod_Usuario = ObjUsuario.cod_usuario;
                        string Rol = ObjUsuario.rol.nombre;

                        Session["NombreUsuario"] = NombreUsuario;
                        Session["DepartamentoUsuario"] = DepartamentoUsuario;
                        Session["Usuario"] = usuario.Value;
                        Session["ID"] = Cod_Usuario;
                        Session["Rol"] = Rol;

                        // Auditoria de Inicio de Sesión
                        UsuarioLN.getInstancia().AuditoriaSesion(ObjUsuario, navegador);

                        // Redirect a Inicio.aspx
                        Response.Redirect("~/Inicio.aspx", false);
                    }
                    else
                    {
                       string titulo = "Inicio de Sesión";
                       string mensaje = "El usuario o la contraseña ingresada es incorrecta.<br><br><b>RECUPERAR CONTRASEÑA :</b> Envía la contraseña del usuario al correo.<br><b>CERRAR :</b> Cerrar la ventana y volver a intentar.<br>";
                       ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "mensajeConfirmLogin('" + mensaje + "', '" + titulo + "','bootstrap','red','fa-warning'); ", true);
                    }
                }
            }
            catch(Exception ex) {

                string titulo = "Report Manager";
                string mensaje = "Contacte al administrador del sistema, " + ex.Message.Replace("'", "\"");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "mensajeError('" + mensaje + "', '" + titulo + "');", true);
            }
        }

        [WebMethod]
        public static String EnviarClaveEmail(String usuario)
        {
            string clave     = "";
            String resultado;
            bool resultadoEnvio = false;

            try
            {
                clave = UsuarioLN.getInstancia().verificarUsuarioRM(usuario,"V");

                if (clave != null)
                {
                    string navegador = (System.Web.HttpContext.Current.Session["navegador"].ToString());

                    resultadoEnvio = UsuarioLN.getInstancia().enviarClaveEmail(usuario, clave, navegador, 1);

                    if (resultadoEnvio)
                    {
                        resultado = "Enviado";

                        //*** Auditoría
                        Auditoria(0, 15, usuario);

                    }
                    else
                    {
                        resultado = "ErrorEnvio";
                    }
                }
                else
                {
                    resultado = "ErrorUsuario";
                }
            }
            catch (Exception ex)
            {
                resultado = ex.Message;
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

