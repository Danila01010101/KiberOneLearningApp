using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KiberOneLearningApp
{
	public class RuntimeLessonEditorManagerView : UIWindow
	{
        [Header("UI Elements")]
        public TMP_InputField themeInputField;
        public TMP_InputField lessonNameInputField;
        public Button createNewButton;
        public Button loadExistingButton;
        public TMP_Dropdown existingLessonsDropdown;

        private RuntimeLessonEditorManager manager;

        public static System.Action<RuntimeTutorialData> OnLessonToEditSelected;
        
        public override void Show()
        {
            RefreshDropdown();
        }

        public override void Initialize()
        {
            manager = RuntimeLessonEditorManager.Instance;
            createNewButton.onClick.AddListener(OnCreateLesson);
            loadExistingButton.onClick.AddListener(OnLoadLesson);
        }

        private void RefreshDropdown()
        {
            var lessons = manager.GetAvailableLessonFiles();
            existingLessonsDropdown.ClearOptions();
            existingLessonsDropdown.AddOptions(lessons);
        }

        private void OnCreateLesson()
        {
            string theme = themeInputField.text.Trim();
            string name = lessonNameInputField.text.Trim();

            if (string.IsNullOrEmpty(theme) || string.IsNullOrEmpty(name))
            {
                Debug.LogWarning("Укажите и тему, и имя урока.");
                return;
            }

            manager.CreateNewLesson(theme, name);
            Debug.Log($"Создан урок: {name}");
        }

        private void OnLoadLesson()
        {
            string selected = existingLessonsDropdown.options[existingLessonsDropdown.value].text;
            if (manager.SelectChangeLesson(selected))
            {
                OnLessonToEditSelected?.Invoke(manager.CurrentLesson);
                Debug.Log($"Урок будет загружен: {manager.CurrentLesson?.TutorialName}");
            }
            else
                Debug.LogError("Ошибка загрузки.");
        }
    }
}