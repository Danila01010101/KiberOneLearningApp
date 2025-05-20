using System;
using System.Collections.Generic;

namespace KiberOneLearningApp
{
		[Serializable]
		public class TutorialDataDTO
		{
			public string ThemeName;
			public int LessonNumber;
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
			public List<InteractablePlacementDTO> InteractableImages;
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

		[Serializable]
		public class InteractablePlacementDTO
		{
			public ImagePlacementDTO imagePlacement;
			public ColliderType colliderType;
			public SerializableVector3 colliderPosition;
			public SerializableVector3 colliderSize;
			public SerializableQuaternion rotation;
		}
		
		[Serializable]
		public enum ColliderType
		{
			rectangle,
			circle
		}
}