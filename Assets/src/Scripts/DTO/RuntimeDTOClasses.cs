using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace KiberOneLearningApp
{
	public class RuntimeTutorialData
	{
		public string ThemeName;
		public int LessonNumber;
		public int TaskID;
		public string TutorialName;
		public Sprite DefaultBackground;
		public Sprite DefaultText;
		public List<RuntimeTutorialData> Tasks;
		public List<RuntimeSentenceData> Sentences;
	}

	public class RuntimeSentenceData
	{
		public Sprite Background;
		public List<RuntimeImagePlacement> Images;
		public List<RuntimeInteractablePlacement> InteractableImages;
		public bool isInOrder;
		public bool IsBeforeTask;
		public bool HideCharacter;
		public Sprite CharacterIcon;
		public Vector3 CharacterPosition;
		public string Text;
		public string TutorialVideoPath;
	}

	public class RuntimeImagePlacement
	{
		public Vector3 position;
		public Vector3 size;
		public Quaternion rotation;
		public Sprite sprite;
	}

	public class RuntimeInteractablePlacement : RuntimeImagePlacement
	{
		public RuntimeImagePlacement imagePlacement;
		public ColliderType colliderType;
		public Vector3 colliderPosition;
		public ColliderType colliderSize;
		public Quaternion rotation;
		
		public enum ColliderType
		{
			rectangle,
			circle
		}
	}
}