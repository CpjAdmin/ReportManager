using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CP_AccesoDatos;
using CP_Entidades;
using System.Web;
using System.Data;

namespace CP_LogicaNegocio
{
    public class UsuarioLN
    {
        #region "Logica de Negocio Usuario"
        private static UsuarioLN ObjUsuario = null;
        private UsuarioLN() { }
        public static UsuarioLN getInstancia()
        {
            if (ObjUsuario == null)
            {
                ObjUsuario = new UsuarioLN();
            }
            return ObjUsuario;
        }
        #endregion
        // Acceso al Sistema - LOGIN
        public Usuario AccesoSistema(String usuario,String clave)
        {
            Usuario usuarioLogin = new Usuario();

            try
            {
               return UsuarioDAO.getInstancia().AccesoSistema(usuario, clave);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Mantenimiento de Usuario
        public bool MantenimientoUsuario(Usuario usuario,String proceso_crud)
        {
            try
            {
                return UsuarioDAO.getInstancia().MantenimientoUsuario(usuario, proceso_crud);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Cargar Usuarios GetUsuarios
        public List<Usuario> GetUsuarios()
        {
            try
            {
                return UsuarioDAO.getInstancia().GetUsuarios();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Cargar Usuarios para DrodDownList
        public DataTable GetUsuariosDDL()
        {
            try
            {
                return UsuarioDAO.getInstancia().GetUsuariosDDL();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Cargar Directorios
        public DataTable CargarDirectorios()
        {
            try
            {
                return ReportesDAO.getInstancia().CargarDirectorios();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //*** SIC - Verifica si existe el usuario
        public bool VerificaUsuarioSIC(Usuario usuario,string sistema)
        {
            try
            {
                return UsuarioDAO.getInstancia().VerificaUsuarioSIC(usuario, sistema);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //*** SIC - Actualiza la contraseña
        public bool ActualizarClaveSIC(Usuario usuarioSIC)
        {
            try
            {
                return UsuarioDAO.getInstancia().ActualizarClaveSIC(usuarioSIC);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //*** ActualizarClaveCendeisss - Actualiza la contraseña
        public bool ActualizarClaveCendeisss(Usuario usuarioSIC)
        {
            try
            {
                return UsuarioDAO.getInstancia().ActualizarClaveCendeisss(usuarioSIC);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //*** AUDITORIA - Auditoria de Sesión
        public bool AuditoriaSesion(Usuario usuario, String navegador)
        {
            try
            {
                return UsuarioDAO.getInstancia().AuditoriaSesion(usuario, navegador);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Usuarios por Perfil
        public List<String> UsuariosPorPerfil(int cod_perfil, String cod_proceso)
        {
            try
            {
                return UsuarioDAO.getInstancia().UsuariosPorPerfil(cod_perfil, cod_proceso);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Verificar si existe usuario en Report Manager
        public String verificarUsuarioRM(String login, String cod_proceso)
        {
            try
            {
                return UsuarioDAO.getInstancia().verificaUsuarioRM(login, cod_proceso);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Envio de clave por Email
        public bool enviarClaveEmail(String login, String clave, String navegador, int cod_proceso)
        {
            try
            {
                return UsuarioDAO.getInstancia().enviarClaveEmail(login, clave, navegador, cod_proceso);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Auditoría de Procesos
        public bool usuarioInsertarAuditoria(int cod_proceso, int cod_usuario, String navegador, String pagina, String descripcion,String terminal_id)
        {
            try
            {
                return UsuarioDAO.getInstancia().usuarioInsertarAuditoria(cod_proceso, cod_usuario, navegador, pagina, descripcion, terminal_id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Consultar Auditoria de Procesos
        public List<String> usuarioConsultarAuditoria()
        {
            try
            {
                return UsuarioDAO.getInstancia().usuarioConsultarAuditoria();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
