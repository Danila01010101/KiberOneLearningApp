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
        [SerializeField] private Button toggleButton;

        private RuntimeImagePlacement placement;
        private RectTransform rectTransform;
        private Canvas canvas;

        private Vector2 resizeStartMouse;
        private Vector2 resizeStartSize;

        private bool isInResizeMode;
        
        public event Action OnEditorChanged;

        private void Awake()
        {
            changeImageButton.onClick.AddListener(PickNewSprite);
            toggleButton.onClick.AddListener(ToggleResizeMode);
        }

        public virtual void InitAndResetSubscribes(RuntimeImagePlacement linkedPlacement, Canvas parentCanvas)
        {
            OnEditorChanged = null;
            placement = linkedPlacement;
            rectTransform = GetComponent<RectTransform>();
            canvas = parentCanvas;

            ApplyDataToUI();
        }

        private void ApplyDataToUI()
        {
            if (placement == null || imageRenderer == null || rectTransform == null) return;

            imageRenderer.sprite = placement.sprite;
            imageRenderer.color = placement.sprite != null ? Color.white : Color.clear;

            rectTransform.localPosition = placement.position;
            rectTransform.localRotation = placement.rotation;
            rectTransform.sizeDelta = new Vector2(placement.size.x, placement.size.y);
        }

        public void PickNewSprite()
        {
    #if UNITY_EDITOR || UNITY_STANDALONE
            if (placement == null) return;

            bool picked = RuntimeSpriteManager.PickAndAssignSprite(placement);
            if (picked)
            {
                ApplyDataToUI();
                CallEditorChangedEvent();
            }
    #endif
        }

        public void ToggleResizeMode()
        {
            isInResizeMode = !isInResizeMode;
            Debug.Log("Режим редактирования: " + (isInResizeMode ? "Изменение размера" : "Перемещение"));
            // Если есть UI-индикатор — обнови его здесь
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (isInResizeMode)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, eventData.pressEventCamera, out resizeStartMouse);
                resizeStartSize = rectTransform.sizeDelta;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (canvas == null || rectTransform == null || placement == null) return;

            if (isInResizeMode)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, eventData.pressEventCamera, out var localPoint);
                Vector2 delta = localPoint - resizeStartMouse;
                Vector2 newSize = resizeStartSize + new Vector2(delta.x, delta.x);
                newSize = Vector2.Max(newSize, new Vector2(10f, 10f)); // минимальный размер

                rectTransform.sizeDelta = newSize;
                placement.size = new Vector3(newSize.x, newSize.y, 0f);
            }
            else
            {
                Vector2 moveDelta = eventData.delta / canvas.scaleFactor;
                rectTransform.anchoredPosition += moveDelta;
                placement.position = rectTransform.localPosition;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (placement != null && rectTransform != null)
            {
                placement.position = rectTransform.localPosition;
                placement.size = new Vector3(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y, 0f);
                CallEditorChangedEvent();
            }
        }
        
        protected void CallEditorChangedEvent() => OnEditorChanged?.Invoke();

        private void OnDestroy()
        {
            changeImageButton.onClick.RemoveListener(PickNewSprite);
            toggleButton.onClick.RemoveListener(ToggleResizeMode);
        }
    }
}