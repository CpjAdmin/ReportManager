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
    public class DepartamentoDAO
    {
        #region "Constructor de Departamento"
        private static DepartamentoDAO Departamento = null;
        private DepartamentoDAO() { }
        public static DepartamentoDAO getInstancia()
        {
            if (Departamento == null)
            {
                Departamento = new DepartamentoDAO();
            }
            return Departamento;
        }
        #endregion

        //Get Departamentos
        public DataTable GetDepartamentos()
        {
            SqlConnection conexion = null;
            SqlCommand cmd = null;
            DataTable dt = null;

            try
            {
                conexion = Conexion.ConexionBD();
                cmd = new SqlCommand("Reportes.sp_rm_crud_departamentos", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@cod_departamento", 0);
                cmd.Parameters.AddWithValue("@cod_alterno", "0");
                cmd.Parameters.AddWithValue("@nombre", "");
                cmd.Parameters.AddWithValue("@proceso_crud", "R");

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

        //Mantenimiento Departamentos
        public bool MantenimientoDepartamento(Departamento departamento, String proceso_crud)
        {
            SqlConnection conexion = null;
            SqlCommand cmd = null;
            bool resultado = false;

            try
            {
                conexion = Conexion.ConexionBD();
                cmd = new SqlCommand("Reportes.sp_rm_crud_departamentos", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@cod_departamento", departamento.cod_departamento);
                cmd.Parameters.AddWithValue("@cod_alterno", departamento.cod_alterno);
                cmd.Parameters.AddWithValue("@nombre", departamento.nombre);
                cmd.Parameters.AddWithValue("@proceso_crud", proceso_crud);

                conexion.Open();
                int filas = cmd.ExecuteNonQuery();

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


    }
}
