using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Reportes_Planeacion.Cuentas_Estimadas.Negocio;

namespace Formulario_Cuentas_Estimadas
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //*******************************************************************************
        //NOMBRE DE LA FUNCIÓN:Btn_Prueba_Click
        //DESCRIPCIÓN: 
        //PARAMETROS: 
        //CREO       : Hugo Enrique Ramírez Aguilera
        //FECHA_CREO : 07/Abril/2016
        //MODIFICO:
        //FECHA_MODIFICO:
        //CAUSA_MODIFICACIÓN:
        //*******************************************************************************
        private void Btn_Prueba_Click(object sender, EventArgs e)
        {
            try
            {
                Actualizar_Informacion();
                MessageBox.Show("Proceso exitos", "Mensaje", MessageBoxButtons.OK);
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Eror:   " + Ex.Message, "Mensaje", MessageBoxButtons.OK);
            }
        }


        //*******************************************************************************
        //NOMBRE DE LA FUNCIÓN:Actualizar_Informacion
        //DESCRIPCIÓN: Metodo que permite llenar el Grid con la informacion de la consulta
        //PARAMETROS: 
        //CREO       : Hugo Enrique Ramírez Aguilera
        //FECHA_CREO : 07/Abril/2016
        //MODIFICO:
        //FECHA_MODIFICO:
        //CAUSA_MODIFICACIÓN:
        //*******************************************************************************
        public void Actualizar_Informacion()
        {
            Cls_Rpt_Plan_Cuentas_Estimadas_Negocio Rs_Consulta = new Cls_Rpt_Plan_Cuentas_Estimadas_Negocio();
            DataTable Dt_Consulta = new DataTable();
            DateTime Dtime_Fecha_Actual;
            DataTable Dt_Cuenta_Estimada = new DataTable();

            try
            {
                Dtime_Fecha_Actual = DateTime.Now;

                Rs_Consulta.P_Dti_Periodo = Dtime_Fecha_Actual;
                Dt_Consulta = Rs_Consulta.Consultar_Reporte_Cuentas_Estimadas();

                //  se ingresara la informacion a la tabla de planeacion cuentas estimadas
                Rs_Consulta.P_Anio = Dtime_Fecha_Actual.Year;
                Rs_Consulta.P_Mes = Dtime_Fecha_Actual.Month;
                Rs_Consulta.P_Usuario = "Prueba";

                foreach (DataRow Registro in Dt_Consulta.Rows)
                {
                    Dt_Cuenta_Estimada.Clear();
                    Rs_Consulta.P_Rpu = Registro["Rpu"].ToString();

                    Dt_Cuenta_Estimada = Rs_Consulta.Consultar_Si_Existe_Cuenta_Registrada();

                    //  validamos que no exista
                    if (Dt_Cuenta_Estimada != null && Dt_Cuenta_Estimada.Rows.Count == 0)
                    {
                        Rs_Consulta.P_Dr_Registro = Registro;
                        Rs_Consulta.Insetar_Registro();
                    }
                }



            }
            catch (Exception Ex)
            {
                
            }
        }

    }
}
