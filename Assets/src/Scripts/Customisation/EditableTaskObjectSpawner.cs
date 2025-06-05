using System.Collections.Generic;
using UnityEngine;

namespace KiberOneLearningApp
{
	public class EditableTaskObjectSpawner : MonoBehaviour
	{
		[SerializeField] private RuntimeLessonEditorView runtimeLessonEditorView;
		[SerializeField] private ObjectForTask taskObjectPrefab;
		[SerializeField] private Transform taskObjectContainer;

		private readonly List<ObjectForTask> spawnedTaskObjects = new();

		public void RefreshTaskObjects()
		{
			// Удаляем старые объекты
			foreach (var taskObject in spawnedTaskObjects)
			{
				if (taskObject != null)
					Destroy(taskObject.gameObject);
			}
			spawnedTaskObjects.Clear();

			var sentence = runtimeLessonEditorView.GetCurrentSentence();
			if (sentence == null || sentence.InteractableImages == null) return;

			foreach (var placement in sentence.InteractableImages)
			{
				var newTaskObject = Instantiate(taskObjectPrefab, taskObjectContainer);
				newTaskObject.Initialize(placement);
				newTaskObject.SetKeyCode(placement.keyCode); // если ты добавил поле keyCode

				spawnedTaskObjects.Add(newTaskObject);
			}
		}
	}
}