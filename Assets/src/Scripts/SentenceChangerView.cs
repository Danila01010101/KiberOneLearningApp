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
        [SerializeField] private ImagePlacementView imageViewPrefab;
        [SerializeField] private Transform imagesParent;
        [Header("Sentence buttons")]
        [SerializeField] protected Button nextButton;
        [SerializeField] protected Button openCurrentTaskButton;
        
        [SerializeField] private Button backButton;
        [SerializeField] private Button openGifButton;
        [Header("Gif properties")]
        [SerializeField] private VideoPlayer player;
        [SerializeField] private Transform videoWindow;
    
        private GifOpener gifOpener;

        public Action TaskCompleted;
		
        public void SetupTask(List<RuntimeInteractablePlacement> taskImagesData)
        {
            ObjectForTask previousTask = null;
			
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

            previousTask.OnCompleted += CompleteTask;
        }

        public void Initialize()
        {
            gifOpener = new GifOpener(player, videoWindow);
        }

        public void UpdateView(
            RuntimeSentenceData sentenceData,
            Sprite defaultBackground,
            int currentIndex,
            int sentenceCount)
        {
            characterText.text = sentenceData.Text;

            // Персонаж
            if (sentenceData.CharacterIcon != null)
            {
                character.color = Color.white;
                character.sprite = sentenceData.CharacterIcon;
            }
            else
            {
                character.color = Color.clear;
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

            // Очистка старых изображений
            foreach (Transform child in imagesParent)
            {
                Destroy(child.gameObject);
            }

            // Отображение новых изображений
            foreach (var placement in sentenceData.Images)
            {
                ImagePlacementView imageView = Instantiate(imageViewPrefab, imagesParent);
                imageView.Initialize(placement.sprite, placement.position, placement.size, placement.rotation);
            }

            // Кнопка задания — вызывается отдельно
            // openTask добавляется в другом методе (UpdateTaskButton)
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
        }
        
        private void CompleteTask()
        {
            UnlockNextButton();
            TaskCompleted?.Invoke();
        }
    }
}
