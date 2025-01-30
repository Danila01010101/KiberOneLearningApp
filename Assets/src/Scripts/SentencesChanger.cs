using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.Video;

namespace KiberOneLearningApp
{
    public class SentencesChanger : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private Image character;
        [SerializeField] private Image background;
        [SerializeField] private TextMeshProUGUI characterText;
        [SerializeField] private TutorialData tutorialData;
        [SerializeField] private Slider sentenceSlider;
        [FormerlySerializedAs("taskCreater")] [FormerlySerializedAs("taskWindow")] [SerializeField] private TaskCreator taskCreator;
        [Header("Sentence buttons")]
        [SerializeField] private Button nextButton;
        [SerializeField] private Button backButton;
        [SerializeField] private Button openSelectedTaskButton;
        [SerializeField] private OpenTaskButton openTaskButton;
        [SerializeField] private Button completeTaskButton;
        [Header("Gif properties")]
        [SerializeField] private VideoPlayer player;
        [SerializeField] private Transform videoWindow;
        [SerializeField] private Button openGifButton;
        [SerializeField] private Button closeGifButton;

        private GifOpener gifOpener;
        private List<int> taskIndexes = new List<int>();
        private int currentTaskIndex;
        private int currentIndex = -1;
        
        public Action<int> OnTaskUnlocked;
        public Action<int> OnTaskSolved;

        public string LessonName => tutorialData.TutorialName;
        
        private bool ShouldShowBackButton => currentIndex != 0;

        private void Awake()
        {
            gifOpener = new GifOpener(player, videoWindow);
            
            if (tutorialData != null)
                ShowNextSentence();
            
            if (openTaskButton != null)
                openTaskButton.Initialize(this, taskCreator);

            if (taskCreator != null)
            {
                for (int i = 0; i < tutorialData.Sentences.Count; i++)
                {
                    if (tutorialData.Sentences[i].IsBeforeTask)
                        taskIndexes.Add(i);
                }
                
                taskCreator.SetTasksData(tutorialData.Tasks, this);
            }
        }

        public void ShowTaskWindow() => UIWindowManager.Show<TutorialWindow>();
        
        public void ChangeSentence(TutorialData sentence)
        {
            tutorialData = sentence;
            
            if (tutorialData != null)
            {
                ShowNextSentence();
            }
            else
            {
                throw new Exception("No data setted before activation!");
            }
        }

        private void ShowNextSentence()
        {
            if (currentIndex + 1 >= tutorialData.Sentences.Count)
            {
                OnTaskSolved?.Invoke(currentTaskIndex);
                return;
            }
            
            ShowSentence(tutorialData.Sentences[++currentIndex]);
        }
        
        private void ShowPreviousSentence() => ShowSentence(tutorialData.Sentences[--currentIndex]);

        private void ShowSentence(TutorialData.SentenceData sentenceData)
        {
            sentenceSlider.value = (currentIndex + 1) / (float)tutorialData.Sentences.Count;
            Debug.Log(sentenceSlider.value);
            character.sprite = sentenceData.CharacterIcon;
            character.transform.localPosition = sentenceData.CharacterPosition;
            characterText.text = sentenceData.Text;
            background.sprite = sentenceData.Background != null ? background.sprite = sentenceData.Background : background.sprite = tutorialData.DefaultBackground;

            backButton.gameObject.SetActive(ShouldShowBackButton);
            
            if (sentenceData.IsBeforeTask && (nextButton.IsActive() || !openTaskButton.isActiveAndEnabled))
            {
                int taskIndex = taskIndexes.IndexOf(currentIndex);

                if (taskCreator != null && taskCreator.IsSentenceCompleted(taskIndex) == false)
                {
                    nextButton.gameObject.SetActive(false);
                    openTaskButton.gameObject.SetActive(true);
                    OnTaskUnlocked?.Invoke(currentTaskIndex);
                }
                else
                {
                    nextButton.gameObject.SetActive(true);
                    openTaskButton.gameObject.SetActive(false);
                }
            }
            else if (!nextButton.IsActive() || openTaskButton.isActiveAndEnabled)
            {
                nextButton.gameObject.SetActive(true);
                openTaskButton.gameObject.SetActive(false);
            }
            
            if (sentenceData.TutorialVideo != null)
            {
                openGifButton.gameObject.SetActive(true);
                gifOpener.SetNewVideo(sentenceData.TutorialVideo);
            }
            else
            {
                openGifButton.gameObject.SetActive(false);
            }
            
            //CheckIfSentenceLast();
        }

        //private void CheckIfSentenceLast() => completeTaskButton
        
        private void OnEnable()
        {
            if (currentIndex > 0)
                ShowSentence(tutorialData.Sentences[currentIndex]);
            
            nextButton.onClick.AddListener(ShowNextSentence);
            backButton.onClick.AddListener(ShowPreviousSentence);
            gifOpener.Subscribe(openGifButton, closeGifButton);
        }
        
        private void OnDisable()
        {
            nextButton.onClick.RemoveListener(ShowNextSentence);
            backButton.onClick.RemoveListener(ShowPreviousSentence);
            gifOpener.Unsubscribe(openGifButton, closeGifButton);
        }
    }
}
