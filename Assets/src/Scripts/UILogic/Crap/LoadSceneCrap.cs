using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KiberOneLearningApp
{
    public class LoadSceneCrap : MonoBehaviour
    {
        [SerializeField] private string ExelMainMenuSceneName;
        [SerializeField] private string ExelFirstLevelSceneName;

        public void GoToExelMainScene()
        {
            SceneManager.LoadScene(ExelMainMenuSceneName);
        }

        public void GoToExelFirstLevelScene()
        {
            SceneManager.LoadScene(ExelFirstLevelSceneName);
        }
    }
}
