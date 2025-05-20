using System;
using System.Collections.Generic;
using UnityEngine;

namespace KiberOneLearningApp
{
	public class LessonWithTasksWindow : SentencesChanger
    {
        private List<ObjectForTask> tasks = new List<ObjectForTask>();
		private Transform tasksParent;

		public Action TaskLessonCompleted;

		public void SetNewData(RuntimeTutorialData newData)
		{
			if (tasksParent != null)
				Destroy(tasksParent.gameObject);

			runtimeData = newData;
			tasks.Clear();

			var emptyTaskPrefab = Resources.Load<EmptyTask>("EmptyTask");
			var taskPrefab = Resources.Load<ObjectForTask>("ObjectForTask");

			tasksParent = new GameObject("Tasks").transform;
			tasksParent.SetParent(transform);

			for (int i = 0; i < runtimeData.Sentences.Count; i++)
			{
				var sentence = runtimeData.Sentences[i];

				if (sentence.IsBeforeTask && sentence.InteractableImages != null)
				{
					ObjectForTask task = Instantiate(taskPrefab, tasksParent);
					task.Initialize(sentence.InteractableImages[i]); // ⬅️ вот здесь мы и передаём DTO

					task.OnCompleted += () =>
					{
						sentenceChangerView.UnlockNextButton();
						task.gameObject.SetActive(false);
					};

					SetupNewTask(task);
					sentenceChangerView.BlockNextButton();
				}
			}
		}

		protected override void ShowNextSentence()
		{
			if (CurrentIndex + 1 >= runtimeData.Sentences.Count)
			{
				TaskLessonCompleted?.Invoke();
				return;
			}
			
			base.ShowNextSentence();
			DeactivateIfExist(CurrentIndex - 1);
            ResetCurrentSentence();
        }

		protected override void ShowPreviousSentence()
		{
			base.ShowPreviousSentence();
			ResetCurrentSentence();
            DeactivateIfExist(CurrentIndex + 1);
        }

		private void SetupNewTask(ObjectForTask spawnedTask)
		{
			tasks.Add(spawnedTask);
			spawnedTask.transform.SetParent(tasksParent.transform);
		}

		private void ResetCurrentSentence()
		{
			if (CurrentIndex < tasks.Count)
			{
				var task = tasks[CurrentIndex];
				if (task != null)
				{
					sentenceChangerView.BlockNextButton();
				}
			}
		}

		private void DeactivateIfExist(int index)
		{
			if (index >= 0 && index < tasks.Count)
			{
				tasks[index].gameObject.SetActive(false);
			}
		}
    }
}