using System;
using TMPro;
using UnityEngine;

namespace KiberOneLearningApp
{
    public class ReplicSentenceChanger : MonoBehaviour
    {
        [SerializeField] private Image character;
        [SerializeField] private Image background;
        [SerializeField] private TextMeshProUGUI characterText;
        [SerializeField] private TutorialData tutorialData;

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
            character.image = currentSentance.CharacterIcon;
            background.image = currentSentance.Background;
            characterText.text = currentSentance.Text;
        }
    }
}
