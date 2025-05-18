using System;
using UnityEngine;

namespace KiberOneLearningApp
{
	public class LessonWindow : SentencesChanger
	{
        [SerializeField] private LessonWindowView lessonWindowView;
        [SerializeField] private OpenTaskButton openTaskButton;
        [SerializeField] private GameObject writeLessonButton;

        public override void Initialize()
        {
            base.Initialize();
            writeLessonButton.SetActive(GlobalValueSetter.Instance.IsTeacher);
        }

        private void Start()
        {
            if (runtimeData.Tasks != null)
            {
                // Передаём задания, если есть
                List<LessonWithTasksWindow> taskWindows = taskWindowsCreator.SetRuntimeTasks(runtimeData.Tasks, this);
                List<int> lessonsIndexes = new List<int>();

                for (int j = 0; j < runtimeData.Sentences.Count; j++)
                {
                    if (runtimeData.Sentences[j].IsBeforeTask)
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

            ShowNextSentence(); // Начинаем отображение
        }

        protected override void ShowNextSentence()
        {
            base.ShowNextSentence();

            var sentence = runtimeData.Sentences[CurrentIndex];

            if (sentence.IsBeforeTask)
            {
                RuntimeTutorialData taskForThisSentence = GetTaskBySentenceID(CurrentIndex);

                if (!taskForThisSentence.IsCompleted)
                {
                    lessonWindowView.SetupTask();
                    sentenceChangerView.UpdateTaskButton(false, sentence, () =>
                        SelectCurrentTask(taskForThisSentence.TaskIndex));
                }
                else
                {
                    sentenceChangerView.UpdateTaskButton(true, sentence, null);
                }
            }
            else
            {
                sentenceChangerView.UpdateTaskButton(true, sentence, null);
            }
            
            lessonWindowView.SetupTasks(sentence);
        }

        public void DetectCompletedTasks() => ShowNextSentence();

        private void SelectCurrentTask(int index) => lessonWindowView.UpdateTask();

        private RuntimeTutorialData GetTaskBySentenceID(int sentenceID)
        {
            foreach (var task in runtimeData.Tasks)
            {
                if (task.TaskID == sentenceID)
                    return task;
            }

            throw new Exception("Not valid sentence ID for task");
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