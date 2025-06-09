using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KiberOneLearningApp
{
	public class LessonChooseWindow : UIWindow
	{
		[Header("UI References")]
		[SerializeField] private Transform lessonListParent;
		[SerializeField] private LessonChooseButton lessonButtonPrefab;

		public static System.Action<RuntimeTutorialData> OnLessonSelected;

		public static System.Action<RuntimeTutorialData> OnLessonToDeleteSelected;
		
		public override void Initialize()
		{
			
		}
		
		public void DisplayLessons(string themeName, List<RuntimeTutorialData> lessons)
		{
			foreach (Transform child in lessonListParent)
				Destroy(child.gameObject);

			foreach (var lesson in lessons)
			{
				string title = $"Урок {lesson.LessonNumber}: {lesson.TutorialName}";

				LessonChooseButton buttonGO = Instantiate(lessonButtonPrefab, lessonListParent);
				buttonGO.GetComponentInChildren<TextMeshProUGUI>().text = title;

				buttonGO.ChooseButton.onClick.AddListener(() =>
				{
					Debug.Log($"Выбран урок: {title}");
					OnLessonSelected?.Invoke(lesson);
				});

				buttonGO.DeleteButton.onClick.AddListener(() =>
				{
					Debug.Log($"Выбран на удаление урок: {title}");
					OnLessonToDeleteSelected?.Invoke(lesson);
				});
			}

			LayoutRebuilder.ForceRebuildLayoutImmediate(lessonListParent as RectTransform);
		}
	}
}