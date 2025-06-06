using System;
using TMPro;
using UnityEngine;

namespace KiberOneLearningApp
{
	public class RuntimeInteractablePlacementEditor : RuntimeSpriteEditor
	{
		[Header("Task-specific")]
		[SerializeField] private TMP_Dropdown keyCodeDropdown;
		[SerializeField] private TMP_Dropdown colliderTypeDropdown;

		private RuntimeInteractablePlacement taskPlacement;
		
		public static event Action<RuntimeInteractablePlacement> InteractableRemoved;

		public void InitAndResetSubscribesPlacement(RuntimeInteractablePlacement placement, Canvas canvas)
		{
			taskPlacement = placement;

			base.InitAndResetSubscribes(placement.imagePlacement, canvas);

			SetupKeyDropdown();
			SetupColliderDropdown();
		}

		public void SetKeyCode(KeyCode keyCode)
		{
			var index = keyCodeDropdown.options.FindIndex(o => o.text == keyCode.ToString());
			if (index >= 0)
			{
				keyCodeDropdown.value = index;
				taskPlacement.keyCode = keyCode;
			}
		}

		public void SetColliderType(ColliderType type)
		{
			if (colliderTypeDropdown == null)
				return;
			
			int index = (type == ColliderType.rectangle) ? 0 : 1;
			colliderTypeDropdown.value = index;
			taskPlacement.colliderType = type;
		}

		private void SetupKeyDropdown()
		{
			keyCodeDropdown.ClearOptions();
			var keys = System.Enum.GetNames(typeof(KeyCode));
			keyCodeDropdown.AddOptions(new System.Collections.Generic.List<string>(keys));

			int index = System.Array.IndexOf(keys, taskPlacement.keyCode.ToString());
			if (index >= 0)
				keyCodeDropdown.value = index;

			keyCodeDropdown.onValueChanged.AddListener(i =>
			{
				if (System.Enum.TryParse(keyCodeDropdown.options[i].text, out KeyCode key))
				{
					taskPlacement.keyCode = key;
					CallEditorChangedEvent();
				}
			});
		}

		private void SetupColliderDropdown()
		{
			if (colliderTypeDropdown == null)
				return;
			
			colliderTypeDropdown.ClearOptions();
			colliderTypeDropdown.AddOptions(new System.Collections.Generic.List<string> { "rectangle", "circle" });

			colliderTypeDropdown.value = taskPlacement.colliderType == ColliderType.rectangle ? 0 : 1;

			colliderTypeDropdown.onValueChanged.AddListener(i =>
			{
				taskPlacement.colliderType = (ColliderType)i;
				CallEditorChangedEvent();
			});
		}

		protected override void Delete() => InteractableRemoved?.Invoke(taskPlacement);
	}
}