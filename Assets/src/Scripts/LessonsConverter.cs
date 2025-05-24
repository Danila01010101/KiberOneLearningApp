using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace KiberOneLearningApp
{
	public static class TutorialConverter
	{
		public static TutorialDataDTO ToDTO(TutorialData data)
		{
			return new TutorialDataDTO
			{
				ThemeName = data.ThemeName,
				TutorialName = data.TutorialName,
				DefaultBackgroundPath = SafeGetSpritePath(data.DefaultBackground),
				DefaultTextPath = SafeGetSpritePath(data.DefaultText),

				Tasks = data.Tasks?.Select(ToDTO).ToList(),

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
			string assetPath = AssetDatabase.GetAssetPath(sprite);
			if (string.IsNullOrEmpty(assetPath)) return "";

			string fileName = sprite.name + ".png";
			string exportPath = Path.Combine(Application.persistentDataPath, "UserImages");

			if (!Directory.Exists(exportPath))
				Directory.CreateDirectory(exportPath);

			string fullDest = Path.Combine(exportPath, fileName);

			if (!File.Exists(fullDest))
			{
				File.Copy(assetPath, fullDest); // копируем PNG в persistentDataPath
				Debug.Log($"Скопирован спрайт в билд: {fullDest}");
			}

			return $"UserImages/{fileName}";
#else
    // В билде — путь уже должен быть в JSON
    return $"UserImages/{sprite.name}.png";
#endif
		}
		/*
		 private static string SafeGetVideoPath(VideoClip clip)
			{
			    if (clip == null) return "";

			#if UNITY_EDITOR
			    string assetPath = AssetDatabase.GetAssetPath(clip);
			    if (string.IsNullOrEmpty(assetPath)) return "";

			    string fileName = clip.name + ".mp4";
			    string exportPath = Path.Combine(Application.persistentDataPath, "UserVideos");

			    if (!Directory.Exists(exportPath))
			        Directory.CreateDirectory(exportPath);

			    string fullDest = Path.Combine(exportPath, fileName);

			    if (!File.Exists(fullDest))
			    {
			        File.Copy(assetPath, fullDest);
			        Debug.Log($"Скопировано видео в билд: {fullDest}");
			    }

			    return $"UserVideos/{fileName}";
			#else
			    return $"UserVideos/{clip.name}.mp4";
			#endif
			}
		 */

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