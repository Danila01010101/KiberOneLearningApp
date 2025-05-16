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
				DefaultBackgroundPath = GetAssetPath(data.DefaultBackground),
				DefaultTextPath = GetAssetPath(data.DefaultText),
				Sentences = data.Sentences.Select(s => new SentenceDataDTO
				{
					BackgroundPath = GetAssetPath(s.Background),
					CharacterIconPath = GetAssetPath(s.CharacterIcon),
					TutorialVideoPath = GetAssetPath(s.TutorialVideo),
					CharacterPosition = SerializableVector3.From(s.CharacterPosition),
					IsBeforeTask = s.IsBeforeTask,
					HideCharacter = s.HideCharacter,
					TaskPrefabName = s.TaskForThisSentence?.name, // string reference
					Text = s.Text,
					Images = s.Images?.Select(i => new ImagePlacementDTO
					{
						position = SerializableVector3.From(i.position),
						size = SerializableVector3.From(i.size),
						rotation = SerializableQuaternion.From(i.rotation),
						spritePath = GetAssetPath(i.sprite)
					}).ToList()
				}).ToList()
			};
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