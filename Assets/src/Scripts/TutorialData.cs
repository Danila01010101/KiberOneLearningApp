using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace KiberOneLearningApp
{
    using UnityEngine;
    using UnityEngine.Video;
    using System;
    using System.Collections.Generic;
    using UnityEngine.Serialization;

    [CreateAssetMenu(menuName = "ScriptableObjects/New Tutorial Info", fileName = "Tutorial Info")]
    public class TutorialData : ScriptableObject
    {
        [field: SerializeField] public string ThemeName { get; private set; }
        [field: SerializeField] public int LessonNumber { get; private set; }
        [field: SerializeField] public int TaskID { get; private set; }

        [field: SerializeField] public string TutorialName { get; private set; }
        [field: SerializeField] public Sprite DefaultBackground { get; private set; }
        [field: SerializeField] public Sprite DefaultText { get; private set; }

        [field: SerializeField] public List<TutorialData> Tasks { get; private set; }

        [field: SerializeField, FormerlySerializedAs("Sentences")] // если структура изменилась
        public List<SentenceData> Sentences { get; private set; }

        [Serializable]
        public class SentenceData
        {
            [field: SerializeField] public Sprite Background { get; private set; }
            [field: SerializeField] public List<ImagePlacement> Images { get; private set; }

            [field: SerializeField] public List<InteractablePlacement> InteractableImages { get; private set; }

            [field: SerializeField] public bool IsBeforeTask { get; private set; }
            
            [field: SerializeField]
            public BasicTask DeprecatedTaskReference { get; private set; }

            [field: SerializeField] public bool HideCharacter { get; private set; }

            [field: SerializeField] public Sprite CharacterIcon { get; private set; }

            [field: SerializeField]
            public Vector3 CharacterPosition { get; private set; } = new Vector3(353, -195, 0);

            [field: SerializeField] public string Text { get; private set; }

            [field: SerializeField] public VideoClip TutorialVideo { get; private set; }

            [field: SerializeField] public bool isInOrder { get; private set; }
        }

        [Serializable]
        public class ImagePlacement
        {
            public Vector3 position;
            public Vector3 size = Vector3.one * 100f;
            public Quaternion rotation;
            public string videoPath;
            public Sprite sprite;
        }

        [Serializable]
        public class InteractablePlacement
        {
            public ImagePlacement imagePlacement;
            public ColliderType colliderType;
            public Vector3 colliderPosition;
            public Vector3 colliderSize;
            public Quaternion rotation;
            public KeyCode keyCode = KeyCode.Mouse0;
        }

        public enum ColliderType
        {
            rectangle,
            circle
        }
    }
}
