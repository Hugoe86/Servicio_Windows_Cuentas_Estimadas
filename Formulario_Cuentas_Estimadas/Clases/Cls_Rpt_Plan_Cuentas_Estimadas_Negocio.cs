using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Reportes_Planeacion.Cuentas_Estimadas.Datos;
using System.Data;


namespace Reportes_Planeacion.Cuentas_Estimadas.Negocio
{
    public class Cls_Rpt_Plan_Cuentas_Estimadas_Negocio
    {
        public Cls_Rpt_Plan_Cuentas_Estimadas_Negocio()
        {
            //
            // TODO: Agregar aquí la lógica del constructor
            //
        }

        #region Variables_Publicas
        public String P_Rpu { get; set; }
        public String P_No_Cuenta { get; set; }
        public Int32 P_Anio { get; set; }
        public Int32 P_Mes { get; set; }
        public DateTime P_Dti_Periodo { get; set; }
        public DataRow P_Dr_Registro { get; set; }
        public String P_Usuario { get; set; }
        #endregion


        #region Consultas
        public DataTable Consultar_Reporte_Cuentas_Estimadas()
        {
            return Cls_Rpt_Plan_Cuentas_Estimadas_Datos.Consultar_Reporte_Cuentas_Estimadas(this);
        }
        #endregion


        #region Servicio

        public DataTable Consultar_Si_Existe_Cuenta_Registrada() { return Cls_Rpt_Plan_Cuentas_Estimadas_Datos.Consultar_Si_Existe_Cuenta_Registrada(this); }
        public DataTable Consultar_Tabla_Historicos() { return Cls_Rpt_Plan_Cuentas_Estimadas_Datos.Consultar_Tabla_Historicos(this); }
        
        public void Insetar_Registro() {  Cls_Rpt_Plan_Cuentas_Estimadas_Datos.Insetar_Registro(this); }

        #endregion Fin Servicio
    }
}