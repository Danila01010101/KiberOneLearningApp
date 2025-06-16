using TMPro;
using UnityEngine;

namespace KiberOneLearningApp
{
	public class RuntimeTextEditorView : MonoBehaviour
	{
		[SerializeField] private TMP_InputField inputField;

		private RuntimeLessonEditorView lessonEditorView;
		private int sentenceIndex = 0;

		public void Start()
		{
			lessonEditorView = GetComponent<RuntimeLessonEditorView>();
			sentenceIndex = 0;

			inputField.onValueChanged.AddListener(OnTextChanged);
			RefreshText();
		}

		public void SetPreviousSentenceIndex()
		{
			sentenceIndex--;
			RefreshText();
		}

		public void SetNextSentenceIndex()
		{
			sentenceIndex++;
			RefreshText();
		}

		public void RefreshText()
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

			lessonEditorView.currentData.Sentences[sentenceIndex].Text = newText;
		}

		private bool IsSentenceAvailable()
		{
			return lessonEditorView.currentData != null &&
			       lessonEditorView.currentData.Sentences != null &&
			       sentenceIndex >= 0 &&
			       sentenceIndex < lessonEditorView.currentData.Sentences.Count;
		}

		private RuntimeSentenceData GetSentence() => lessonEditorView.currentData.Sentences[sentenceIndex];
	}
}