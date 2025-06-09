using System.Collections.Generic;
using UnityEngine;

namespace KiberOneLearningApp
{
	public class NotEditableRuntimeViewManager : MonoBehaviour
	{
    	[Header("Prefabs")]
        [SerializeField] private ImagePlacementView imageViewPrefab;
        [SerializeField] private ObjectForTask objectForTaskPrefab;

        [Header("Containers")]
        [SerializeField] private Transform imageContainer;
        [SerializeField] private Transform taskObjectContainer;

        private readonly List<GameObject> spawnedImages = new();
        private readonly List<ObjectForTask> spawnedTasks = new();

        public void RefreshVisuals(RuntimeSentenceData sentenceData)
        {
            ClearAll();
            ShowImages(sentenceData);
            ShowTasks(sentenceData);
        }

        private void ShowImages(RuntimeSentenceData sentenceData)
        {
            if (sentenceData.Images == null) return;

            foreach (var placement in sentenceData.Images)
            {
                var view = Instantiate(imageViewPrefab, imageContainer);
                view.Initialize(
                    placement.sprite,
                    placement.position,
                    placement.size,
                    placement.rotation
                );
                spawnedImages.Add(view.gameObject);
            }
        }

        private void ShowTasks(RuntimeSentenceData sentenceData)
        {
            if (sentenceData.InteractableImages == null) return;

            ObjectForTask previous = null;

            foreach (var placement in sentenceData.InteractableImages)
            {
                var taskObject = Instantiate(objectForTaskPrefab, taskObjectContainer);
                taskObject.Initialize(placement);

                if (previous != null)
                    previous.OnCompleted += taskObject.Activate;

                spawnedTasks.Add(taskObject);
                previous = taskObject;
            }

            if (previous != null)
                previous.OnCompleted += () => Debug.Log("Все задания завершены");
        }

        public void ClearAll()
        {
            foreach (var go in spawnedImages)
                if (go != null) Destroy(go);
            spawnedImages.Clear();

            foreach (var obj in spawnedTasks)
                if (obj != null) Destroy(obj.gameObject);
            spawnedTasks.Clear();
        }
	}
}