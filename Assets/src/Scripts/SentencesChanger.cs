using System;
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
        [FormerlySerializedAs("taskWindow")] [SerializeField] private TaskCreater taskCreater;
        [Header("Sentence buttons")]
        [SerializeField] private Button nextButton;
        [SerializeField] private Button backButton;
        [SerializeField] private OpenTaskButton openTaskButton;
        [Header("Gif properties")]
        [SerializeField] private VideoPlayer player;
        [SerializeField] private Transform videoWindow;
        [SerializeField] private Button openGifButton;
        [SerializeField] private Button closeGifButton;

        private GifOpener gifOpener;
        private int currentTaskIndex;
        private int currentIndex = -1;
        
        public Action<int> OnTaskUnlocked;

        public string LessonName => tutorialData.TutorialName;

        private void Awake()
        {
            gifOpener = new GifOpener(player, videoWindow);
            
            if (tutorialData != null)
                ShowNextSentence();
            
            if (openTaskButton != null)
                openTaskButton.Initialize(this, taskCreater);

            if (taskCreater != null)
            {
                taskCreater.SetTasksData(tutorialData.Tasks);
                taskCreater.Initialize();
            }
        }
        
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

        private void ShowNextSentence() => ShowSentence(tutorialData.Sentences[++currentIndex]);
        
        private void ShowPreviousSentence() => ShowSentence(tutorialData.Sentences[--currentIndex]);

        private void ShowSentence(TutorialData.SentenceData sentenceData)
        {
            sentenceSlider.value = (currentIndex + 1) / (float)tutorialData.Sentences.Count;
            Debug.Log(sentenceSlider.value);
            character.sprite = sentenceData.CharacterIcon;
            character.transform.localPosition = sentenceData.CharacterPosition;
            characterText.text = sentenceData.Text;
            background.sprite = sentenceData.Background != null ? background.sprite = sentenceData.Background : background.sprite = tutorialData.DefaultBackground;

            if (sentenceData.IsBeforeTask && (nextButton.IsActive() || !openTaskButton.isActiveAndEnabled))
            {
                nextButton.gameObject.SetActive(false);
                openTaskButton.gameObject.SetActive(true);
                OnTaskUnlocked?.Invoke(currentTaskIndex);
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
        }
        
        private void OnEnable()
        {
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
