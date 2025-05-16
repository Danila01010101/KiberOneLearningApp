using System.IO;
using SFB;
using UnityEngine;
using UnityEngine.UI;

namespace KiberOneLearningApp
{
	public class LessonLoadingWindow : UIWindow
	{
		[SerializeField] private Button backButton;
		[SerializeField] private Button loadButton;
		[SerializeField] private Button selectFilesButton;
		[SerializeField] private TMPro.TMP_InputField inputField;
		
		public override void Initialize()
		{
			backButton.onClick.AddListener(BackToMenu);
			loadButton.onClick.AddListener(LoadTutorialFromInputField);
			selectFilesButton.onClick.AddListener(SelectTutorialFromPath);
		}

		private void BackToMenu() => UIWindowManager.ShowLast();
		
		private void LoadTutorialFromInputField() => LoadTutorialFromFile(inputField.text);
		private void LoadTutorialFromFile(string path)
		{
			var dto = JsonIO.LoadFromJson<TutorialDataDTO>(path);
			
			string fileName = Path.GetFileName(path);
			string targetPath = Path.Combine(Application.persistentDataPath, "UserLessons", fileName);

			File.Copy(path, targetPath, overwrite: true);
		}

		private void SelectTutorialFromPath()
		{
			var paths = StandaloneFileBrowser.OpenFilePanel("Выбери json файл", Application.persistentDataPath, "json", true);

			foreach (var path in paths)
			{
				LoadTutorialFromFile(path);
			}
		}
	}
}