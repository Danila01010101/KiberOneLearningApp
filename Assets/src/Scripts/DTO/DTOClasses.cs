using System;
using System.Collections.Generic;

namespace KiberOneLearningApp
{
		[Serializable]
		public class TutorialDataDTO
		{
			public string TutorialName;
			public string DefaultBackgroundPath;
			public string DefaultTextPath;
			public List<TutorialDataDTO> Tasks;
			public List<SentenceDataDTO> Sentences;
		}

		[Serializable]
		public class SentenceDataDTO
		{
			public string BackgroundPath;
			public List<ImagePlacementDTO> Images;
			public bool IsBeforeTask;
			public string TaskPrefabName;
			public bool HideCharacter;
			public string CharacterIconPath;
			public SerializableVector3 CharacterPosition;
			public string Text;
			public string TutorialVideoPath;
		}

		[Serializable]
		public class ImagePlacementDTO
		{
			public SerializableVector3 position;
			public SerializableVector3 size;
			public SerializableQuaternion rotation;
			public string spritePath;
		}
}