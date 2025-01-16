using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KiberOneLearningApp
{
    public static class SceneNaming
    {
        public const string StartSceneName = "StartScene";
        public const string ExelMenuSceneName = "Exel";
        public const string ComputerEducationMenuSceneName = "ComputerEducation";

        public static string GetExelLessonSceneName(string sceneName, int index) => sceneName + index;
    }
}
