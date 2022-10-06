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
    public class DepartamentoLN
    {

        #region "Logica de Negocio Departamento"
        private static DepartamentoLN ObjDepartamento = null;
        private DepartamentoLN() { }
        public static DepartamentoLN getInstancia()
        {
            if (ObjDepartamento == null)
            {
                ObjDepartamento = new DepartamentoLN();
            }
            return ObjDepartamento;
        }
        #endregion

        //Get Departamentos
        public DataTable GetDepartamentos()
        {
            try
            {
                return DepartamentoDAO.getInstancia().GetDepartamentos();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Mantenimiento Departamentos
        public bool MantenimientoDepartamento(Departamento departamento,String proceso_crud)
        {
            try
            {
                return DepartamentoDAO.getInstancia().MantenimientoDepartamento(departamento,proceso_crud);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
