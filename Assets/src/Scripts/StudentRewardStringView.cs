using TMPro;
using UnityEngine;

namespace KiberOneLearningApp
{
    public class StudentRewardStringView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI studentNameString;
        [SerializeField] private TMP_InputField rewardInputString;
        
        public int GetReward => int.Parse(rewardInputString.text);
        
        public void Initialize(string studentName) => studentNameString.text = studentName;
    }
}
