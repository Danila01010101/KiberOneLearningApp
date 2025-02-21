using UnityEngine.Events;

namespace KiberOneLearningApp
{
	public class LessonWindowView : SentenceChangerView
	{
		private void UpdateTaskButton(TaskWindowsCreator taskCreator, TutorialData.SentenceData sentenceData, UnityAction openTask)
		{
			if (sentenceData.IsBeforeTask && (nextButton.IsActive() || !openCurrentTaskButton.isActiveAndEnabled))
			{
				if (taskCreator != null)
				{
					nextButton.gameObject.SetActive(false);
					openCurrentTaskButton.gameObject.SetActive(true);
					openCurrentTaskButton.onClick.RemoveAllListeners();
					openCurrentTaskButton.onClick.AddListener(openTask);
					Добавить вызов этого метода
				}
				else
				{
					nextButton.gameObject.SetActive(true);
					openCurrentTaskButton.gameObject.SetActive(false);
				}
			}
			else if (!nextButton.IsActive() || openCurrentTaskButton.isActiveAndEnabled)
			{
				nextButton.gameObject.SetActive(true);
				openCurrentTaskButton.gameObject.SetActive(false);
			}
		}
	}
}