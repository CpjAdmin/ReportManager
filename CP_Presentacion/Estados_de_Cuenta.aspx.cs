using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CP_Entidades;
using CP_LogicaNegocio;
using System.Web.Services;
using System.Configuration;
using System.Net;
using System.Web.Script.Serialization;
using Microsoft.Reporting.WebForms;
using System.Security.Principal;

namespace CP_Presentacion
{
    public partial class Estados_de_Cuenta : System.Web.UI.Page
    {
        public static string servidor;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Usuario"] == null || (bool)Session["EstadoCuenta"] == false)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                if (!Page.IsPostBack)
                {
                    try
                    {
                        servidor = ConfigurationManager.AppSettings["EstadosServidorEnviosEmail"]; ;

                        // *** Cargar Controles
                        CargarControles();
                    }
                    catch (Exception ex)
                    {
                        string titulo = Path.GetFileName(Request.Url.AbsolutePath);
                        string mensaje = ex.Message;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "mensajeError('" + mensaje + "', '" + titulo + "');", true);
                    }

                }
            }
        }

        //********** Vaciar Directorio
        private static void VaciarDirectorio(string directorio, ArrayList archivosBloqueados)
        {
            DirectoryInfo dir = new DirectoryInfo(directorio);

            if (Directory.Exists(directorio))
            {
                foreach (FileInfo fi in dir.GetFiles())
                {
                    if (!archivosBloqueados.Contains(fi.Name))
                    {
                        fi.Delete();
                    }
                }
            }
        }
        //********** Archivo en Uso
        private static ArrayList ArchivosBloqueado(string directorio, ArrayList archivosBloqueados)
        {
            FileStream stream = null;

            DirectoryInfo dir = new DirectoryInfo(directorio);

            if (Directory.Exists(directorio))
            {
                foreach (FileInfo file in dir.GetFiles())
                {
                    try
                    {
                        stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
                    }
                    catch (IOException)
                    {
                        //El archivo no está disponible porque: ( Está abierto, Usado por otro proceso o No existe )
                        archivosBloqueados.Add(file.Name);
                    }
                    finally
                    {
                        if (stream != null)
                            stream.Close();
                    }
                }
            }

            return archivosBloqueados;
        }

        //********** Crear Directorio
        private static bool CrearDirectorio(String path)
        {
            bool creado = false;
            try
            {
                //*** Verificar no Existe.
                if (!Directory.Exists(path))
                {
                    // DirectoryInfo di = Directory.CreateDirectory(path);
                    //Response.Write("<script>alert('El directorio fue creado con éxito en "+ Directory.GetCreationTime(path)+"')</script>");

                    //*** Crear el directorio.
                    Directory.CreateDirectory(path);
                    creado = true;
                }

                return creado;
            }
            catch (Exception)
            {
                //Response.Write("<script>alert('El proceso falló: {0}'," + ex.Message + "')</script>");
                return creado;
            }
            finally { }
        }

        //********** Cargar Discos
        private ArrayList GetDiscos()
        {
            DriveInfo[] infoDiscos = DriveInfo.GetDrives();
            ArrayList ListaDiscos = new ArrayList();

            foreach (DriveInfo d in infoDiscos)
            {
                if (d.IsReady == true && d.TotalFreeSpace > 0)
                {
                    if (d.DriveType.ToString() == "Fixed" || d.DriveType.ToString() == "Network")
                    {
                        ListaDiscos.Add(d.Name);
                    }
                }
            }

            //** Reversar el Orden
            ListaDiscos.Reverse();

            return ListaDiscos;
        }

        //********** DropDownList Provincias
        private void CargarProvincias()
        {
            ddlProvincia.DataSource = ProvinciaLN.getInstancia().GetProvincias();
            ddlProvincia.DataTextField = "nombre";
            ddlProvincia.DataValueField = "cod_provincia";
            ddlProvincia.DataBind();
        }

        //********** DropDownList Centros
        private void CargarCentros()
        {
            ddlCentro.DataSource = CentroLN.getInstancia().GetCentros();
            ddlCentro.DataTextField = "nombre";
            ddlCentro.DataValueField = "cod_centro";
            ddlCentro.DataBind();
        }

        //********** Cargar Controles
        private void CargarControles()
        {
            //DropDownList Modal Discos
            ddlDisco.DataSource = GetDiscos();
            ddlDisco.DataBind();

            //********** DropDownList Provincias
            CargarProvincias();

            //********** DropDownList Centros
            CargarCentros();
        }

        // WebMethod - Cargar Cantones
        [WebMethod]
        public static List<Canton> CargarCantones(string provinciaId)
        {
            List<Canton> listaCantones = new List<Canton>();

            //********** DropDownList Cantones
            listaCantones = CantonLN.getInstancia().GetCantones(provinciaId);

            return listaCantones;
        }

        // WebMethod - Cargar Distritos
        [WebMethod]
        public static List<Distrito> CargarDistritos(string provinciaId, string cantonId)
        {
            List<Distrito> listaDistritoes = new List<Distrito>();

            //********** DropDownList Distritoes
            listaDistritoes = DistritoLN.getInstancia().GetDistritos(provinciaId, cantonId);

            return listaDistritoes;
        }

        // WebMethod - Cargar Instituciones
        [WebMethod]
        public static List<Institucion> CargarInstituciones(string centroId)
        {
            List<Institucion> listaInstituciones = new List<Institucion>();

            //********** DropDownList Cantones
            listaInstituciones = InstitucionLN.getInstancia().GetInstituciones(centroId);

            return listaInstituciones;
        }

        // WebMethod - Cargar LugaresTrabajo
        [WebMethod]
        public static List<LugarTrabajo> CargarLugaresTrabajo(string centroId, string institucionId)
        {
            List<LugarTrabajo> listaLugarTrabajo = new List<LugarTrabajo>();

            //********** DropDownList Distritoes
            listaLugarTrabajo = LugarTrabajoLN.getInstancia().GetLugarTrabajo(centroId, institucionId);

            return listaLugarTrabajo;
        }

        // WebMethod - Cargar Asociados
        [WebMethod]
        public static List<Asociado> CargarAsociados(BusquedaAsociado data)
        {
            List<Asociado> listaAsociados;
            AsociadoLN oAsociadoLN = AsociadoLN.getInstancia();

            string codigo;
            string identificacion;
            string cod_provincia;
            string cod_canton;
            string cod_distrito;
            string cod_centro;
            string cod_institucion;
            string cod_lugar;
            string con_email;
            string tipo_consulta;
            int ult_cod_cliente_gen;

            try
            {
                //*** Extracción de los Datos JObjects NO Funciona en WebMethod
                codigo = data.codigo;
                identificacion = data.identificacion;
                cod_provincia = data.cod_provincia;
                cod_canton = data.cod_canton;
                cod_distrito = data.cod_distrito;
                cod_centro = data.cod_centro;
                cod_institucion = data.cod_institucion;
                cod_lugar = data.cod_lugar;
                con_email = data.con_email;
                tipo_consulta = data.tipo_consulta;
                ult_cod_cliente_gen = data.ult_cod_cliente_gen;

                BusquedaAsociado oBusquedaAsociado = new BusquedaAsociado
                {
                    codigo = codigo,
                    identificacion = identificacion,
                    cod_provincia = cod_provincia,
                    cod_canton = cod_canton,
                    cod_distrito = cod_distrito,
                    cod_centro = cod_centro,
                    cod_institucion = cod_institucion,
                    cod_lugar = cod_lugar,
                    con_email = con_email,
                    tipo_consulta = tipo_consulta.ToUpper(),
                    ult_cod_cliente_gen = ult_cod_cliente_gen
                };

                //*** Carga de Asociados
                listaAsociados = oAsociadoLN.GetAsociados(oBusquedaAsociado);

            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.StatusCode = 500;

                if (HttpContext.Current.Session["Usuario"] != null)
                {
                    throw new Exception("Se ha producido un error al cargar los asociados de PSBANK ( " + ex.Message + " )", ex);
                }
                else
                {
                    throw new Exception("Su sesion ha expirado, sera redireccionado a la página principal");
                }
            }

            return listaAsociados;
        }

        //**********Generar Estados
        [WebMethod]
        public static string GenerarEstados(string[] codigos, string[] cedulas, string ruta, string formato, string id, string borrarDirectorio)
        {
            string directorio;
            string msj = "";
            string mensajeDetalle;
            string json = "";
            //string mensaje = "";  --TRY 1

            ArrayList archivosBloqueados = new ArrayList();
            ArrayList archivosBloqueadosGenerados = new ArrayList();
            ArrayList archivosConErrores = new ArrayList();

            JavaScriptSerializer Javaserializer = new JavaScriptSerializer();

            string[] resultado = new string[2];

            try
            {
                int id_usuario = Int32.Parse(id);

                //*** Directorio Seleccionado
                directorio = ruta;
                //*** Formato Seleccionado
                string formatoArchivo = formato;

                //Verificar archivos blqueados
                archivosBloqueados = ArchivosBloqueado(directorio, archivosBloqueados);

                //*** VaciarDirectorio
                if (borrarDirectorio == "S")
                {
                    //Borra todos los archivos, menos los bloqueados.
                    VaciarDirectorio(directorio, archivosBloqueados);
                }

                //CrearDirectorio
                CrearDirectorio(directorio);

                string Servidor = ConfigurationManager.AppSettings["ReportServer"];
                string Reporte = ConfigurationManager.AppSettings["rpt_EstadoCuenta"];
                string URLReport = Servidor + "?" + Reporte;
                string Command = "Render";

                //formatoArchivo del Estado de Cuenta
                string Format = formatoArchivo;
                string ext = formatoArchivo == "PDF" ? "pdf" : "xls";
                //Nombre Archivo
                //string nombreArchivo = "EstadoCuenta";

                URLReport = URLReport + "&rs:Command=" + Command + "&rs:Format=" + Format;

                //Convierte String[] a int[] 
                int[] intCodigos = Array.ConvertAll(codigos, delegate (string s) { return int.Parse(s); });

                //***COMPANIA
                string compania = ConfigurationManager.AppSettings["EstadosMasivoCompania"];
                //***COD_COMPANIA
                string cod_compania = ConfigurationManager.AppSettings["EstadosMasivoCodCompania"];

                //string paramCultureName = "es-GT";        //Cultura
                string paramCodigoCompania = cod_compania;  //"01001001";
                string paramNombreCompania = compania;      //"COOPECAJA RL";
                string paramCodigoUsuario = ConfigurationManager.AppSettings["EstadosMasivoUsuario"];
                string paramPFecha = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");

                string CodInicial = intCodigos[0].ToString();
                string CodFinal = intCodigos[intCodigos.GetUpperBound(0)].ToString();

                //*** Auditoría = intCodigos.GetUpperBound(0).ToString();
                Auditoria(id_usuario, 11, formatoArchivo + " - " + directorio + " - CodInicio: " + CodInicial + " - CodFinal: " + CodFinal);

                //*** Si hay datos en la tabla iniciar el proceso
                if (intCodigos.Length > 0)
                {
                    //*** Definición URL Inicial
                    string URL = URLReport + "&NombreCompania=" + paramNombreCompania
                                             + "&P_COD_COMPANIA=" + paramCodigoCompania
                                             + "&P_USUARIO=" + paramCodigoUsuario
                                             + "&PFECHA=" + paramPFecha;

                    string fechaHora = DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss");

                    using (System.IO.StreamWriter Log = new System.IO.StreamWriter(directorio + "\\Log" + id + "_" + fechaHora + ".txt"))
                    {
                        Log.WriteLine("Generación de Estados de Cuenta - " + fechaHora);

                        for (int i = 0; i < intCodigos.Length; i++)
                        {
                            //*** Cédula y Código de Cliente
                            string cedulaCliente = cedulas[i].ToString().Trim();
                            Int32 paramPCOD_CLIENTE = intCodigos[i];

                            //*** URL Final
                            string URLFinal = URL + "&P_COD_CLIENTE=" + paramPCOD_CLIENTE;

                            System.Net.HttpWebRequest Req = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(URLFinal);

                            //System.Net.CredentialCache.DefaultCredentials;
                            Req.Credentials = ICredenciales();
                            Req.Method = "GET";

                            //Especifique la ruta para guardar.
                            string NombreArchivo = cedulaCliente + @"." + ext;
                            string path = directorio + "\\" + NombreArchivo + "";

                            // Si no está bloqueado lo crea
                            if (!archivosBloqueados.Contains(NombreArchivo))
                            {
                                //Controla los errores provenientes de SSRS
                                try
                                {
                                    using (WebResponse objResponse = Req.GetResponse())
                                    {
                                        System.IO.FileStream fs = new System.IO.FileStream(path, System.IO.FileMode.Create);
                                        System.IO.Stream stream = objResponse.GetResponseStream();

                                        byte[] buf = new byte[1024];
                                        int len = stream.Read(buf, 0, 1024);
                                        while (len > 0)
                                        {
                                            fs.Write(buf, 0, len);
                                            len = stream.Read(buf, 0, 1024);
                                        }
                                        stream.Close();
                                        fs.Close();

                                        Log.WriteLine(NombreArchivo);
                                    }
                                }
                                catch (WebException) // ex)  // TRY 1
                                {
                                    archivosConErrores.Add(NombreArchivo);

                                    //using (WebResponse response = ex.Response)
                                    //{
                                    //    HttpWebResponse httpResponse = (HttpWebResponse)response;
                                    //    mensaje = "Error codigo: " + httpResponse.StatusCode;

                                    //    using (Stream data = response.GetResponseStream())
                                    //    using (var reader = new StreamReader(data))
                                    //    {
                                    //        string text = reader.ReadToEnd();
                                    //        mensaje = " - " + text + " - ";
                                    //        return mensaje;
                                    //    }
                                    //}
                                }
                            }
                            else
                            {
                                archivosBloqueadosGenerados.Add(NombreArchivo);
                            }
                        }

                        //*** Log Bloqueados y Error 
                        Log.WriteLine("Archivos Bloqueados: " + String.Join(" , ", archivosBloqueadosGenerados.ToArray()));
                        Log.WriteLine("Archivos con Error: " + String.Join(" , ", archivosConErrores.ToArray()));
                        Log.WriteLine();
                    }
                 

                    msj = "0";
                    string condicion = (archivosBloqueadosGenerados.Count > 0) ? "<br><br>Archivos en directorio " + directorio + " que están Abiertos <b class='text-danger'>(NO GENERADOS)</b>:<br>" + String.Join(" , ", archivosBloqueadosGenerados.ToArray()) : "";
                    string condicionError = (archivosConErrores.Count > 0) ? "<br><br>Archivos con Errores <b class='text-danger'>(NO GENERADOS)</b>:<br>" + String.Join(" , ", archivosConErrores.ToArray()) : "";
                    mensajeDetalle = "<b>Proceso ejecutado con exito!</b>" + condicion + condicionError;
                    resultado = new string[] { msj, mensajeDetalle };

                    json = Javaserializer.Serialize(new { resultado });
                }
                else
                {
                    msj = "1";
                    mensajeDetalle = "<b>No hay Estados de Cuenta para procesar!</b>";
                    resultado = new string[] { msj, mensajeDetalle };

                    json = Javaserializer.Serialize(new { resultado });

                }
                return json;
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.StatusCode = 500;

                if (HttpContext.Current.Session["Usuario"] != null)
                {
                    throw new Exception("Se ha producido un error al generar los Estados de Cuenta ( " + ex.Message + " )", ex);
                }
                else
                {
                    throw new Exception("Su sesion ha expirado, sera redireccionado a la página principal");
                }
            }
        }

        //**********Enviar Estados
        [WebMethod]
        public static string EnviarEstadoCuenta(string codigo, string cedula, string nombre, string email, string ruta, string id)
        {
            string directorio;
            string directorioEnvio;
            string msj = "";
            string mensajeDetalle = "";
            string json = "";
            string formato = "pdf";

            ArrayList archivosBloqueados = new ArrayList();
            ArrayList archivosBloqueadosGenerados = new ArrayList();
            ArrayList archivosConErrores = new ArrayList();

            JavaScriptSerializer Javaserializer = new JavaScriptSerializer();

            string[] resultado = new string[2];

            try
            {
                int id_usuario = Int32.Parse(id);

                //*** Directorio Seleccionado
                directorio = ruta;
                directorioEnvio = "\\\\" + servidor + ruta.Substring(2);

                //*** Formato Seleccionado
                string formatoArchivo = formato;

                //Verificar archivos blqueados
                archivosBloqueados = ArchivosBloqueado(directorio, archivosBloqueados);

                //CrearDirectorio
                CrearDirectorio(directorio);

                string Servidor = ConfigurationManager.AppSettings["ReportServer"];
                string Reporte = ConfigurationManager.AppSettings["rpt_EstadoCuenta"];
                string URLReport = Servidor + "?" + Reporte;
                string Command = "Render";

                //formatoArchivo del Estado de Cuenta
                string Format = formatoArchivo;
                string ext = formatoArchivo;

                URLReport = URLReport + "&rs:Command=" + Command + "&rs:Format=" + Format;

                //***COMPANIA
                string compania = ConfigurationManager.AppSettings["EstadosMasivoCompania"];
                //***COD_COMPANIA
                string cod_compania = ConfigurationManager.AppSettings["EstadosMasivoCodCompania"];

                string paramCodigoCompania = cod_compania;
                string paramNombreCompania = compania;
                string paramCodigoUsuario = ConfigurationManager.AppSettings["EstadosMasivoUsuario"]; ;
                string paramPFecha = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");


                string cedulaCliente = cedula;
                Int32 paramPCOD_CLIENTE = Int32.Parse(codigo);

                String URL = URLReport + "&NombreCompania=" + paramNombreCompania
                                       + "&P_COD_COMPANIA=" + paramCodigoCompania
                                       + "&P_COD_CLIENTE=" + paramPCOD_CLIENTE
                                       + "&P_USUARIO=" + paramCodigoUsuario
                                       + "&PFECHA=" + paramPFecha;

                System.Net.HttpWebRequest Req = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(URL);

                //System.Net.CredentialCache.DefaultCredentials;
                Req.Credentials = ICredenciales();
                Req.Method = "GET";

                //Especifique la ruta para guardar.
                string path = directorio + "\\" + cedulaCliente + @"." + ext + "";
                string NombreArchivo = cedulaCliente + @"." + ext;
                string pathEnvio = directorioEnvio + @"\" + NombreArchivo + "";

                // Si no está bloqueado lo crea
                if (!archivosBloqueados.Contains(NombreArchivo))
                {
                    //Controla los errores provenientes de SSRS
                    try
                    {
                        using (WebResponse objResponse = Req.GetResponse())
                        {
                            System.IO.FileStream fs = new System.IO.FileStream(path, System.IO.FileMode.Create);
                            System.IO.Stream stream = objResponse.GetResponseStream();

                            byte[] buf = new byte[1024];
                            int len = stream.Read(buf, 0, 1024);
                            while (len > 0)
                            {
                                fs.Write(buf, 0, len);
                                len = stream.Read(buf, 0, 1024);
                            }
                            stream.Close();
                            fs.Close();
                        }
                    }
                    catch (WebException)
                    {
                        archivosConErrores.Add(NombreArchivo);
                        msj = "-1";
                        mensajeDetalle = "NO Generado";
                    }
                }
                else
                {
                    archivosBloqueadosGenerados.Add(NombreArchivo);
                    msj = "-1";
                    mensajeDetalle = "Bloqueado";
                }

                //Enviar por Email
                if (archivosBloqueadosGenerados.Count == 0 || archivosConErrores.Count == 0)
                {
                    bool resultadoEnvio = EnviarEstadoCuentaEmail(id_usuario, cedula, nombre, email, pathEnvio);

                    if (resultadoEnvio)
                    {
                        msj = "0";
                        mensajeDetalle = "Enviado";

                        //*** Auditoría
                        Auditoria(id_usuario, 29, "Cédula: " + cedula + ", Email: " + email + ", Directorio: " + directorioEnvio);
                    }
                    else
                    {
                        msj = "-2";
                        mensajeDetalle = "No Enviado";
                    }
                }

                resultado = new string[] { msj, mensajeDetalle };

                json = Javaserializer.Serialize(new { resultado });

                return json;
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.StatusCode = 500;

                if (HttpContext.Current.Session["Usuario"] != null)
                {
                    throw new Exception("Se ha producido un error al enviar el Estado de Cuenta ( " + ex.Message + " )", ex);
                }
                else
                {
                    throw new Exception("Su sesion ha expirado, sera redireccionado a la página principal");
                }
            }
        }

        [WebMethod]
        public static bool EnviarEstadoCuentaEmail(int id_usuario, string cedula, string nombre, string email, string ruta)
        {
            bool resultadoEnvio = false;

            EstadoCuenta oEstadoCuenta = new EstadoCuenta
            {
                tipo_consulta = "UNIFICADO",
                id_usuario = id_usuario,
                identificacion = cedula,
                nombre = nombre,
                email = email,
                ruta_envio = ruta
            };


            try
            {
                resultadoEnvio = ProcesoLN.getInstancia().EnviarEstadoCuentaEmail(oEstadoCuenta);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return resultadoEnvio;
        }


        // Implementación de ICredentials 
        public static ICredentials ICredenciales()
        {
            // Usuario
            string usuario = ConfigurationManager.AppSettings["ReportViewerUsuario"];

            if (string.IsNullOrEmpty(usuario))
                throw new Exception(
                    "Falta el usuario del archivo web.config");
            // Clave
            string clave = ConfigurationManager.AppSettings["ReportViewerClave"];

            if (string.IsNullOrEmpty(clave))
                throw new Exception(
                    "Falta la contraseña del archivo web.config");
            // Dominio
            string dominio = ConfigurationManager.AppSettings["ReportViewerDominio"];

            if (string.IsNullOrEmpty(dominio))
                throw new Exception(
                    "Falta el dominio del archivo web.config");

            return new NetworkCredential(usuario, clave, dominio);

        }

        //Auditoría
        public static void Auditoria(int id_usuario, int id_proceso, string detalle)
        {
            try
            {
                //*** Auditoría
                int id = id_usuario;
                int cod_proceso = id_proceso;
                string descripcion = detalle;

                Proceso proceso = new Proceso
                {
                    id_usuario = id,
                    cod_proceso = cod_proceso,
                    descripcion = descripcion
                };

                ProcesoLN.getInstancia().AuditarProceso(proceso, System.Web.HttpContext.Current);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

