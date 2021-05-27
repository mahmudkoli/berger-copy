using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Services.Excel.Interface;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace BergerMsfaApi.Services.Excel.Implementation
{
    public class ExcelReaderService : IExcelReaderService
    {
        public async Task<List<T>> LoadDataAsync<T>(IFormFile file) where T : class, new()
        {

            if (file == null || file.Length == 0)
                throw new Exception("File Not Selected");

            string fileExtension = Path.GetExtension(file.FileName);

            if (fileExtension != ".xls" && fileExtension != ".xlsx")
                throw new Exception("Invalid File Selected");


            DataSet result;

            using (var stream = new MemoryStream())
            {
                // System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                await file.CopyToAsync(stream);

                IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream);
                var conf = new ExcelDataSetConfiguration
                {
                    ConfigureDataTable = _ => new ExcelDataTableConfiguration
                    {
                        UseHeaderRow = true
                    }
                };

                result = reader.AsDataSet(conf);
            }

            return result.Tables[0].ToList<T>();
        }

        public async Task<DataSet> LoadDataAsync(IFormFile file)
        {

            if (file == null || file.Length == 0)
                throw new Exception("File Not Selected");

            string fileExtension = Path.GetExtension(file.FileName);

            if (fileExtension != ".xls" && fileExtension != ".xlsx")
                throw new Exception("Invalid File Selected");


            DataSet result;

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);


                IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream);
                var conf = new ExcelDataSetConfiguration
                {
                    ConfigureDataTable = _ => new ExcelDataTableConfiguration
                    {
                        UseHeaderRow = true
                    }
                };

                result = reader.AsDataSet(conf);
            }

            return result;
        }

        public byte[] WriteToFile<T>(List<T> data)
        {
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("Sheet1");
            ICreationHelper cH = workbook.GetCreationHelper();

            DataTable dt = data.ToDataTable();
            byte[] byteArray;

            using (MemoryStream stream = new MemoryStream())
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    IRow row = sheet.CreateRow(i);

                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        ICell cell = row.CreateCell(j);
                        cell.SetCellValue(cH.CreateRichTextString(dt.Rows[i].ItemArray[j].ToString()));
                        sheet.AutoSizeColumn(j);
                    }
                }
                workbook.Write(stream);
                byteArray = stream.ToArray();
            }

            return byteArray;

        }

    }
}
