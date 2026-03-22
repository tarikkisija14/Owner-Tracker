using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Text.RegularExpressions;

namespace OwnerTrack.Infrastructure.Parsing
{
   
    internal static class ExcelCellReader
    {
        
        public static string GetCellValue(WorkbookPart workbookPart, Row row, int columnIndex)
        {
            try
            {
                var cell = row.Elements<Cell>()
                    .FirstOrDefault(c => GetColumnIndex(c.CellReference?.Value) == columnIndex);

                if (cell is null)
                    return string.Empty;

                if (cell.DataType is null)
                    return cell.CellValue?.Text ?? string.Empty;

                if (cell.DataType.Value == CellValues.SharedString)
                {
                    if (!int.TryParse(cell.CellValue?.Text, out int sharedIndex))
                        return string.Empty;

                    return workbookPart.SharedStringTablePart?.SharedStringTable
                        .Elements<SharedStringItem>()
                        .ElementAt(sharedIndex)
                        .InnerText ?? string.Empty;
                }

                return cell.CellValue?.Text ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

       
        public static int GetColumnIndex(string? cellReference)
        {
            if (string.IsNullOrWhiteSpace(cellReference))
                return 0;

            string letters = Regex.Replace(cellReference, @"\d", string.Empty);
            int index = 0;
            foreach (char ch in letters)
                index = index * 26 + (ch - 'A' + 1);

            return index;
        }
    }
}