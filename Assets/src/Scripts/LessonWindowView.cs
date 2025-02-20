namespace KiberOneLearningApp
{
	public class LessonWindowView : SentenceChangerView
	{
		/*
		
		protected override void UpdateView(TutorialData.SentenceData sentenceData)
		{
			base.ShowSentence(sentenceData);
			
			if (sentenceData.IsBeforeTask && (nextButton.IsActive() || !openTaskButton.isActiveAndEnabled))
			{
				int taskIndex = taskIndexes.IndexOf(currentIndex);

				if (taskCreator != null && taskCreator.IsSentenceCompleted(taskIndex) == false)
				{
					nextButton.gameObject.SetActive(false);
					openTaskButton.gameObject.SetActive(true);
					OnTaskUnlocked?.Invoke(currentTaskIndex);
				}
				else
				{
					nextButton.gameObject.SetActive(true);
					openTaskButton.gameObject.SetActive(false);
				}
			}
			else if (!nextButton.IsActive() || openTaskButton.isActiveAndEnabled)
			{
				nextButton.gameObject.SetActive(true);
				openTaskButton.gameObject.SetActive(false);
			}
		}
		*/
	}
}