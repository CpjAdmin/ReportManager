using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CP_Entidades;
using CP_LogicaNegocio;
using System.Drawing;
using System.Web.Services;
using System.IO;

namespace CP_Presentacion
{
    public partial class Registro_Usuarios : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Usuario"] == null || Session["Rol"].ToString() != "ADMINISTRADOR")
            {
                Response.Redirect("~/Login.aspx", true);
            }

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

                    //Define los valores Iniciales
                    valoresIniciales();
                }
                catch (Exception ex)
                {

                    string titulo = Path.GetFileName(Request.Url.AbsolutePath);
                    string mensaje = ex.Message;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "mensajeError('" + mensaje + "', '" + titulo + "');", true);
                }

            }
        }

        public void CargarComponentes(int id_usuario)
        {
            bool agregarUsuario;
            bool editarUsuario;

            try
            {
                // Menú de Perfiles
                agregarUsuario = ProcesoLN.getInstancia().VerificarPermiso(id_usuario, "btn_Usuarios_Agregar", "");
                editarUsuario = ProcesoLN.getInstancia().VerificarPermiso(id_usuario, "btn_Usuarios_Editar", "");

                btn_Usuarios_Agregar.Visible = agregarUsuario;

                btn_Usuarios_Editar.Visible = editarUsuario;
                FnEditar.Visible = editarUsuario;

            }
            catch (Exception)
            {
                throw;
            }
        }

        private Usuario GetUsuario()
        {
            Encriptar encriptar = new Encriptar();

            Usuario usuario = new Usuario
            {
                cod_usuario = 0,
                nombre = txtNombre.Text
            };
            usuario.departamento.cod_departamento = Int32.Parse(ddlDepartamento.SelectedValue);
            usuario.sucursal.cod_sucursal = Int32.Parse(ddlSucursal.SelectedValue);
            usuario.rol.cod_rol = Int32.Parse(ddlRol.SelectedValue);
            usuario.foto = "";
            usuario.login = txtUsuario.Text;
            usuario.clave = encriptar.Crypto(txtClave.Text.Trim());

            return usuario;
        }

        private void CargarControles()
        {
            ListItem[] items;

            //********** DropDownList Departamento
            ddlDepartamento.DataSource = DepartamentoLN.getInstancia().GetDepartamentos();
            ddlDepartamento.DataTextField = "nombre";
            ddlDepartamento.DataValueField = "cod_departamento";
            ddlDepartamento.DataBind();

            //DropDownList MODAL Departamento
            items = new ListItem[ddlDepartamento.Items.Count];
            ddlDepartamento.Items.CopyTo(items, 0);
            ModalDdlDepartamento.Items.AddRange(items);

            //********** DropDownList Sucursal
            ddlSucursal.DataSource = SucursalLN.getInstancia().GetSucursales();
            ddlSucursal.DataTextField = "nombre";
            ddlSucursal.DataValueField = "cod_sucursal";
            ddlSucursal.DataBind();

            //DropDownList MODAL Sucursal
            items = new ListItem[ddlSucursal.Items.Count];
            ddlSucursal.Items.CopyTo(items, 0);
            ModalDdlSucursal.Items.AddRange(items);

            //********** DropDownList Rol
            ddlRol.DataSource = RolLN.getInstancia().GetRoles();
            ddlRol.DataTextField = "nombre";
            ddlRol.DataValueField = "cod_rol";
            ddlRol.DataBind();

            //DropDownList MODAL Rol
            items = new ListItem[ddlRol.Items.Count];
            ddlRol.Items.CopyTo(items, 0);
            ModalDdlRol.Items.AddRange(items);

            //Limpiar Campos
            txtUsuario.Text = "";
            txtNombre.Text = "";
            txtUsuario.Text = "";

        }

        // *** Valores Iniciales
        private void valoresIniciales()
        {
            ddlRol.SelectedValue = "2";
            ddlDepartamento.SelectedValue = "1";
            ddlSucursal.SelectedValue = "1";
            txtNombre.Text = "";
            txtUsuario.Text = "";
            txtClave.Text = "";
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Inicio.aspx", true);
        }

        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                //Cod Usuario
                int id_usuario = Int32.Parse(Session["ID"].ToString());

                //Registro de Usuario
                Usuario usuario = GetUsuario();

                //Enviar Usuario a Capa de Negocio
                bool resultado = UsuarioLN.getInstancia().MantenimientoUsuario(usuario, "C");
                if (resultado)
                {
                    valoresIniciales();

                    //*** Auditoría
                    Auditoria(id_usuario, 18, "Crear - " + usuario.login);
                }
            }
            catch (Exception ex)
            {
                string titulo = "ALERTA - REGISTRO DE USUARIOS";
                string mensaje = "Contacte al administrador del sistema...! - " + ex.Message;
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "Alerta('mensajesAlerta', 'La Contraseña no se actualizó, contacte al administrador del sistema!.', 'danger', 2000);", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "mensajeError('" + mensaje + "', '" + titulo + "');", true);
            }
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

        [WebMethod]
        public static bool ActualizarUsuario(String id, String cod_usuario_actualzar, String login_actualizar,  String nombre, String cod_sucursal, String cod_departamento, String cod_rol, String clave, String i_estado)
        {
            bool resultado;

            try
            {
                int id_usuario = Int32.Parse(id);
                int cod_usuario = Int32.Parse(cod_usuario_actualzar);

                Encriptar encriptar = new Encriptar();
                //Instancia de Usuario
                Usuario usuario = new Usuario
                {
                    cod_usuario = cod_usuario,
                    nombre = nombre
                };

                usuario.sucursal.cod_sucursal = Int32.Parse(cod_sucursal);
                usuario.departamento.cod_departamento = Int32.Parse(cod_departamento);
                usuario.rol.cod_rol = Int32.Parse(cod_rol);
                usuario.clave = clave.Trim() == "" ? "" : encriptar.Crypto(clave);
                usuario.i_estado = i_estado;

                //Actualizar Usuario  
                resultado = UsuarioLN.getInstancia().MantenimientoUsuario(usuario, "U");

                if (resultado)
                {
                    //*** Auditoría
                    Auditoria(id_usuario, 19, "Editar - " + login_actualizar);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return resultado;
        }

        [WebMethod]
        public static bool EliminarUsuario(String id, String cod_usuario, String login)
        {
            bool resultado;

            try
            {
                int id_usuario = Int32.Parse(id);
                int id_usuario_eliminar = Int32.Parse(cod_usuario);

                //Instancia de Usuario
                Usuario usuario = new Usuario
                {
                    cod_usuario = id_usuario_eliminar
                };

                //Actualizar Usuario  
                resultado = UsuarioLN.getInstancia().MantenimientoUsuario(usuario, "D");

                if (resultado)
                {
                    //*** Auditoría
                    Auditoria(id_usuario, 19, "Eliminar - " + login);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return resultado;
        }

        //Auditoría
        public static void Auditoria(int id_usuario,int id_proceso,string detalle)
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