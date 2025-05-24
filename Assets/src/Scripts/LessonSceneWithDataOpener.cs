using UnityEngine;
using UnityEngine.SceneManagement;

namespace KiberOneLearningApp
{
	public class LessonSceneWithDataOpener : MonoBehaviour
	{
		public static RuntimeTutorialData CurrentTutorialData { get; private set; }

		public void Start()
		{
			DontDestroyOnLoad(gameObject);
			LessonChooseWindow.OnLessonSelected += LoadLessonScene;
		}

		private void LoadLessonScene(RuntimeTutorialData data)
		{
			CurrentTutorialData = data;
			SceneManager.LoadScene("BasicLessonScene");
		}
	}
}