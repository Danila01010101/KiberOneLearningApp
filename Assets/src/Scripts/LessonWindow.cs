using System;
using System.Collections.Generic;
using UnityEngine;

namespace KiberOneLearningApp
{
    public class LessonWindow : SentencesChanger
    {
        [SerializeField] private LessonWindowView lessonWindowView;
        [SerializeField] private TaskWindowsCreator taskWindowsCreator;
        [SerializeField] private EditorTaskWindowsCreator editTaskWindowsCreator;
        [SerializeField] private OpenTaskButton openTaskButton;
        [SerializeField] private GameObject writeLessonResultsButton;
        [SerializeField] private GameObject writeLessonButton;

        private List<LessonWithTasksWindow> taskWindows;
        private RuntimeLessonEditorView editableTaskWindow;
        private List<int> taskSentenceIndexes = new();

        public override void Initialize()
        {
            RuntimeLessonEditorView.TaskCreated += RefreshTaskIndexes;
            if (GetComponent<RuntimeLessonEditorView>() == null)
            {
                runtimeData = LessonSceneWithDataOpener.CurrentTutorialData;
            }
            else
            {
                runtimeData = RuntimeLessonEditorManager.Instance.CurrentLesson;
            }
            
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
                if (editTaskWindowsCreator != null)
                {
                    editableTaskWindow = editTaskWindowsCreator.SetRuntimeLessons(RuntimeLessonEditorManager.Instance.CurrentLesson.Tasks);
                }
                else if (taskWindowsCreator != null)
                {
                    taskWindows = taskWindowsCreator.SetRuntimeTasks(runtimeData.Tasks, this);
                }

                RefreshTaskIndexes();
            }

            ShowNextSentence();
        }

        protected override void ShowNextSentence()
        {
            base.ShowNextSentence();

            var sentence = RuntimeLessonEditorManager.Instance != null ? RuntimeLessonEditorManager.Instance.CurrentLesson.Sentences[CurrentIndex] : runtimeData.Sentences[CurrentIndex];

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

        private void RefreshTaskIndexes()
        {
            taskSentenceIndexes.Clear();
            
            for (int i = 0; i < RuntimeLessonEditorManager.Instance.CurrentLesson.Sentences.Count; i++)
            {
                Debug.Log(gameObject.name);
                if (RuntimeLessonEditorManager.Instance.CurrentLesson.Sentences[i].IsBeforeTask)
                    taskSentenceIndexes.Add(i);
            }
        }

        public void DetectCompletedTasks()
        {
            ShowNextSentence(); // Переход к следующему после выполнения
        }

        private void SelectCurrentTask(int index)
        {
            if (editTaskWindowsCreator == null)
                taskWindowsCreator.OpenTaskWindow(index);
            else
            {
                editTaskWindowsCreator.OpenEditorWindow(index);
            }
        }

        private int GetTaskIndexBySentence(int sentenceID)
        {
            // Индекс задания = его порядковый номер среди всех IsBeforeTask
            return taskSentenceIndexes.IndexOf(sentenceID);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            RuntimeLessonEditorView.TaskCreated -= RefreshTaskIndexes;
        }
    }
}