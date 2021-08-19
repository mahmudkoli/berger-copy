using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.Report;
using BergerMsfaApi.Services.Excel.Interface;
using ExcelDataReader;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace BergerMsfaApi.Services.Excel.Implementation
{
    public class ExcelReaderService : IExcelReaderService
    {
        private readonly IWebHostEnvironment hostEnvironment;

        public ExcelReaderService(IWebHostEnvironment hostEnvironment)
        {
            this.hostEnvironment = hostEnvironment;
        }
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


        public async Task<MemoryStream> WriteToFileWithImage(dynamic datas)
        {
            //List<string> property = new List<string>();

            var rootFolder = hostEnvironment.WebRootPath;


            string sFileName = @"SnapShotResult.xlsx";

            var memory = new MemoryStream();

            using (var fs = new FileStream(sFileName, FileMode.Create, FileAccess.Write))

            {

                IWorkbook workbook;

                workbook = new XSSFWorkbook();

                ISheet excelSheet = workbook.CreateSheet("SnapShot");
                excelSheet.DefaultRowHeight =(short)((int)excelSheet.DefaultRowHeight * 2);
                int rowcount = 0;

                IRow row = excelSheet.CreateRow(rowcount);
                rowcount = rowcount + 1;
                IRow row2 = excelSheet.CreateRow(rowcount);

                //int rowcount

                int lastcellspan = 0;
                int firstcellspan = 0;
                foreach (var item in datas)
                {
                    int i = 0;
                    foreach (KeyValuePair<string, object> kvp in item)
                    {
                        if (kvp.Key.Contains("_Image") || kvp.Key.Contains("_Remarks"))
                        {
                            row2.CreateCell(i).SetCellValue(kvp.Key.Replace('_', ' '));
                            lastcellspan++;
                        }
                        else 
                        {
                            row.CreateCell(i).SetCellValue(kvp.Key);
                            

                            firstcellspan++;
                        }


                        i++;

                    }

                    
                        //var cra = new NPOI.SS.Util.CellRangeAddress(0, 1, j, j);


                    var cra = new NPOI.SS.Util.CellRangeAddress(0, 1, 0, 0);
                    var cra2 = new NPOI.SS.Util.CellRangeAddress(0, 1, 1, 1);
                    var cra3 = new NPOI.SS.Util.CellRangeAddress(0, 1, 2, 2);
                    var cra4 = new NPOI.SS.Util.CellRangeAddress(0, 1, 3, 3);
                    var cra5 = new NPOI.SS.Util.CellRangeAddress(0, 1, 4, 4);
                    var cra6 = new NPOI.SS.Util.CellRangeAddress(0, 1, 5, 5);
                    var cra7 = new NPOI.SS.Util.CellRangeAddress(0, 1, 6, 6);





                    excelSheet.AddMergedRegion(cra);

                    excelSheet.AddMergedRegion(cra2);

                    excelSheet.AddMergedRegion(cra3);

                    excelSheet.AddMergedRegion(cra4);

                    excelSheet.AddMergedRegion(cra5);

                    excelSheet.AddMergedRegion(cra6);

                    excelSheet.AddMergedRegion(cra7);


                    var cra1 = new NPOI.SS.Util.CellRangeAddress(0, 0, firstcellspan, lastcellspan + firstcellspan);

                    excelSheet.AddMergedRegion(cra1);
                    row.CreateCell(firstcellspan).SetCellValue("Snapshot Type");
                    break;
                }


                rowcount++;
                row = excelSheet.CreateRow(rowcount);

                foreach (var item in datas)
                {
                    int i = 0;
                    foreach (KeyValuePair<string, object> kvp in item)
                    {
                        
                            if (kvp.Key.Contains("_Image"))
                            {
                                if (!string.IsNullOrEmpty(kvp.Value.ToString()) && File.Exists(Path.Combine(rootFolder, kvp.Value.ToString())))
                                {
                                    byte[] bytes = File.ReadAllBytes(Path.Combine(rootFolder, kvp.Value.ToString()));

                                    int pic = workbook.AddPicture(bytes, PictureType.JPEG);




                                    //Add a picture shape and set its position

                                    IDrawing drawing = excelSheet.CreateDrawingPatriarch();

                                    IClientAnchor anchor = workbook.GetCreationHelper().CreateClientAnchor();

                                    //anchor.Dx1 = 0;

                                    //anchor.Dy1 = 0;

                                    anchor.Col1 = i;

                                    anchor.Row1 = rowcount;

                                    IPicture picture = drawing.CreatePicture(anchor, pic);


                                    //Automatically adjust the image size

                                    picture.Resize(1);
                                }
                                else
                                {
                                    row.CreateCell(i).SetCellValue(kvp.Value.ToString());

                                }


                            }
                            else 
                            {
                                row.CreateCell(i).SetCellValue(kvp.Value.ToString());

                            }
                        
                        i++;
                    }
                    rowcount++;
                    row = excelSheet.CreateRow(rowcount);
                }


               
                workbook.Write(fs);

            }

            using (var stream = new FileStream(sFileName, FileMode.Open))

            {

                await stream.CopyToAsync(memory);

            }

            memory.Position = 0;
            return memory;
        }

    }
}
