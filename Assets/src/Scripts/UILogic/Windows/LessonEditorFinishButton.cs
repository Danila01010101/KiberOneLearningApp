using UnityEngine;
using UnityEngine.SceneManagement;

namespace KiberOneLearningApp
{
	public class LessonEditorFinishButton : MonoBehaviour
	{
		private RuntimeLessonEditorManager manager;
		
		private void Start()
		{
			manager = RuntimeLessonEditorManager.Instance;
		}

		public void OnFinishEditingLesson()
		{
			manager.SaveCurrentLesson();
			Debug.Log("Урок сохранён.");
			SceneManager.LoadScene("StartScene");
		}

		public void OnFinishEditingTask()
		{
			manager.SaveCurrentLesson();
			UIWindowManager.ShowLast();
		}
	}
}