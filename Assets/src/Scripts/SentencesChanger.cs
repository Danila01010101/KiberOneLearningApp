using KiberOneLearningApp;
using UnityEngine;

public class SentencesChanger : UIWindow
{
    [SerializeField] private SentenceChangerView sentenceChangerView;
    [SerializeField] private TutorialData tutorialData;
    
    private int currentIndex = -1;

    public override void Initialize()
    {
        sentenceChangerView.SubscribeButtons(ShowNextSentence, ShowPreviousSentence);
        if (tutorialData != null)
            ShowNextSentence();
    }

    private void ShowNextSentence()
    {
        if (currentIndex + 1 >= tutorialData.Sentences.Count)
        {
            return;
        }

        currentIndex++;
        sentenceChangerView.UpdateView(tutorialData.Sentences[currentIndex], tutorialData, currentIndex);
    }

    private void ShowPreviousSentence()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            sentenceChangerView.UpdateView(tutorialData.Sentences[currentIndex], tutorialData, currentIndex);
        }
    }

    private void OnDestroy() => sentenceChangerView.UnsubscribeButtons();
}