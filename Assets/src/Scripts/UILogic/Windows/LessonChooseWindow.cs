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
		[SerializeField] private MenuButton lessonButtonPrefab;

		public static System.Action<RuntimeTutorialData> OnLessonSelected;
		
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

				GameObject buttonGO = Instantiate(lessonButtonPrefab, lessonListParent).gameObject;
				buttonGO.GetComponentInChildren<TextMeshProUGUI>().text = title;

				buttonGO.GetComponent<Button>().onClick.AddListener(() =>
				{
					Debug.Log($"Выбран урок: {title}");
					OnLessonSelected?.Invoke(lesson);
				});
			}

			LayoutRebuilder.ForceRebuildLayoutImmediate(lessonListParent as RectTransform);
		}
	}
}