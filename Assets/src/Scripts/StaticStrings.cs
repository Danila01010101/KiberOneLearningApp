using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KiberOneLearningApp
{
    public static class StaticStrings
    {
        public const string StartSceneName = "StartScene";
        public const string ExelMenuSceneName = "Exel";
        public const string ComputerEducationMenuSceneName = "ComputerEducation";
        public const string LessonSavesFloulderName = "ExportedLessons";
        public const string ImagesSavesFloulderName = "UserImages";
        public const string VideoSavesFloulderName = "UserVideos";

        public static string GetExelLessonSceneName(string sceneName, int index) => sceneName + index;
    }
}
