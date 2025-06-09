using System;
using UnityEngine;
using UnityEngine.UI;

namespace KiberOneLearningApp
{
	public class LessonChooseButton : MonoBehaviour
	{
		[field : SerializeField] public Button ChooseButton { get; private set; }
		[field : SerializeField] public Button DeleteButton { get; private set; }

	    private void Awake()
	    {
		    DeleteButton.gameObject.SetActive(GlobalValueSetter.Instance.IsTeacher);
	    }
	}
}