using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CP_AccesoDatos;
using CP_Entidades;

namespace CP_LogicaNegocio
{

    //Provincia - Canton - Distrito
    public class ProvinciaLN
    {
        #region "Logica de Negocio Provincia"
        private static ProvinciaLN ObjProvincia = null;
        private ProvinciaLN() { }
        public static ProvinciaLN getInstancia()
        {
            if (ObjProvincia == null)
            {
                ObjProvincia = new ProvinciaLN();
            }
            return ObjProvincia;
        }
        #endregion

        //Get Provincias
        public List<Provincia> GetProvincias()
        {
            try
            {
                return ProvinciaDAO.getInstancia().GetProvincias();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

    public class CantonLN
    {
        #region "Logica de Negocio Canton"
        private static CantonLN ObjCanton = null;
        private CantonLN() { }
        public static CantonLN getInstancia()
        {
            if (ObjCanton == null)
            {
                ObjCanton = new CantonLN();
            }
            return ObjCanton;
        }
        #endregion

        //Get Cantones
        public List<Canton> GetCantones(string provinciaId)
        {
            try
            {
                return CantonDAO.getInstancia().GetCantones(provinciaId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public class DistritoLN
    {
        #region "Logica de Negocio Distrito"
        private static DistritoLN ObjDistrito = null;
        private DistritoLN() { }
        public static DistritoLN getInstancia()
        {
            if (ObjDistrito == null)
            {
                ObjDistrito = new DistritoLN();
            }
            return ObjDistrito;
        }
        #endregion

        //Get Distritos
        public List<Distrito> GetDistritos(string provinciaId, string cantonId)
        {
            try
            {
                return DistritoDAO.getInstancia().GetDistritos(provinciaId, cantonId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

    // Centro - Institucion - Lugares
    public class CentroLN
    {
        #region "Logica de Negocio Centro"
        private static CentroLN ObjCentro = null;
        private CentroLN() { }
        public static CentroLN getInstancia()
        {
            if (ObjCentro == null)
            {
                ObjCentro = new CentroLN();
            }
            return ObjCentro;
        }
        #endregion

        //Get Centros
        public List<Centro> GetCentros()
        {
            try
            {
                return CentroDAO.getInstancia().GetCentros();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

    public class InstitucionLN
    {
        #region "Logica de Negocio Institucion"
        private static InstitucionLN ObjInstitucion = null;
        private InstitucionLN() { }
        public static InstitucionLN getInstancia()
        {
            if (ObjInstitucion == null)
            {
                ObjInstitucion = new InstitucionLN();
            }
            return ObjInstitucion;
        }
        #endregion

        //Get Instituciones
        public List<Institucion> GetInstituciones(string centroId)
        {
            try
            {
                return InstitucionDAO.getInstancia().GetInstituciones(centroId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public class LugarTrabajoLN
    {
        #region "Logica de Negocio LugarTrabajo"
        private static LugarTrabajoLN ObjLugarTrabajo = null;
        private LugarTrabajoLN() { }
        public static LugarTrabajoLN getInstancia()
        {
            if (ObjLugarTrabajo == null)
            {
                ObjLugarTrabajo = new LugarTrabajoLN();
            }
            return ObjLugarTrabajo;
        }
        #endregion

        //Get LugarTrabajo
        public List<LugarTrabajo> GetLugarTrabajo(string centroId, string institucionId)
        {
            try
            {
                return LugarTrabajoDAO.getInstancia().GetLugarTrabajo(centroId, institucionId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

    //*** Asociados
    public class AsociadoLN
    {
        #region "Logica de Negocio Asociado"
        private static AsociadoLN ObjAsociado = null;
        private AsociadoLN() { }
        public static AsociadoLN getInstancia()
        {
            if (ObjAsociado == null)
            {
                ObjAsociado = new AsociadoLN();
            }
            return ObjAsociado;
        }
        #endregion

        //***Get Asociados
        public List<Asociado> GetAsociados(BusquedaAsociado oBusquedaAsociado)
        {
            try
            {
                if (oBusquedaAsociado.codigo != "0" || oBusquedaAsociado.identificacion != "0")
                {
                    oBusquedaAsociado.cod_proceso = 1;

                    //*** Para buscar sin filtros
                    oBusquedaAsociado.cod_provincia = "0";
                    oBusquedaAsociado.cod_canton = "0";
                    oBusquedaAsociado.cod_distrito = "0";
                    oBusquedaAsociado.cod_centro = "0";
                    oBusquedaAsociado.cod_institucion = "0";
                    oBusquedaAsociado.cod_lugar = "0";
                }
                else
                {
                    oBusquedaAsociado.cod_proceso = 2;
                }

                return AsociadoDAO.getInstancia().GetAsociados(oBusquedaAsociado);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

    // CPC
    public class CPCLN
    {
        #region "Logica de Negocio LugarTrabajo"
        private static CPCLN ObjCPC = null;
        private CPCLN() { }
        public static CPCLN getInstancia()
        {
            if (ObjCPC == null)
            {
                ObjCPC = new CPCLN();
            }
            return ObjCPC;
        }

        //VerificarUsuarioCPC CPC
        #endregion
        public bool VerificarUsuarioCPC(CPC usuarioCPC)
        {
            try
            {
                return CPCDAO.getInstancia().VerificarUsuarioCPC(usuarioCPC);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //ActualizarClave CPC
        public bool ActualizarClave(CPC usuarioCPC)
        {
            try
            {
                return CPCDAO.getInstancia().ActualizarClave(usuarioCPC);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

}
