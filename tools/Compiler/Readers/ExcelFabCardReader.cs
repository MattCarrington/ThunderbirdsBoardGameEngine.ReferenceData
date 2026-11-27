using ClosedXML.Excel;
using ThunderbirdsBoardGameEngine.ReferenceData.Compiler.Helpers;
using ThunderbirdsBoardGameEngine.ReferenceData.Compiler.Inputs;

namespace ThunderbirdsBoardGameEngine.ReferenceData.Compiler.Readers
{
    internal sealed class ExcelFabCardReader
    {
        public List<FabCardInput> ReadFrom(IXLWorksheet worksheet)
        {
            var table = ExcelWorksheetTableHelper.From(worksheet);
            var columnMap = table.Columns;

            var fabCards = new List<FabCardInput>();

            foreach (var row in table.DataRows)
            {
                var name = row.Cell(columnMap["Name"]).GetString();

                if (string.IsNullOrWhiteSpace(name))
                {
                    continue;
                }

                var effectDescription = row.Cell(columnMap["Effect Description"]).GetString();

                var instructionalText = columnMap.HasColumn("Instructional Text")
                    ? row.Cell(columnMap["Instructional Text"]).GetString()
                    : string.Empty;

                fabCards.Add(new FabCardInput(
                    name,
                    effectDescription,
                    string.IsNullOrWhiteSpace(instructionalText)
                        ? null
                        : instructionalText));
            }

            return fabCards;
        }
    }
}
