using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using static Data;

public class ExcelImporter
{
    public static Data ImportFromExcel(string filePath)
    {
        var data = new Data
        {
            teacherPassword = "admin",
            studentGroups = new List<GroupData>()
        };

        using (var package = new ExcelPackage(new FileInfo(filePath)))
        {
            foreach (var worksheet in package.Workbook.Worksheets)
            {
                int rowCount = worksheet.Dimension.Rows;
                int colCount = worksheet.Dimension.Columns;

                var group = new GroupData
                {
                    groupName = worksheet.Dimension.Worksheet.Name,
                    lessonResults = new List<LessonResult>()
                };

                for (int row = 2; row <= rowCount; row++)
                {
                    var firstCell = worksheet.Cells[row, 1].Text?.ToLower();

                    if (string.IsNullOrWhiteSpace(firstCell)) continue;
                    if (firstCell.Contains("сумма") || firstCell.Contains("допы") || 
                        firstCell.Contains("заявки по призам за кибероны")) continue;

                    var lesson = new LessonResult
                    {
                        date = worksheet.Cells[row, 1].Text.Trim(),
                        topicName = worksheet.Cells[row, 2].Text.Trim(),
                        studentResultData = new Dictionary<string, int>()
                    };

                    for (int col = 3; col <= colCount; col++)
                    {
                        var studentName = worksheet.Cells[1, col].Text.Trim();
                        if (string.IsNullOrWhiteSpace(studentName)) continue;

                        var valueText = worksheet.Cells[row, col].Text;
                        if (int.TryParse(valueText, out int result))
                        {
                            lesson.studentResultData[studentName] = result;
                        }
                    }

                    group.lessonResults.Add(lesson);
                }

                data.studentGroups.Add(group);
            }
        }

        return data;
    }
}
