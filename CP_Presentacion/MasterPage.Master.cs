using CP_AccesoDatos;
using CP_Entidades;
using CP_LogicaNegocio;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CP_Presentacion
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {

        // string usuarioRpt;
        // Encriptar encriptar;

        // String directorio = "D";
        // String subdirectorio = "S";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Usuario"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                if (!Page.IsPostBack)
                {
                    try
                    {
                        string usuarioActivo = Session["Usuario"].ToString();

                        // Usuario Encriptado
                        //encriptar = new Encriptar();
                        //usuarioRpt = HttpUtility.UrlEncode(encriptar.Crypto(usuarioActivo));

                        String ID = Session["ID"].ToString();
                        String RolUsuario = Session["Rol"].ToString();

                        UsuarioID.Value = ID;
                        Rol.Value = RolUsuario;

                        // Codigo de Usuario
                        int Cod_Usuario = Int32.Parse(ID);
                        // Login
                        Login.Value = usuarioActivo;

                        // *** Cargar Controles
                        CargarComponentes(Cod_Usuario, usuarioActivo, RolUsuario);

                        //************************************************************************************* Arbol TreeView Nodos de Reporte
                        //DataTable dt = ReportesLN.getInstancia().CargarTreeview(0, directorio, 0);

                        //this.PopulateTreeView(dtParent: dt, parentId: 0, treeNode: null, nivel: 0);

                        // Define la cantidad de Reportes en Lista de Reportes ()
                        totalReportes.Text = CantidadReportes();
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                }
            }

        }

        public void CargarComponentes(int id_usuario, string usuarioActivo, string rol)
        {
            //********** Verificar Existencia de Usuarios
            bool existeEnCPC = false;
            bool existeEnSIC = false;
            bool existeEnCENDEISSS = false;

            //bool analisisCredido, analisisAhorros;
            bool procesoEstadoCuenta, procesoEstadoCuentaTD, procesoPerfiles;
            bool listaReportes;
            bool pbi, pbi_mesa1, pbi_consejo1, pbi_consejo2, pbi_consejo3, pbi_psbanker1, pbi_psbanker2, pbi_qmatic1;

            try
            {

                // *** Instancia de Usuario CPC
                CPC usuarioCPC = new CPC
                {
                    usuario = usuarioActivo
                };
                // *** Instancia de Usuario SIC
                Usuario usuarioSIC = new Usuario
                {
                    login = usuarioActivo
                };

                //Verificar Usuarios  
                existeEnCPC = CPCLN.getInstancia().VerificarUsuarioCPC(usuarioCPC);
                existeEnSIC = UsuarioLN.getInstancia().VerificaUsuarioSIC(usuarioSIC, "SIC");
                existeEnCENDEISSS = UsuarioLN.getInstancia().VerificaUsuarioSIC(usuarioSIC, "CENDEISSS");

                linkCPC.Visible = existeEnCPC;
                linkSIC.Visible = existeEnSIC;
                linkCendeisss.Visible = existeEnCENDEISSS;

                // ***Opciones Adminstrador
                if (rol != "ADMINISTRADOR")
                {
                    sidebarDerecha.Visible = false;
                    btnSidebarDerecho.Visible = false;
                }

                // ***Menú de Procesos
                procesoEstadoCuenta = ProcesoLN.getInstancia().VerificarPermiso(id_usuario, "li_menuProcesos_EstadoCuenta", "");
                procesoEstadoCuentaTD = ProcesoLN.getInstancia().VerificarPermiso(id_usuario, "li_menuProcesos_EstadoCuenta_TD", "");
                procesoPerfiles = ProcesoLN.getInstancia().VerificarPermiso(id_usuario, "MasterPage_menuProcesos", "");
                Session["EstadoCuenta"] = procesoEstadoCuenta;
                Session["EstadoCuentaTD"] = procesoEstadoCuentaTD;
                Session["Perfiles"] = procesoPerfiles;

                if (!procesoEstadoCuenta && !procesoEstadoCuentaTD && !procesoPerfiles)
                {
                    MasterPage_menuProcesos.Visible = false;
                }
                else
                {
                    li_menuProcesos_EstadoCuenta.Visible = procesoEstadoCuenta;
                    li_menuProcesos_EstadoCuenta_TD.Visible = procesoEstadoCuentaTD;
                    li_menuProcesos_Perfiles.Visible = procesoPerfiles;
                }

                // *** Menú Analisis de Información
                /*
                analisisCredido = ProcesoLN.getInstancia().VerificarPermiso(id_usuario, "li_menuAnalisisDatos_AnalisisCredito", "");
                analisisAhorros = ProcesoLN.getInstancia().VerificarPermiso(id_usuario, "li_menuAnalisisDatos_AnalisisAhorros", "");
                Session["AnalisisCredito"] = procesoPerfiles;
                Session["AnalisisAhorros"] = procesoPerfiles;

                if (!analisisAhorros && !analisisCredido)
                {
                    MasterPage_menuAnalisisDatos.Visible = false;
                }
                else
                {
                    li_menuAnalisisDatos_AnalisisCredito.Visible = analisisCredido;
                    li_menuAnalisisDatos_AnalisisAhorros.Visible = analisisAhorros;
                }
                */

                //*** Menú Lista de Reportes
                listaReportes = ProcesoLN.getInstancia().VerificarPermiso(id_usuario, "MasterPage_menuListaReportes", "");
                Session["ListaReportes"] = listaReportes;

                if (!listaReportes)
                {
                    MasterPage_menuListaReportes.Visible = false;
                }

                // *** Menú de Power BI
                pbi = ProcesoLN.getInstancia().VerificarPermiso(id_usuario, "", "PBI");
                Session["PBI"] = pbi;

                if (!pbi)
                {
                    menuPBI.Visible = false;

                }
                else
                {
                    pbi_mesa1 = ProcesoLN.getInstancia().VerificarPermiso(id_usuario, "li_menuPBI_mesaservicio_1", "");
                    pbi_consejo1 = ProcesoLN.getInstancia().VerificarPermiso(id_usuario, "li_menuPBI_Consejo_1", "");
                    pbi_consejo2 = ProcesoLN.getInstancia().VerificarPermiso(id_usuario, "li_menuPBI_Consejo_2", "");
                    pbi_consejo3 = ProcesoLN.getInstancia().VerificarPermiso(id_usuario, "li_menuPBI_Consejo_3", "");
                    pbi_psbanker1 = ProcesoLN.getInstancia().VerificarPermiso(id_usuario, "li_menuPBI_Psbanker_1", "");
                    pbi_psbanker2 = ProcesoLN.getInstancia().VerificarPermiso(id_usuario, "li_menuPBI_Psbanker_2", "");
                    pbi_qmatic1 = ProcesoLN.getInstancia().VerificarPermiso(id_usuario, "li_menuPBI_Qmatic_1", "");

                    //Mesa
                    if (!pbi_mesa1)
                    {
                        menuPBI_mesaservicio.Visible = false;
                    }
                    else
                    {
                        li_menuPBI_mesaservicio_1.Visible = pbi_mesa1;
                    }
                    //Consejo
                    if (!pbi_consejo1 && !pbi_consejo2 && !pbi_consejo3)
                    {
                        menuPBI_Consejo.Visible = false;
                    }
                    else
                    {
                        li_menuPBI_Consejo_1.Visible = pbi_consejo1;
                        li_menuPBI_Consejo_2.Visible = pbi_consejo2;
                        li_menuPBI_Consejo_3.Visible = pbi_consejo3;
                    }
                    //PSBANKER
                    if (!pbi_psbanker1 && !pbi_psbanker2)
                    {
                        menuPBI_Psbanker.Visible = false;
                    }
                    else
                    {
                        li_menuPBI_Psbanker_1.Visible = pbi_psbanker1;
                        li_menuPBI_Psbanker_2.Visible = pbi_psbanker2;
                    }
                    //Qmatic
                    if (!pbi_qmatic1)
                    {
                        menuPBI_Qmatic.Visible = false;
                    }
                    else
                    {
                        li_menuPBI_Qmatic_1.Visible = pbi_qmatic1;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private string CantidadReportes()
        {
            try
            {
                SqlConnection conexion = null;
                SqlCommand cmd = null;

                //Instancia de Conexión
                conexion = Conexion.ConexionBD();

                cmd = new SqlCommand("select count(*) cantidad from reportes.rm_reportes", conexion);

                conexion.Open();

                Int32 count = (Int32)cmd.ExecuteScalar();

                return count.ToString();
            }
            catch (Exception)
            {

                throw;
            }
        }

        //private void PopulateTreeView(DataTable dtParent, int parentId, TreeNode treeNode, int nivel)
        //{
        //    TreeNode child;

        //    foreach (DataRow row in dtParent.Rows)
        //    {

        //        // Nivel 1: Para los Reportes ( NODO HOJA )
        //        if (nivel == 1)
        //        {
        //            //Datos del reporte
        //            string cod_reporte = row["cod_reporte"].ToString();
        //            string nombreReporte = row["nombre"].ToString();
        //            string path = row["ubicacion"].ToString();
        //            string cod_alterno = row["cod_alterno"].ToString();
        //            string estado = row["i_estado"].ToString();

        //            // Nodo Hoja
        //            if (cod_reporte == "" || estado != "A")
        //            {
        //                child = new TreeNode()
        //                {
        //                    Text = nombreReporte,
        //                    Expanded = false,
        //                    ImageUrl = "~/img/folder/candado.png",
        //                    SelectAction = TreeNodeSelectAction.None,
        //                    ImageToolTip = "NO TIENE PERMISO!",
        //                    ToolTip = row["cod_alterno"].ToString()
        //                };
        //            }
        //            else
        //            {
        //                //Ubicación Encriptada
        //                string id = HttpUtility.UrlEncode(encriptar.Crypto(path));
        //                string nombreReporteEncript = HttpUtility.UrlEncode(encriptar.Crypto(cod_alterno + " - " + nombreReporte));

        //                child = new TreeNode()
        //                {
        //                    Text = nombreReporte,
        //                    Value = path,
        //                    Expanded = false,
        //                    ImageUrl = "~/img/folder/2.png",
        //                    SelectAction = TreeNodeSelectAction.Select,
        //                    NavigateUrl = "~/Reporte.aspx?irpt=" + id + "&nrpt=" + nombreReporteEncript + "&urpt=" + usuarioRpt,
        //                    Target = "_blank",
        //                    ImageToolTip = cod_alterno,
        //                    ToolTip = row["cod_alterno"].ToString()
        //                };
        //            }
        //        }
        //        // Nivel 0: Para los Nodos Raiz
        //        else if (nivel == 0)
        //        {
        //            child = new TreeNode()
        //            {
        //                Text = row["nombre"].ToString(),
        //                Value = row["dir"].ToString(),
        //                ImageUrl = "~/img/iconos/nodo_raiz.png",
        //                SelectAction = TreeNodeSelectAction.Expand,
        //                Expanded = false
        //                //ImageToolTip = row["nombre"].ToString(),
        //                // ToolTip = row["nombre"].ToString()
        //            };
        //        }
        //        //Nivel X: Nodos Padre
        //        else
        //        {
        //            child = new TreeNode()
        //            {
        //                Text = row["nombre"].ToString(),
        //                Value = row["dir"].ToString(),
        //                ImageUrl = "~/img/iconos/nodo_padre.png",
        //                SelectAction = TreeNodeSelectAction.Expand,
        //                Expanded = false
        //                //ImageToolTip = row["nombre"].ToString(),
        //                //ToolTip = row["nombre"].ToString()
        //            };
        //        }


        //        if (nivel == 0)
        //        {
        //            //Agrega el Nodo Padre 
        //            TreeView1.Nodes.Add(child);

        //            //*** Selecciona los Subdirectorios
        //            DataTable dtChild = ReportesLN.getInstancia().CargarTreeview(0, subdirectorio, Int32.Parse(child.Value));

        //            if (dtChild != null && dtChild.Rows.Count > 0)
        //            {
        //                //Sigue con los Subdirectorios del Nivel 1
        //                PopulateTreeView(dtParent: dtChild, parentId: int.Parse(child.Value), treeNode: child, nivel: 2);
        //            }
        //            else
        //            {
        //                // No tiene Subdirectorios - Agrega los Reportes del Directorio
        //                DataTable dtChildReportes = ReportesLN.getInstancia().CargarTreeview(Cod_Usuario, directorio, Int32.Parse(child.Value));

        //                //Cantidad de Reportes
        //                cantidadReportes += dtChildReportes.Rows.Count;

        //                PopulateTreeView(dtParent: dtChildReportes, parentId: int.Parse(child.Value), treeNode: child, nivel: 1);
        //            }

        //        }
        //        else if (nivel == 1)
        //        {
        //            //Agrega el Nodo Reporte al Directorio
        //            treeNode.ChildNodes.Add(child);

        //        }
        //        else if (nivel == 2)
        //        {
        //            //Agrega el Nodo Subdirectorio Nivel 1
        //            treeNode.ChildNodes.Add(child);

        //            //No tiene Subdirectorios - Agrega los Reportes del Directorio
        //            DataTable dtChildReportes = ReportesLN.getInstancia().CargarTreeview(Cod_Usuario, subdirectorio, Int32.Parse(child.Value));

        //            //Cantidad de Reportes
        //            cantidadReportes += dtChildReportes.Rows.Count;

        //            PopulateTreeView(dtParent: dtChildReportes, parentId: int.Parse(child.Value), treeNode: child, nivel: 1);
        //        }
        //        else
        //        {
        //            treeNode.ChildNodes.Add(child);
        //        }
        //    }
        //}

    } // Partial Class END 
} // namespace END