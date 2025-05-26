using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace KiberOneLearningApp
{
    public class MenuButton : MonoBehaviour
    {
        private Button button;

        public void LoadMainScene() => SceneManager.LoadScene(StaticStrings.StartSceneName);

        public void LoadExelMenuScene() => SceneManager.LoadScene(StaticStrings.ExelMenuSceneName);

        public void OpenExelLesson(int index) => SceneManager.LoadScene(StaticStrings.GetExelLessonSceneName(StaticStrings.ExelMenuSceneName, index));

        public void LoadComputerEducationMenuScene() => SceneManager.LoadScene(StaticStrings.ComputerEducationMenuSceneName);

        public void OpenComputerEducationLesson(int index) => SceneManager.LoadScene(StaticStrings.GetExelLessonSceneName(StaticStrings.ComputerEducationMenuSceneName, index));
    }
}
