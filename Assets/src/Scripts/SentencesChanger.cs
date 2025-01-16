using TMPro;
using UnityEngine;
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
        [SerializeField] private TextMeshProUGUI sentenceCounter;
        [Header("Sentence buttons")]
        [SerializeField] private Button nextButton;
        [SerializeField] private Button backButton;
        [Header("Gif properties")]
        [SerializeField] private VideoPlayer player;
        [SerializeField] private Transform videoWindow;
        [SerializeField] private Button openGifButton;
        [SerializeField] private Button closeGifButton;

        private GifOpener gifOpener;
        private int currentIndex = -1;

        private void Awake()
        {
            gifOpener = new GifOpener(player, videoWindow);
            ShowNextSentence();
        }

        private void ShowNextSentence() => ShowSentence(tutorialData.Sentences[++currentIndex]);
        
        private void ShowPreviousSentence() => ShowSentence(tutorialData.Sentences[--currentIndex]);

        private void ShowSentence(TutorialData.SentenceData sentenceData)
        {
            sentenceCounter.text = (currentIndex + 1) + "/" + tutorialData.Sentences.Count;
            character.sprite = sentenceData.CharacterIcon;
            character.transform.localPosition = sentenceData.CharacterPosition;
            characterText.text = sentenceData.Text;

            if (sentenceData.Background != null)
            {
                background.sprite = sentenceData.Background;
            }
            else
            {
                background.sprite = tutorialData.DefaultBackground;
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
