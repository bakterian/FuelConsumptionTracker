using System.Threading.Tasks;
using FCT.Infrastructure.Interfaces;
using FCT.Infrastructure.Attributes;
using System.Data;
using System;
using System.Collections.Generic;
using OfficeOpenXml;
using System.Linq;
using OfficeOpenXml.Style;

namespace FCT.Control.Services
{
    public class SpreadsheetWriter : ISpreadsheetWriter
    {
        private readonly ILogger _logger;

        public SpreadsheetWriter(
            ILogger logger
            )
        {
            _logger = logger;
        }   

        public Task<bool> WriteToExcelFileAsync(string filePath, IEnumerable<Tuple<string,DataTable>> worksheetPayloads)
        {
            return Task.FromResult<bool>(WriteToExcelFile(filePath, worksheetPayloads));
        }

        public bool WriteToExcelFile(string filePath, IEnumerable<Tuple<string, DataTable>> worksheetPayloads)
        {
            var writtingWasSuccessfull = false;
            var itemCount = worksheetPayloads.Select(_ => _.Item2.Rows.Count * _.Item2.Columns.Count).Sum();
            var saveFileInfo = new System.IO.FileInfo(filePath);

            if (itemCount == 0)
            {
                _logger.Error("[SpreadsheetWriter] Provided data Tables contain no elements.");
            }
            else
            {
                if(saveFileInfo.Exists) saveFileInfo.Delete();
                using (var package = new ExcelPackage(saveFileInfo))
                {
                    foreach (var worksheetPayload in worksheetPayloads)
                    {
                        var worksheetHeading = worksheetPayload.Item1;
                        var dataTable = worksheetPayload.Item2;
                        var columnsWithDateTime = GetColumnIdsWithDateTimes(dataTable);
                        var workSheet = package.Workbook.Worksheets.Add(worksheetHeading);

                        // add the content into the Excel file  
                        workSheet.Cells["A1"].LoadFromDataTable(dataTable, true);

                        // autofit width of cells with small content  
                        int columnIndex = 1;
                        foreach (DataColumn column in dataTable.Columns)
                        {
                            ExcelRange columnCells = workSheet.Cells[workSheet.Dimension.Start.Row, columnIndex, workSheet.Dimension.End.Row, columnIndex];
                            int maxLength = columnCells.Max(cell => cell.Value.ToString().Count());
                            if (maxLength < 150)
                            {
                                workSheet.Column(columnIndex).AutoFit();
                            }
                            columnIndex++;
                        }

                        // format header - bold, yellow on black  
                        using (ExcelRange r = workSheet.Cells[1, 1, 1, dataTable.Columns.Count])
                        {
                            r.Style.Font.Color.SetColor(System.Drawing.Color.White);
                            r.Style.Font.Bold = true;
                            r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            r.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#1fb5ad"));
                        }

                        // format cells - add borders  
                        using (ExcelRange r = workSheet.Cells[2, 1, 1 + dataTable.Rows.Count, dataTable.Columns.Count])
                        {
                            r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            r.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                            r.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                            r.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                            r.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                            r.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                        }

                        if(columnsWithDateTime.Count > 0)
                        {
                            foreach (var columnId in columnsWithDateTime)
                            {
                                using (ExcelRange col = workSheet.Cells[2, columnId, 2 + dataTable.Rows.Count, columnId])
                                {
                                    col.Style.Numberformat.Format = "dd-MM-yyyy";
                                    //col.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                }
                            }
                        }

                    }
                    package.SaveAs(saveFileInfo);
                }
            }

            saveFileInfo.Refresh();

            if (!saveFileInfo.Exists)
            {
                _logger.Error("[SpreadsheetWriter] Provided file does not exist.");
                writtingWasSuccessfull = false;
            }
            else
            {
                writtingWasSuccessfull = true;
            }

            return writtingWasSuccessfull;
        }

        private IList<int> GetColumnIdsWithDateTimes(DataTable dataTable)
        {
            var columnIds = new List<int>();

            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                if (dataTable.Columns[i].DataType.Equals(typeof(DateTime)))
                {
                    columnIds.Add(i + 1);
                }
            }

            return columnIds;
        }
    }
}
