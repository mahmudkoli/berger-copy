using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using BergerMsfaApi.Services.Excel.Interface;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;

namespace BergerMsfaApi.Services.Excel.Implementation
{
    public class ExcelReaderService : IExcelReaderService
    {
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

    }
}
