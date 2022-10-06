using Oracle.ManagedDataAccess.Client;
using System;
using System.Configuration;
using System.Data.SqlClient;

namespace CP_AccesoDatos
{
    public class Conexion
    {
        public static SqlConnection ConexionBD()
        {
            try
            {
                string conexionString = ConfigurationManager.ConnectionStrings["ConexionSQLServer"].ConnectionString;

                SqlConnection conexion = new SqlConnection(conexionString);
                return conexion;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error al conectar SqlConnection ", ex);
            }
        }
    }

    public class ConexionCendeisss
    {
        public static SqlConnection ConexionBD()
        {
            try
            {
                string conexionString = ConfigurationManager.ConnectionStrings["ConexionCendeisss"].ConnectionString;

                SqlConnection conexion = new SqlConnection(conexionString);
                return conexion;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error al conectar SqlConnection ", ex);
            }
        }
    }
    public class ConexionGnosis
    {
        public static SqlConnection ConexionBD()
        {
            try
            {
                string conexionString = ConfigurationManager.ConnectionStrings["ConexionGnosis"].ConnectionString;

                SqlConnection conexion = new SqlConnection(conexionString);
                return conexion;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error al conectar SqlConnection ", ex);
            }
        }
    }

    public class ConexionOracle
    {
        public static OracleConnection ConexionBDOracle()
        {
            try
            {
                string conexionString = ConfigurationManager.ConnectionStrings["ConexionOracle"].ConnectionString;
                OracleConnection conexion = new OracleConnection(conexionString);
                return conexion;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error al conectar OracleConnection ", ex);
            }
        }
    }
}
