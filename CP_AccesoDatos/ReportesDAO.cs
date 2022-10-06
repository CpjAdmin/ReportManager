using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CP_Entidades;
using System.Web;

namespace CP_AccesoDatos
{
    public class ReportesDAO
    {
        #region "Constructor de Reportes"
        private static ReportesDAO Reportes = null;
        private ReportesDAO() { }
        public static ReportesDAO getInstancia()
        {
            if (Reportes == null)
            {
                Reportes = new ReportesDAO();
            }
            return Reportes;
        }
        #endregion

        public List<Reporte> CargarReportes(int cod_perfil, String listaReportes, String cod_proceso)
        {
            List<Reporte> lista = new List<Reporte>();

            SqlConnection conexion = null;
            SqlCommand cmd = null;
            SqlDataReader dr = null;

            try
            {
                //Instancia de Conexión
                conexion = Conexion.ConexionBD();
                //Tipo y nombre del proceso a ejecutar
                cmd = new SqlCommand("reportes.sp_rm_crud_reportes", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                
                // Parametros
                cmd.Parameters.AddWithValue("@cod_perfil", cod_perfil);
                cmd.Parameters.AddWithValue("@listaReportes", listaReportes);

                cmd.Parameters.AddWithValue("@proceso_crud", cod_proceso);

                conexion.Open();
                dr = cmd.ExecuteReader();

                // Devolver todos los reportes.
                while (dr.Read())
                {
                    Reporte ObjReportes = new Reporte()
                    { 
                        cod_reporte = dr["cod_reporte"].ToString(),
                        cod_alterno = dr["cod_alterno"].ToString(),
                        nombre = dr["nombre"].ToString(),
                        nombre_archivo = dr["nombre_archivo"].ToString(),
                        propietario = dr["propietario"].ToString(),
                        ubicacion = dr["ubicacion"].ToString(),
                        sistema = dr["sistema"].ToString(),
                        estado = dr["estado"].ToString()
                    };

                    //Añadir a la lista
                    lista.Add(ObjReportes);
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

        public List<String> ReportesPorPerfil(int cod_perfil, String cod_proceso)
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
                cmd = new SqlCommand("reportes.sp_rm_crud_reportes", conexion);
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
                    lista.Add(dr["cod_reporte"].ToString());
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

        public List<Reporte> ReportesPorUsuario( String login, String tipo, int cod_proceso)
        {
            List<Reporte> lista = new List<Reporte>();

            SqlConnection conexion = null;
            SqlCommand cmd = null;
            SqlDataReader dr = null;

            string usuarioRpt;
            string urlReporte = "";
            Encriptar encriptar;


            try
            {
                //Instancia de Conexión
                conexion = Conexion.ConexionBD();
                //Tipo y nombre del proceso a ejecutar
                cmd = new SqlCommand("reportes.sp_rm_cargar_reportes_por_usuario", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                // Parametros
                cmd.Parameters.AddWithValue("@login", login);
                cmd.Parameters.AddWithValue("@tipo", tipo);
                cmd.Parameters.AddWithValue("@cod_proceso", cod_proceso);

                conexion.Open();
                dr = cmd.ExecuteReader();

                // Usuario Encriptado
                encriptar = new Encriptar();
                usuarioRpt = HttpUtility.UrlEncode(encriptar.Crypto(login));

                // Devolver todos los reportes.
                while (dr.Read())
                {
                    //Ubicación Encriptada
                    string id = HttpUtility.UrlEncode(dr["ubicacion"].ToString());
                    string nombreReporteEncript = HttpUtility.UrlEncode(dr["cod_alterno"].ToString() + " - " + dr["nombre"].ToString());

                    //URL Encriptada
                    urlReporte = "Reporte.aspx?irpt=" + id + "&nrpt=" + nombreReporteEncript + "&urpt=" + usuarioRpt;

                    Reporte ObjReportes = new Reporte()
                    {
                        cod_reporte = dr["cod_reporte"].ToString(),
                        cod_alterno = dr["cod_alterno"].ToString(),
                        nombre = dr["nombre"].ToString(),
                        nombre_archivo = dr["nombre_archivo"].ToString(),
                        descripcion = dr["descripcion"].ToString(),
                        propietario = dr["propietario"].ToString(),
                        ubicacion = dr["ubicacion"].ToString(),
                        directorio = dr["directorio"].ToString(),
                        subdirectorio = dr["subdirectorio"].ToString(),
                        sistema = dr["sistema"].ToString(),
                        estado = dr["estado"].ToString(),
                        url = urlReporte
                    };

                    //Añadir a la lista
                    lista.Add(ObjReportes);
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

        public bool MantenimientoReportes(Reporte reporte, String cod_proceso)
        {
            SqlConnection conexion = null;
            SqlCommand cmd = null;

            bool resultado = false;

            try
            {
                //Instancia de Conexión
                conexion = Conexion.ConexionBD();
                //Tipo y nombre del proceso a ejecutar
                cmd = new SqlCommand("reportes.sp_rm_crud_reportes", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                // Parametros
                cmd.Parameters.AddWithValue("@cod_reporte", reporte.cod_reporte);
                cmd.Parameters.AddWithValue("@cod_alterno", reporte.cod_alterno);
                cmd.Parameters.AddWithValue("@nombre", reporte.nombre);
                cmd.Parameters.AddWithValue("@nombre_archivo", reporte.nombre_archivo);
                cmd.Parameters.AddWithValue("@cod_directorio", Int32.Parse(reporte.cod_directorio));
                cmd.Parameters.AddWithValue("@cod_subdirectorio", Int32.Parse(reporte.cod_subdirectorio));
                cmd.Parameters.AddWithValue("@cod_sistema", Int32.Parse(reporte.cod_sistema));
                cmd.Parameters.AddWithValue("@descripcion", reporte.descripcion);
                cmd.Parameters.AddWithValue("@ubicacion", reporte.ubicacion);
                cmd.Parameters.AddWithValue("@sistema", reporte.sistema);
                cmd.Parameters.AddWithValue("@i_estado", reporte.estado);
                cmd.Parameters.AddWithValue("@cod_propietario",Int32.Parse(reporte.propietario));

                cmd.Parameters.AddWithValue("@proceso_crud", cod_proceso);

                //Paramtero de Salida @msg
                cmd.Parameters.Add("@msg", SqlDbType.VarChar,200).Direction = ParameterDirection.Output;

                conexion.Open();

                int filas = cmd.ExecuteNonQuery();  // Poner set nocount off en el SP

                String mensaje = cmd.Parameters["@msg"].Value.ToString();

                // Mensaje de Salida
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

        public DataTable CargarTreeview(int cod_usuario, String tipo_directorio, int codigo, String proceso)
        {
            DataTable tabla = new DataTable();

            SqlConnection conexion = null;
            SqlCommand cmd = null;
            SqlDataReader dr = null;

            try
            {
                //Instancia de Conexión
                conexion = Conexion.ConexionBD();
                //Tipo y nombre del proceso a ejecutar
                cmd = new SqlCommand("reportes.sp_rm_treeview", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                // Parametros
                cmd.Parameters.AddWithValue("@cod_usuario", cod_usuario);
                cmd.Parameters.AddWithValue("@tipo_directorio", tipo_directorio);
                cmd.Parameters.AddWithValue("@codigo", codigo);

                conexion.Open();
                dr = cmd.ExecuteReader();

                // Devolver todos los reportes.
                if (proceso == "Directorio")
                {
                    tabla.Columns.Add(new DataColumn("dir"));
                    tabla.Columns.Add(new DataColumn("nombre"));

                    while (dr.Read())
                    {

                        //Añadir a la lista
                        tabla.Rows.Add(dr["dir"].ToString(), dr["nombre"].ToString());
                    }
                }
                else
                {
                    tabla.Columns.Add(new DataColumn("cod_reporte"));
                    tabla.Columns.Add(new DataColumn("cod_alterno"));
                    tabla.Columns.Add(new DataColumn("nombre"));
                    tabla.Columns.Add(new DataColumn("ubicacion"));
                    tabla.Columns.Add(new DataColumn("i_estado"));

                    while (dr.Read())
                    {
                        //Añadir a la lista
                        tabla.Rows.Add(dr["cod_reporte"].ToString(), dr["cod_alterno"].ToString(), dr["nombre"].ToString(), dr["ubicacion"].ToString(), dr["i_estado"].ToString());
                    }
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

            return tabla;
        }

        //Cargar Sistemas
        public DataTable CargarSistemas()
        {
            SqlConnection conexion = null;
            SqlCommand cmd = null;
            DataTable dt = null;

            try
            {
                conexion = Conexion.ConexionBD();
                cmd = new SqlCommand("Reportes.sp_rm_cargar_ubicaciones", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@cod_directorio", 0);
                cmd.Parameters.AddWithValue("@cod_subdirectorio", 0);
                cmd.Parameters.AddWithValue("@cod_proceso", 3);

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


        //Cargar Directorios
        public DataTable CargarDirectorios()
        {
            SqlConnection conexion = null;
            SqlCommand cmd = null;
            DataTable dt = null;

            try
            {
                conexion = Conexion.ConexionBD();
                cmd = new SqlCommand("Reportes.sp_rm_cargar_ubicaciones", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@cod_directorio", 0);
                cmd.Parameters.AddWithValue("@cod_subdirectorio", 0);
                cmd.Parameters.AddWithValue("@cod_proceso", 1);

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

        //Cargar Subdirectorios
        public List<Subdirectorio> CargarSubdirectorios(int cod_directorio)
        {
            List<Subdirectorio> lista = new List<Subdirectorio>();

            SqlConnection conexion = null;
            SqlCommand cmd = null;
            SqlDataReader dr = null;

            try
            {
                conexion = Conexion.ConexionBD();
                cmd = new SqlCommand("Reportes.sp_rm_cargar_ubicaciones", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@cod_directorio", cod_directorio);
                cmd.Parameters.AddWithValue("@cod_subdirectorio", 0);
                cmd.Parameters.AddWithValue("@cod_proceso", 2);

                conexion.Open();
                dr = cmd.ExecuteReader();

                // Devolver todos los Subdirectorios.
                while (dr.Read())
                {
                    Subdirectorio ObjSubdirectorio = new Subdirectorio()
                    {
                        cod_subdirectorio = dr["codigo"].ToString(),
                        nombre = dr["nombre"].ToString()
                    };

                    //Añadir a la lista
                    lista.Add(ObjSubdirectorio);
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
