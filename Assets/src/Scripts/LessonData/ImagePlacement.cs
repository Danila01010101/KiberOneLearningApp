using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace KiberOneLearningApp
{
	[Serializable]
	public class ImagePlacement
	{
		public Vector3 position;
		[FormerlySerializedAs("scale")] public Vector3 size = Vector3.one * 100f;
		public Quaternion rotation;
		public Sprite sprite;
	}
}