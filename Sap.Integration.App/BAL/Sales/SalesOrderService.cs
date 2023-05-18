using Sap.Integration.App.DAL;
using Sap.Integration.App.Helpers;
using Sap.Integration.App.Lib;
using Sap.Integration.App.Models.Sales;
using Sap.Integration.App.SAP;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Sap.Integration.App.BAL.Sales
{
    public class SalesOrderService
    {
        public static void ImportToSap()
        {
            LogHelper.WriteLog($"{MethodBase.GetCurrentMethod().DeclaringType}>{MethodBase.GetCurrentMethod().Name} starts.");
            try
            {
                string query = "";
                string connectionString = AppConfig.SqlConnectionString;
                SqlDataAccessLayer sqlDataAccessLayer = new SqlDataAccessLayer(connectionString);

                query = "SELECT * FROM WmsSalesOrder WHERE isProcessed = 'N'";
                List<SalesOrder> oDocList = sqlDataAccessLayer.ExecuteQuery<List<SalesOrder>>(query);

                foreach (var doc in oDocList)
                {
                    Documents oSalesOrder = (Documents)SapCompany.GetCompany().GetBusinessObject(BoObjectTypes.oOrders);

                    oSalesOrder.CardCode = doc.CardCode;
                    oSalesOrder.DocDate = doc.DocDate;
                    oSalesOrder.DocDueDate = doc.DocDueDate;

                    query = $"SELECT * FROM WmsSalesOrderDetail WHERE isProcessed = 'N' AND DocEntry = {doc.DocEntry}";
                    List<SalesOrderLine> oLineList = sqlDataAccessLayer.ExecuteQuery<List<SalesOrderLine>>(query);

                    foreach (var line in oLineList)
                    {
                        oSalesOrder.Lines.ItemCode = line.ItemCode;
                        oSalesOrder.Lines.Quantity = line.Quantity;
                        oSalesOrder.Lines.Price = line.Price;
                        oSalesOrder.Lines.Add();
                    }

                    if (oSalesOrder.Add() != 0)
                    {
                        string error = SapCompany.GetCompany().GetLastErrorDescription();
                        LogHelper.WriteLog($"Error while adding SalesOrder: {error}");
                    }
                    else
                    {
                        LogHelper.WriteLog($"SalesOrder added successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog($"Exception occurs due to {ex.Message}");
            }
            LogHelper.WriteLog($"{MethodBase.GetCurrentMethod().DeclaringType}>{MethodBase.GetCurrentMethod().Name} ends.");
        }
    }
}
