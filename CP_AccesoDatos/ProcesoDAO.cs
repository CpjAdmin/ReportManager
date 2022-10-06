using CP_Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_AccesoDatos
{
    public class ProcesoDAO
    {
        #region "Constructor de Proceso"
        private static ProcesoDAO Proceso = null;
        private ProcesoDAO() { }
        public static ProcesoDAO getInstancia()
        {
            if (Proceso == null)
            {
                Proceso = new ProcesoDAO();
            }
            return Proceso;
        }
        #endregion

        //*** VerificarPermiso
        public bool VerificarPermiso(int id_usuario, string etiqueta_id, string clase)
        {
            SqlConnection conexion = null;
            SqlCommand cmd = null;
            bool resultado = false;

            try
            {
                conexion = Conexion.ConexionBD();
                cmd = new SqlCommand("reportes.sp_rm_verifica_permisos", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@cod_usuario", id_usuario);
                cmd.Parameters.AddWithValue("@etiqueta_id", etiqueta_id);
                cmd.Parameters.AddWithValue("@clase", clase);
                cmd.Parameters.AddWithValue("@proceso", 1);

                conexion.Open();

                int filas = (int)cmd.ExecuteScalar();

                if (filas > 0)
                {
                    resultado = true;
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                throw ex;
            }
            finally
            {
                conexion.Close();
            }

            return resultado;
        }

        //*** SP Envio Estado Cuenta - reportes.sp_rm_email_estadocuenta
        public bool EnviarEstadoCuentaEmail(EstadoCuenta oEstadoCuenta)
        {
            bool resultado = false;

            SqlConnection conexion = null;
            SqlCommand cmd = null;

            try
            {
                //*** Instancia de Conexión
                conexion = Conexion.ConexionBD();
                cmd = new SqlCommand("reportes.sp_rm_email_estadocuenta", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                //*** Parametros
                cmd.Parameters.AddWithValue("@cod_usuario", oEstadoCuenta.id_usuario);
                cmd.Parameters.AddWithValue("@cedula", oEstadoCuenta.identificacion);
                cmd.Parameters.AddWithValue("@nombre", oEstadoCuenta.nombre);
                cmd.Parameters.AddWithValue("@email", oEstadoCuenta.email);
                cmd.Parameters.AddWithValue("@ruta", oEstadoCuenta.ruta_envio);
                cmd.Parameters.AddWithValue("@tipo_consulta", oEstadoCuenta.tipo_consulta);

                conexion.Open();

                int filas = (int)cmd.ExecuteScalar();

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

        //*** Bitacora Estado de Cuenta - Encabezado
        public int BitacoraEstadoCuentaEnc(BitacoraEstadoEnc oBitacoraEnc, string proceso)
        {
            int resultado = 0;
            string p_msg = "";

            string cod_proceso = proceso;
            string sp = "reportes.sp_crud_bitacora_estado_cuenta_enc";

            SqlConnection conexion = null;
            SqlCommand cmd = null;

            try
            {
                //*** Instancia de Conexión
                conexion = Conexion.ConexionBD();
                cmd = new SqlCommand(sp, conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                //*** Parametros
                cmd.Parameters.AddWithValue("@id", oBitacoraEnc.id);
                cmd.Parameters.AddWithValue("@cod_proceso", oBitacoraEnc.cod_proceso);
                cmd.Parameters.AddWithValue("@cod_usuario", oBitacoraEnc.cod_usuario);
                cmd.Parameters.AddWithValue("@pagina", oBitacoraEnc.pagina);
                cmd.Parameters.AddWithValue("@num_lote", oBitacoraEnc.num_lote);
                cmd.Parameters.AddWithValue("@num_registros", oBitacoraEnc.num_registros);

                cmd.Parameters.AddWithValue("@cod_cliente_inicio", oBitacoraEnc.cod_cliente_inicio);
                cmd.Parameters.AddWithValue("@cod_cliente_final", oBitacoraEnc.cod_cliente_final);
                cmd.Parameters.AddWithValue("@i_pruebas", oBitacoraEnc.i_pruebas);
                cmd.Parameters.AddWithValue("@i_borrar_dir", oBitacoraEnc.i_borrar_dir);
                cmd.Parameters.AddWithValue("@fecha_corte", DateTime.ParseExact(oBitacoraEnc.fecha_corte, "dd/MM/yyyy", new CultureInfo("es-CR")));
                cmd.Parameters.AddWithValue("@servidor_genera", oBitacoraEnc.servidor_genera);

                cmd.Parameters.AddWithValue("@servidor_ssrs", oBitacoraEnc.servidor_ssrs);
                cmd.Parameters.AddWithValue("@url_ssrs", oBitacoraEnc.url_ssrs);
                cmd.Parameters.AddWithValue("@dir_local", oBitacoraEnc.dir_local);
                cmd.Parameters.AddWithValue("@dir_remoto", oBitacoraEnc.dir_remoto);
                cmd.Parameters.AddWithValue("@archivos_bloqueados", oBitacoraEnc.archivos_bloqueados);
                cmd.Parameters.AddWithValue("@archivos_errores", oBitacoraEnc.archivos_errores);

                cmd.Parameters.AddWithValue("@request", oBitacoraEnc.request);
                cmd.Parameters.AddWithValue("@response", oBitacoraEnc.response);
                cmd.Parameters.AddWithValue("@navegador", oBitacoraEnc.navegador);
                cmd.Parameters.AddWithValue("@terminal_id", oBitacoraEnc.terminal_id);

                cmd.Parameters.AddWithValue("@proceso", cod_proceso);

                //*** Parametros de Salida
                var retorno = cmd.Parameters.Add("@retorno", SqlDbType.Int);
                retorno.Direction = ParameterDirection.ReturnValue;

                var msg = cmd.Parameters.Add("@msg", SqlDbType.VarChar, size: 200);
                msg.Direction = ParameterDirection.Output;

                //*** Abrir Conexión
                conexion.Open();

                using (conexion)
                {
                    //*** Retorno del SP ( DML )
                    int resp = (int)cmd.ExecuteNonQuery();

                    //*** Retorno SQL
                    var p_retorno = (int)retorno.Value;
                    p_msg = Convert.ToString(msg.Value);

                    if (p_retorno > 0)
                    {
                        resultado = p_retorno;
                    }
                }

            }
            catch (Exception ex)
            {
                string msg = sp + " - " + Convert.ToString(p_msg);
                throw new Exception(msg, ex);
            }

            return resultado;
        }

        //*** Bitacora Estado de Cuenta - Detalle
        public int BitacoraEstadoCuentaDet(BitacoraEstadoDet oBitacoraDet, string proceso)
        {
            int resultado = 0;
            string p_msg = "";

            string cod_proceso = proceso;
            string sp = "reportes.sp_crud_bitacora_estado_cuenta_det";

            SqlConnection conexion = null;
            SqlCommand cmd = null;

            try
            {
                //*** Instancia de Conexión
                conexion = Conexion.ConexionBD();
                cmd = new SqlCommand(sp, conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                //*** Parametros
                cmd.Parameters.AddWithValue("@id", oBitacoraDet.id);
                cmd.Parameters.AddWithValue("@num_lote", oBitacoraDet.num_lote);
                cmd.Parameters.AddWithValue("@num_registro", oBitacoraDet.num_registro);
                cmd.Parameters.AddWithValue("@cod_cliente", oBitacoraDet.cod_cliente);
                cmd.Parameters.AddWithValue("@identificacion", oBitacoraDet.identificacion);
                cmd.Parameters.AddWithValue("@num_contrato", oBitacoraDet.num_contrato);
                cmd.Parameters.AddWithValue("@estado", oBitacoraDet.estado);
                cmd.Parameters.AddWithValue("@response", oBitacoraDet.response);

                cmd.Parameters.AddWithValue("@proceso", cod_proceso);

                //*** Parametros de Salida
                var retorno = cmd.Parameters.Add("@retorno", SqlDbType.Int);
                retorno.Direction = ParameterDirection.ReturnValue;

                var msg = cmd.Parameters.Add("@msg", SqlDbType.VarChar, size: 200);
                msg.Direction = ParameterDirection.Output;

                //*** Abrir Conexión
                conexion.Open();

                using (conexion)
                {
                    //*** Retorno del SP ( DML )
                    int resp = (int)cmd.ExecuteNonQuery();

                    //*** Retorno SQL
                    var p_retorno = (int)retorno.Value;
                    p_msg = Convert.ToString(msg.Value);

                    if (p_retorno > 0)
                    {
                        resultado = p_retorno;
                    }
                }

            }
            catch (Exception ex)
            {
                string msg = sp + " - " + Convert.ToString(p_msg);
                throw new Exception(msg, ex);
            }

            return resultado;
        }
    }
}
