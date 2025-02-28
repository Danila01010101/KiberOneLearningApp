using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Video;

namespace KiberOneLearningApp
{
    public class SentenceChangerView : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private Image character;
        [SerializeField] private Image background;
        [SerializeField] private TextMeshProUGUI characterText;
        [SerializeField] private Slider sentenceSlider;
        [Header("Sentence buttons")]
        [SerializeField] protected Button nextButton;
        [SerializeField] protected Button openCurrentTaskButton;
        
        [SerializeField] private Button backButton;
        [SerializeField] private Button openGifButton;
        [Header("Gif properties")]
        [SerializeField] private VideoPlayer player;
        [SerializeField] private Transform videoWindow;
    
        private GifOpener gifOpener;

        public void Initialize()
        {
            gifOpener = new GifOpener(player, videoWindow);
        }

        public void UpdateView(TutorialData.SentenceData sentenceData, TutorialData tutorialData, int currentIndex)
        {
            characterText.text = sentenceData.Text;
            
            if (sentenceData.CharacterIcon != null)
            {
                character.color = Color.white;
                character.sprite = sentenceData.CharacterIcon;
            }
            else
                character.color = Color.clear;
            
            character.transform.localPosition = sentenceData.CharacterPosition;
            backButton.gameObject.SetActive(currentIndex != 0);
            
            sentenceSlider.value = (currentIndex + 1) / (float)tutorialData.Sentences.Count;
            
            background.sprite = sentenceData.Background != null ? sentenceData.Background : tutorialData.DefaultBackground;

            if (sentenceData.TutorialVideo != null)
            {
                openGifButton.gameObject.SetActive(true);
                gifOpener.SetNewVideo(sentenceData.TutorialVideo);
            }
            else
            {
                openGifButton.gameObject.SetActive(false);
            }
            
            character.gameObject.SetActive(!tutorialData.Sentences[currentIndex].HideCharacter);
        }

        public virtual void UpdateTaskButton(bool isTaskSolved, TutorialData.SentenceData sentenceData,
            UnityAction openTask)
        {
            
        }

        public void BlockNextButton(Action UnlockAction)
        {
            nextButton.interactable = false;
            UnlockAction += UnlockNextButton;
        }

        private void UnlockNextButton()
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
    }
}
