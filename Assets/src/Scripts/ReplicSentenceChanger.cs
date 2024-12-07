using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KiberOneLearningApp
{
    public class ReplicSentenceChanger : MonoBehaviour
    {
        [SerializeField] private Image character;
        [SerializeField] private Image background;
        [SerializeField] private TextMeshProUGUI characterText;
        [SerializeField] private TutorialData tutorialData;
        [SerializeField] private TextMeshProUGUI replicNumberText;

        private int currentIdex = 0;
        
        private void Start()
        {
            ChangeReplic();
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                ChangeReplic();
            }
        }

        private void ChangeReplic()
        {
            TutorialData.SentenceData currentSentance = tutorialData.Sentences[currentIdex++];
            replicNumberText.text = currentIdex + "/" + tutorialData.Sentences.Count + 1;
            character.sprite = currentSentance.CharacterIcon;
            background.sprite = currentSentance.Background;
            characterText.text = currentSentance.Text;
        }
    }
}
