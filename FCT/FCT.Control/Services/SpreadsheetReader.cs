using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using FCT.Infrastructure.Interfaces;

namespace FCT.Control.Services
{
    public class SpreadsheetReader : ISpreadsheetReader
    {
        public Task<IList<DataTable>> ReadFromExcelFileAsync(string filePath)
        {
            return Task.FromResult(ReadFromExcelFile(filePath));
        }

        public IList<DataTable> ReadFromExcelFile(string filePath)
        {
            var dataTables = new List<DataTable>();
            using (var pck = new OfficeOpenXml.ExcelPackage())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    pck.Load(stream);
                }

                foreach (var ws in pck.Workbook.Worksheets)
                {
                    var tbl = new DataTable();
                    tbl.TableName = ws.Name;
                    foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                    {
                        tbl.Columns.Add(firstRowCell.Text);
                    }

                    var startRow =  2;
                    for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                    {
                        var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                        var row = tbl.Rows.Add();
                        foreach (var cell in wsRow)
                        {
                            row[cell.Start.Column - 1] = cell.Text;
                        }
                    }
                    dataTables.Add(tbl);
                }
            }

            return dataTables;
        }
    }
}
