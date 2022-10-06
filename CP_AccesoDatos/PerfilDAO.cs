using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CP_Entidades;
using System.Globalization;

namespace CP_AccesoDatos
{
    public class PerfilDAO
    {

        #region "Constructor de Perfil"
        private static PerfilDAO Perfil = null;
        private PerfilDAO() { }
        public static PerfilDAO getInstancia()
        {
            if (Perfil == null)
            {
                Perfil = new PerfilDAO();
            }
            return Perfil;
        }
        #endregion

        //*** Get Perfiles
        public List<Perfil> GetPerfiles()
        {
            List<Perfil> lista = new List<Perfil>();

            SqlConnection conexion = null;
            SqlCommand cmd = null;
            SqlDataReader dr = null;

            String proceso_crud = "R";

            try
            {
                conexion = Conexion.ConexionBD();
                cmd = new SqlCommand("reportes.sp_rm_crud_perfiles", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@cod_perfil", 0);
                cmd.Parameters.AddWithValue("@nombre", "");
                cmd.Parameters.AddWithValue("@descripcion", "");
                cmd.Parameters.AddWithValue("@i_estado", "");

                cmd.Parameters.AddWithValue("@proceso_crud", proceso_crud);

                conexion.Open();
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Perfil ObjPerfil = new Perfil();

                    ObjPerfil.id = Int32.Parse(dr["cod_perfil"].ToString());
                    ObjPerfil.nombre = dr["nombre"].ToString();
                    ObjPerfil.descripcion = dr["descripcion"].ToString();
                    ObjPerfil.estado = dr["estado"].ToString();
                    //ObjPerfil.fecha_creacion = DateTime.Parse(dr["fecha_creacion"].ToString()).ToString("MM/dd/yyyy HH:mm:ss tt", CultureInfo.InvariantCulture);

                    //Añadir a la lista
                    lista.Add(ObjPerfil);
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

        //*** Mantenimiento de Perfiles
        public bool MantenimientoPerfil(Perfil perfil, String proceso_crud)
        {
            SqlConnection conexion = null;
            SqlCommand cmd = null;
            bool resultado = false;

            try
            {
                conexion = Conexion.ConexionBD();
                cmd = new SqlCommand("reportes.sp_rm_crud_perfiles", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@cod_perfil", perfil.id);
                cmd.Parameters.AddWithValue("@nombre", perfil.nombre);
                cmd.Parameters.AddWithValue("@descripcion", perfil.descripcion);
                cmd.Parameters.AddWithValue("@i_estado", perfil.estado);
                cmd.Parameters.AddWithValue("@listaReportes", perfil.listaReportes);
                cmd.Parameters.AddWithValue("@listaUsuarios", perfil.listaUsuarios);

                if (proceso_crud == "C")
                {
                    cmd.Parameters.AddWithValue("@login", perfil.login_creacion);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@login", perfil.login_modifica);
                }

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
