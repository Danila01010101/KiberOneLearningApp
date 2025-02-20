using System.Collections.Generic;
using UnityEngine;

namespace KiberOneLearningApp
{
	public class LessonWindow : SentencesChanger
	{
		[SerializeField] private TaskCreator taskCreator;
		[SerializeField] private OpenTaskButton openTaskButton;

		public override void Initialize()
		{
			base.Initialize();

			if (taskCreator != null)
			{
				taskCreator.SetTasksData(tutorialData.Tasks, this);
			}
            
			if (openTaskButton != null)
				openTaskButton.Initialize(this, taskCreator);
		}
	}
}