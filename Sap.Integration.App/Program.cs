using Sap.Integration.App.BAL.Sales;

namespace Sap.Integration.App
{
    public class Program
    {
        static void Main(string[] args)
        {
            SalesOrderService.ImportToSap();
        }
    }
}
