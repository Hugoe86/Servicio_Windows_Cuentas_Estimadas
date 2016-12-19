using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Reportes_Planeacion.Cuentas_Estimadas.Negocio;
using System.Data;
using SIAC.Constantes;
using SharpContent.ApplicationBlocks.Data;
using System.Data.SqlClient;
using System.Globalization;


/// <summary>
/// Descripción breve de Cls_Rpt_Plan_Cuentas_Estimadas_Datos
/// </summary>
namespace Reportes_Planeacion.Cuentas_Estimadas.Datos
{
    public class Cls_Rpt_Plan_Cuentas_Estimadas_Datos
    {
        public Cls_Rpt_Plan_Cuentas_Estimadas_Datos()
        {
            //
            // TODO: Agregar aquí la lógica del constructor
            //
        }


        //*******************************************************************************
        //NOMBRE_FUNCION:  Consultar_Reporte_Cuentas_Congeladas_Cobranza
        //DESCRIPCION: Metodo que Consulta las cuentas congeladas con estatus de cobranza
        //PARAMETROS : 1.- Cls_Rpt_Cor_Cc_Reportes_Varios_Neogcio Datos, objeto de la clase de negocios
        //CREO       : Hugo Enrique Ramírez Aguilera
        //FECHA_CREO : 11/Abril/2016
        //MODIFICO   :
        //FECHA_MODIFICO:
        //CAUSA_MODIFICO:
        //*******************************************************************************
        public static DataTable Consultar_Reporte_Cuentas_Estimadas(Cls_Rpt_Plan_Cuentas_Estimadas_Negocio Datos)
        {
            DataTable Dt_Consulta = new DataTable();
            String Str_My_Sql = "";
            SqlDataAdapter da;
            DataSet ds;


            try
            {
                using (SqlConnection Obj_Conexion = new SqlConnection(Cls_Constantes.Str_Conexion))
                {
                    Obj_Conexion.Open();

                    using (SqlCommand Obj_Comando = Obj_Conexion.CreateCommand())
                    {

                        //  ****************************************************************************************************************************************
                        //  ****************************************************************************************************************************************
                        //  ****************************************************************************************************************************************
                        Str_My_Sql = "select " + " p.RPU";
                        Str_My_Sql += ", p.No_Cuenta";
                        Str_My_Sql += ", rg.Numero_Region as Sector";
                        Str_My_Sql += ", rr.No_Ruta as Ruta";
                        Str_My_Sql += ", t.Abreviatura as Tarifa";
                        Str_My_Sql += ", CASE" +
                                            " when pm.PREDIO_ID is NULL then" +
                                                " 'FALSO'" +
                                            " ELSE 'VERDADERO'" +
                                            " END" +
                                            " as Tiene_Medidor";
                        Str_My_Sql += ", f.Consumo as Consumo_M3";
                        Str_My_Sql += ", case " +
                                        " when ml.Lectura_Estimada = 'SI'" +
                                            " THEN 'VERDADERO'" +
                                        " else 'FALSO'" +
                                        " END as Estatus_Estimado";

                        Str_My_Sql += ", f.Lectura_Anterior";
                        Str_My_Sql += ", f.Lectura_Actual";


                        Str_My_Sql += ", (" +
                                            " select isnull(mnl.CLAVE , '')" +
                                            " from Cat_Cor_Medidores_Lecturas_Detalles ml" +
                                            " join Cat_Cor_Motivos_No_Lectura mnl on mnl.MOTIVO_NO_LECTURA_ID = ml.Clave_Anomalia_ID_1" +
                                            " where ml.Medidor_Detalle_ID = f.Medidor_Detalle_ID" +
                                            " ) as Anomalia_Actual";

                        Str_My_Sql += ", ( " +
                                                " Select isnull(mnl.clave , '')" +
                                                " from Cat_Cor_Medidores_Lecturas_Detalles ml" +
                                                " join Cat_Cor_Motivos_No_Lectura mnl on mnl.MOTIVO_NO_LECTURA_ID = ml.Clave_Anomalia_ID_1" +
                                                " where ml.Medidor_Detalle_ID = (" +
                                                                                    " select top 1 fs.Medidor_Detalle_ID" +
                                                                                    " from Ope_Cor_Facturacion_Recibos fs" +
                                                                                    " where MONTH(fs.Fecha_Emision) = " + Datos.P_Dti_Periodo.AddMonths(-1).Month +
                                                                                    " AND YEAR(fs.Fecha_Emision) = " + Datos.P_Dti_Periodo.AddMonths(-1).Year +
                                                                                    " and fs.RPU = p.rpu	" +
                                                                                    " order by fs.No_Factura_Recibo desc" +
                                                                                " )" +
                                                " ) as   Anomalia_Anterior";


                        Str_My_Sql += ", sum(fd.importe) as Monto_Facturado_Agua";
                        Str_My_Sql += ", isnull(le.NOMBRE, '') as Lecturista";
                        Str_My_Sql += ", month(f.Fecha_Emision) as Mes";
                        Str_My_Sql += ", YEAR(f.Fecha_Emision) as Año";

                        Str_My_Sql += ", p.Estatus";
                        Str_My_Sql += ", p.Cortado";


                        Str_My_Sql += ", case	" +
                                            " when p.Cortado = 'SI' THEN" +
                                                "(" +
                                                    " select top 1 o.Fecha" +
                                                    " from OPE_COR_ORDENES_TRABAJO o" +
                                                    " where o.Tipo_Falla_ID in ('00005', '00006')" +
                                                    " and o.Predio_ID = p.Predio_ID" +
                                                    " order by No_Orden_Trabajo desc" +
                                                ")" +
                                            " end" +
                                        " as Fecha_Corte";

                        Str_My_Sql += ", case " +
                                            " when p.Cortado = 'SI' THEN" +
                                                "(" +
                                                    " select top 1 tf.DESCRIPCION" +
                                                    " from OPE_COR_ORDENES_TRABAJO o" +
                                                    " join CAT_COR_TIPOS_FALLAS tf on tf.TIPO_FALLA_ID = o.Tipo_Falla_ID" +
                                                    " where o.Tipo_Falla_ID in ('00005', '00006')" +
                                                    " and o.Predio_ID = p.Predio_ID" +
                                                    " order by No_Orden_Trabajo desc" +
                                                ")" +
                                            " end" +
                                        " as Tipo_Corte";

                        //  ****************************************************************************************************************************************
                        //  ****************************************************************************************************************************************
                        //  from **********************************************************************************************************************************
                        Str_My_Sql += " from Cat_Cor_Predios p";
                        Str_My_Sql += " join Cat_Cor_Regiones rg on rg.Region_ID = p.Region_ID";
                        Str_My_Sql += " join Cat_Cor_Rutas_Reparto rr on rr.Ruta_Reparto_ID = p.Ruta_Reparto_ID";
                        Str_My_Sql += " join Cat_Cor_Tarifas t on t.Tarifa_ID = p.Tarifa_ID";
                        Str_My_Sql += " left outer join Cat_Cor_Predios_Medidores pm on pm.PREDIO_ID = p.Predio_ID ";
                        Str_My_Sql += " join Ope_Cor_Facturacion_Recibos f on f.Predio_ID = p.Predio_ID";
                        Str_My_Sql += " join Ope_Cor_Facturacion_Recibos_Detalles fd on fd.No_Factura_Recibo = f.No_Factura_Recibo";
                        Str_My_Sql += " join Cat_Cor_Conceptos_Cobros cc on cc.Concepto_ID = fd.Concepto_ID";
                        Str_My_Sql += " JOIN Cat_Cor_Medidores_Lecturas_Detalles ml ON ml.Medidor_Detalle_ID = f.Medidor_Detalle_ID";
                        Str_My_Sql += " join Cat_Cor_Lecturistas le on le.LECTURISTA_ID= ml.Lecturista_ID";

                        //  ****************************************************************************************************************************************
                        //  ****************************************************************************************************************************************
                        //  Where ********************************************************************************************************************************
                        Str_My_Sql += " where  p.Estatus = 'ACTIVO' ";
                        Str_My_Sql += " and MONTH(f.Fecha_Emision) =  " + Datos.P_Dti_Periodo.Month;
                        Str_My_Sql += " and YEAR(f.Fecha_Emision) = " + Datos.P_Dti_Periodo.Year;

                        Str_My_Sql += " and(cc.Concepto_ID = (select p.CONCEPTO_AGUA from Cat_Cor_Parametros p) " +
                                                " OR  cc.Concepto_ID = (select p.Concepto_Agua_Comercial from Cat_Cor_Parametros p)" +
                                                " OR cc.Concepto_ID = (select p.CONCEPTO_DRENAJE from Cat_Cor_Parametros p) " +
                                                " OR cc.Concepto_ID = (select p.CONCEPTO_SANAMIENTO from Cat_Cor_Parametros p))";

                        //Str_My_Sql += "and  p.RPU = '000870300182'";

                        //  ****************************************************************************************************************************************
                        //  ****************************************************************************************************************************************
                        //  GROUP  by  **********************************************************************************************************
                        Str_My_Sql += " GROUP by ";
                        Str_My_Sql += " p.RPU";
                        Str_My_Sql += ", p.No_Cuenta";
                        Str_My_Sql += ", rg.Numero_Region ";
                        Str_My_Sql += ", rr.No_Ruta";
                        Str_My_Sql += ", t.Abreviatura ";
                        Str_My_Sql += ", pm.PREDIO_ID";
                        Str_My_Sql += ", f.Consumo";
                        Str_My_Sql += ", f.No_Factura_Recibo";
                        Str_My_Sql += ", le.NOMBRE";
                        Str_My_Sql += ", f.Fecha_Emision";
                        Str_My_Sql += ", f.Estimado";
                        Str_My_Sql += ", ml.Lectura_Estimada";
                        Str_My_Sql += ", p.Estatus";
                        Str_My_Sql += ", p.Cortado";
                        Str_My_Sql += ", p.predio_id";
                        Str_My_Sql += ", f.Lectura_Anterior";
                        Str_My_Sql += ", f.Lectura_Actual";
                        Str_My_Sql += ", f.Medidor_Detalle_ID";
                        //  ****************************************************************************************************************************************
                        //  ****************************************************************************************************************************************
                        //  order by  **********************************************************************************************************
                        Str_My_Sql += " order by p.no_cuenta asc";

                        // Dt_Consulta = SqlHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Str_My_Sql).Tables[0];


                        Obj_Comando.CommandText = Str_My_Sql;
                        Obj_Comando.CommandTimeout = 300;
                        da = new SqlDataAdapter(Obj_Comando);
                        ds = new DataSet();
                        da.Fill(ds);

                        Dt_Consulta = ds.Tables[0];
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }

            return Dt_Consulta;
        
        }// fin de consulta




        //*******************************************************************************
        //NOMBRE_FUNCION:  Consultar_Tabla_Historicos
        //DESCRIPCION: Metodo que Consulta las cuentas que ya se registraron
        //PARAMETROS : 1.- Cls_Rpt_Cor_Cc_Reportes_Varios_Neogcio Datos, objeto de la clase de negocios
        //CREO       : Hugo Enrique Ramírez Aguilera
        //FECHA_CREO : 11/Abril/2016
        //MODIFICO   :
        //FECHA_MODIFICO:
        //CAUSA_MODIFICO:
        //*******************************************************************************
        public static DataTable Consultar_Tabla_Historicos(Cls_Rpt_Plan_Cuentas_Estimadas_Negocio Datos)
        {
            DataTable Dt_Consulta = new DataTable();
            String Str_My_Sql = "";
            try
            {
                //  ****************************************************************************************************************************************
                //  ****************************************************************************************************************************************
                //  ****************************************************************************************************************************************
                Str_My_Sql = "select  ";

                Str_My_Sql += "RPU";
                Str_My_Sql += ", No_cuentas";
                Str_My_Sql += ", Sector";
                Str_My_Sql += ", Ruta";
                Str_My_Sql += ", Tarifa";
                Str_My_Sql += ", Tiene_Medidor";
                Str_My_Sql += ", Consumo_M3";
                Str_My_Sql += ", Estatus_Estimado";
                Str_My_Sql += ", Monto_Facturado_Agua";
                Str_My_Sql += ", Lecturista";
                Str_My_Sql += ", Año";
                Str_My_Sql += ", Mes";
                Str_My_Sql += ", Estatus";
                Str_My_Sql += ", Cortado";
                Str_My_Sql += ", Fecha_Corte";
                Str_My_Sql += ",Tipo_Corte";

                //  ****************************************************************************************************************************************
                //  ****************************************************************************************************************************************
                //  ****************************************************************************************************************************************
                Str_My_Sql += " From Ope_Cor_Plan_Cuentas_Estimadas";

                //  ****************************************************************************************************************************************
                //  ****************************************************************************************************************************************
                //  ****************************************************************************************************************************************
                Str_My_Sql += " where Año =" + Datos.P_Anio;
                Str_My_Sql += " and Mes =" + Datos.P_Mes;

                //  ****************************************************************************************************************************************
                //  ****************************************************************************************************************************************
                //  ****************************************************************************************************************************************
                Dt_Consulta = SqlHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Str_My_Sql).Tables[0];

            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }

            return Dt_Consulta;

        }// fin de consulta



        //*******************************************************************************
        //NOMBRE_FUNCION:  Consultar_Si_Existe_Cuenta_Registrada
        //DESCRIPCION: Metodo que Consulta las cuentas que ya se registraron
        //PARAMETROS : 1.- Cls_Rpt_Cor_Cc_Reportes_Varios_Neogcio Datos, objeto de la clase de negocios
        //CREO       : Hugo Enrique Ramírez Aguilera
        //FECHA_CREO : 11/Abril/2016
        //MODIFICO   :
        //FECHA_MODIFICO:
        //CAUSA_MODIFICO:
        //*******************************************************************************
        public static DataTable Consultar_Si_Existe_Cuenta_Registrada(Cls_Rpt_Plan_Cuentas_Estimadas_Negocio Datos)
        {
            DataTable Dt_Consulta = new DataTable();
            String Str_My_Sql = "";
            try
            {
                //  ****************************************************************************************************************************************
                //  ****************************************************************************************************************************************
                //  ****************************************************************************************************************************************
                Str_My_Sql = "select * ";
                Str_My_Sql += " From Ope_Cor_Plan_Cuentas_Estimadas";
                
                //  ****************************************************************************************************************************************
                //  ****************************************************************************************************************************************
                //  ****************************************************************************************************************************************
                Str_My_Sql += " where rpu = '" + Datos.P_Rpu + "'";
                Str_My_Sql += " and Año =" + Datos.P_Anio;
                Str_My_Sql += " and Mes =" + Datos.P_Mes;

                //  ****************************************************************************************************************************************
                //  ****************************************************************************************************************************************
                //  ****************************************************************************************************************************************
                Dt_Consulta = SqlHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Str_My_Sql).Tables[0];

            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }

            return Dt_Consulta;

        }// fin de consulta


        //*******************************************************************************
        //NOMBRE_FUNCION:  Insetar_Registro
        //DESCRIPCION: Metodo que ingresa la informacion
        //PARAMETROS : 1.- Cls_Rpt_Plan_Montos_Negocio Clase_Negocios, objeto de la clase de negocios
        //CREO       : Hugo Enrique Ramírez Aguilera
        //FECHA_CREO : 25-Octubre-2016
        //MODIFICO   :
        //FECHA_MODIFICO:
        //CAUSA_MODIFICO:
        //*******************************************************************************
        public static void Insetar_Registro(Cls_Rpt_Plan_Cuentas_Estimadas_Negocio Datos)
        {
            //Declaración de las variables
            SqlTransaction Obj_Transaccion = null;
            SqlConnection Obj_Conexion = new SqlConnection(Cls_Constantes.Str_Conexion);
            SqlCommand Obj_Comando = new SqlCommand();
            String Mi_SQL = "";
            Boolean Estatus_Parametro = false;
            DateTime Dtime_Fecha_Corte = new DateTime();

            try
            {
                Obj_Conexion.Open();
                Obj_Transaccion = Obj_Conexion.BeginTransaction();
                Obj_Comando.Transaction = Obj_Transaccion;
                Obj_Comando.Connection = Obj_Conexion;


                #region historico


                Mi_SQL = "INSERT INTO  Ope_Cor_Plan_Cuentas_Estimadas (";
                Mi_SQL += "RPU";                                    //  1
                Mi_SQL += ", No_cuentas";                           //  2
                Mi_SQL += ", Sector";                               //  3
                Mi_SQL += ", Ruta";                                 //  4
                Mi_SQL += ", Tarifa";                               //  5
                Mi_SQL += ", Tiene_Medidor";                        //  6
                Mi_SQL += ", Consumo_M3";                           //  7
                Mi_SQL += ", Estatus_Estimado";                     //  8
                Mi_SQL += ", Monto_Facturado_Agua";                 //  9
                Mi_SQL += ", Lecturista";                           //  10
                Mi_SQL += ", Año";                                  //  11
                Mi_SQL += ", Mes";                                  //  12
                Mi_SQL += ", Estatus";                              //  13
                Mi_SQL += ", Cortado";                              //  14      

                if (!String.IsNullOrEmpty(Datos.P_Dr_Registro["Fecha_Corte"].ToString()))
                {

                    Mi_SQL += ", Fecha_Corte";                          //  15
                }
                
                Mi_SQL += ", Tipo_Corte";                           //  16
                Mi_SQL += ", Usuario_Creo";                         //  17
                Mi_SQL += ", fecha_creo";                           //  18
                Mi_SQL += ", Lectura_Anterior";                     //  19
                Mi_SQL += ", Lectura_Actual";                       //  20
                Mi_SQL += ", Anomalia_Anterior";                    //  21
                Mi_SQL += ", Anomalia_Actual";                      //  22
                Mi_SQL += ")";
                //***************************************************************************
                Mi_SQL += " values ";
                //***************************************************************************
                Mi_SQL += "(";
                Mi_SQL += " '" + Datos.P_Dr_Registro["Rpu"].ToString() + "'";              //  1
                Mi_SQL += ", '" + Datos.P_Dr_Registro["no_cuenta"].ToString() + "'";            //  2
                Mi_SQL += ", '" + Datos.P_Dr_Registro["sector"].ToString() + "'";               //  3
                Mi_SQL += ", '" + Datos.P_Dr_Registro["ruta"].ToString() + "'";                 //  4
                Mi_SQL += ", '" + Datos.P_Dr_Registro["tarifa"].ToString() + "'";               //  5
                Mi_SQL += ", '" + Datos.P_Dr_Registro["tiene_medidor"].ToString() + "'";        //  6
                Mi_SQL += ", '" + Convert.ToDouble(Datos.P_Dr_Registro["Consumo_M3"].ToString()).ToString(new CultureInfo("es-MX")) + "'";           //  7
                Mi_SQL += ", '" + Datos.P_Dr_Registro["Estatus_Estimado"].ToString() + "'";     //  8
                Mi_SQL += ", '" + Convert.ToDouble(Datos.P_Dr_Registro["Monto_Facturado_Agua"].ToString()).ToString(new CultureInfo("es-MX")) + "'"; //  9
                Mi_SQL += ", '" + Datos.P_Dr_Registro["Lecturista"].ToString() + "'";           //  10
                Mi_SQL += ", '" + Convert.ToDouble(Datos.P_Dr_Registro["Año"].ToString()).ToString(new CultureInfo("es-MX")) + "'";                  //  11
                Mi_SQL += ", '" + Convert.ToDouble(Datos.P_Dr_Registro["Mes"].ToString()).ToString(new CultureInfo("es-MX")) + "'";                  //  12
                Mi_SQL += ", '" + Datos.P_Dr_Registro["Estatus"].ToString() + "'";              //  13
                Mi_SQL += ", '" + Datos.P_Dr_Registro["Cortado"].ToString() + "'";              //  14

                if (!String.IsNullOrEmpty(Datos.P_Dr_Registro["Fecha_Corte"].ToString()))
                {
                    Mi_SQL += ", @fecha";                                                       //  15
                    Dtime_Fecha_Corte = Convert.ToDateTime(Datos.P_Dr_Registro["Fecha_Corte"].ToString());
                    Estatus_Parametro = true;
                }
                else
                {
                   // Mi_SQL += ", '" + Datos.P_Dr_Registro["Fecha_Corte"].ToString() + "'";      //  15
                }
                
                //comando.Parameters.AddWithValue("@laFecha", p.fechaRecepcionMuestra);
                
                Mi_SQL += ", '" + Datos.P_Dr_Registro["Tipo_Corte"].ToString() + "'";           //  16
                Mi_SQL += ", '" + Datos.P_Usuario + "'";                                        //  17
                Mi_SQL += ", getdate()";                                                        //  18
                Mi_SQL += ", '" + Datos.P_Dr_Registro["Lectura_Anterior"].ToString() + "'";     //  19
                Mi_SQL += ", '" + Datos.P_Dr_Registro["Lectura_Actual"].ToString() + "'";       //  20
                Mi_SQL += ", '" + Datos.P_Dr_Registro["Anomalia_Anterior"].ToString() + "'";    //  21
                Mi_SQL += ", '" + Datos.P_Dr_Registro["Anomalia_Actual"].ToString() + "'";      //  22
                Mi_SQL += ")";


                if (Estatus_Parametro == true)
                {
                    Obj_Comando.Parameters.Add("@fecha", SqlDbType.DateTime).Value = Dtime_Fecha_Corte;
                }
                
                Obj_Comando.CommandText = Mi_SQL;
                Obj_Comando.ExecuteNonQuery();

                #endregion Fin historico

                //***********************************************************************************************************************
                //***********************************************************************************************************************
                //***********************************************************************************************************************
                //***********************************************************************************************************************
                //ejecucion de la transaccion    ***********************************************************************************
                Obj_Transaccion.Commit();


            }
            catch (SqlException Ex)
            {
                Obj_Transaccion.Rollback();
                throw new Exception("Error: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {
                Obj_Transaccion.Rollback();
                throw new Exception("Error: " + Ex.Message);
            }
            catch (Exception Ex)
            {
                Obj_Transaccion.Rollback();
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
                Obj_Conexion.Close();
            }


        }// fin del metodo



    }
}