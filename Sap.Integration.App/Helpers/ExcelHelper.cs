using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Sap.Integration.App.Helpers
{
    public class ExcelHelper
    {
        public static List<T> ReadExcelToList<T>(string filePath, int worksheetIndex = 0) where T : new()
        {
            try
            {
                List<T> resultList = new List<T>();

                using (var package = new ExcelPackage(new FileInfo(filePath)))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[worksheetIndex]; // Assuming the data is in the first worksheet

                    int rowCount = worksheet.Dimension.Rows;
                    int columnCount = worksheet.Dimension.Columns;

                    PropertyInfo[] propertyInfos = typeof(T).GetProperties();

                    Dictionary<int, PropertyInfo> columnMappings = new Dictionary<int, PropertyInfo>();

                    for (int col = 1; col <= columnCount; col++)
                    {
                        string columnName = Convert.ToString(worksheet.Cells[1, col].Value);

                        PropertyInfo propertyInfo = propertyInfos.FirstOrDefault(p =>
                        {
                            var attribute = p.GetCustomAttribute<ColumnAttribute>(false);
                            return attribute != null && attribute.Name.Equals(columnName, StringComparison.OrdinalIgnoreCase);
                        });

                        if (propertyInfo != null)
                        {
                            columnMappings.Add(col, propertyInfo);
                        }
                    }

                    for (int row = 2; row <= rowCount; row++) // Assuming the data starts from the second row
                    {
                        T obj = new T();

                        foreach (var mapping in columnMappings)
                        {
                            int col = mapping.Key;
                            PropertyInfo propertyInfo = mapping.Value;

                            object cellValue = worksheet.Cells[row, col].Value;
                            if (cellValue != null)
                            {
                                Type propertyType = propertyInfo.PropertyType;
                                object convertedValue = Convert.ChangeType(cellValue, propertyType);
                                propertyInfo.SetValue(obj, convertedValue);
                            }
                        }

                        resultList.Add(obj);
                    }
                }
                return resultList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static void ExportToExcel<T>(List<T> oList, string filePath)
        {
            ExcelPackage excelPackage = new ExcelPackage();
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");

            // Get the properties of the generic type T
            var properties = typeof(T).GetProperties();

            // Add the header row
            for (int i = 0; i < properties.Length; i++)
            {
                worksheet.Cells[1, i + 1].Value = properties[i].Name;
            }

            // Populate the data rows
            for (int row = 2; row <= oList.Count + 1; row++)
            {
                for (int col = 0; col < properties.Length; col++)
                {
                    worksheet.Cells[row, col + 1].Value = properties[col].GetValue(oList[row - 2]);
                }
            }

            FileInfo file = new FileInfo(filePath);
            excelPackage.SaveAs(file);
        }
    }
}
