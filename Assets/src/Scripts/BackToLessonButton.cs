using UnityEngine;

namespace KiberOneLearningApp
{
    public class BackToLessonButton : MonoBehaviour
    {
        public void BackToLesson() => UIWindowManager.ShowLast();
    }
}
