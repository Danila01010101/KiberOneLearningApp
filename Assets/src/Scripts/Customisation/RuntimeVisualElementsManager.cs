using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace KiberOneLearningApp
{
	public class RuntimeVisualElementsManager : MonoBehaviour
	{
    	[Header("Prefabs")]
        [SerializeField] private RuntimeSpriteEditor spriteEditorPrefab;
        [FormerlySerializedAs("taskObjectPrefab")] [SerializeField] private RuntimeInteractablePlacementEditor taskPlacementPrefab;

        [Header("Containers")]
        [SerializeField] private Transform imageContainer;
        [SerializeField] private Transform taskObjectContainer;
        [SerializeField] private Canvas canvas;

        private readonly List<RuntimeSpriteEditor> spawnedImageEditors = new();
        private readonly List<RuntimeInteractablePlacementEditor> spawnedTaskObjects = new();

        /// <summary>
        /// Обновляет все визуальные элементы текущего предложения
        /// </summary>
        public void RefreshVisuals(RuntimeSentenceData sentence)
        {
            RefreshImageEditors(sentence);
            RefreshTaskObjects(sentence);
        }

        private void RefreshImageEditors(RuntimeSentenceData sentence)
        {
            foreach (var editor in spawnedImageEditors)
            {
                if (editor != null)
                    Destroy(editor.gameObject);
            }
            spawnedImageEditors.Clear();

            if (sentence?.Images == null) return;

            foreach (var image in sentence.Images)
            {
                var editor = Instantiate(spriteEditorPrefab, imageContainer);
                editor.InitAndResetSubscribes(image, canvas);
                spawnedImageEditors.Add(editor);
            }
        }

        private void RefreshTaskObjects(RuntimeSentenceData sentence)
        {
            foreach (var obj in spawnedTaskObjects)
            {
                if (obj != null)
                    Destroy(obj.gameObject);
            }
            spawnedTaskObjects.Clear();

            if (sentence?.InteractableImages == null) return;

            foreach (var placement in sentence.InteractableImages)
            {
                RuntimeInteractablePlacementEditor taskObject = Instantiate(taskPlacementPrefab, taskObjectContainer);
                taskObject.InitAndResetSubscribesPlacement(placement, canvas);
                taskObject.SetKeyCode(placement.keyCode);
                taskObject.SetColliderType(placement.colliderType);
                spawnedTaskObjects.Add(taskObject);
            }
        }

        public void ClearAll()
        {
            foreach (var editor in spawnedImageEditors)
                if (editor != null) Destroy(editor.gameObject);
            spawnedImageEditors.Clear();

            foreach (var obj in spawnedTaskObjects)
                if (obj != null) Destroy(obj.gameObject);
            spawnedTaskObjects.Clear();
        }
	}
}