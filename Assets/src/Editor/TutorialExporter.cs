using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace KiberOneLearningApp
{
	public static class TutorialExporter
	{
		private const string ExportFolder = "Assets/StreamingAssets/ExportedLessons";

		[MenuItem("Tools/Export All TutorialData to JSON")]
		public static void ExportAllTutorialsToJson()
		{
			if (!Directory.Exists(ExportFolder))
				Directory.CreateDirectory(ExportFolder);

			string[] guids = AssetDatabase.FindAssets("t:TutorialData");
			HashSet<string> usedFileNames = new();

			foreach (string guid in guids)
			{
				string assetPath = AssetDatabase.GUIDToAssetPath(guid);
				TutorialData tutorial = AssetDatabase.LoadAssetAtPath<TutorialData>(assetPath);
				if (tutorial == null) continue;

				TutorialDataDTO dto = TutorialConverter.ToDTO(tutorial);
				string json = JsonUtility.ToJson(dto, true);

				// Уникализируем имя
				string baseName = tutorial.name.Replace(" ", "_");
				string fileName = baseName;
				int counter = 1;

				while (usedFileNames.Contains(fileName.ToLower()) || File.Exists(Path.Combine(ExportFolder, fileName + ".json")))
				{
					fileName = $"{baseName}_{counter}";
					counter++;
				}

				usedFileNames.Add(fileName.ToLower());

				string jsonPath = Path.Combine(ExportFolder, fileName + ".json");
				File.WriteAllText(jsonPath, json);

				Debug.Log($"Экспортирован: {tutorial.name} → {jsonPath}");
			}

			AssetDatabase.Refresh();
		}
	}
}