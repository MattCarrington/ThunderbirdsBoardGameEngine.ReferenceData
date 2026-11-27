using ClosedXML.Excel;
using ThunderbirdsBoardGameEngine.ReferenceData.Compiler.Helpers;
using ThunderbirdsBoardGameEngine.ReferenceData.Compiler.Inputs;
using ThunderbirdsBoardGameEngine.ReferenceData.Core.Identities;

namespace ThunderbirdsBoardGameEngine.ReferenceData.Compiler.Readers
{
    internal sealed class ExcelEventCardReader
    {
        public List<EventCardInput> ReadFrom(IXLWorksheet worksheet)
        {
            var table = ExcelWorksheetTableHelper.From(worksheet);
            var columnMap = table.Columns;

            var eventCards = new List<EventCardInput>();

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

                eventCards.Add(new EventCardInput(
                    name,
                    effectDescription,
                    string.IsNullOrWhiteSpace(instructionalText)
                        ? null
                        : instructionalText));
            }

            return eventCards;
        }
    }
}
