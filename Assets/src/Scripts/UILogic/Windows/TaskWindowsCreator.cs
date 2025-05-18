using System.Collections.Generic;
using KiberOneLearningApp;
using UnityEngine;
using UnityEngine.UI;

public class TaskWindowsCreator : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Dropdown dropdown;
    [SerializeField] private Button startTaskButton;
    [SerializeField] private LessonWithTasksWindow tutorialWindowPrefab;
    
    private List<RuntimeTutorialData> tasksData;
    private List<LessonWithTasksWindow> spawnedTutorialWindows = new List<LessonWithTasksWindow>();
    private LessonWindow lessonWindow;
    private SentencesChanger currentTask;

    public List<LessonWithTasksWindow> SetTasksData(List<RuntimeTutorialData> tasksData, LessonWindow lessonWindow)
    {
        this.lessonWindow = lessonWindow;
        this.tasksData = tasksData;
        return SpawnTasks();
    }

    public void OpenTaskWindow(int taskId)
    {
        if (currentTask != null)
        {
            currentTask.gameObject.SetActive(false);
        }
        
        currentTask = spawnedTutorialWindows[taskId];
        UIWindowManager.Show(currentTask);
        currentTask.gameObject.SetActive(true);
    }
    
    private List<LessonWithTasksWindow> SpawnTasks()
    {
        dropdown.options.Clear();

        for (int i = 0; i < tasksData.Count; i++)
        {
            LessonWithTasksWindow newWindow = Instantiate(tutorialWindowPrefab, transform);
            newWindow.SetNewData(tasksData[i]);
            spawnedTutorialWindows.Add(newWindow);
            UIWindowManager.AddWindow(newWindow);
            newWindow.TaskLessonCompleted += DetectLessonWithTasksCompleted;
            dropdown.options.Add(new TMPro.TMP_Dropdown.OptionData() { text = tasksData[i].TutorialName });
        }

        return spawnedTutorialWindows;
    }

    private void ChangeCurrentTask() => OpenTaskWindow(dropdown.value);
    
    private void DetectLessonWithTasksCompleted()
    {
        UIWindowManager.ShowLast();
        lessonWindow.DetectCompletedTasks();
    }

    public void Subscribe()
    {
        startTaskButton.onClick.AddListener(ChangeCurrentTask);
    }

    public void Unsubscribe()
    {
        startTaskButton.onClick.RemoveListener(ChangeCurrentTask);
    }

    private void OnEnable()
    {
        Subscribe();
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    private void OnDestroy()
    {
        foreach (var window in spawnedTutorialWindows)
        {
            window.TaskLessonCompleted -= DetectLessonWithTasksCompleted;
        }
    }
}