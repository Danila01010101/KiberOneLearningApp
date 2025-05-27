using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace KiberOneLearningApp
{
	public class RuntimeLessonEditorManager
	{
		public static RuntimeLessonEditorManager Instance { get; private set; }
		public RuntimeTutorialData CurrentLesson { get; private set; }

		public RuntimeLessonEditorManager()
		{
			if (Instance != null)
				Instance = this;
		}

		public List<string> GetAvailableLessonFiles()
		{
			string folder = Path.Combine(Application.persistentDataPath, "UserLessons");
			if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

			List<string> files = new();
			foreach (string path in Directory.GetFiles(folder, "*.json"))
			{
				files.Add(Path.GetFileName(path));
			}

			return files;
		}

		public void CreateNewLesson(string theme, string lessonName)
		{
			CurrentLesson = new RuntimeTutorialData
			{
				ThemeName = theme,
				TutorialName = lessonName,
				LessonNumber = Random.Range(100, 10000),
				Sentences = new List<RuntimeSentenceData>(),
				Tasks = new List<RuntimeTutorialData>()
			};
		}

		public bool SelectChangeLesson(string filename)
		{
			string path = Path.Combine(Application.persistentDataPath, StaticStrings.LessonSavesFloulderName, filename);
			if (!File.Exists(path)) return false;

			var dto = JsonIO.LoadFromJson<TutorialDataDTO>(path);
			if (dto == null) return false;

			CurrentLesson = TutorialRuntimeBuilder.FromDTO(dto);
			return true;
		}

		public void SaveCurrentLesson()
		{
			if (CurrentLesson == null) return;

			string folder = Path.Combine(Application.persistentDataPath, StaticStrings.LessonSavesFloulderName);
			if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

			string fileName = $"{CurrentLesson.TutorialName.Replace(" ", "_")}.json";
			string path = Path.Combine(folder, fileName);

			var dto = TutorialConverter.ToDTO(CurrentLesson);
			File.WriteAllText(path, JsonUtility.ToJson(dto, true));
		}
	}
}