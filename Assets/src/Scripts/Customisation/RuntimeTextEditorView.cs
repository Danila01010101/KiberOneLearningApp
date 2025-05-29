using TMPro;
using UnityEngine;

namespace KiberOneLearningApp
{
	public class RuntimeTextEditorView : MonoBehaviour
	{
		[SerializeField] private TMP_InputField inputField;

		private RuntimeLessonEditorManager lessonManager;
		private int sentenceIndex = 0;

		public void Start()
		{
			lessonManager = RuntimeLessonEditorManager.Instance;
			sentenceIndex = 0;

			inputField.onValueChanged.AddListener(OnTextChanged);
			RefreshText();
		}

		public void SetPreviousSentenceIndex()
		{
			sentenceIndex++;
			RefreshText();
		}

		public void SetNextSentenceIndex()
		{
			sentenceIndex++;
			RefreshText();
		}

		private void RefreshText()
		{
			if (!IsSentenceAvailable())
			{
				inputField.text = "";
				inputField.placeholder.GetComponent<TMP_Text>().text = "Введите текст...";
				return;
			}

			var sentence = GetSentence();
			inputField.text = sentence.Text;
		}

		private void OnTextChanged(string newText)
		{
			if (!IsSentenceAvailable()) return;

			lessonManager.CurrentLesson.Sentences[sentenceIndex].Text = newText;
		}

		private bool IsSentenceAvailable()
		{
			return lessonManager != null &&
			       lessonManager.CurrentLesson != null &&
			       lessonManager.CurrentLesson.Sentences != null &&
			       sentenceIndex >= 0 &&
			       sentenceIndex < lessonManager.CurrentLesson.Sentences.Count;
		}

		private RuntimeSentenceData GetSentence() => lessonManager.CurrentLesson.Sentences[sentenceIndex];
	}
}