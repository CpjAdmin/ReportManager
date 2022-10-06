using CP_Entidades;
using CP_Entidades.Response;
using CP_LogicaNegocio;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Script.Serialization;

namespace RM_WebAPI.Controllers
{
    public class EstadoCuentaTDController : ApiController
    {
        //*** VERIFICADO - LISTA DE SECUNDAR MOCIONES
        [HttpPost]
        public IHttpActionResult CargarAsociados([FromBody] JObject data)
        {
            AsociadoLN oAsociadoLN = AsociadoLN.getInstancia();

            Respuesta oRespuesta = new Respuesta();
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
                //*** Extracción de JObject
                dynamic oEventoRequest = data.ToObject<dynamic>();
                codigo = oEventoRequest.codigo;
                identificacion = oEventoRequest.identificacion;
                cod_provincia = oEventoRequest.cod_provincia;
                cod_canton = oEventoRequest.cod_canton;
                cod_distrito = oEventoRequest.cod_distrito;
                cod_centro = oEventoRequest.cod_centro;
                cod_institucion = oEventoRequest.cod_institucion;
                cod_lugar = oEventoRequest.cod_lugar;
                con_email = oEventoRequest.con_email;
                tipo_consulta = oEventoRequest.tipo_consulta;
                ult_cod_cliente_gen = oEventoRequest.ult_cod_cliente_gen;

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

                var lista = oAsociadoLN.GetAsociados(oBusquedaAsociado);

                if (lista.Count > 0)
                {
                    oRespuesta.Mensaje = "Exito";
                }
                else
                {
                    oRespuesta.Mensaje = "No se encontraton asociados que cumplan con los filtros ingresados";
                }

                oRespuesta.Exito = 1;
                oRespuesta.Data = lista;
            }
            catch (Exception ex)
            {
                oRespuesta.Mensaje = ex.InnerException == null ? ex.Message : ex.Message + " --> " + ex.InnerException.Message;
            }

            return Ok(oRespuesta);
        }

        //*** VERIFICADO - ENVIAR ESTADO DE CUENTA
        [HttpPost]
        public IHttpActionResult EnviarEstadoCuentaTD([FromBody] EstadoCuenta oEstadoCuenta)
        {
            Respuesta oRespuesta = new Respuesta();

            ArrayList archivosBloqueados = new ArrayList();
            ArrayList archivosBloqueadosGenerados = new ArrayList();
            ArrayList archivosConErrores = new ArrayList();

            //*** Auditoria
            int id_proceso = 38;

            string directorio;
            string directorioEnvio;
            string formatoArchivo = "pdf";

            //*** Formato y Patch del archivo de Estado de Cuenta
            string Format = formatoArchivo;
            string ext = formatoArchivo;
            string pathEnvio;

            try
            {

                //*** Valores del archivo Web.config
                string ServidorEnvios = ConfigurationManager.AppSettings["EstadosTD_ServidorEnvios"];
                string ServidorSSRS = ConfigurationManager.AppSettings["ReportServer"];
                string Reporte = ConfigurationManager.AppSettings["EstadosTD_Rpt_AhorroVista"];

                string URLReport = ServidorSSRS + "?" + Reporte;
                string Command = "Render";
                string URLReportCompleto = URLReport + "&rs:Command=" + Command + "&rs:Format=" + Format;

                //*** Directorio de Generación
                string IDPruebas = ConfigurationManager.AppSettings["Estados_PruebasEnvio"];
                if (IDPruebas == "S")
                {
                    string RutaPruebas = ConfigurationManager.AppSettings["Estados_RutaGeneracion"];
                    directorio = RutaPruebas + oEstadoCuenta.ruta_genera.Substring(2);
                }
                else
                {   //*** Ruta Local del Servidor IIS
                    directorio = oEstadoCuenta.ruta_genera;
                }

                //*** 1 - Directorio de Envío
                directorioEnvio = @"\\" + ServidorEnvios + oEstadoCuenta.ruta_genera.Substring(2);
                //*** 2 - Despues de Mapear la ruta original, la sustituyo por la nueva
                oEstadoCuenta.ruta_genera = directorio;

                //*** Verificar archivos bloqueados
                archivosBloqueados = ArchivosBloqueado(directorio, archivosBloqueados);

                //*** CrearDirectorio
                CrearDirectorio(directorio);

                //************************************************ INICIO Generación del reporte SSRS 
                //*** Parametros del Reporte
                string rpt_p_Identificacion = oEstadoCuenta.identificacion;
                int rpt_p_NumContrato = oEstadoCuenta.num_contrato;
                string rpt_p_FechaCorte = Convert.ToDateTime(oEstadoCuenta.fec_corte).ToString(new CultureInfo("en-US"));
                /* 
                int rpt_p_CodCliente = cod_cliente;
                */

                //*** Definición URL Inicial
                string URL = URLReportCompleto + "&IP_VCH_CEDULA=" + rpt_p_Identificacion
                                               + "&IP_INT_NUM_CONTRATO=" + rpt_p_NumContrato
                                               + "&IP_DT_FECHA_CORTE=" + rpt_p_FechaCorte;
                HttpWebRequest Req = (HttpWebRequest)WebRequest.Create(URL);

                //*** Definir Credenciales de Acceso al SSRS - ICredenciales()
                Req.Credentials = ICredenciales();
                Req.Method = "GET";

                //*** Nombre Fecha de Corte
                String[] dateParts = oEstadoCuenta.fec_corte.Split('/');
                string archivo_ano = dateParts[2];
                string archivo_mes = new DateTime(Int32.Parse(dateParts[2]), Int32.Parse(dateParts[1]), Int32.Parse(dateParts[0])).ToString("MMMM");

                //*** Especifique la ruta para guardar el Estado de Cuenta
                string NombreArchivo = rpt_p_Identificacion + "-" + rpt_p_NumContrato.ToString() + "-" + archivo_mes + "-" + archivo_ano + @"." + ext;
                oEstadoCuenta.archivo = NombreArchivo;

                string pathGeneracion = directorio + @"\" + NombreArchivo;
                pathEnvio = directorioEnvio + @"\" + NombreArchivo + "";
                oEstadoCuenta.ruta_envio = pathEnvio;

                //*** Si el estado de cuenta en proceso no está en la lista de bloqueados lo GENERA
                if (!archivosBloqueados.Contains(NombreArchivo))
                {
                    //*** Controla los errores provenientes de SSRS al generar el reporte
                    try
                    {
                        using (WebResponse objResponse = Req.GetResponse())
                        {
                            FileStream fs = new FileStream(pathGeneracion, FileMode.Create);
                            Stream stream = objResponse.GetResponseStream();

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
                    catch (WebException ex)
                    {
                        archivosConErrores.Add(NombreArchivo);
                        var mensaje = "";

                        using (WebResponse response = ex.Response)
                        {
                            HttpWebResponse httpResponse = (HttpWebResponse)response;
                            mensaje = "Error codigo: " + httpResponse.StatusCode;

                            using (Stream data = response.GetResponseStream())
                            using (var reader = new StreamReader(data))
                            {
                                string text = reader.ReadToEnd();
                                mensaje = text;
                            }
                        }

                        oRespuesta.Exito = -1;
                        oRespuesta.Mensaje = "No Generado " + mensaje;
                    }
                }
                else
                {
                    archivosBloqueadosGenerados.Add(NombreArchivo);
                    oRespuesta.Exito = -2;
                    oRespuesta.Mensaje = "Archivo Bloqueado - " + pathEnvio;
                }
                //************************************************ FIN Generación en reporte SSRS 

                //************************************************ INICIO Envio por Email
                //*** Enviar el estado de cuenta generado por Email
                if (!archivosBloqueadosGenerados.Contains(NombreArchivo) && !archivosConErrores.Contains(NombreArchivo))
                {

                    //*** Llamado al proceso de Envío de Estado de Cuenta
                    var oResultado = EnviarEstadoCuentaEmail(oEstadoCuenta);

                    //*** Resultado del WepApi Method, resultado tipo IHttpActionResult
                    if (oResultado is OkNegotiatedContentResult<Respuesta>)
                    {
                        // Here's how you can do it. 
                        var resultado = oResultado as OkNegotiatedContentResult<Respuesta>;
                        var content = resultado.Content;

                        if (content.Exito == 1)
                        {
                            oRespuesta.Exito = 1;
                            oRespuesta.Mensaje = "Enviado - " + oEstadoCuenta.ruta_envio;

                            //*** Auditoría
                            string mensajeResponse = "Cédula: " + oEstadoCuenta.identificacion + ", Email: " + oEstadoCuenta.email + ", Contrato: " + oEstadoCuenta.num_contrato.ToString() + ", Directorio: " + oEstadoCuenta.ruta_genera + ", Archivo: " + oEstadoCuenta.archivo;
                            Auditoria(oEstadoCuenta.id_usuario, id_proceso, mensajeResponse);
                        }
                        else
                        {
                            oRespuesta.Exito = -3;
                            oRespuesta.Mensaje = "No Enviado - " + pathEnvio;
                        }
                    }
                    else
                    {
                        oRespuesta.Exito = -4;
                        oRespuesta.Mensaje = "No Enviado - " + pathEnvio;
                    }
                }
                //************************************************ FIN Envio por Email

                oRespuesta.Data = oEstadoCuenta;
            }
            catch (Exception ex)
            {
                oRespuesta.Mensaje = ex.InnerException == null ? ex.Message : ex.Message + " --> " + ex.InnerException.Message;
            }

            return Ok(oRespuesta);
        }
        [HttpPost]
        public IHttpActionResult EnviarEstadoCuentaEmail([FromBody] EstadoCuenta oEstadoCuenta)
        {
            Respuesta oRespuesta = new Respuesta();

            try
            {
                bool resultadoEnvio = ProcesoLN.getInstancia().EnviarEstadoCuentaEmail(oEstadoCuenta);
                if (resultadoEnvio)
                {

                    string mensajeResponse = "Cédula: " + oEstadoCuenta.identificacion + ", Email: " + oEstadoCuenta.email + ", Contrato: " + oEstadoCuenta.num_contrato.ToString() + ", Directorio: " + oEstadoCuenta.ruta_genera + ", Archivo: " + oEstadoCuenta.archivo;

                    oRespuesta.Exito = 1;
                    oRespuesta.Mensaje = "Enviado - " + mensajeResponse;
                }
                else
                {
                    oRespuesta.Exito = -1;
                    oRespuesta.Mensaje = "No Enviado";
                }

                oRespuesta.Data = oEstadoCuenta;

            }
            catch (Exception ex)
            {
                oRespuesta.Mensaje = ex.InnerException == null ? ex.Message : ex.Message + " --> " + ex.InnerException.Message;
            }

            return Ok(oRespuesta);
        }

        //*** VERIFICADO - GENERAR ESTADOS DE CUENTA MASIVO
        [HttpPost]
        public IHttpActionResult GenerarEstadoCuentaTD([FromBody] EstadoMasivo oEstadoMasivo)
        {

            if (oEstadoMasivo == null)
            {
                throw new Exception("El parametro oEstadoMasivo del método GenerarEstadoCuentaTD no se pudo inicializar, el valor actual es NULL.");
            }

            Respuesta oRespuesta = new Respuesta();
            BitacoraEstadoEnc oBitacoraEnc = null;
            BitacoraEstadoDet oBitacoraDet = null;

            ArrayList archivosBloqueados = new ArrayList();
            ArrayList archivosBloqueadosGenerados = new ArrayList();
            ArrayList archivosConErrores = new ArrayList();

            //*** Auditoria
            int id_proceso = 37;

            string directorio;
            string directorioGenerarMasivo;
            string formatoArchivo = oEstadoMasivo.formato.ToLower();

            int cantidadContratos = 0;
            string procesoSP;
            int sp_id_proceso;
            string mensajeFinal;

            //*** Formato y Patch del archivo de Estado de Cuenta
            string Format = formatoArchivo;
            string ext = formatoArchivo;

            try
            {
                //*** Valores del archivo Web.config
                string ServidorGeneracion = ConfigurationManager.AppSettings["EstadosTD_ServidorGeneracion"];
                string ServidorSSRS = ConfigurationManager.AppSettings["ReportServer"];
                string Reporte = ConfigurationManager.AppSettings["EstadosTD_Rpt_AhorroVista"];

                string URLReport = ServidorSSRS + "?" + Reporte;
                string Command = "Render";
                string URLReportCompleto = URLReport + "&rs:Command=" + Command + "&rs:Format=" + Format;

                //*** Directorio de Generación
                string IDPruebas = ConfigurationManager.AppSettings["Estados_PruebasEnvio"];
                if (IDPruebas == "S")
                {
                    string RutaPruebas = ConfigurationManager.AppSettings["Estados_RutaGeneracion"];
                    directorio = RutaPruebas + oEstadoMasivo.ruta.Substring(2);
                }
                else
                {   //*** Ruta Local del Servidor IIS
                    directorio = oEstadoMasivo.ruta;
                }

                //*** 1 - Directorio de Generación
                directorioGenerarMasivo = @"\\" + ServidorGeneracion + oEstadoMasivo.ruta.Substring(2);

                //*** Verificar archivos bloqueados
                archivosBloqueados = ArchivosBloqueado(directorio, archivosBloqueados);

                //*** CrearDirectorio
                CrearDirectorio(directorio);

                //Verificar archivos blqueados
                archivosBloqueados = ArchivosBloqueado(directorio, archivosBloqueados);

                //*** VaciarDirectorio 
                if (oEstadoMasivo.borrar_directorio == "S")
                {
                    // Borra todos los archivos, menos los bloqueados.
                    VaciarDirectorio(directorio, archivosBloqueados);
                }

                //************************************************ INICIO Generación del reporte SSRS 
                //*** Parametros del Reporte
                string rpt_p_FechaCorte = Convert.ToDateTime(oEstadoMasivo.fec_corte).ToString(new CultureInfo("en-US"));

                //*** Código de Cliente Inicial y Final
                string CodInicial = oEstadoMasivo.lista_contratos[0].cod_cliente.ToString();
                string CodFinal = oEstadoMasivo.lista_contratos[oEstadoMasivo.lista_contratos.GetUpperBound(0)].cod_cliente.ToString();

                //*** Si existen registros en el arreglo "CODIGOS", se inicia el proceso
                cantidadContratos = oEstadoMasivo.lista_contratos.Length;

                if (cantidadContratos > 0)
                {
                    //*** Request 
                    string oRequest = "num_lote: " + oEstadoMasivo.num_lote.ToString() +
                                      ", fec_corte: " + oEstadoMasivo.fec_corte +
                                      ", lista_contratos: " + oEstadoMasivo.lista_contratos.Length.ToString() +
                                      ", ruta: " + oEstadoMasivo.ruta +
                                      ", formato: " + oEstadoMasivo.formato +
                                      ", borrar_directorio: " + oEstadoMasivo.borrar_directorio +
                                      ", id_usuario: " + oEstadoMasivo.id_usuario.ToString() +
                                      ", navegador: " + oEstadoMasivo.navegador;

                    //*** Terminal   
                    string terminal = ProcesoLN.getInstancia().GetIPAddress(HttpContext.Current);

                    //*** Auditoría 
                    Auditoria(oEstadoMasivo.id_usuario, id_proceso, "Fecha Corte - " + oEstadoMasivo.fec_corte + " - " + formatoArchivo + " - " + directorio + " - Cod_Cliente_Inicio: " + CodInicial + " - Cod_Cliente_Final: " + CodFinal);

                    //*** Crear oBitacoraEnc
                    oBitacoraEnc = new BitacoraEstadoEnc
                    {
                        cod_proceso = id_proceso,
                        cod_usuario = oEstadoMasivo.id_usuario,
                        pagina = "Estados_de_Cuenta_TD.aspx",
                        num_lote = oEstadoMasivo.num_lote,
                        num_registros = cantidadContratos,
                        cod_cliente_inicio = Int32.Parse(CodInicial),
                        cod_cliente_final = Int32.Parse(CodFinal),
                        i_pruebas = IDPruebas,
                        i_borrar_dir = oEstadoMasivo.borrar_directorio,
                        fecha_corte = oEstadoMasivo.fec_corte,
                        servidor_genera = ServidorGeneracion,
                        servidor_ssrs = ServidorSSRS,
                        url_ssrs = URLReportCompleto,
                        dir_local = oEstadoMasivo.ruta,
                        dir_remoto = directorioGenerarMasivo,
                        archivos_bloqueados = archivosBloqueadosGenerados.Count,
                        archivos_errores = archivosConErrores.Count,
                        request = oRequest,
                        response = "",
                        navegador = oEstadoMasivo.navegador,
                        terminal_id = terminal
                    };

                    //*** ID del proceso ejecutado en el SP
                    procesoSP = "C";
                    sp_id_proceso = BitacoraEstadoCuentaEnc(oBitacoraEnc, procesoSP);

                    //*** Validar ID generado de oBitacoraEnc
                    if (sp_id_proceso > 0)
                    {
                        //*** Actualizar oBitacoraEnc
                        oBitacoraEnc.id = sp_id_proceso;

                        //*** LOG - Fecha y Hora de Generación
                        string fechaHora = DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss");

                        //*** Nombre Archivo - Fecha de Corte
                        String[] dateParts = oEstadoMasivo.fec_corte.Split('/');
                        string archivo_ano = dateParts[2];
                        string archivo_mes = new DateTime(Int32.Parse(dateParts[2]), Int32.Parse(dateParts[1]), Int32.Parse(dateParts[0])).ToString("MMMM");

                        //*** Ciclo de generación de Estados de Cuenta ( oEstadoMasivo.lista_contratos )
                        for (int i = 0; i < cantidadContratos; i++)
                        {

                            //*** Parametros del Reporte
                            int p_Cod_Cliente = oEstadoMasivo.lista_contratos[i].cod_cliente;
                            string p_Identificacion = oEstadoMasivo.lista_contratos[i].identificacion.Trim();
                            int p_NumContrato = oEstadoMasivo.lista_contratos[i].num_contrato;
                            //*** Asignación
                            string rpt_p_Cod_Cliente = p_Cod_Cliente.ToString().Trim();
                            string rpt_p_Identificacion = p_Identificacion;
                            int rpt_p_NumContrato = p_NumContrato;

                            //*** Definición URL Inicial
                            string URL = URLReportCompleto + "&IP_VCH_CEDULA=" + rpt_p_Identificacion
                                                           + "&IP_INT_NUM_CONTRATO=" + rpt_p_NumContrato
                                                           + "&IP_DT_FECHA_CORTE=" + rpt_p_FechaCorte;
                            //*** Request SSRS
                            HttpWebRequest Req = (HttpWebRequest)WebRequest.Create(URL);

                            //*** Definir Credenciales de Acceso al SSRS - ICredenciales()
                            Req.Credentials = ICredenciales();
                            Req.Method = "GET";

                            //*** Especifique la ruta para guardar el Estado de Cuenta
                            string NombreArchivo = rpt_p_Identificacion + "-" + rpt_p_NumContrato.ToString() + "-" + archivo_mes + "-" + archivo_ano + @"." + ext;
                            string pathGeneracion = directorio + @"\" + NombreArchivo;

                            //*** Crear oBitacoraDet
                            oBitacoraDet = new BitacoraEstadoDet()
                            {
                                id = sp_id_proceso,
                                num_lote = oEstadoMasivo.num_lote,
                                num_registro = i + 1,
                                cod_cliente = p_Cod_Cliente,
                                identificacion = p_Identificacion,
                                num_contrato = p_NumContrato,
                                response = NombreArchivo,
                            };

                            //*** Si el estado de cuenta en proceso no está en la lista de bloqueados lo GENERA
                            if (!archivosBloqueados.Contains(NombreArchivo))
                            {
                                //*** Controla los errores provenientes de SSRS al generar el reporte
                                try
                                {
                                    using (WebResponse objResponse = Req.GetResponse())
                                    {
                                        FileStream fs = new FileStream(pathGeneracion, FileMode.Create);
                                        Stream stream = objResponse.GetResponseStream();

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

                                    //*** Crear oBitacoraDet
                                    oBitacoraDet.estado = "Exito";
                                }
                                catch (WebException ex)
                                {
                                    archivosConErrores.Add(NombreArchivo);
                                    var mensaje = "";

                                    using (WebResponse response = ex.Response)
                                    {
                                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                                        mensaje = "Error codigo: " + httpResponse.StatusCode;

                                        using (Stream data = response.GetResponseStream())
                                        using (var reader = new StreamReader(data))
                                        {
                                            string text = reader.ReadToEnd();
                                            mensaje = text;
                                        }
                                    }

                                    //*** Crear oBitacoraDet
                                    oBitacoraDet.estado = "Error";
                                    oBitacoraDet.response = oBitacoraDet.response + " - " + mensaje;
                                }
                            }
                            else
                            {
                                //*** Agregar a archivos Bloqueados
                                archivosBloqueadosGenerados.Add(NombreArchivo);

                                //*** Crear oBitacoraDet
                                oBitacoraDet.estado = "Bloqueado";
                            }

                            //*** ID del proceso ejecutado en el SP
                            procesoSP = "C";
                            BitacoraEstadoCuentaDet(oBitacoraDet, procesoSP);
                        }
                    }

                    string validaBloqueados = (archivosBloqueadosGenerados.Count > 0) ? "</br></br><b>Directorio: </b>" + directorio + "</br></br><b class='text-danger'>BLOQUEADOS</b></br><div>" + String.Join(" </br>", archivosBloqueadosGenerados.ToArray()) + "</div>" : "";
                    string validaErrores = (archivosConErrores.Count > 0) ? "</br></br><b class='text-danger'>CON ERRORES</b></br><div>" + String.Join(" </br>", archivosConErrores.ToArray()) + "</div>" : "";
                    mensajeFinal = validaBloqueados + validaErrores;

                    oRespuesta.Mensaje = mensajeFinal != "" ? mensajeFinal : "Ejecutado con Exito"; ;
                    oRespuesta.Data = cantidadContratos;
                    oRespuesta.Exito = 1;
                }
                else
                {
                    mensajeFinal = "<b>No hay Estados de Cuenta por procesar!</b>";
                    oRespuesta.Mensaje = mensajeFinal;
                    oRespuesta.Data = cantidadContratos;
                    oRespuesta.Exito = 1;
                }

                //*** Actualizar oBitacoraEnc
                oBitacoraEnc.archivos_bloqueados = archivosBloqueadosGenerados.Count;
                oBitacoraEnc.archivos_errores = archivosConErrores.Count;
                oBitacoraEnc.response = oRespuesta.Mensaje;

                //*** ID del proceso ejecutado en el SP
                procesoSP = "U";
                sp_id_proceso = BitacoraEstadoCuentaEnc(oBitacoraEnc, procesoSP);
            }
            catch (Exception ex)
            {
                oRespuesta.Mensaje = "Se ha producido un error al generar los Estados de Cuenta ( " + ex.InnerException == null ? ex.Message : ex.Message + " --> " + ex.InnerException.Message + " )";
                oRespuesta.Data = cantidadContratos - archivosBloqueadosGenerados.Count - archivosConErrores.Count;

                //*** Actualizar oBitacoraEnc
                oBitacoraEnc.archivos_bloqueados = archivosBloqueadosGenerados.Count;
                oBitacoraEnc.archivos_errores = archivosConErrores.Count;
                oBitacoraEnc.response = oRespuesta.Mensaje;

                //*** ID del proceso ejecutado en el SP
                procesoSP = "U";
                sp_id_proceso = BitacoraEstadoCuentaEnc(oBitacoraEnc, procesoSP);
            }

            return Ok(oRespuesta);
        }

        //*** Archivo de Estado de Cuenta en Uso ( Ocupados por otro proceso )
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

        //*** Crear Directorio en el ServidorSSRS
        private static bool CrearDirectorio(String pathGeneracion)
        {
            bool creado = false;
            try
            {
                //*** Verificar no Existe.
                if (!Directory.Exists(pathGeneracion))
                {
                    // DirectoryInfo di = Directory.CreateDirectory(pathGeneracion);
                    // Response.Write("<script>alert('El directorio fue creado con éxito en "+ Directory.GetCreationTime(pathGeneracion)+"')</script>");

                    //*** Crear el directorio.
                    Directory.CreateDirectory(pathGeneracion);
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

        //*** Implementación de ICredentials para los credenciales de SSRS
        public static ICredentials ICredenciales()
        {
            //*** Usuario
            string usuario = ConfigurationManager.AppSettings["ReportViewerUsuario"];

            if (string.IsNullOrEmpty(usuario))
                throw new Exception(
                    "Falta el usuario del archivo web.config");
            //*** Clave
            string clave = ConfigurationManager.AppSettings["ReportViewerClave"];

            if (string.IsNullOrEmpty(clave))
                throw new Exception(
                    "Falta la contraseña del archivo web.config");
            //*** Dominio
            string dominio = ConfigurationManager.AppSettings["ReportViewerDominio"];

            if (string.IsNullOrEmpty(dominio))
                throw new Exception(
                    "Falta el dominio del archivo web.config");

            return new NetworkCredential(usuario, clave, dominio);

        }

        //*** Proceso Pistas de Auditoría
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

        //*** Vaciar Directorio
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

        //*** Proceso Pistas de Auditoría - Encabezado
        public static int BitacoraEstadoCuentaEnc(BitacoraEstadoEnc oBitacoraEnc, string proceso)
        {
            int respuesta;

            try
            {
                respuesta = ProcesoLN.getInstancia().BitacoraEstadoCuentaEnc(oBitacoraEnc, proceso);
            }
            catch (Exception)
            {
                throw;
            }

            return respuesta;
        }

        //*** Proceso Pistas de Auditoría - Detalle
        public static int BitacoraEstadoCuentaDet(BitacoraEstadoDet oBitacoraDet, string proceso)
        {
            int respuesta;

            try
            {
                respuesta = ProcesoLN.getInstancia().BitacoraEstadoCuentaDet(oBitacoraDet, proceso);
            }
            catch (Exception)
            {
                throw;
            }

            return respuesta;
        }
    }
}
