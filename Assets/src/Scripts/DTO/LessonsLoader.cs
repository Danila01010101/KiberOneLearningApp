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
			List<RuntimeTutorialData> datas = LoadAllCustomLessons();

			LessonsDictionary = datas
				.GroupBy(l => l.ThemeName)
				.OrderBy(g => g.Key)
				.ToDictionary(
					g => g.Key,
					g => g.OrderBy(l => l.LessonNumber).ToList()
				);
		}
		
		private List<RuntimeTutorialData> LoadAllCustomLessons()
		{
			string folder = Path.Combine(Application.persistentDataPath, "UserLessons");
			var result = new List<RuntimeTutorialData>();

			if (!Directory.Exists(folder)) return result;

			foreach (var file in Directory.GetFiles(folder, "*.json"))
			{ 
				var dto = JsonIO.LoadFromJson<TutorialDataDTO>(file);
				if (dto != null)
				{
					result.Add(TutorialRuntimeBuilder.FromDTO(dto));
				}
			}

			return result;
		}
	}
}