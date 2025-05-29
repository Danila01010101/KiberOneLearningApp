using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KiberOneLearningApp
{
	public class RuntimeSpriteEditor : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
	{
		[SerializeField] private Image imageRenderer;
		[SerializeField] private Button changeImageButton;
		private RuntimeImagePlacement placement;

		private RectTransform rectTransform;
		private Canvas canvas;

		public void Init(RuntimeImagePlacement linkedPlacement, Canvas parentCanvas)
		{
			placement = linkedPlacement;
			rectTransform = GetComponent<RectTransform>();
			canvas = parentCanvas;
			changeImageButton.onClick.AddListener(PickNewSprite);

			ApplyDataToUI();
		}

		private void ApplyDataToUI()
		{
			if (placement == null || imageRenderer == null) return;

			imageRenderer.sprite = placement.sprite;
			imageRenderer.color = placement.sprite != null ? Color.white : Color.clear;

			if (rectTransform != null)
			{
				rectTransform.localPosition = placement.position;
				rectTransform.localRotation = placement.rotation;
				rectTransform.sizeDelta = placement.size;
			}
		}

		public void PickNewSprite()
		{
			if (placement == null) return;

			bool picked = RuntimeSpriteManager.PickAndAssignSprite(placement);
			if (picked)
			{
				ApplyDataToUI();
			}
		}

		public void OnBeginDrag(PointerEventData eventData) { }

		public void OnDrag(PointerEventData eventData)
		{
			if (rectTransform == null || canvas == null) return;

			Vector2 moveDelta = eventData.delta / canvas.scaleFactor;
			rectTransform.anchoredPosition += moveDelta;

			placement.position = rectTransform.localPosition;
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			placement.position = rectTransform.localPosition;
		}

		private void OnDestroy()
		{
			changeImageButton.onClick.RemoveListener(PickNewSprite);
		}
	}
}