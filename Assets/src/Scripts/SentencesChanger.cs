using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

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
        [Header("UI Elements")]
        [SerializeField] private Button nextButton;
        [SerializeField] private Button backButton;

        private int currentIndex = -1;

        private void Start()
        {
            ShowNextSentence();
        }

        private void ShowNextSentence() => ShowSentence(tutorialData.Sentences[++currentIndex]);
        
        private void ShowPreviousSentence() => ShowSentence(tutorialData.Sentences[--currentIndex]);

        private void ShowSentence(TutorialData.SentenceData sentenceData)
        {
            sentenceCounter.text = (currentIndex + 1) + "/" + tutorialData.Sentences.Count;
            character.sprite = sentenceData.CharacterIcon;
            background.sprite = sentenceData.Background;
            characterText.text = sentenceData.Text;
        }
        
        private void OnEnable()
        {
            nextButton.onClick.AddListener(ShowNextSentence);
            backButton.onClick.AddListener(ShowPreviousSentence);
        }
        
        private void OnDisable()
        {
            nextButton.onClick.RemoveListener(ShowNextSentence);
            backButton.onClick.RemoveListener(ShowPreviousSentence);
        }
    }
}
