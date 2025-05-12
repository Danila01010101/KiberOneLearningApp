using UnityEngine;
using UnityEngine.UI;

namespace KiberOneLearningApp
{
	public class ImagePlacementView : MonoBehaviour
	{
		[SerializeField] private Image image;

		public void Initialize(Sprite sprite, Vector3 position, Vector3 size, Quaternion rotation)
		{
			image.sprite = sprite;

			RectTransform rt = GetComponent<RectTransform>();
			rt.anchoredPosition3D = position;
			rt.sizeDelta = size;
			rt.localRotation = rotation;
		}
	}
}