using KiberOneLearningApp;
using UnityEngine;

public class SentencesChanger : UIWindow
{
    [SerializeField] protected SentenceChangerView sentenceChangerView;
    [SerializeField] protected TutorialData tutorialData;
    
    private int currentIndex = -1;

    public int CurrentIndex => currentIndex;

    public override void Initialize()
    {
        sentenceChangerView.SubscribeButtons(ShowNextSentence, ShowPreviousSentence);
        if (tutorialData != null)
            ShowNextSentence();
    }

    protected virtual void ShowNextSentence()
    {
        if (currentIndex + 1 >= tutorialData.Sentences.Count)
        {
            return;
        }

        currentIndex++;
        sentenceChangerView.UpdateView(tutorialData.Sentences[currentIndex], tutorialData, currentIndex);
    }

    protected virtual void ShowPreviousSentence()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            sentenceChangerView.UpdateView(tutorialData.Sentences[currentIndex], tutorialData, currentIndex);
        }
    }

    protected virtual void OnDestroy() => sentenceChangerView.UnsubscribeButtons();
}