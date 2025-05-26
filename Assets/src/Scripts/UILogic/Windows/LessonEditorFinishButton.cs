using System;
using UnityEngine;
using UnityEngine.UI;

namespace KiberOneLearningApp
{
	public class LessonEditorFinishButton : MonoBehaviour
	{
		public Button finishEditingButton;

		private RuntimeLessonEditorManager manager;
		
		private void Start()
		{
			finishEditingButton.onClick.AddListener(OnFinishEditing);
		}

		private void OnFinishEditing()
		{
			manager.SaveCurrentLesson();
			Debug.Log("Урок сохранён.");
		}
	}
}