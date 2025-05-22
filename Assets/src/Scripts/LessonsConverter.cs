using System.Linq;
using UnityEngine;

namespace KiberOneLearningApp
{
	public static class TutorialConverter
	{
		public static TutorialDataDTO ToDTO(TutorialData data)
		{
			return new TutorialDataDTO
			{
				TutorialName = data.TutorialName,
				DefaultBackgroundPath = SafeGetSpritePath(data.DefaultBackground),
				DefaultTextPath = SafeGetSpritePath(data.DefaultText),

				Tasks = data.Tasks?.Select(ToDTO).ToList(), // Рекурсивный экспорт

				Sentences = data.Sentences.Select(s => new SentenceDataDTO
				{
					BackgroundPath = SafeGetSpritePath(s.Background),
					CharacterIconPath = SafeGetSpritePath(s.CharacterIcon),
					TutorialVideoPath = s.TutorialVideo != null ? s.TutorialVideo.name + ".mp4" : "",
					CharacterPosition = SerializableVector3.From(s.CharacterPosition),
					IsBeforeTask = s.IsBeforeTask,
					HideCharacter = s.HideCharacter,
					TaskPrefabName = s.TaskForThisSentence?.name ?? "",
					Text = s.Text,

					Images = s.Images?.Select(i => new ImagePlacementDTO
					{
						position = SerializableVector3.From(i.position),
						size = SerializableVector3.From(i.size),
						rotation = SerializableQuaternion.From(i.rotation),
						spritePath = SafeGetSpritePath(i.sprite)
					}).ToList()
				}).ToList()
			};
		}
		
		private static string SafeGetSpritePath(Sprite sprite)
		{
			if (sprite == null) return "";

#if UNITY_EDITOR
			return UnityEditor.AssetDatabase.GetAssetPath(sprite);
#else
			string expectedPath = Path.Combine(Application.persistentDataPath, "UserImages", sprite.name + ".png");
				return File.Exists(expectedPath)
				? $"UserImages/{sprite.name}.png"
				: "";
#endif
		}



		private static string GetAssetPath(UnityEngine.Object obj)
		{
			#if UNITY_EDITOR
				return obj ? UnityEditor.AssetDatabase.GetAssetPath(obj) : null;
			#else
				return null;
			#endif
		}
	}
}