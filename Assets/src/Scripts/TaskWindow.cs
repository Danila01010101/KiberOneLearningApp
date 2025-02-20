namespace KiberOneLearningApp
{
	public class TaskWindow : SentencesChanger
	{
		public void SetNewData(TutorialData newSentenceData)
		{
			tutorialData = newSentenceData;
		}

		protected override void ShowNextSentence()
		{
			base.ShowNextSentence();
			//tutorialData.Sentences[CurrentIndex].TaskForThisSentence;
		}

		protected override void ShowPreviousSentence()
		{
			base.ShowPreviousSentence();
		}
		
		/*
		public void ChangeSentence(TutorialData sentence)
		{
			tutorialData = sentence;

			if (tutorialData != null)
			{
				ShowNextSentence();
			}
			else
			{
				throw new Exception("No data setted before activation!");
			}
		}
		*/
	}
}