using System;
using System.Collections.Generic;

[Serializable]
public class Data
{
    public List<LessonResult> lessonResults;

    [Serializable]
    public class LessonResult
    {
        public List<StudentData> studentResultData;
        public List<StudentData> studentAdditionalResultData;
        public string date;
        public string topicName;
    }

    [Serializable]
    public class StudentData
    {
        public string name;
        public int reward;
    }
}