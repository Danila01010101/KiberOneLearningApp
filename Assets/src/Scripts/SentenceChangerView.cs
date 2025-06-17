using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Video;

namespace KiberOneLearningApp
{
    public class SentenceChangerView : MonoBehaviour
    {
        [SerializeField] private ObjectForTask taskPrefab;
        [SerializeField] private Transform taskObjectsParent;
        [Header("UI Elements")]
        [SerializeField] private Image character;
        [SerializeField] private Image background;
        [SerializeField] private TextMeshProUGUI characterText;
        [SerializeField] private Slider sentenceSlider;
        [SerializeField] private NotEditableRuntimeViewManager visualElementsManager;
        [Header("Sentence buttons")]
        [SerializeField] protected Button nextButton;
        [SerializeField] protected Button openCurrentTaskButton;
        
        [SerializeField] private Button backButton;
        [SerializeField] private Button openGifButton;
        [Header("Gif properties")]
        [SerializeField] private VideoPlayer player;
        [SerializeField] private Transform videoWindow;
    
        private GifOpener gifOpener;
        private TextMeshProUGUI buttonText;
        private string buttonNameFromStart;
        private bool isEditing;

        public Action TaskCompleted;
		
        public void SetupTask(List<RuntimeInteractablePlacement> taskImagesData)
        {
            ObjectForTask previousTask = null;
			
            if (taskImagesData != null && isEditing == false)
            {
                foreach (var placement in taskImagesData)
                {
                    ObjectForTask imageView = Instantiate(taskPrefab, taskObjectsParent);
                    imageView.Initialize(placement);
                    if (previousTask != null)
                    {
                        previousTask.OnCompleted += imageView.Activate;
                    }

                    previousTask = imageView;
                }
            }

            if (previousTask != null)
                previousTask.OnCompleted += CompleteTask;
        }

        public void Initialize()
        {
            gifOpener = new GifOpener(player, videoWindow);
            RuntimeLessonEditorManager.Instance.SentenceChanged += UpdateView;
            isEditing = GetComponent<RuntimeLessonEditorView>();
            buttonText = nextButton.GetComponentInChildren<TextMeshProUGUI>();
            buttonNameFromStart = nextButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text;
            background.enabled = false;
        }

        public void UpdateView(
            RuntimeSentenceData sentenceData,
            Sprite defaultBackground,
            int currentIndex,
            int sentenceCount)
        {
            characterText.text = sentenceData.Text;
            
            /*
            if (sentenceData.CharacterIcon != null)
            {
                character.color = Color.white;
            }
            else
            {
                character.color = Color.clear;
            }
            */

            if (sentenceData.CharacterIcon != null && isEditing == false)
            {
                character.sprite = sentenceData.CharacterIcon;
                character.transform.localPosition = sentenceData.CharacterPosition;

                character.rectTransform.sizeDelta = new Vector2(
                    sentenceData.CharacterSize.x,
                    sentenceData.CharacterSize.y
                );
            }

            character.transform.localPosition = sentenceData.CharacterPosition;
            backButton.gameObject.SetActive(currentIndex != 0);

            sentenceSlider.value = (currentIndex + 1) / (float)sentenceCount;
            background.sprite = sentenceData.Background != null ? sentenceData.Background : defaultBackground;

            // Видео
            
            if (!string.IsNullOrEmpty(sentenceData.TutorialVideoPath))
            {
                openGifButton.gameObject.SetActive(true);
                gifOpener.SetNewVideo(Path.Combine(Application.persistentDataPath, sentenceData.TutorialVideoPath));
            }
            else
            {
            
                openGifButton.gameObject.SetActive(false);
            }

            character.gameObject.SetActive(!sentenceData.HideCharacter);
            
            if (currentIndex == sentenceCount - 1)
            {
                buttonText.text = "Закончить";
            }
            else
            {
                buttonText.text = buttonNameFromStart;
            }
            
            if (isEditing == true)
                return;
            
            visualElementsManager.RefreshVisuals(sentenceData);
        }


        public virtual void UpdateTaskButton(bool isTaskSolved, RuntimeSentenceData sentenceData,
            UnityAction openTask)
        {
            
        }

        public void BlockNextButton()
        {
            nextButton.interactable = false;
        }

        public void UnlockNextButton()
        {
            nextButton.interactable = true;
        }

        public void SubscribeButtons(UnityAction nextButtonAction, UnityAction backButtonAction)
        {
            nextButton.onClick.AddListener(nextButtonAction);
            backButton.onClick.AddListener(backButtonAction);
        }

        public void UnsubscribeButtons()
        {
            nextButton.onClick.RemoveAllListeners();
            backButton.onClick.RemoveAllListeners();
            
            if (RuntimeLessonEditorManager.Instance != null)
                RuntimeLessonEditorManager.Instance.SentenceChanged -= UpdateView;
        }
        
        private void CompleteTask()
        {
            UnlockNextButton();
            TaskCompleted?.Invoke();
        }
    }
}
