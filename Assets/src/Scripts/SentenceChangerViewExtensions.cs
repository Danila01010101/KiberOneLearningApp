using UnityEngine;

namespace KiberOneLearningApp
{
	public static class SentenceChangerViewExtensions
	{
		public static void UpdateView(this SentenceChangerView view, TutorialData.SentenceData sentenceData, TutorialData tutorialData, int currentIndex)
		{
			view.UpdateView(sentenceData, currentIndex);
            
			view.SentenceSlider.value = (currentIndex + 1) / (float)tutorialData.Sentences.Count;
			Debug.Log(view.SentenceSlider.value);
            
			view.Background.sprite = sentenceData.Background != null ? sentenceData.Background : tutorialData.DefaultBackground;
            
			if (sentenceData.TutorialVideo != null)
			{
				view.OpenGifButton.gameObject.SetActive(true);
				view.GifOpener.SetNewVideo(sentenceData.TutorialVideo);
			}
			else
			{
				view.OpenGifButton.gameObject.SetActive(false);
			}
            
			view.Character.gameObject.SetActive(!tutorialData.Sentences[currentIndex].HideCharacter);
		}
	}
}