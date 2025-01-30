using UnityEngine;
using UnityEngine.UI;

namespace KiberOneLearningApp
{
    public class OpenTaskButton : MonoBehaviour
    {
        private SentencesChanger sentencesChanger;
        private TaskCreator taskCreator;
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
            taskCreator.OpenTaskWindow(taskID);
        }

        public void Initialize(SentencesChanger sentencesChanger, TaskCreator taskCreator)
        {
            this.sentencesChanger = sentencesChanger;
            this.taskCreator = taskCreator;
            sentencesChanger.OnTaskUnlocked += InitializeTask;
        }

        private void OnDestroy()
        {
            button.onClick.RemoveAllListeners();
            sentencesChanger.OnTaskUnlocked -= InitializeTask;
        }
    }
}
