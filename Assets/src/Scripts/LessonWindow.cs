using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace KiberOneLearningApp
{
	public class LessonWindow : SentencesChanger
	{
		[SerializeField] private TaskWindowsCreator taskWindowsCreator;
		[SerializeField] private OpenTaskButton openTaskButton;

		public override void Initialize()
		{
			base.Initialize();

			if (taskWindowsCreator != null)
			{
				taskWindowsCreator.SetTasksData(tutorialData.Tasks);
			}
            
			if (openTaskButton != null)
				openTaskButton.Initialize(this, taskWindowsCreator);
		}

		protected override void ShowNextSentence()
		{
			base.ShowNextSentence();
		}
	}
}