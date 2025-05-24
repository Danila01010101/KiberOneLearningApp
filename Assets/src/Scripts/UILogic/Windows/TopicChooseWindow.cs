using System.Collections.Generic;
using KiberOneLearningApp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TopicChooseWindow : UIWindow
{
    [Header("Button generation")]
    public Transform themeListParent; // Контейнер Scroll View для тем
    public GameObject themeButtonPrefab; // Префаб кнопки темы
    public LessonChooseWindow lessonSelector; // Ссылка на второй компонент
    
    [Header("Basic settings")]
    [SerializeField] private UIWindow nextWindow;
    [SerializeField] private Button nextWindowButton;
    [SerializeField] private Button lessonSettingsButton;
    
    private bool IsButtonExist => nextWindowButton != null;
    
    public override void Initialize()
    {
        if (IsButtonExist)
            nextWindowButton.onClick.AddListener(OpenNextWindow);
        
        DisplayThemes(LessonsLoader.LessonsDictionary);
    }
    
    public void DisplayThemes(Dictionary<string, List<RuntimeTutorialData>> lessonsByTheme)
    {
        // Очистить предыдущие кнопки
        foreach (Transform child in themeListParent)
            Destroy(child.gameObject);

        // Создать кнопку на каждую тему
        foreach (var kvp in lessonsByTheme)
        {
            string themeName = kvp.Key;
            List<RuntimeTutorialData> lessons = kvp.Value;

            GameObject buttonGO = Instantiate(themeButtonPrefab, themeListParent);
            buttonGO.GetComponentInChildren<TextMeshProUGUI>().text = themeName;

            buttonGO.GetComponent<Button>().onClick.AddListener(() =>
            {
                lessonSelector.DisplayLessons(themeName, lessons);
                UIWindowManager.Show<LessonChooseWindow>();
            });
        }
    }

    public override void Show()
    {
        base.Show();
        lessonSettingsButton.gameObject.SetActive(GlobalValueSetter.Instance.IsTeacher);
    }

    private void OpenNextWindow() => UIWindowManager.Show(nextWindow, true);
}