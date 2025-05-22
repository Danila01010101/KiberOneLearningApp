using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
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
				.GroupBy(l => l.ThemeName)
				.OrderBy(g => g.Key)
				.ToDictionary(
					g => g.Key,
					g => g.OrderBy(l => l.LessonNumber).ToList()
				);
		}
		
		public static List<RuntimeTutorialData> LoadAllLessons()
		{
			var allLessons = new List<RuntimeTutorialData>();

			// 1. Загрузка встроенных (заготовленных)
			string builtInFolder = Path.Combine(Application.streamingAssetsPath, "ExportedLessons");
			if (Directory.Exists(builtInFolder))
			{
				foreach (var file in Directory.GetFiles(builtInFolder, "*.json"))
				{
					var dto = JsonIO.LoadFromJson<TutorialDataDTO>(file);
					if (dto != null)
						allLessons.Add(TutorialRuntimeBuilder.FromDTO(dto));
				}
			}

			string userFolder = Path.Combine(Application.persistentDataPath, "UserLessons");
			if (Directory.Exists(userFolder))
			{
				foreach (var file in Directory.GetFiles(userFolder, "*.json"))
				{
					var dto = JsonIO.LoadFromJson<TutorialDataDTO>(file);
					if (dto != null)
						allLessons.Add(TutorialRuntimeBuilder.FromDTO(dto));
				}
			}

			return allLessons;
		}
	}
}