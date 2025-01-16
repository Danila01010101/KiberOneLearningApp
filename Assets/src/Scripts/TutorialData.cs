using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace KiberOneLearningApp
{
    [CreateAssetMenu(menuName = "ScriptableObjects/New Tutorial Info", fileName = "Tutorial Info")]
    public class TutorialData : ScriptableObject
    {
        [field: SerializeField] public string TutorialName { get; private set; }
        [field: SerializeField] public Sprite DefaultBackground { get; private set; }
        [field: SerializeField] public List<SentenceData> Sentences { get; private set; }
        
        [Serializable]
        public class SentenceData
        {
            [field : SerializeField] public Sprite Background { get; private set; }
            [field : SerializeField] public Sprite CharacterIcon { get; private set; }
            [field: SerializeField] public Vector3 CharacterPosition { get; private set; } = new Vector3(353, -195, 0);
            [field : SerializeField] public string Text { get; private set; }

            [field : SerializeField] public VideoClip TutorialVideo { get; private set; }
        }
    }
}
