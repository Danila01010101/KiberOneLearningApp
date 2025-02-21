using System.Collections.Generic;
using KiberOneLearningApp;
using UnityEngine;
using UnityEngine.UI;

public class TaskWindowsCreator : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Dropdown dropdown;
    [SerializeField] private Button startTaskButton;
    [SerializeField] private LessonWithTasksWindow tutorialWindowPrefab;
    
    private List<TutorialData> tasksData;
    private List<LessonWithTasksWindow> spawnedTutorialWindows = new List<LessonWithTasksWindow>();
    private SentencesChanger currentTask;

    public void SetTasksData(List<TutorialData> tasksData)
    {
        this.tasksData = tasksData;
        SpawnTasks();
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
    
    private void SpawnTasks()
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
    }

    private void ChangeCurrentTask() => OpenTaskWindow(dropdown.value);
    
    private void DetectLessonWithTasksCompleted()
    {
        UIWindowManager.ShowLast();
    }

    public void Subscribe()
    {
        startTaskButton.onClick.AddListener(ChangeCurrentTask);
    }

    public void Unsubscribe()
    {
        startTaskButton.onClick.RemoveListener(ChangeCurrentTask);
    }

    private void OnDestroy()
    {
        foreach (var window in spawnedTutorialWindows)
        {
            window.TaskLessonCompleted -= DetectLessonWithTasksCompleted;
        }
    }
}