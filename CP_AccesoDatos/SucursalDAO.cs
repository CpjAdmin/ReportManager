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
    public class SucursalDAO
    {
        #region "Constructor de Sucursal"
        private static SucursalDAO Sucursal = null;
        private SucursalDAO() { }
        public static SucursalDAO getInstancia()
        {
            if (Sucursal == null)
            {
                Sucursal = new SucursalDAO();
            }
            return Sucursal;
        }
        #endregion

        //Get Sucursals
        public DataTable GetSucursales()
        {
            SqlConnection conexion = null;
            SqlCommand cmd = null;
            DataTable dt = null;

            try
            {
                conexion = Conexion.ConexionBD();
                cmd = new SqlCommand("Reportes.sp_rm_crud_sucursales", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@cod_sucursal", 0);
                cmd.Parameters.AddWithValue("@cod_alterno", "");
                cmd.Parameters.AddWithValue("@nombre", "");
                cmd.Parameters.AddWithValue("@direccion", "");
                cmd.Parameters.AddWithValue("@telefono1", "");
                cmd.Parameters.AddWithValue("@telefono2", "");
                cmd.Parameters.AddWithValue("@fax", "");
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

        //Mantenimiento Sucursals
        public bool MantenimientoSucursal(Sucursal sucursal, String proceso_crud)
        {
            SqlConnection conexion = null;
            SqlCommand cmd = null;
            bool resultado = false;

            try
            {
                conexion = Conexion.ConexionBD();
                cmd = new SqlCommand("Reportes.sp_rm_crud_sucursales", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@cod_sucursal", sucursal.cod_sucursal);
                cmd.Parameters.AddWithValue("@cod_alterno", sucursal.cod_alterno);
                cmd.Parameters.AddWithValue("@nombre", sucursal.nombre);
                cmd.Parameters.AddWithValue("@direccion", sucursal.direccion);
                cmd.Parameters.AddWithValue("@telefono1", sucursal.telefono1);
                cmd.Parameters.AddWithValue("@telefono2", sucursal.telefono2);
                cmd.Parameters.AddWithValue("@fax", sucursal.fax);
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
