using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace KiberOneLearningApp
{
    public class MenuButton : MonoBehaviour
    {
        private Button button;

        public void LoadMainScene() => SceneManager.LoadScene(SceneNaming.StartSceneName);

        public void LoadExelMenuScene() => SceneManager.LoadScene(SceneNaming.ExelMenuSceneName);

        public void OpenExelLesson(int index) => SceneManager.LoadScene(SceneNaming.GetExelLessonSceneName(SceneNaming.ExelMenuSceneName, index));

        public void LoadComputerEducationMenuScene() => SceneManager.LoadScene(SceneNaming.ComputerEducationMenuSceneName);

        public void OpenComputerEducationLesson(int index) => SceneManager.LoadScene(SceneNaming.GetExelLessonSceneName(SceneNaming.ComputerEducationMenuSceneName, index));
    }
}
