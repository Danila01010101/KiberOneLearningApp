using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KiberOneLearningApp
{
	public class EditorTaskWindowsCreator : MonoBehaviour
	{
        [SerializeField] private TMP_Dropdown dropdown;
        [SerializeField] private Button startEditButton;
        [SerializeField] private RuntimeLessonEditorView editorWindowPrefab;

        private List<RuntimeTutorialData> lessonsData;
        private RuntimeLessonEditorView spawnedEditorWindows;
        private RuntimeLessonEditorView currentEditor;

        public RuntimeLessonEditorView SetRuntimeLessons(List<RuntimeTutorialData> lessons)
        {
            this.lessonsData = lessons;
            return SpawnLessonEditors();
        }

        public RuntimeLessonEditorView SpawnLessonEditors()
        {
            dropdown.options.Clear();
            spawnedEditorWindows = null;

            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i).gameObject;
                if (child.TryGetComponent(out UIWindow window))
                    UIWindowManager.RemoveWindow(window);
                Destroy(child);
            }
            
            var newWindow = Instantiate(editorWindowPrefab, transform);

            newWindow.InitializeAsTask();
            spawnedEditorWindows = newWindow;
            UIWindowManager.AddWindow(newWindow.gameObject.GetComponent<UIWindow>());

            for (int i = 0; i < lessonsData.Count; i++)
            {
                dropdown.options.Add(new TMP_Dropdown.OptionData
                {
                    text = lessonsData[i].TutorialName
                });
            }

            dropdown.value = 0;
            dropdown.RefreshShownValue();

            return spawnedEditorWindows;
        }
        
        public void OpenEditorWindow(int lessonIndex)
        {
            if (RuntimeLessonEditorManager.Instance != null && RuntimeLessonEditorManager.Instance.CurrentLesson.Tasks != null && lessonIndex < RuntimeLessonEditorManager.Instance.CurrentLesson.Tasks.Count)
            {
                RuntimeLessonEditorManager.Instance.CurrentTask = RuntimeLessonEditorManager.Instance.CurrentLesson.Tasks[lessonIndex];
            }

            // Открываем окно редактора
            if (currentEditor != null)
                currentEditor.gameObject.SetActive(false);

            currentEditor = spawnedEditorWindows;
            currentEditor.InitializeAsTask();
            UIWindowManager.Show(currentEditor.gameObject.GetComponent<UIWindow>());
            currentEditor.gameObject.SetActive(true);
        }

        private void OnEnable() => startEditButton.onClick.AddListener(() => OpenEditorWindow(dropdown.value));
        private void OnDisable() => startEditButton.onClick.RemoveListener(() => OpenEditorWindow(dropdown.value));
	}
}