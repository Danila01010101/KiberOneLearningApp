using System;
using UnityEngine;
using UnityEngine.UI;

namespace KiberOneLearningApp
{
	[RequireComponent(typeof(Button))]
	public class ExitButton : MonoBehaviour
	{
    	private Button button;

	    private void Awake()
	    {
		    button = GetComponent<Button>();
		    button.onClick.AddListener(Exit);
	    }
	    
	    private void Exit() => Application.Quit();
	}
}