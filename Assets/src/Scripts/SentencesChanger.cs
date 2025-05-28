using KiberOneLearningApp;
using UnityEngine;
using Zenject;

public class SentencesChanger : UIWindow
{
    [SerializeField] protected SentenceChangerView sentenceChangerView;
    
    protected RuntimeTutorialData runtimeData;
    
    private int currentIndex = -1;

    public int CurrentIndex => currentIndex;

    public override void Initialize()
    {
        RuntimeLessonEditorView.NewSentenceAdded += ShowNextSentence;
        RuntimeLessonEditorView.CurrentSentenceDeleted += ShowPreviousSentence;
        sentenceChangerView.SubscribeButtons(ShowNextSentence, ShowPreviousSentence);
    }

    protected virtual void ShowNextSentence()
    {
        if (currentIndex + 1 >= runtimeData.Sentences.Count)
        {
            return;
        }

        currentIndex++;
        sentenceChangerView.UpdateView(runtimeData.Sentences[currentIndex], runtimeData.DefaultBackground, currentIndex, runtimeData.Sentences.Count);
    }

    protected virtual void ShowPreviousSentence()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            sentenceChangerView.UpdateView(runtimeData.Sentences[currentIndex], runtimeData.DefaultBackground, currentIndex, runtimeData.Sentences.Count);
        }
    }

    protected virtual void OnDestroy() => sentenceChangerView.UnsubscribeButtons();
}