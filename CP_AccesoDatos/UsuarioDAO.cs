using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CP_Entidades;

namespace CP_AccesoDatos
{
    public class UsuarioDAO
    {

        #region "Constructor de Usuario"
        private static UsuarioDAO Usuario = null;
        private UsuarioDAO() { }
        public static UsuarioDAO getInstancia()
        {
            if (Usuario == null)
            {
                Usuario = new UsuarioDAO();
            }
            return Usuario;
        }
        #endregion

        //AccesoSistema
        public Usuario AccesoSistema(String usuario, String clave)
        {
            SqlConnection conexion = null;
            SqlCommand cmd = null;
            SqlDataReader dr = null;
            Usuario ObjUsuario = null;

            try
            {
                conexion = Conexion.ConexionBD();
                cmd = new SqlCommand("Reportes.sp_rm_autenticacion_web", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@login",usuario);
                cmd.Parameters.AddWithValue("@clave", clave);

                //Pendiente logica para controlar error de login
                //SqlParameter validacion = cmd.Parameters.Add("@validacion", SqlDbType.Int);
                //validacion.Direction = ParameterDirection.Output;

                conexion.Open();
                dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    ObjUsuario = new Usuario();
                    ObjUsuario.departamento = new Departamento();
                    ObjUsuario.sucursal = new Sucursal();
                    ObjUsuario.rol = new Rol();

                    ObjUsuario.cod_usuario         = Int32.Parse(dr["cod_usuario"].ToString());
                    ObjUsuario.nombre              = dr["nombre"].ToString();
                    ObjUsuario.departamento.nombre = dr["departamento"].ToString();
                    ObjUsuario.sucursal.nombre     = dr["sucursal"].ToString();
                    ObjUsuario.rol.nombre          = dr["rol"].ToString();
                    ObjUsuario.foto                = dr["foto"].ToString();
                    ObjUsuario.login               = dr["login"].ToString();
                    ObjUsuario.clave               = dr["clave"].ToString();
                }
            }
            catch (Exception ex)
            {
                ObjUsuario = null;
                throw ex;
            }
            finally
            {
                conexion.Close();
            }

            return ObjUsuario;
        }
        //MantenimientoUsuario
        public bool MantenimientoUsuario(Usuario usuario,String proceso_crud)
        {
            SqlConnection conexion = null;
            SqlCommand cmd = null;
            bool resultado = false;

            //Encriptar encriptar = new Encriptar();

            try
            {
                conexion = Conexion.ConexionBD();
                cmd = new SqlCommand("reportes.sp_rm_crud_usuarios", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@cod_usuario", usuario.cod_usuario);
                cmd.Parameters.AddWithValue("@nombre", usuario.nombre);
                cmd.Parameters.AddWithValue("@cod_departamento", usuario.departamento.cod_departamento);
                cmd.Parameters.AddWithValue("@cod_sucursal", usuario.sucursal.cod_sucursal);
                cmd.Parameters.AddWithValue("@cod_rol", usuario.rol.cod_rol);
                cmd.Parameters.AddWithValue("@foto", usuario.foto);
                cmd.Parameters.AddWithValue("@login", usuario.login);
                cmd.Parameters.AddWithValue("@clave", usuario.clave); 
                //cmd.Parameters.AddWithValue("@clave", encriptar.Crypto(usuario.clave));
                cmd.Parameters.AddWithValue("@i_estado", usuario.i_estado);
                cmd.Parameters.AddWithValue("@proceso_crud", proceso_crud);

                //Pendinete logica para controlar error de login
                //SqlParameter validacion = cmd.Parameters.Add("@validacion", SqlDbType.Int);
                //validacion.Direction = ParameterDirection.Output;

                conexion.Open();
                int filas = cmd.ExecuteNonQuery();  // Poner set nocount off en el SP

                if (filas > 0)
                {
                    resultado = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conexion.Close();
            }

            return resultado;
        }

        //GetUsuarios
        public List<Usuario> GetUsuarios()
        {
            List<Usuario> lista = new List<Usuario>();

            SqlConnection conexion = null;
            SqlCommand cmd = null;
            SqlDataReader dr = null;

            String proceso_crud = "R";

            try
            {
                conexion = Conexion.ConexionBD();
                cmd = new SqlCommand("Reportes.sp_rm_crud_usuarios", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@cod_usuario", 0);
                cmd.Parameters.AddWithValue("@nombre", "");
                cmd.Parameters.AddWithValue("@cod_departamento", 0);
                cmd.Parameters.AddWithValue("@cod_sucursal", 0);
                cmd.Parameters.AddWithValue("@cod_rol", 0);
                cmd.Parameters.AddWithValue("@foto", "");
                cmd.Parameters.AddWithValue("@login", "");
                cmd.Parameters.AddWithValue("@clave", "");
                cmd.Parameters.AddWithValue("@i_estado", "");
                cmd.Parameters.AddWithValue("@proceso_crud", proceso_crud);

                conexion.Open();
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Usuario ObjUsuario = new Usuario();

                    ObjUsuario.departamento = new Departamento();
                    ObjUsuario.sucursal = new Sucursal();
                    ObjUsuario.rol = new Rol();

                    ObjUsuario.cod_usuario = Int32.Parse(dr["cod_usuario"].ToString());
                    ObjUsuario.nombre = dr["nombre"].ToString();
                    ObjUsuario.sucursal.cod_sucursal = Int32.Parse(dr["cod_sucursal"].ToString());
                    ObjUsuario.sucursal.nombre = dr["sucursal"].ToString();
                    ObjUsuario.departamento.cod_departamento = Int32.Parse(dr["cod_departamento"].ToString());
                    ObjUsuario.departamento.nombre = dr["departamento"].ToString();
                    ObjUsuario.rol.cod_rol = Int32.Parse(dr["cod_rol"].ToString());
                    ObjUsuario.rol.nombre = dr["rol"].ToString();
                    ObjUsuario.foto = dr["foto"].ToString();
                    ObjUsuario.login = dr["login"].ToString();
                    ObjUsuario.clave = dr["clave"].ToString();
                    ObjUsuario.i_estado = dr["estado"].ToString();

                    //Añadir a la lista
                    lista.Add(ObjUsuario);
                }
            }
            catch (Exception ex)
            {
                lista = null;
                throw ex;
            }
            finally
            {
                conexion.Close();
            }
            return lista;
        }

        //Cargar Usuarios para DrodDownList
        public DataTable GetUsuariosDDL()
        {
            SqlConnection conexion = null;
            SqlCommand cmd = null;
            DataTable dt = null;

            String proceso_crud = "X";

            try
            {
                conexion = Conexion.ConexionBD();
                cmd = new SqlCommand("Reportes.sp_rm_crud_usuarios", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@cod_usuario", 0);
                cmd.Parameters.AddWithValue("@nombre", "");
                cmd.Parameters.AddWithValue("@cod_departamento", 0);
                cmd.Parameters.AddWithValue("@cod_sucursal", 0);
                cmd.Parameters.AddWithValue("@cod_rol", 0);
                cmd.Parameters.AddWithValue("@foto", "");
                cmd.Parameters.AddWithValue("@login", "");
                cmd.Parameters.AddWithValue("@clave", "");
                cmd.Parameters.AddWithValue("@i_estado", "");
                cmd.Parameters.AddWithValue("@proceso_crud", proceso_crud);

                conexion.Open();
                dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                dt = null;
                throw ex;
            }
            finally
            {
                conexion.Close();
            }
            return dt;
        }

        //VerificaUsuarioSIC
        public bool VerificaUsuarioSIC(Usuario usuario,string sistema)
        {
            SqlConnection conexion = null;
            SqlCommand cmd = null;
            bool resultado = false;

            try
            {
                conexion = Conexion.ConexionBD();
                cmd = new SqlCommand("reportes.sp_rm_verificar_usuario", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@login", usuario.login);
                cmd.Parameters.AddWithValue("@sistema", sistema);

                conexion.Open();
                int filas = (int)cmd.ExecuteScalar();

                if (filas == 1)
                {
                    resultado = true;
                }
            }
            catch (Exception)
            {
                resultado = false;
            }
            finally
            {
                conexion.Close();
            }

            return resultado;
        }

        //ActualizarClaveSIC
        public bool ActualizarClaveSIC(Usuario usuarioSIC)
        {
            SqlConnection conexion = null;
            SqlCommand cmd = null;
            bool resultado = false;

            try
            {
                conexion = Conexion.ConexionBD();
                cmd = new SqlCommand("Reportes.sp_rm_actualizar_clave_sic", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@usuario", usuarioSIC.login);
                cmd.Parameters.AddWithValue("@clave", usuarioSIC.clave);

                conexion.Open();

                int filas = (int)cmd.ExecuteNonQuery();

                if (filas > 0)
                {
                    resultado = true;
                }

                return resultado;
            }
            catch (Exception)
            {
                return resultado;
            }
            finally
            {
                conexion.Close();
            }
        }

        //ActualizarClaveCendeisss
        public bool ActualizarClaveCendeisss(Usuario usuarioSIC)
        {
            SqlConnection conexion = null;
            SqlCommand cmd = null;
            bool resultado = false;

            try
            {
                conexion = Conexion.ConexionBD();
                cmd = new SqlCommand("Reportes.sp_rm_actualizar_clave_cendeisss", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@usuario", usuarioSIC.login);
                cmd.Parameters.AddWithValue("@clave", usuarioSIC.clave);

                conexion.Open();

                int filas = (int)cmd.ExecuteNonQuery();

                if (filas > 0)
                {
                    resultado = true;
                }

                return resultado;
            }
            catch (Exception)
            {
                return resultado;
            }
            finally
            {
                conexion.Close();
            }
        }

        public bool AuditoriaSesion(Usuario usuario, String navegador)
        {
            SqlConnection conexion = null;
            SqlCommand cmd = null;
            bool resultado = false;

            try
            {
                conexion = Conexion.ConexionBD();
                cmd = new SqlCommand("Reportes.sp_rm_auditoria_sesiones", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@cod_usuario", SqlDbType.VarChar).Value = usuario.cod_usuario;
                cmd.Parameters.Add("@login", SqlDbType.VarChar).Value = usuario.login.Trim();
                cmd.Parameters.Add("@nombre", SqlDbType.VarChar).Value = usuario.nombre.Trim();
                cmd.Parameters.Add("@navegador", SqlDbType.VarChar).Value = navegador;

                conexion.Open();

                int filas = (int)cmd.ExecuteNonQuery();

                if (filas > 0)
                {
                    resultado = true;
                }
            }
            catch (Exception)
            {
                resultado = false;
            }
            finally
            {
                conexion.Close();
            }

            return resultado;
        }

        public List<String> UsuariosPorPerfil(int cod_perfil, String cod_proceso)
        {
            List<String> lista = new List<String>();

            SqlConnection conexion = null;
            SqlCommand cmd = null;
            SqlDataReader dr = null;

            try
            {
                //Instancia de Conexión
                conexion = Conexion.ConexionBD();
                //Tipo y nombre del proceso a ejecutar
                cmd = new SqlCommand("reportes.sp_rm_crud_usuarios", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                // Parametros
                cmd.Parameters.AddWithValue("@cod_perfil", cod_perfil);

                cmd.Parameters.AddWithValue("@proceso_crud", cod_proceso);

                conexion.Open();
                dr = cmd.ExecuteReader();

                // Devolver todos los reportes.
                while (dr.Read())
                {
                    //Añadir a la lista
                    lista.Add(dr["cod_usuario"].ToString());
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                conexion.Close();
            }

            return lista;
        }

        public String verificaUsuarioRM(String login, String cod_proceso)
        {
            string clave;

            SqlConnection conexion = null;
            SqlCommand cmd = null;

            try
            {
                //Instancia de Conexión
                conexion = Conexion.ConexionBD();
                //Tipo y nombre del proceso a ejecutar
                cmd = new SqlCommand("reportes.sp_rm_crud_usuarios", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                // Parametros
                cmd.Parameters.AddWithValue("@login", login);

                cmd.Parameters.AddWithValue("@proceso_crud", cod_proceso);

                conexion.Open();
                clave = (string)cmd.ExecuteScalar();

                // Verificar si existe el usuario y devuelve la contraseña
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conexion.Close();
            }
            return clave;
        }

        public bool enviarClaveEmail(String login, String clave, String navegador, int cod_proceso)
        {
            bool resultado = false;

            SqlConnection conexion = null;
            SqlCommand cmd = null;

            try
            {
                //Instancia de Conexión
                conexion = Conexion.ConexionBD();
              
                //Desencriptar y Llamar al metodo enviar correo
                Encriptar encriptar = new Encriptar();

                cmd = new SqlCommand("reportes.sp_rm_recuperar_clave_email", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                // Parametros
                cmd.Parameters.AddWithValue("@login", login);
                cmd.Parameters.AddWithValue("@clave", encriptar.Decrypto(clave));
                cmd.Parameters.AddWithValue("@navegador", navegador);

                cmd.Parameters.AddWithValue("@cod_proceso", 1);

                conexion.Open();

                int filas = (int)cmd.ExecuteNonQuery();

                if (filas > 0)
                {
                    resultado = true;
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conexion.Close();
            }

            return resultado;
        }

        public bool usuarioInsertarAuditoria(int cod_proceso, int cod_usuario, String navegador, String pagina, String descripcion,String terminal_id)
        {
            bool resultado = false;

            SqlConnection conexion = null;
            SqlCommand cmd = null;

            try
            {
                //Instancia de Conexión
                conexion = Conexion.ConexionBD();

                cmd = new SqlCommand("reportes.sp_rm_auditoria_procesos", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                // Parametros
                cmd.Parameters.AddWithValue("@cod_proceso", cod_proceso);
                cmd.Parameters.AddWithValue("@cod_usuario", cod_usuario);
                cmd.Parameters.AddWithValue("@navegador", navegador);
                cmd.Parameters.AddWithValue("@pagina", pagina);
                cmd.Parameters.AddWithValue("@descripcion", descripcion);
                cmd.Parameters.AddWithValue("@terminal_id", terminal_id);

                cmd.Parameters.AddWithValue("@proceso", 1);

                conexion.Open();

                int filas = (int)cmd.ExecuteNonQuery();

                if (filas > 0)
                {
                    resultado = true;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conexion.Close();
            }

            return resultado;
        }

        public List<String> usuarioConsultarAuditoria()
        {
            List<String> lista = new List<String>();

            SqlConnection conexion = null;
            SqlCommand cmd = null;
            SqlDataReader dr = null;

            try
            {
                //Instancia de Conexión
                conexion = Conexion.ConexionBD();
                //Tipo y nombre del proceso a ejecutar
                cmd = new SqlCommand("reportes.sp_rm_auditoria_procesos", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                // Parametros
                cmd.Parameters.AddWithValue("@proceso", 2);

                conexion.Open();
                dr = cmd.ExecuteReader();

                // Devolver todos los reportes.
                while (dr.Read())
                {
                    //Añadir a la lista
                    lista.Add(dr["cod_proceso"].ToString());
                    lista.Add(dr["nombre_proceso"].ToString());
                    lista.Add(dr["login"].ToString());
                    lista.Add(dr["nombre_usuario"].ToString());
                    lista.Add(dr["pagina"].ToString());
                    lista.Add(dr["descripcion"].ToString());
                    lista.Add(dr["spid"].ToString());
                    lista.Add(dr["navegador"].ToString());
                    lista.Add(dr["fecha_ejecucion"].ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conexion.Close();
            }

            return lista;
        }



    }
}
