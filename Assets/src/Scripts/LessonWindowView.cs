using UnityEngine.Events;

namespace KiberOneLearningApp
{
	public class LessonWindowView : SentenceChangerView
	{
		public override void UpdateTaskButton(bool isTaskSolved, RuntimeSentenceData sentenceData, UnityAction openTask)
		{
			if (isTaskSolved || (!sentenceData.IsBeforeTask && !nextButton.IsActive() || openCurrentTaskButton.isActiveAndEnabled))
			{
				nextButton.gameObject.SetActive(true);
				openCurrentTaskButton.gameObject.SetActive(false);
				return;
			}

			nextButton.gameObject.SetActive(false);
			openCurrentTaskButton.gameObject.SetActive(true);
			openCurrentTaskButton.onClick.RemoveAllListeners();
			openCurrentTaskButton.onClick.AddListener(openTask);
		}
	}
}