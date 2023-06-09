﻿namespace Sap.Integration.App.Lib
{
    public class AppConfig
    {
        public static string SqlConnectionString { get { return "Data Source = 192.168.1.1; Initial Catalog = TEST; User Id = username; Password = Secret;"; } }
        public static string HanaConnectionString { get { return "Server = 192.168.33.33:30015; UserId = Manager; Password = Pass@123; CS = TEST"; } }

        public static string Server { get { return "192.168.1.1"; } }
        public static SAPbobsCOM.BoDataServerTypes DbServerType { get { return SAPbobsCOM.BoDataServerTypes.dst_HANADB; } }
        public static string UserName { get { return "testuser"; } }
        public static string Password { get { return "Test@123"; } }
        public static string CompanyDB { get { return "TEST"; } }
    }
}
