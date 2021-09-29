using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.Report;
using BergerMsfaApi.Services.Excel.Interface;
using ExcelDataReader;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace BergerMsfaApi.Services.Excel.Implementation
{
    public class ExcelReaderService : IExcelReaderService
    {
        private readonly IWebHostEnvironment _hostEnvironment;

        public ExcelReaderService(IWebHostEnvironment hostEnvironment)
        {
            this._hostEnvironment = hostEnvironment;
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

            var rootFolder = _hostEnvironment.WebRootPath;


            string sFileName = @"SnapShotResult.xlsx";

            var memory = new MemoryStream();

            using (var fs = new FileStream(sFileName, FileMode.Create, FileAccess.Write))

            {

                IWorkbook workbook;

                workbook = new XSSFWorkbook();

                ISheet excelSheet = workbook.CreateSheet("SnapShot");
                excelSheet.DefaultRowHeight = (short)((int)excelSheet.DefaultRowHeight * 2);
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



        public async Task<FileContentResult> GetExcelWithImage<T>(string fileName, string sheetName, IList<T> data, Dictionary<string, string> colNames, Dictionary<string, string> imageColNames)
        {
            var rootFolder = _hostEnvironment.WebRootPath;

            using (var exportData = new MemoryStream())
            {
                IWorkbook workbook = new XSSFWorkbook();

                ISheet excelSheet = workbook.CreateSheet(sheetName);
                excelSheet.DefaultRowHeight = (short)((int)excelSheet.DefaultRowHeight * 2);
                int rowcount = 0;

                IRow row = excelSheet.CreateRow(rowcount);

                if (!colNames.Any())
                {

                    var properties = typeof(T).GetProperties()
                        .Select(x => x.Name).ToList();

                    foreach (string property in properties)
                    {
                        colNames.Add(property, property);
                    }
                }


                int i = 0;
                foreach (var prop in typeof(T).GetProperties())
                {
                    row.CreateCell(i)
                        .SetCellValue(colNames.TryGetValue(prop.Name, out string colName) ? colName : prop.Name);

                    i++;
                }

                rowcount++;
                row = excelSheet.CreateRow(rowcount);

                foreach (var item in data)
                {
                    i = 0;
                    foreach (var prop in typeof(T).GetProperties())
                    {
                        var rowValue = prop.GetValue(item, null)?.ToString();

                        if (imageColNames.ContainsKey(prop.Name))
                        {
                            if (!string.IsNullOrEmpty(rowValue) && File.Exists(Path.Combine(rootFolder, rowValue)))
                            {
                                byte[] bytes = await File.ReadAllBytesAsync(Path.Combine(rootFolder, rowValue));
                                int pic = workbook.AddPicture(bytes, PictureType.JPEG);

                                IDrawing drawing = excelSheet.CreateDrawingPatriarch();
                                IClientAnchor anchor = workbook.GetCreationHelper().CreateClientAnchor();

                                anchor.Col1 = i;
                                anchor.Row1 = rowcount;

                                IPicture picture = drawing.CreatePicture(anchor, pic);

                                picture.Resize(1);
                            }
                        }
                        else
                        {
                            row.CreateCell(i).SetCellValue(rowValue?.ToString());
                        }

                        i++;
                    }

                    rowcount++;
                    row = excelSheet.CreateRow(rowcount);
                }

                workbook.Write(exportData);
                var fileContentResult = new FileContentResult(exportData.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    FileDownloadName = fileName
                };

                return fileContentResult;
            }

        }

        public async Task<FileContentResult> GetExcelWithImage(string fileName, string sheetName, dynamic data,
            Dictionary<string, string> colNames = null,
            List<string> ignoreColNames = null, 
            List<string> imageColNames = null, 
            Dictionary<string, List<string>> parentChildHeaders = null,
            Dictionary<string, string> parentHeaderNames = null)
        {
            if (colNames == null)
            {
                colNames = new Dictionary<string, string>();
                foreach (var item in data)
                {
                    foreach (KeyValuePair<string, object> kvp in item)
                    {
                        colNames.Add(kvp.Key, kvp.Key.AddSpacesToSentence(true));
                    }
                    break;
                }
            }
            if (imageColNames == null) imageColNames = new List<string>();
            if (ignoreColNames == null) ignoreColNames = new List<string>();
            if (parentHeaderNames == null)
            {
                parentHeaderNames = new Dictionary<string, string>();
                foreach (KeyValuePair<string, List<string>> kvp in parentChildHeaders)
                {
                    parentHeaderNames.Add(kvp.Key, kvp.Key.AddSpacesToSentence(true));
                }
            }

            var allChildHeaders = new List<string>();
            if (parentChildHeaders != null & parentChildHeaders.Any())
                allChildHeaders = parentChildHeaders.SelectMany(x => x.Value).ToList();

            var rootFolder = _hostEnvironment.WebRootPath;

            using (var exportData = new MemoryStream())
            {
                IWorkbook workbook = new XSSFWorkbook();

                ISheet excelSheet = workbook.CreateSheet(sheetName);
                excelSheet.DefaultRowHeight = (short)((int)excelSheet.DefaultRowHeight * 2);
                int rowIndex = 0;
                int colIndex = 0;

                #region Header set......................................
                IRow row1 = excelSheet.CreateRow(rowIndex);
                IRow row2 = null;

                if (allChildHeaders.Any())
                {
                    rowIndex = rowIndex + 1;
                    row2 = excelSheet.CreateRow(rowIndex);
                }

                // Header row set
                foreach (var item in data)
                {
                    colIndex = 0;
                    var groupColValue = string.Empty;

                    foreach (KeyValuePair<string, object> kvp in item)
                    {
                        //ignore column set
                        if (!colNames.ContainsKey(kvp.Key) || ignoreColNames.Contains(kvp.Key)) continue;

                        var colValue = colNames.TryGetValue(kvp.Key, out string val) ? val : kvp.Key;

                        // check if has group column
                        if(allChildHeaders.Any())
                        {
                            // without group and row span
                            if (!allChildHeaders.Contains(kvp.Key))
                            {
                                row1.CreateCell(colIndex).SetCellValue(colValue);
                                var cra = new NPOI.SS.Util.CellRangeAddress(0, 1, colIndex, colIndex);
                                excelSheet.AddMergedRegion(cra);
                            }
                            else
                            {
                                var grpVal = parentChildHeaders.FirstOrDefault(x => x.Value.Contains(kvp.Key));
                                // first time col span for same group
                                if (groupColValue!=grpVal.Key)
                                {
                                    groupColValue = grpVal.Key;
                                    var grpColVal = parentHeaderNames.TryGetValue(grpVal.Key, out string valt) ? valt : grpVal.Key;
                                    row1.CreateCell(colIndex).SetCellValue(grpColVal);
                                    var craP = new NPOI.SS.Util.CellRangeAddress(0, 0, colIndex, colIndex+grpVal.Value.Count()-1);
                                    excelSheet.AddMergedRegion(craP);
                                }
                                row2.CreateCell(colIndex).SetCellValue(colValue);
                                //var cra = new NPOI.SS.Util.CellRangeAddress(1, 1, colIndex, colIndex);
                                //excelSheet.AddMergedRegion(cra);
                            }
                        }
                        else
                        {
                            row1.CreateCell(colIndex).SetCellValue(colValue);
                            //var cra = new NPOI.SS.Util.CellRangeAddress(0, 0, colIndex, colIndex);
                            //excelSheet.AddMergedRegion(cra);
                        }

                        colIndex++;
                    }

                    break;
                }
                #endregion

                rowIndex++;
                row1 = excelSheet.CreateRow(rowIndex);

                foreach (var item in data)
                {
                    colIndex = 0;
                    foreach (KeyValuePair<string, object> kvp in item)
                    {
                        //ignore column set
                        if (!colNames.ContainsKey(kvp.Key) || ignoreColNames.Contains(kvp.Key)) continue;

                        var rowValue = kvp.Value.ToString();
                        if (imageColNames.Contains(kvp.Key))
                        {
                            if (!string.IsNullOrEmpty(kvp.Value.ToString()) && File.Exists(Path.Combine(rootFolder, rowValue)))
                            {
                                byte[] bytes = File.ReadAllBytes(Path.Combine(rootFolder, kvp.Value.ToString()));
                                int pic = workbook.AddPicture(bytes, PictureType.JPEG);

                                IDrawing drawing = excelSheet.CreateDrawingPatriarch();

                                IClientAnchor anchor = workbook.GetCreationHelper().CreateClientAnchor();
                                anchor.Col1 = colIndex;
                                anchor.Row1 = rowIndex;

                                IPicture picture = drawing.CreatePicture(anchor, pic);
                                picture.Resize(1);
                            }
                        }
                        else
                        {
                            row1.CreateCell(colIndex).SetCellValue(rowValue);
                        }

                        colIndex++;
                    }

                    rowIndex++;
                    row1 = excelSheet.CreateRow(rowIndex);
                }

                workbook.Write(exportData);
                var fileContentResult = new FileContentResult(exportData.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    FileDownloadName = fileName
                };

                return fileContentResult;
            }

        }

        public async Task<MemoryStream> DealerOpeningWriteToFileWithImage(IList<DealerOpeningReportResultModel> datas)
        {
            var rootFolder = _hostEnvironment.WebRootPath;
            var sFileName = @"DealerOpeningReport.xlsx";

            using (var exportData = new MemoryStream())
            {
                IWorkbook workbook = new XSSFWorkbook();

                ISheet excelSheet = workbook.CreateSheet("DealerOpening");
                excelSheet.DefaultRowHeight = (short)((int)excelSheet.DefaultRowHeight * 2);
                int rowcount = 0;

                IRow row = excelSheet.CreateRow(rowcount);

                var colNames = new Dictionary<string, string>()
                {
                    { nameof(DealerOpeningReportResultModel.UserId),"User Id" },
                    { nameof(DealerOpeningReportResultModel.DealrerOpeningCode),"Dealer Opening Code" },
                    { nameof(DealerOpeningReportResultModel.BusinessArea),"Business Area" },
                    { nameof(DealerOpeningReportResultModel.BusinessAreaName),"Business Area Name" },
                    { nameof(DealerOpeningReportResultModel.SalesOffice),"Sales Office" },
                    { nameof(DealerOpeningReportResultModel.SalesGroup),"Sales Group" },
                    { nameof(DealerOpeningReportResultModel.Territory),"Territory" },
                    { nameof(DealerOpeningReportResultModel.Zone),"Zone" },
                    { nameof(DealerOpeningReportResultModel.EmployeeId),"Employee Id" },
                    { nameof(DealerOpeningReportResultModel.DealershipOpeningApplicationForm),"Dealership Opening Application Form" },
                    { nameof(DealerOpeningReportResultModel.TradeLicensee),"Trade Licensee" },
                    { nameof(DealerOpeningReportResultModel.IdentificationNo),"NID/Passport/Birth Certificate" },
                    { nameof(DealerOpeningReportResultModel.PhotographOfproprietor),"Photograph Of Proprietor" },
                    { nameof(DealerOpeningReportResultModel.NomineeIdentificationNo),"Nominee NID/Passport/Birth Certificate" },
                    { nameof(DealerOpeningReportResultModel.NomineePhotograph),"Nominee Photograph" },
                    { nameof(DealerOpeningReportResultModel.Cheque),"Cheque" },
                    { nameof(DealerOpeningReportResultModel.CurrentStatusOfThisApplication),"Current Status Of This Application" },
                };

                var imageColNames = new Dictionary<string, string>()
                {
                    { nameof(DealerOpeningReportResultModel.DealershipOpeningApplicationForm),"Dealership Opening Application Form" },
                    { nameof(DealerOpeningReportResultModel.TradeLicensee),"Trade Licensee" },
                    { nameof(DealerOpeningReportResultModel.IdentificationNo),"NID/Passport/Birth Certificate" },
                    { nameof(DealerOpeningReportResultModel.PhotographOfproprietor),"Photograph Of Proprietor" },
                    { nameof(DealerOpeningReportResultModel.NomineeIdentificationNo),"Nominee NID/Passport/Birth Certificate" },
                    { nameof(DealerOpeningReportResultModel.NomineePhotograph),"Nominee Photograph" },
                    { nameof(DealerOpeningReportResultModel.Cheque),"Cheque" },
                };

                int i = 0;
                foreach (var prop in typeof(DealerOpeningReportResultModel).GetProperties())
                {
                    if (colNames.TryGetValue(prop.Name, out string colName))
                        row.CreateCell(i).SetCellValue(colName);
                    else
                        row.CreateCell(i).SetCellValue(prop.Name);

                    i++;
                }

                rowcount++;
                row = excelSheet.CreateRow(rowcount);

                foreach (var item in datas)
                {
                    i = 0;
                    foreach (var prop in typeof(DealerOpeningReportResultModel).GetProperties())
                    {
                        var rowValue = (string)prop.GetValue(item, null);

                        if (imageColNames.ContainsKey(prop.Name))
                        {
                            if (!string.IsNullOrEmpty(rowValue) && File.Exists(Path.Combine(rootFolder, rowValue)))
                            {
                                byte[] bytes = File.ReadAllBytes(Path.Combine(rootFolder, rowValue));
                                int pic = workbook.AddPicture(bytes, PictureType.JPEG);

                                IDrawing drawing = excelSheet.CreateDrawingPatriarch();
                                IClientAnchor anchor = workbook.GetCreationHelper().CreateClientAnchor();

                                anchor.Col1 = i;
                                anchor.Row1 = rowcount;

                                IPicture picture = drawing.CreatePicture(anchor, pic);

                                picture.Resize(1);
                            }
                        }
                        else
                        {
                            row.CreateCell(i).SetCellValue(rowValue);
                        }

                        i++;
                    }

                    rowcount++;
                    row = excelSheet.CreateRow(rowcount);
                }

                workbook.Write(exportData);
                exportData.Position = 0;
                return exportData;
            }
        }
    }
}
