using System.Collections.Generic;
using UnityEngine;

namespace KiberOneLearningApp
{
	public class LessonWindow : SentencesChanger
	{
		[SerializeField] private TaskCreator taskCreator;
		[SerializeField] private OpenTaskButton openTaskButton;
		
		private List<int> taskIndexes = new List<int>();

		/*
		public override void Initialize()
		{
			base.Initialize();

			if (taskCreator != null)
			{
				for (int i = 0; i < TutorialData.Sentences.Count; i++)
				{
					if (TutorialData.Sentences[i].IsBeforeTask)
						taskIndexes.Add(i);
				}

				taskCreator.SetTasksData(TutorialData.Tasks, this);
			}
            
			if (openTaskButton != null)
				openTaskButton.Initialize(this, taskCreator);
		}
		
		protected override void ShowSentence(TutorialData.SentenceData sentenceData)
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