using Sap.Integration.App.Lib;
using System;

namespace Sap.Integration.App.SAP
{
    public class SapCompany
    {
        private SAPbobsCOM.Company company = new SAPbobsCOM.Company();
        private int connectionResult;
        private int errorCode = 0;
        private string errorMessage = "";

        private static SapCompany Instance;
 
        private int Connect()
        {
            company.Server = AppConfig.Server;
            company.DbServerType = AppConfig.DbServerType;
            company.UserName = AppConfig.UserName;
            company.Password = AppConfig.Password;
            company.CompanyDB = AppConfig.CompanyDB;

            connectionResult = company.Connect();

            if (connectionResult != 0)
            {
                string err = company.GetLastErrorDescription();
                company.GetLastError(out errorCode, out errorMessage);
            }
            return connectionResult;
        }
     
        public static SAPbobsCOM.Company GetCompany()
        {
            //ServerConnection Instance;
            if (Instance == null)
            {
                Instance = new SapCompany();
                Instance.Connect();
            }
            return Instance.company;
        }

        public int GetErrorCode() => this.errorCode;

        public String GetErrorMessage() => this.errorMessage;
    }
}
