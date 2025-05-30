using System;
using UnityEngine;

namespace KiberOneLearningApp
{
	public class CharacterEditorPlacement : RuntimeImagePlacement
	{
		private readonly RuntimeSentenceData sentence;

		public CharacterEditorPlacement(RuntimeSentenceData sentence)
		{
			this.sentence = sentence;

			position = sentence.CharacterPosition;
			sprite = sentence.CharacterIcon;
			size = sentence.CharacterSize;
			rotation = Quaternion.identity;
		}

		public void ApplyChanges()
		{
			sentence.CharacterPosition = position;
			sentence.CharacterIcon = sprite;
			sentence.CharacterSize = size;
		}
	}
}