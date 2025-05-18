using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KiberOneLearningApp
{
    public class ObjectForTask : MonoBehaviour
    {
        private KeyCode keyCode = KeyCode.Mouse0;
        private bool isActive;

        public Action OnCompleted;

        public void SetKeyCode(KeyCode keyCode) => this.keyCode = keyCode;

        public void Activate() => isActive = true;

        public void Initialize(RuntimeInteractablePlacement placement)
        {
            // Общая позиция, размер, поворот
            transform.localPosition = placement.position;
            transform.localRotation = placement.rotation;
            transform.localScale = Vector3.one; // масштаб определён через size

            // Добавим SpriteRenderer, если его нет
            var spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
                spriteRenderer = gameObject.AddComponent<SpriteRenderer>();

            spriteRenderer.sprite = placement.sprite;
            spriteRenderer.drawMode = SpriteDrawMode.Sliced;
            spriteRenderer.size = placement.size;

            // Удаляем существующие коллайдеры
            var existingBox = GetComponent<BoxCollider2D>();
            if (existingBox) Destroy(existingBox);

            var existingCircle = GetComponent<CircleCollider2D>();
            if (existingCircle) Destroy(existingCircle);

            // Добавляем нужный коллайдер
            if (placement.colliderType == RuntimeInteractablePlacement.ColliderType.rectangle)
            {
                var box = gameObject.AddComponent<BoxCollider2D>();
                box.size = placement.size;
                box.offset = placement.colliderPosition;
            }
            else if (placement.colliderType == RuntimeInteractablePlacement.ColliderType.circle)
            {
                var circle = gameObject.AddComponent<CircleCollider2D>();
                // Радиус — берём максимальный размер по X или Y
                circle.radius = Mathf.Max(placement.size.x, placement.size.y) / 2f;
                circle.offset = placement.colliderPosition;
            }
        }

        private void Update()
        {
            if (!isActive || !Input.GetKey(keyCode))
                return;

            PointerEventData pointer = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointer, raycastResults);

            foreach (var hit in raycastResults)
            {
                if (hit.gameObject == gameObject)
                {
                    gameObject.SetActive(false);
                    isActive = false;
                    OnCompleted?.Invoke();
                    break;
                }
            }
        }
    }
}
