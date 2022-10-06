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
    public class SucursalLN
    {

        #region "Logica de Negocio Sucursal"
        private static SucursalLN ObjSucursal = null;
        private SucursalLN() { }
        public static SucursalLN getInstancia()
        {
            if (ObjSucursal == null)
            {
                ObjSucursal = new SucursalLN();
            }
            return ObjSucursal;
        }
        #endregion

        //Get Sucursals
        public DataTable GetSucursales()
        {
            try
            {
                return SucursalDAO.getInstancia().GetSucursales();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Mantenimiento Sucursals
        public bool MantenimientoSucursal(Sucursal Sucursal, String proceso_crud)
        {
            try
            {
                return SucursalDAO.getInstancia().MantenimientoSucursal(Sucursal, proceso_crud);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
