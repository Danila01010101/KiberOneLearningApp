using System.IO;
using UnityEngine;

namespace KiberOneLearningApp
{
	public static class JsonIO
	{
		public static void SaveToJson<T>(T obj, string filePath)
		{
			var json = JsonUtility.ToJson(obj, true);
			File.WriteAllText(filePath, json);
		}

		public static T LoadFromJson<T>(string filePath)
		{
			if (!File.Exists(filePath)) return default;
			var json = File.ReadAllText(filePath);
			return JsonUtility.FromJson<T>(json);
		}
	}
}