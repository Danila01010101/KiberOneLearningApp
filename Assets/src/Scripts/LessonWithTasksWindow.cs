using System;
using System.Collections.Generic;
using UnityEngine;

namespace KiberOneLearningApp
{
	public class LessonWithTasksWindow : SentencesChanger
    {
        private List<ITask> tasks = new List<ITask>();
		private Transform tasksParent;

		public Action TaskLessonCompleted;

        public void SetNewData(RuntimeTutorialData newSentenceData)
        {
            if (tasksParent != null)
            {
                Destroy(tasksParent.gameObject);
            }

            runtimeData = newSentenceData;
            EmptyTask emptyTaskPrefab = Resources.Load<EmptyTask>("EmptyTask");
			tasksParent = new GameObject("Tasks").transform;
			tasksParent.SetParent(transform);
			
			/*
            foreach (var sentence in runtimeData.Sentences)
			{
				if (sentence.TaskForThisSentence != null)
				{
                    RuntimeTutorialData spawnedTask = Instantiate(sentence.TaskPrefab, transform);
					SetupNewTask(spawnedTask);
					sentenceChangerView.BlockNextButton();
					spawnedTask.OnTaskComplete += sentenceChangerView.UnlockNextButton;
                }
				else
				{
					ITask spawnedTask = Instantiate(emptyTaskPrefab, gameObject.transform);
					SetupNewTask(spawnedTask);
				}
			}
			*/
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

		private void SetupNewTask(ITask spawnedTask)
        {
            tasks.Add(spawnedTask);
            spawnedTask.GameObject.transform.SetParent(tasksParent.transform);
			spawnedTask.GameObject.SetActive(false);
			spawnedTask.Setup();
        }

		private void ResetCurrentSentence()
		{
            if (tasks[CurrentIndex] != null && tasks[CurrentIndex].IsCompleted == false)
            {
	            sentenceChangerView.BlockNextButton();
                tasks[CurrentIndex].GameObject.SetActive(true);
            }
        }

		private void DeactivateIfExist(int index)
        {
            if (index > 0 && index < tasks.Count)
            {
                tasks[index].GameObject.SetActive(false);
            }
        }
    }
}