using System;
using System.Collections.Generic;

[Serializable]
public class Data
{
    public string teacherPassword = "admin";
    public List<GroupData> studentGroups;

    [Serializable]
    public class GroupData
    {
        public string groupName;
        public List<LessonResult> lessonResults;
    }

    [Serializable]
    public class LessonResult
    {
        public Dictionary<string, int> studentResultData;
        public string date;
        public string topicName;
    }
}