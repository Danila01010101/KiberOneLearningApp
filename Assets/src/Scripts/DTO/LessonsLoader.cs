using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace KiberOneLearningApp
{
	public class LessonsLoader
	{
		public static Dictionary<string, List<RuntimeTutorialData>> LessonsDictionary { get; private set; }

		public LessonsLoader()
		{
			LessonsDictionary = new Dictionary<string, List<RuntimeTutorialData>>();
			List<RuntimeTutorialData> datas = LoadAllLessons();

			LessonsDictionary = datas
				.GroupBy(l => string.IsNullOrEmpty(l.ThemeName) ? "Общие" : l.ThemeName)
				.OrderBy(g => g.Key)
				.ToDictionary(
					g => g.Key,
					g => g.OrderBy(l => l.LessonNumber).ToList()
				);
		}
		
		public static List<RuntimeTutorialData> LoadAllLessons()
		{
			var allLessons = new List<RuntimeTutorialData>();
			
			string userFolder = Path.Combine(Application.persistentDataPath, StaticStrings.LessonSavesFloulderName);
			if (Directory.Exists(userFolder))
			{
				foreach (var file in Directory.GetFiles(userFolder, "*.json"))
				{
					var dto = JsonIO.LoadFromJson<TutorialDataDTO>(file);
					if (dto != null)
						allLessons.Add(TutorialRuntimeBuilder.FromDTO(dto));
				}
			}

			string builtInFolder = Path.Combine(Application.streamingAssetsPath, StaticStrings.LessonSavesFloulderName);
			if (Directory.Exists(builtInFolder))
			{
				foreach (var file in Directory.GetFiles(builtInFolder, "*.json"))
				{
					var dto = JsonIO.LoadFromJson<TutorialDataDTO>(file);
					bool equals = allLessons.All(item => item.TutorialName == dto.TutorialName && item.TutorialName.SequenceEqual(dto.TutorialName));
					if (dto != null && equals)
						allLessons.Add(TutorialRuntimeBuilder.FromDTO(dto));
				}
			}

			return allLessons;
		}
	}
}