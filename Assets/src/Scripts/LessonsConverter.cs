using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace KiberOneLearningApp
{
	public static class TutorialConverter
	{
		public static TutorialDataDTO ToDTO(RuntimeTutorialData data)
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
		            CharacterIconPath = s.CharacterIconPath,
		            CharacterPosition = SerializableVector3.From(s.CharacterPosition),
		            CharacterSize = SerializableVector3.From(s.CharacterSize),
		            IsBeforeTask = s.IsBeforeTask,
		            HideCharacter = s.HideCharacter,
		            TutorialVideoPath = s.TutorialVideoPath,
		            Text = s.Text,

		            Images = s.Images?.Select(i => new ImagePlacementDTO
		            {
		                position = SerializableVector3.From(i.position),
		                size = SerializableVector3.From(i.size),
		                rotation = SerializableQuaternion.From(i.rotation),
		                spritePath = i.spritePath,
		                videoPath = i.videoPath
		            }).ToList(),

		            InteractableImages = s.InteractableImages?.Select(i => new InteractablePlacementDTO
		            {
		                colliderPosition = SerializableVector3.From(i.colliderPosition),
		                colliderSize = SerializableVector3.From(i.colliderSize),
		                colliderType = i.colliderType,
		                keyCode = i.keyCode.ToString(),
		                rotation = SerializableQuaternion.From(i.rotation),
		                imagePlacement = new ImagePlacementDTO
		                {
		                    position = SerializableVector3.From(i.imagePlacement.position),
		                    size = SerializableVector3.From(i.imagePlacement.size),
		                    rotation = SerializableQuaternion.From(i.imagePlacement.rotation),
		                    spritePath = SafeGetSpritePath(i.imagePlacement.sprite)
		                }
		            }).ToList()
		        }).ToList()
		    };
		}
		
		public static TutorialDataDTO ToDTO(TutorialData soData)
		{
		    return new TutorialDataDTO
		    {
		        ThemeName = soData.ThemeName,
		        LessonNumber = soData.LessonNumber,
		        TaskID = soData.TaskID,
		        TutorialName = soData.TutorialName,
		        DefaultBackgroundPath = SafeGetSpritePath(soData.DefaultBackground),
		        DefaultTextPath = SafeGetSpritePath(soData.DefaultText),

		        Tasks = soData.Tasks?.Select(ToDTO).ToList(),

		        Sentences = soData.Sentences?.Select(s => new SentenceDataDTO
		        {
		            BackgroundPath = SafeGetSpritePath(s.Background),
		            CharacterIconPath = SafeGetSpritePath(s.CharacterIcon),
		            TutorialVideoPath = s.TutorialVideo != null ? s.TutorialVideo.name + ".mp4" : "",
		            CharacterPosition = SerializableVector3.From(s.CharacterPosition),
		            IsBeforeTask = s.IsBeforeTask,
		            HideCharacter = s.HideCharacter,
		            Text = s.Text,

		            Images = s.Images?.Select(i => new ImagePlacementDTO
		            {
		                position = SerializableVector3.From(i.position),
		                size = SerializableVector3.From(i.size),
		                rotation = SerializableQuaternion.From(i.rotation),
		                spritePath = SafeGetSpritePath(i.sprite),
		                videoPath = i.videoPath
		            }).ToList(),

		            InteractableImages = s.InteractableImages?.Select(i => new InteractablePlacementDTO
		            {
		                colliderType = (ColliderType)i.colliderType,
		                colliderPosition = SerializableVector3.From(i.colliderPosition),
		                colliderSize = SerializableVector3.From(i.colliderSize),
		                rotation = SerializableQuaternion.From(i.rotation),
		                keyCode = i.keyCode.ToString(),
		                isInOrder = s.isInOrder,

		                imagePlacement = new ImagePlacementDTO
		                {
		                    position = SerializableVector3.From(i.imagePlacement.position),
		                    size = SerializableVector3.From(i.imagePlacement.size),
		                    rotation = SerializableQuaternion.From(i.imagePlacement.rotation),
		                    spritePath = SafeGetSpritePath(i.imagePlacement.sprite)
		                }
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