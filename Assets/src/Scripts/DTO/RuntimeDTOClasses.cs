using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace KiberOneLearningApp
{
	public class RuntimeTutorialData
	{
		public string ThemeName;
		public int LessonNumber;
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
		public bool IsBeforeTask;
		public GameObject TaskPrefab;
		public bool HideCharacter;
		public Sprite CharacterIcon;
		public Vector3 CharacterPosition;
		public string Text;
	}

	public class RuntimeImagePlacement
	{
		public Vector3 position;
		public Vector3 size;
		public Quaternion rotation;
		public Sprite sprite;
	}
}