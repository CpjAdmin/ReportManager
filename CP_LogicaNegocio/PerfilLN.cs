using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CP_AccesoDatos;
using CP_Entidades;

namespace CP_LogicaNegocio
{
    public class PerfilLN
    {
        #region "Logica de Negocio Perfil"
        private static PerfilLN ObjPerfil = null;
        private PerfilLN() { }
        public static PerfilLN getInstancia()
        {
            if (ObjPerfil == null)
            {
                ObjPerfil = new PerfilLN();
            }
            return ObjPerfil;
        }
        #endregion

        //Get Perfiles
        public List<Perfil> GetPerfiles()
        {
            try
            {
                return PerfilDAO.getInstancia().GetPerfiles();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //Mantenimiento de Perfil
        public bool MantenimientoPerfil(Perfil perfil, String proceso_crud)
        {
            try
            {
                return PerfilDAO.getInstancia().MantenimientoPerfil(perfil, proceso_crud);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
