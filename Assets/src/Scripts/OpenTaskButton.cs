using UnityEngine;
using UnityEngine.UI;

namespace KiberOneLearningApp
{
    public class OpenTaskButton : MonoBehaviour
    {
        private SentencesChanger sentencesChanger;
        private TaskWindowsCreator taskWindowsCreator;
        private Button button;
        private int taskID;

        private void Awake()
        {
            button = GetComponent<Button>();
        }

        private void InitializeTask(int newTaskId)
        {
            button.onClick.RemoveAllListeners();
            taskID = newTaskId;
            button.onClick.AddListener(OpenCurrentTask);
        }

        private void OpenCurrentTask()
        {
            taskWindowsCreator.OpenTaskWindow(taskID);
        }

        public void Initialize(SentencesChanger sentencesChanger, TaskWindowsCreator taskWindowsCreator)
        {
            this.sentencesChanger = sentencesChanger;
            this.taskWindowsCreator = taskWindowsCreator;
            //sentencesChanger.OnTaskUnlocked += InitializeTask;
        }

        private void OnDestroy()
        {
            button.onClick.RemoveAllListeners();
            //sentencesChanger.OnTaskUnlocked -= InitializeTask;
        }
    }
}
