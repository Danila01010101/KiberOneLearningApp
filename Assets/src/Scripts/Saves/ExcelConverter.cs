using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using static Data;

public class ExcelConverter
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
    
    public static void ExportToExcel(Data data, string filePath)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using (var package = new ExcelPackage())
        {
            foreach (var group in data.studentGroups)
            {
                var worksheet = package.Workbook.Worksheets.Add(group.groupName);
                var allStudents = new HashSet<string>();
                
                foreach (var lesson in group.lessonResults)
                    
                foreach (var student in lesson.studentResultData.Keys)
                    allStudents.Add(student);

                var studentList = new List<string>(allStudents);
                worksheet.Cells[1, 1].Value = "Дата";
                worksheet.Cells[1, 2].Value = "Тема";
                
                for (int i = 0; i < studentList.Count; i++)
                    worksheet.Cells[1, i + 3].Value = studentList[i];

                for (int row = 0; row < group.lessonResults.Count; row++)
                {
                    var lesson = group.lessonResults[row];
                    worksheet.Cells[row + 2, 1].Value = lesson.date;
                    worksheet.Cells[row + 2, 2].Value = lesson.topicName;

                    for (int col = 0; col < studentList.Count; col++)
                    {
                        if (lesson.studentResultData.TryGetValue(studentList[col], out int score))
                            worksheet.Cells[row + 2, col + 3].Value = score;
                    }
                }
            }

            var fi = new FileInfo(filePath);
            package.SaveAs(fi);
        }
    }
}
