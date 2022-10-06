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
    public class RolDAO
    {

        #region "Constructor de Rol"
        private static RolDAO Rol = null;
        private RolDAO() { }
        public static RolDAO getInstancia()
        {
            if (Rol == null)
            {
                Rol = new RolDAO();
            }
            return Rol;
        }
        #endregion


        //Get Rols
        public List<Rol> GetRoles()
        {
            List<Rol> lista = new List<Rol>();
            SqlConnection conexion = null;
            SqlCommand cmd = null;
            SqlDataReader dr = null;

            String proceso_crud = "R";

            try
            {
                conexion = Conexion.ConexionBD();
                cmd = new SqlCommand("Reportes.sp_rm_crud_roles", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@cod_rol", 0);
                cmd.Parameters.AddWithValue("@nombre", "");
                cmd.Parameters.AddWithValue("@descripcion", "");
                cmd.Parameters.AddWithValue("@proceso_crud", proceso_crud);

                conexion.Open();
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Rol ObjRol = new Rol();

                    ObjRol.cod_rol = Int32.Parse(dr["cod_rol"].ToString());
                    ObjRol.nombre = dr["nombre"].ToString();
                    ObjRol.descripcion = dr["descripcion"].ToString();

                    //Añadir a la lista
                    lista.Add(ObjRol);
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
    }
}
