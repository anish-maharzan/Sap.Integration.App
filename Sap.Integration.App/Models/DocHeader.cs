using System;

namespace Sap.Integration.App.Models
{
    public class DocHeader
    {
        public int DocEntry { get; set; }
        public string DocNum { get; set; }
        public DateTime DocDate { get; set; }
        public DateTime DocDueDate { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string DocStatus { get; set; }
        public decimal DocTotal { get; set; }
        public string SalesPersonCode { get; set; }
        public string ShipToCode { get; set; }
    }
}
