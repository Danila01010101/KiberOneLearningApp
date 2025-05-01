using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace KiberOneLearningApp
{
	[RequireComponent(typeof(Button))]
	public class ExitToMenuButton : MonoBehaviour
	{
		private Button button;
		
		private void Awake()
		{
			button = GetComponent<Button>();
			button.onClick.AddListener(LoadMainMenu);
		}
		
		private void LoadMainMenu() => SceneManager.LoadScene("StartScene");
	}
}