using System;
using System.Collections.Generic;
using KiberOneLearningApp;
using UnityEngine;
using UnityEngine.UI;

public class TaskWindow : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Dropdown dropdown;
    [SerializeField] private Button startTaskButton;
    [SerializeField] private SentencesChanger sentencesChangerPrefab;
    
    private List<TutorialData> tasksData;
    private List<SentencesChanger> spawnedTasksWindows = new List<SentencesChanger>();
    private SentencesChanger currentTask;
    
    public void Initialize()
    {
        dropdown.options.Clear();
        
        foreach (var task in tasksData)
        {
            SentencesChanger newWindow = Instantiate(sentencesChangerPrefab, transform);
            newWindow.ChangeSentence(task);
            spawnedTasksWindows.Add(newWindow);
            dropdown.options.Add(new TMPro.TMP_Dropdown.OptionData() { text = task.TutorialName });
            newWindow.gameObject.SetActive(false);
        }
    }

    public void SetTasksData(List<TutorialData> tasksData)
    {
        this.tasksData = tasksData;
    }

    private void OpenTaskWindow(int taskId)
    {
        if (currentTask != null)
            currentTask.gameObject.SetActive(false);
        
        currentTask = spawnedTasksWindows[taskId];
        currentTask.gameObject.SetActive(true);
    }

    private void ChangeCurrentTask() => OpenTaskWindow(dropdown.value);

    private void OnEnable()
    {
        startTaskButton.onClick.AddListener(ChangeCurrentTask);
    }

    private void OnDisable()
    {
        startTaskButton.onClick.RemoveListener(ChangeCurrentTask);
    }
}