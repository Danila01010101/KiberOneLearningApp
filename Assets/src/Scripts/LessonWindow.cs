using System.Collections.Generic;
using UnityEngine;

namespace KiberOneLearningApp
{
    public class LessonWindow : SentencesChanger
    {
        [SerializeField] private LessonWindowView lessonWindowView;
        [SerializeField] private TaskWindowsCreator taskWindowsCreator;
        [SerializeField] private OpenTaskButton openTaskButton;
        [SerializeField] private GameObject writeLessonResultsButton;
        [SerializeField] private GameObject writeLessonButton;

        private List<LessonWithTasksWindow> taskWindows;
        private List<int> taskSentenceIndexes = new(); // индексы предложений, после которых задания[Inject]

        public override void Initialize()
        {
            runtimeData = LessonSceneWithDataOpener.CurrentTutorialData;
            
            if (runtimeData == null)
                return;
            
            base.Initialize();
            writeLessonButton.SetActive(GlobalValueSetter.Instance.IsTeacher);
            writeLessonResultsButton.SetActive(GlobalValueSetter.Instance.IsTeacher);
            ShowSentences();
        }

        private void ShowSentences()
        {
            if (runtimeData.Tasks != null)
            {
                taskWindows = taskWindowsCreator.SetRuntimeTasks(runtimeData.Tasks, this);

                for (int i = 0; i < runtimeData.Sentences.Count; i++)
                {
                    if (runtimeData.Sentences[i].IsBeforeTask)
                        taskSentenceIndexes.Add(i);
                }
            }

            ShowNextSentence();
        }

        protected override void ShowNextSentence()
        {
            base.ShowNextSentence();

            var sentence = runtimeData.Sentences[CurrentIndex];

            // Обновление визуала предложения
            lessonWindowView.UpdateView(
                sentence,
                runtimeData.DefaultBackground,
                CurrentIndex,
                runtimeData.Sentences.Count); // пока нет задачи

            if (sentence.IsBeforeTask)
            {
                int taskIndex = GetTaskIndexBySentence(CurrentIndex);

                if (taskIndex >= 0)
                {
                    // Показать кнопку "Начать задание"
                    sentenceChangerView.UpdateTaskButton(false, sentence, () =>
                        SelectCurrentTask(taskIndex));
                }
                else
                {
                    Debug.LogWarning($"Для предложения с индексом {CurrentIndex} не найдено задание");
                    sentenceChangerView.UpdateTaskButton(true, sentence, null);
                }
            }
            else
            {
                // Просто показать кнопку "Далее"
                sentenceChangerView.UpdateTaskButton(true, sentence, null);
            }

            lessonWindowView.SetupTask(sentence.InteractableImages); // если надо показать спрайты
            lessonWindowView.TaskCompleted += DetectCompletedTasks;
        }

        public void DetectCompletedTasks()
        {
            ShowNextSentence(); // Переход к следующему после выполнения
        }

        private void SelectCurrentTask(int index)
        {
            taskWindowsCreator.OpenTaskWindow(index);
        }

        private int GetTaskIndexBySentence(int sentenceID)
        {
            // Индекс задания = его порядковый номер среди всех IsBeforeTask
            return taskSentenceIndexes.IndexOf(sentenceID);
        }
    }
}