using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using CP_Entidades;
using Oracle.ManagedDataAccess.Client;

namespace CP_AccesoDatos
{
    //Provincia - Canton - Distrito
    public class ProvinciaDAO
    {

        #region "Constructor de Provincia"
        private static ProvinciaDAO Provincia = null;
        private ProvinciaDAO() { }
        public static ProvinciaDAO getInstancia()
        {
            if (Provincia == null)
            {
                Provincia = new ProvinciaDAO();
            }
            return Provincia;
        }
        #endregion

        //Get Provincias
        public List<Provincia> GetProvincias()
        {
            List<Provincia> lista = new List<Provincia>();
            OracleCommand cmd = null;
            OracleDataReader dr = null;

            String entidad = "PROVINCIA";

            using (OracleConnection conexion = ConexionOracle.ConexionBDOracle())
            {
                cmd = new OracleCommand("P_RM_ORACLE_PSBANK", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("ENTIDAD", OracleDbType.Varchar2).Value = entidad;
                cmd.Parameters.Add("PARAMETRO1", OracleDbType.Varchar2).Value = "0";
                cmd.Parameters.Add("PARAMETRO2", OracleDbType.Varchar2).Value = "0";
                cmd.Parameters.Add("SALIDA", OracleDbType.RefCursor, ParameterDirection.Output);

                conexion.Open();

                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Provincia ObjProvincia = new Provincia();

                    ObjProvincia.cod_provincia = dr["cod_provincia"].ToString();
                    ObjProvincia.nombre = dr["nombre"].ToString();
                    //Añadir a la lista
                    lista.Add(ObjProvincia);
                }

                return lista;
            }
        }

    }

    public class CantonDAO
    {

        #region "Constructor de Canton"
        private static CantonDAO Canton = null;
        private CantonDAO() { }
        public static CantonDAO getInstancia()
        {
            if (Canton == null)
            {
                Canton = new CantonDAO();
            }
            return Canton;
        }
        #endregion

        //Get Cantones
        public List<Canton> GetCantones(string provinciaId)
        {
            List<Canton> lista = new List<Canton>();
            OracleConnection conexion = null;
            OracleCommand cmd = null;
            OracleDataReader dr = null;

            String entidad = "Canton";
            String cod_provincia = provinciaId;

            try
            {
                conexion = ConexionOracle.ConexionBDOracle();
                cmd = new OracleCommand("P_RM_ORACLE_PSBANK", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("ENTIDAD", OracleDbType.Varchar2).Value = entidad;
                cmd.Parameters.Add("PARAMETRO1", OracleDbType.Varchar2).Value = cod_provincia;
                cmd.Parameters.Add("PARAMETRO2", OracleDbType.Varchar2).Value = "0";
                cmd.Parameters.Add("SALIDA", OracleDbType.RefCursor, ParameterDirection.Output);

                conexion.Open();

                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Canton ObjCanton = new Canton();

                    ObjCanton.cod_canton = dr["cod_provincia"].ToString();
                    ObjCanton.cod_canton = dr["cod_canton"].ToString();
                    ObjCanton.nombre = dr["nombre"].ToString();
                    //Añadir a la lista
                    lista.Add(ObjCanton);
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

    public class DistritoDAO
    {

        #region "Constructor de Distrito"
        private static DistritoDAO Distrito = null;
        private DistritoDAO() { }
        public static DistritoDAO getInstancia()
        {
            if (Distrito == null)
            {
                Distrito = new DistritoDAO();
            }
            return Distrito;
        }
        #endregion

        //Get Distritoes
        public List<Distrito> GetDistritos(string provinciaId, string cantonId)
        {
            List<Distrito> lista = new List<Distrito>();
            OracleConnection conexion = null;
            OracleCommand cmd = null;
            OracleDataReader dr = null;

            String entidad = "Distrito";
            String cod_provincia = provinciaId;
            String cod_canton = cantonId;

            try
            {
                conexion = ConexionOracle.ConexionBDOracle();
                cmd = new OracleCommand("P_RM_ORACLE_PSBANK", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("ENTIDAD", OracleDbType.Varchar2).Value = entidad;
                cmd.Parameters.Add("PARAMETRO1", OracleDbType.Varchar2).Value = cod_provincia;
                cmd.Parameters.Add("PARAMETRO2", OracleDbType.Varchar2).Value = cod_canton;
                cmd.Parameters.Add("SALIDA", OracleDbType.RefCursor, ParameterDirection.Output);

                conexion.Open();

                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Distrito ObjDistrito = new Distrito();

                    ObjDistrito.cod_provincia = dr["cod_provincia"].ToString();
                    ObjDistrito.cod_canton = dr["cod_canton"].ToString();
                    ObjDistrito.cod_distrito = dr["cod_distrito"].ToString();
                    ObjDistrito.nombre = dr["nombre"].ToString();
                    //Añadir a la lista
                    lista.Add(ObjDistrito);
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

    // Centro - Institucion - Lugares
    public class CentroDAO
    {

        #region "Constructor de Centro"
        private static CentroDAO Centro = null;
        private CentroDAO() { }
        public static CentroDAO getInstancia()
        {
            if (Centro == null)
            {
                Centro = new CentroDAO();
            }
            return Centro;
        }
        #endregion

        //Get Centros
        public List<Centro> GetCentros()
        {
            List<Centro> lista = new List<Centro>();
            OracleConnection conexion = null;
            OracleCommand cmd = null;
            OracleDataReader dr = null;

            String entidad = "Centro";

            try
            {
                conexion = ConexionOracle.ConexionBDOracle();
                cmd = new OracleCommand("P_RM_CENTROS_PSBANK", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("ENTIDAD", OracleDbType.Varchar2).Value = entidad;
                cmd.Parameters.Add("PARAMETRO1", OracleDbType.Varchar2).Value = "0";
                cmd.Parameters.Add("PARAMETRO2", OracleDbType.Varchar2).Value = "0";
                cmd.Parameters.Add("SALIDA", OracleDbType.RefCursor, ParameterDirection.Output);

                conexion.Open();

                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Centro ObjCentro = new Centro();

                    ObjCentro.cod_centro = dr["cod_centro"].ToString();
                    ObjCentro.nombre = dr["nombre"].ToString();
                    //Añadir a la lista
                    lista.Add(ObjCentro);
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

    public class InstitucionDAO
    {

        #region "Constructor de Institucion"
        private static InstitucionDAO Institucion = null;
        private InstitucionDAO() { }
        public static InstitucionDAO getInstancia()
        {
            if (Institucion == null)
            {
                Institucion = new InstitucionDAO();
            }
            return Institucion;
        }
        #endregion

        //Get Instituciones
        public List<Institucion> GetInstituciones(string centroId)
        {
            List<Institucion> lista = new List<Institucion>();
            OracleConnection conexion = null;
            OracleCommand cmd = null;
            OracleDataReader dr = null;

            String entidad = "Institucion";
            String cod_provincia = centroId;

            try
            {
                conexion = ConexionOracle.ConexionBDOracle();
                cmd = new OracleCommand("P_RM_CENTROS_PSBANK", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("ENTIDAD", OracleDbType.Varchar2).Value = entidad;
                cmd.Parameters.Add("PARAMETRO1", OracleDbType.Varchar2).Value = centroId;
                cmd.Parameters.Add("PARAMETRO2", OracleDbType.Varchar2).Value = "0";
                cmd.Parameters.Add("SALIDA", OracleDbType.RefCursor, ParameterDirection.Output);

                conexion.Open();

                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Institucion ObjInstitucion = new Institucion();

                    ObjInstitucion.cod_centro = dr["cod_centro"].ToString();
                    ObjInstitucion.cod_institucion = dr["cod_institucion"].ToString();
                    ObjInstitucion.nombre = dr["nombre"].ToString();
                    //Añadir a la lista
                    lista.Add(ObjInstitucion);
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

    public class LugarTrabajoDAO
    {

        #region "Constructor de LugarTrabajo"
        private static LugarTrabajoDAO LugarTrabajo = null;
        private LugarTrabajoDAO() { }
        public static LugarTrabajoDAO getInstancia()
        {
            if (LugarTrabajo == null)
            {
                LugarTrabajo = new LugarTrabajoDAO();
            }
            return LugarTrabajo;
        }
        #endregion

        //Get LugarTrabajo
        public List<LugarTrabajo> GetLugarTrabajo(string centroId, string institucionId)
        {
            List<LugarTrabajo> lista = new List<LugarTrabajo>();
            OracleConnection conexion = null;
            OracleCommand cmd = null;
            OracleDataReader dr = null;

            String entidad = "Lugar";
            String cod_provincia = centroId;
            String cod_canton = institucionId;

            try
            {
                conexion = ConexionOracle.ConexionBDOracle();
                cmd = new OracleCommand("P_RM_CENTROS_PSBANK", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("ENTIDAD", OracleDbType.Varchar2).Value = entidad;
                cmd.Parameters.Add("PARAMETRO1", OracleDbType.Varchar2).Value = centroId;
                cmd.Parameters.Add("PARAMETRO2", OracleDbType.Varchar2).Value = institucionId;
                cmd.Parameters.Add("SALIDA", OracleDbType.RefCursor, ParameterDirection.Output);

                conexion.Open();

                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    LugarTrabajo ObjLugarTrabajo = new LugarTrabajo();

                    ObjLugarTrabajo.cod_centro = dr["cod_centro"].ToString();
                    ObjLugarTrabajo.cod_institucion = dr["cod_institucion"].ToString();
                    ObjLugarTrabajo.cod_lugar_trabajo = dr["cod_lugar_trabajo"].ToString();
                    ObjLugarTrabajo.nombre = dr["nombre"].ToString();
                    //Añadir a la lista
                    lista.Add(ObjLugarTrabajo);
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

    //Asociados
    public class AsociadoDAO
    {

        #region "Constructor de Asociado"
        private static AsociadoDAO Asociado = null;
        private AsociadoDAO() { }
        public static AsociadoDAO getInstancia()
        {
            if (Asociado == null)
            {
                Asociado = new AsociadoDAO();
            }
            return Asociado;
        }
        #endregion

        //*** Get Asociado
        public List<Asociado> GetAsociados(BusquedaAsociado oBusquedaAsociado)
        {
            List<Asociado> lista = new List<Asociado>();
            OracleConnection conexion = null;
            OracleCommand cmd = null;
            OracleDataReader dr = null;

            try
            {
                conexion = ConexionOracle.ConexionBDOracle();
                cmd = new OracleCommand("P_RM_ASOCIADOS_PSBANK", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("COD_PROCESO", OracleDbType.Int16).Value = oBusquedaAsociado.cod_proceso;
                cmd.Parameters.Add("IDENTIFICACION", OracleDbType.Varchar2).Value = oBusquedaAsociado.identificacion;
                cmd.Parameters.Add("CODIGO", OracleDbType.Varchar2).Value = oBusquedaAsociado.codigo;
                cmd.Parameters.Add("COD_PROVINCIA", OracleDbType.Varchar2).Value = oBusquedaAsociado.cod_provincia;
                cmd.Parameters.Add("COD_CANTON", OracleDbType.Varchar2).Value = oBusquedaAsociado.cod_canton;
                cmd.Parameters.Add("COD_DISTRITO", OracleDbType.Varchar2).Value = oBusquedaAsociado.cod_distrito;
                cmd.Parameters.Add("COD_CENTRO", OracleDbType.Varchar2).Value = oBusquedaAsociado.cod_centro;
                cmd.Parameters.Add("COD_INSTITUCION", OracleDbType.Varchar2).Value = oBusquedaAsociado.cod_institucion;
                cmd.Parameters.Add("COD_LUGAR", OracleDbType.Varchar2).Value = oBusquedaAsociado.cod_lugar;
                cmd.Parameters.Add("CON_EMAIL", OracleDbType.Varchar2).Value = oBusquedaAsociado.con_email;
                cmd.Parameters.Add("TIPO_CONSULTA", OracleDbType.Varchar2).Value = oBusquedaAsociado.tipo_consulta;
                cmd.Parameters.Add("ULT_COD_CLIENTE_GEN", OracleDbType.Int32).Value = oBusquedaAsociado.ult_cod_cliente_gen;
                cmd.Parameters.Add("SALIDA", OracleDbType.RefCursor, ParameterDirection.Output);

                conexion.Open();

                dr = cmd.ExecuteReader();

                //*** Tipo_consulta = AHORRO-VISTA
                if (oBusquedaAsociado.tipo_consulta == "AHORRO-VISTA")
                {
                    while (dr.Read())
                    {
                        Asociado ObjAsociado = new Asociado
                        (
                            codigo : dr["CODIGO"].ToString(),
                            identificacion : dr["IDENTIFICACION"].ToString(),
                            nombre : dr["NOMBRE"].ToString(),
                            centro : dr["CENTRO"].ToString(),
                            institucion : dr["INSTITUCION"].ToString(),
                            lugar_trabajo : dr["LUGAR_TRABAJO"].ToString(),
                            estado : dr["ESTADO"].ToString(),
                            email : dr["EMAIL"].ToString(),
                            num_contrato : Convert.ToInt32(dr["NUM_CONTRATO"]),
                            tarjeta : dr["TARJETA"].ToString(),
                            cuenta_iban : dr["CUENTA_IBAN"].ToString()
                        );

                        //*** Añadir a la lista
                        lista.Add(ObjAsociado);
                    }
                }
                else
                {
                    //*** Tipo_consulta = UNIFICADA
                    while (dr.Read())
                    {
                        Asociado ObjAsociado = new Asociado(
                            codigo: dr["CODIGO"].ToString(),
                            identificacion: dr["IDENTIFICACION"].ToString(),
                            nombre: dr["NOMBRE"].ToString(),
                            centro: dr["CENTRO"].ToString(),
                            institucion: dr["INSTITUCION"].ToString(),
                            lugar_trabajo: dr["LUGAR_TRABAJO"].ToString(),
                            estado: dr["ESTADO"].ToString(),
                            email: dr["EMAIL"].ToString(),
                            provincia: dr["PROVINCIA"].ToString(),
                            canton: dr["CANTON"].ToString(),
                            distrito: dr["DISTRITO"].ToString()
                        );

                        //*** Añadir a la lista
                        lista.Add(ObjAsociado);
                    }
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

    //CPC
    public class CPCDAO
    {

        #region "Constructor de CPC"
        private static CPCDAO CPC = null;
        private CPCDAO() { }
        public static CPCDAO getInstancia()
        {
            if (CPC == null)
            {
                CPC = new CPCDAO();
            }
            return CPC;
        }
        #endregion

        public bool VerificarUsuarioCPC(CPC usuarioCPC)
        {
            OracleConnection conexion = null;
            OracleCommand cmd = null;
            Int32 resultado = 0;
            bool existe = false;

            try
            {
                conexion = ConexionOracle.ConexionBDOracle();
                cmd = new OracleCommand("select COUNT(*) from all_users where username = :USUARIO ", conexion);
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.Add("USUARIO", OracleDbType.Varchar2).Value = usuarioCPC.usuario.ToUpper();

                conexion.Open();

                resultado = Convert.ToInt32(cmd.ExecuteScalar().ToString());

                if (resultado > 0)
                {
                    existe = true;
                }
                return existe;

            }
            catch (Exception ex)
            {
                return existe;
                throw ex;
            }
            finally
            {
                conexion.Close();
            }
        }

        //ActualizarClave CPC
        public bool ActualizarClave(CPC usuarioCPC)
        {
            OracleConnection conexion = null;
            OracleCommand cmd = null;
            bool resultado = false;

            try
            {
                conexion = ConexionOracle.ConexionBDOracle();
                cmd = new OracleCommand("DB_UTILIDADES.P_RESETEAR_CONTRASENA", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("p_usuario", OracleDbType.Varchar2).Value = usuarioCPC.usuario;
                cmd.Parameters.Add("p_clave", OracleDbType.Varchar2).Value = usuarioCPC.clave;
                cmd.Parameters.Add("p_Salida", OracleDbType.Int32).Direction = ParameterDirection.Output;

                conexion.Open();

                cmd.ExecuteNonQuery();

                int filas = int.Parse(cmd.Parameters["p_Salida"].Value.ToString());

                if (filas == 1) // El SP devuelve 1 si no hay ninguna exepción
                {
                    resultado = true;
                }

                return resultado;
            }
            catch (Exception ex)
            {
                return resultado;
                throw ex;
            }
            finally
            {
                conexion.Close();
            }
        }
    }


}