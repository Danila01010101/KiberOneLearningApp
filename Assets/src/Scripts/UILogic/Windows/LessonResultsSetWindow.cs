using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KiberOneLearningApp
{
    public class LessonResultsSetWindow : UIWindow
    {
        [Header("UI References")]
        public TMP_InputField topicInput;
        public TMP_InputField dateInput;
        public Transform studentListParent;
        public GameObject studentEntryPrefab;
        public Button confirmButton;

        [Header("Excel / Data")]
        public string excelPath;
        private Data loadedData;
        private Data.GroupData currentGroup;

        private List<string> students = new List<string>();
        
        private class StudentUI
        {
            public TMP_InputField inputField;
            public Toggle presentToggle;
        }
        
        private Dictionary<string, StudentUI> studentUIMap = new Dictionary<string, StudentUI>();

        public override void Initialize()
        {
            loadedData = GameSaver.Instance.Data;
            
            if (loadedData == null || loadedData.studentGroups.Count == 0)
            {
                Debug.Log("No Exel Data Loaded");
                return;
            }
            
            currentGroup = loadedData.studentGroups[0];

            var studentSet = new HashSet<string>();
            foreach (var lesson in currentGroup.lessonResults)
                foreach (var name in lesson.studentResultData.Keys)
                    studentSet.Add(name);

            students = new List<string>(studentSet);
            int i = 0;

            foreach (var student in students)
            {
                var entry = Instantiate(studentEntryPrefab, studentListParent);

                entry.transform.Find("Name").GetComponent<TMP_Text>().text = student;
                entry.gameObject.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0, entry.gameObject.GetComponent<RectTransform>().rect.height * 1.25f * i++);

                var input = entry.transform.Find("ScoreInput").GetComponent<TMP_InputField>();
                var toggle = entry.transform.Find("PresentToggle").GetComponent<Toggle>();

                toggle.isOn = true;
                input.interactable = true;

                toggle.onValueChanged.AddListener(isOn => input.interactable = isOn);

                studentUIMap[student] = new StudentUI
                {
                    inputField = input,
                    presentToggle = toggle
                };
            }

            confirmButton.onClick.AddListener(OnConfirmClicked);
        }

        private void OnConfirmClicked()
        {
            var newLesson = new Data.LessonResult
            {
                date = dateInput.text.Trim(),
                topicName = topicInput.text.Trim(),
                studentResultData = new Dictionary<string, int>()
            };

            foreach (var student in students)
            {
                var ui = studentUIMap[student];

                if (!ui.presentToggle.isOn) continue;

                string input = ui.inputField.text.Trim();
                if (int.TryParse(input, out int score))
                {
                    newLesson.studentResultData[student] = score;
                }
            }

            currentGroup.lessonResults.Add(newLesson);
            ExcelConverter.ExportToExcel(loadedData, excelPath);

            Debug.Log("Занятие проведено и добавлено в Excel.");
        }
    }
}
