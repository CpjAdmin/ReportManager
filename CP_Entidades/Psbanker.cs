using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_Entidades
{
    //Provincia - Canton - Distrito
    public class Provincia
    {
        public String cod_provincia { get; set; }
        public String nombre { get; set; }

        public Provincia()
        {
            cod_provincia = "0";
            nombre = "";
        }
    }

    public class Canton
    {
        public String cod_provincia { get; set; }
        public String cod_canton { get; set; }
        public String nombre { get; set; }
        public Canton()
        {
            cod_provincia = "0";
            cod_canton = "0";
            nombre = "";
        }

    }

    public class Distrito
    {
        public String cod_provincia { get; set; }
        public String cod_canton { get; set; }
        public String cod_distrito { get; set; }
        public String nombre { get; set; }

        public Distrito()
        {
            cod_provincia = "0";
            cod_canton = "0";
            cod_distrito = "0";
            nombre = "";
        }
    }

    // Centro - Institucion - Lugares
    public class Centro
    {
        public String cod_centro { get; set; }
        public String nombre { get; set; }

        public Centro()
        {
            cod_centro = "0";
            nombre = "";
        }
    }

    public class Institucion
    {
        public String cod_centro { get; set; }
        public String cod_institucion { get; set; }
        public String nombre { get; set; }
        public Institucion()
        {
            cod_centro = "0";
            cod_institucion = "0";
            nombre = "";
        }
    }

    public class LugarTrabajo
    {
        public String cod_centro { get; set; }
        public String cod_institucion { get; set; }
        public String cod_lugar_trabajo { get; set; }
        public String nombre { get; set; }

        public LugarTrabajo()
        {
            cod_centro = "0";
            cod_institucion = "0";
            cod_lugar_trabajo = "0";
            nombre = "";
        }
    }

    public class Asociado
    {
        public String codigo { get; set; }
        public String identificacion { get; set; }
        public String nombre { get; set; }
        public String centro { get; set; }
        public String institucion { get; set; }
        public String lugar_trabajo { get; set; }
        public String provincia { get; set; }
        public String canton { get; set; }
        public String distrito { get; set; }
        public String estado { get; set; }
        public String email { get; set; }
        //*** Agregado para Estados de Cuenta Ahorro Vista
        public int num_contrato { get; set; }
        public String tarjeta { get; set; }
        public String cuenta_iban { get; set; }


        public Asociado()
        {
            codigo = "0";
            identificacion = "0";
            nombre = "";
            centro = "";
            institucion = "";
            lugar_trabajo = "";
            estado = "";
            email = "";
            provincia = "";
            canton = "";
            distrito = "";
            //*** Agregado para Estados de Cuenta Ahorro Vista
            num_contrato = 0;
            tarjeta = "";
            cuenta_iban = "";
        }

        public Asociado( string codigo,string identificacion,string nombre, string centro,string institucion,string lugar_trabajo, string estado, string email, string provincia,string canton,string distrito)
        {
            this.codigo = codigo;
            this.identificacion = identificacion;
            this.nombre = nombre;
            this.centro = centro;
            this.institucion = institucion;
            this.lugar_trabajo = lugar_trabajo;
            this.estado = estado;
            this.email = email;

            this.provincia = provincia;
            this.canton = canton;
            this.distrito = distrito;
        }

        public Asociado(string codigo, string identificacion, string nombre, string centro, string institucion, string lugar_trabajo, string estado, string email, int num_contrato, string tarjeta, string cuenta_iban)
        {
            this.codigo = codigo;
            this.identificacion = identificacion;
            this.nombre = nombre;
            this.centro = centro;
            this.institucion = institucion;
            this.lugar_trabajo = lugar_trabajo;
            this.estado = estado;
            this.email = email;

            //*** Agregado para Estados de Cuenta Ahorro Vista
            this.num_contrato = num_contrato;
            this.tarjeta = tarjeta;
            this.cuenta_iban = cuenta_iban;
        }
    }

    public class CPC
    {
        public String usuario { get; set; }
        public String clave { get; set; }

        public CPC()
        {

            usuario = "";
            clave   = "";
        }

        public CPC(string usuario, string clave)
        {
            this.usuario = usuario;
            this.clave = clave;
        }

    }
}

