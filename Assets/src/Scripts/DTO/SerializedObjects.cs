using System;
using UnityEngine;

namespace KiberOneLearningApp
{
	[Serializable]
	public struct SerializableVector3
	{
		public float x, y, z;
		public static SerializableVector3 From(Vector3 v) => new SerializableVector3 { x = v.x, y = v.y, z = v.z };
		public Vector3 ToVector3() => new Vector3(x, y, z);
	}

	[Serializable]
	public struct SerializableQuaternion
	{
		public float x, y, z, w;
		public static SerializableQuaternion From(Quaternion q) => new SerializableQuaternion { x = q.x, y = q.y, z = q.z, w = q.w };
		public Quaternion ToQuaternion() => new Quaternion(x, y, z, w);
	}
}