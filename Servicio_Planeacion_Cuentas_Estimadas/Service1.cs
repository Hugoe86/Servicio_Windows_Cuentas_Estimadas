using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using Reportes_Planeacion.Cuentas_Estimadas.Negocio;
using System.IO;

namespace Servicio_Planeacion_Cuentas_Estimadas
{
    public partial class Service1 : ServiceBase
    {
        public Timer Tiempo;

        /////*******************************************************************************************************
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        ///// <creo>Hugo Enrique Ramírez Aguilera</creo>
        ///// <fecha_creo>1</fecha_creo>
        ///// <modifico></modifico>
        ///// <fecha_modifico></fecha_modifico>
        ///// <causa_modificacion></causa_modificacion>
        ///*******************************************************************************************************
        public Service1()
        {
            InitializeComponent();
            Tiempo = new Timer();
            Tiempo.Interval = 900000; // 900000 = 15 minutos     // 600000 = 10 minutos  //  1200000 = 20 minutos
            Tiempo.Elapsed += new ElapsedEventHandler(Tiempo_Contador);
        }
        /////*******************************************************************************************************
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        ///// <creo>Hugo Enrique Ramírez Aguilera</creo>
        ///// <fecha_creo>1</fecha_creo>
        ///// <modifico></modifico>
        ///// <fecha_modifico></fecha_modifico>
        ///// <causa_modificacion></causa_modificacion>
        ///*******************************************************************************************************
        protected override void OnStart(string[] args)
        {
            Tiempo.Enabled = true;
        }
        /////*******************************************************************************************************
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        ///// <creo>Hugo Enrique Ramírez Aguilera</creo>
        ///// <fecha_creo>1</fecha_creo>
        ///// <modifico></modifico>
        ///// <fecha_modifico></fecha_modifico>
        ///// <causa_modificacion></causa_modificacion>
        ///*******************************************************************************************************
        protected override void OnStop()
        {
        }



        

        /////*******************************************************************************************************
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        ///// <creo>Hugo Enrique Ramírez Aguilera</creo>
        ///// <fecha_creo>1</fecha_creo>
        ///// <modifico></modifico>
        ///// <fecha_modifico></fecha_modifico>
        ///// <causa_modificacion></causa_modificacion>
        ///*******************************************************************************************************
        public void Tiempo_Contador(object Sender, EventArgs e)
        {

            //StreamWriter SW = new StreamWriter("C:\\Servicios_siac\\Historial.txt", true);
            DateTime Dtime_Hora = DateTime.Now;

            try
            {
                //SW.WriteLine("************************************************************");

                if (Dtime_Hora.Hour >= 18 && Dtime_Hora.Hour <= 19)
                {
                    //SW.WriteLine("************************************************************");
                    Actualizar_Informacion();
                    //SW.WriteLine("************************************************************");
                   
                }
            }
            catch (Exception Ex)
            {
                //SW.WriteLine(Ex.Message);

            }
            finally
            {
                //SW.Close();
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
            StreamWriter SW = new StreamWriter("C:\\Servicios_siac\\Historial.txt", true);
           

            try
            {
                Dtime_Fecha_Actual = DateTime.Now;

                Rs_Consulta.P_Dti_Periodo = Dtime_Fecha_Actual;
                Dt_Consulta = Rs_Consulta.Consultar_Reporte_Cuentas_Estimadas();

                //  se ingresara la informacion a la tabla de planeacion cuentas estimadas
                Rs_Consulta.P_Anio = Dtime_Fecha_Actual.Year;
                Rs_Consulta.P_Mes = Dtime_Fecha_Actual.Month;
                Rs_Consulta.P_Usuario = "Servicio";

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
                SW.WriteLine(Ex.Message);

            }
            finally
            {
                SW.Close();
            }
        }



    }
}
