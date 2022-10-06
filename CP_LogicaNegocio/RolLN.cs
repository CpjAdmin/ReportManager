using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CP_AccesoDatos;
using CP_Entidades;
using System.Data;

namespace CP_LogicaNegocio
{
    public class RolLN
    {

        #region "Logica de Negocio Rol"
        private static RolLN ObjRol = null;
        private RolLN() { }
        public static RolLN getInstancia()
        {
            if (ObjRol == null)
            {
                ObjRol = new RolLN();
            }
            return ObjRol;
        }
        #endregion

        //Get Rols
        public List<Rol> GetRoles()
        {
            try
            {
                return RolDAO.getInstancia().GetRoles();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
