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
    
    private List<TutorialData> tutorialData;
    private List<SentencesChanger> spawnedTutorialWindows = new List<SentencesChanger>();
    private List<MouseTask> spawnedTasksWindows = new List<MouseTask>();
    private List<bool> completedTasks = new List<bool>();
    private SentencesChanger currentTask;
    private SentencesChanger mainSentenceChanger;
    
    public Action TaskCompleted;
    
    public bool IsSentenceCompleted(int index) => completedTasks[index];
    
    private void SpawnTasks()
    {
        dropdown.options.Clear();

        for (int i = 0; i < tutorialData.Count; i++)
        {
            SentencesChanger newWindow = Instantiate(sentencesChangerPrefab, transform);
            newWindow.ChangeSentence(tutorialData[i]);

            if (tutorialData[i].TaskForThisSentence != null)
            {
                var spawnedTask = Instantiate(tutorialData[i].TaskForThisSentence, newWindow.transform);
                spawnedTasksWindows.Add(spawnedTask);
                newWindow.SetTask(spawnedTask);
            }

            spawnedTutorialWindows.Add(newWindow);
            newWindow.OnTaskSolved += DetectTaskCompleted;
            completedTasks.Add(false);
            dropdown.options.Add(new TMPro.TMP_Dropdown.OptionData() { text = tutorialData[i].TutorialName });
            newWindow.gameObject.SetActive(false);
        }
    }

    public void SetTasksData(List<TutorialData> tasksData, SentencesChanger mainSentenceChanger)
    {
        this.tutorialData = tasksData;
        this.mainSentenceChanger = mainSentenceChanger;
        SpawnTasks();
    }

    public void OpenTaskWindow(int taskId)
    {
        UIWindowManager.Show<TutorialWindow>();
        
        if (currentTask != null)
            currentTask.gameObject.SetActive(false);
        currentTask = spawnedTutorialWindows[taskId];
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
        foreach (var window in spawnedTutorialWindows)
        {
            window.OnTaskSolved -= DetectTaskCompleted;
        }
    }
}