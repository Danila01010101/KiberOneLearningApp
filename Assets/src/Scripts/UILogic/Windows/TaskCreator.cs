using System;
using System.Collections.Generic;
using KiberOneLearningApp;
using UnityEngine;
using UnityEngine.UI;

public class TaskCreator : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Dropdown dropdown;
    [SerializeField] private Button startTaskButton;
    [SerializeField] private SentencesChanger sentencesChangerPrefab;
    
    private List<TutorialData> tasksData;
    private List<SentencesChanger> spawnedTaskWindows = new List<SentencesChanger>();
    private List<bool> completedTasks = new List<bool>();
    private SentencesChanger currentTask;
    private SentencesChanger mainSentenceChanger;
    
    public Action TaskCompleted;
    
    public bool IsSentenceCompleted(int index) => completedTasks[index];
    
    private void SpawnTasks()
    {
        dropdown.options.Clear();
        
        foreach (var task in tasksData)
        {
            SentencesChanger newWindow = Instantiate(sentencesChangerPrefab, transform);
            newWindow.ChangeSentence(task);
            spawnedTaskWindows.Add(newWindow);
            newWindow.OnTaskSolved += DetectTaskCompleted;
            completedTasks.Add(false);
            dropdown.options.Add(new TMPro.TMP_Dropdown.OptionData() { text = task.TutorialName });
            newWindow.gameObject.SetActive(false);
        }
    }

    public void SetTasksData(List<TutorialData> tasksData, SentencesChanger mainSentenceChanger)
    {
        this.tasksData = tasksData;
        this.mainSentenceChanger = mainSentenceChanger;
        SpawnTasks();
    }

    public void OpenTaskWindow(int taskId)
    {
        UIWindowManager.Show<TutorialWindow>();
        
        if (currentTask != null)
            currentTask.gameObject.SetActive(false);
        currentTask = spawnedTaskWindows[taskId];
        currentTask.gameObject.SetActive(true);
    }

    private void ChangeCurrentTask() => OpenTaskWindow(dropdown.value);
    
    private void DetectTaskCompleted(int index)
    {
        completedTasks[index] = true;
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
        foreach (var window in spawnedTaskWindows)
        {
            window.OnTaskSolved -= DetectTaskCompleted;
        }
    }
}