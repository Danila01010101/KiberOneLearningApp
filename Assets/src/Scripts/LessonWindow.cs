using System;
using System.Collections.Generic;
using UnityEngine;

namespace KiberOneLearningApp
{
	public class LessonWindow : SentencesChanger
	{
		[SerializeField] private TaskWindowsCreator taskWindowsCreator;
		[SerializeField] private OpenTaskButton openTaskButton;
		[SerializeField] private GameObject writeLessonButton;
		
		private List<TaskData> tasks = new List<TaskData>();

        public override void Initialize()
        {
            base.Initialize();
			writeLessonButton.SetActive(GlobalValueSetter.Instance.IsTeacher);
        }

        private void Start()
		{
			if (taskWindowsCreator != null)
			{
				List<LessonWithTasksWindow> taskWindows = taskWindowsCreator.SetTasksData(tutorialData.Tasks, this);
				List<int> lessonsIndexes = new List<int>();

				for (int j = 0; j < tutorialData.Sentences.Count; j++)
				{
					if (tutorialData.Sentences[j].IsBeforeTask)
					{
						lessonsIndexes.Add(j);
					}
				}

				for (int i = 0; i < taskWindows.Count; i++)
				{
					var data = new TaskData(i, lessonsIndexes[i]);
					tasks.Add(data);
				}
			}
		}

		protected override void ShowNextSentence()
		{
			base.ShowNextSentence();
			
			if (tutorialData.Sentences[CurrentIndex].IsBeforeTask)
			{
				TaskData taskForThisSentence = GetTaskBySentenceID(CurrentIndex);

				if (taskForThisSentence.IsCompleted == false)
				{
					sentenceChangerView.UpdateTaskButton(taskForThisSentence.IsCompleted, tutorialData.Sentences[CurrentIndex],
					() => ShowTask(taskForThisSentence.TaskIndex));
				}
				else
				{
					sentenceChangerView.UpdateTaskButton(true, tutorialData.Sentences[CurrentIndex], null);
				}
			}
			else
			{
				sentenceChangerView.UpdateTaskButton(true, tutorialData.Sentences[CurrentIndex], null);
			}
		}

		public void DetectCompletedTasks() => ShowNextSentence();

		private void ShowTask(int index) => taskWindowsCreator.OpenTaskWindow(index);

		private TaskData GetTaskBySentenceID(int sentenceID)
		{
			foreach (var task in tasks)
			{
				if (task.SentenceIndex == sentenceID)
				{
					return task;
				}
			}

			throw new Exception("Not valid ID");
		}

		public struct TaskData
		{
			public readonly int TaskIndex;
			public readonly int SentenceIndex;
			
			public bool IsCompleted;

			public TaskData(int taskIndex, int sentenceIndex)
			{
				TaskIndex = taskIndex;
				SentenceIndex = sentenceIndex;
				IsCompleted = false;
			}
		}
	}
}