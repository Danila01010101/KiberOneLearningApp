using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

namespace KiberOneLearningApp
{
    public class RuntimeLessonEditorView : MonoBehaviour
    {
        [Header("UI Buttons")]
        [SerializeField] private Button changeVideoButton;
        [SerializeField] private Button addSentenceButton;
        [SerializeField] private Button removeSentenceButton;
        [SerializeField] private Button addImageButton;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button previousButton;
        
        [Header("Editor References")]
        [SerializeField] private RuntimeSpriteEditor characterEditor;
        [SerializeField] private RectTransform imageContainer;
        [SerializeField] private Canvas canvas;
        [SerializeField] private RuntimeSpriteEditor spriteEditorPrefab;
        [SerializeField] private TMP_InputField lessonNameInputField;

        private RuntimeLessonEditorManager lessonManager;
        private int currentSentenceIndex = 0;
        private readonly List<RuntimeSpriteEditor> spawnedImageEditors = new();
        
        public static Action NewSentenceAdded;
        public static Action CurrentSentenceDeleted;
        public static Action<int> SentenceIndexChanged;

        private void Start()
        {
            lessonManager = RuntimeLessonEditorManager.Instance;

            changeVideoButton.onClick.AddListener(OnChangeTutorialVideo);
            addSentenceButton.onClick.AddListener(OnAddSentence);
            removeSentenceButton.onClick.AddListener(OnRemoveCurrentSentence);
            addImageButton.onClick.AddListener(OnAddImage);
            nextButton.onClick.AddListener(SetNextSentenceIndex);
            previousButton.onClick.AddListener(SetPreviousSentenceIndex);
            lessonNameInputField.onValueChanged.AddListener(OnLessonNameChange);
            lessonNameInputField.placeholder.GetComponent<TMP_Text>().text = lessonManager.CurrentLesson.TutorialName;
            RuntimeLessonEditorManager.Instance.SentenceChanged += DetectSentenceChange;
            InitializeCharacterIcon();
        }
        
        public void RefreshImageEditors()
        {
            foreach (var editor in spawnedImageEditors)
            {
                if (editor != null)
                    Destroy(editor.gameObject);
            }

            spawnedImageEditors.Clear();

            var sentence = GetCurrentSentence();
            if (sentence == null || sentence.Images == null) return;

            foreach (var image in sentence.Images)
            {
                var newEditor = Instantiate(spriteEditorPrefab, imageContainer);
                newEditor.InitAndResetSubscribes(image, canvas);
                spawnedImageEditors.Add(newEditor);
            }
        }

        private void InitializeCharacterIcon()
        {
            var sentence = GetCurrentSentence();
            
            if (sentence == null)
            {
                Debug.LogWarning("Нет доступного предложения.");
                return;
            }
            
            var characterPlacement = new CharacterEditorPlacement(sentence);
            characterEditor.InitAndResetSubscribes(characterPlacement, canvas);

            characterEditor.OnEditorChanged += characterPlacement.ApplyChanges;
        }

        private RuntimeSentenceData GetCurrentSentence()
        {
            if (lessonManager.CurrentLesson == null || lessonManager.CurrentLesson.Sentences == null || lessonManager.CurrentLesson.Sentences.Count == 0)
                return null;

            currentSentenceIndex = Mathf.Clamp(currentSentenceIndex, 0, lessonManager.CurrentLesson.Sentences.Count - 1);
            return lessonManager.CurrentLesson.Sentences[currentSentenceIndex];
        }

        /*
        private void OnChangeCharacterIcon()
        {
            var sentence = GetCurrentSentence();
            if (sentence == null)
            {
                Debug.LogWarning("Нет доступного предложения.");
                return;
            }

            RuntimeImagePlacement tempPlacement = new RuntimeImagePlacement();
            if (RuntimeSpriteManager.PickAndAssignSprite(tempPlacement))
            {
                sentence.CharacterIcon = tempPlacement.sprite;
                sentence.CharacterIcon.name = tempPlacement.sprite.name;
                characterEditor.Init(tempPlacement, canvas);
                Debug.Log("Иконка персонажа обновлена.");
            }
        }
        */
        

        public void SetPreviousSentenceIndex()
        {
            SentenceIndexChanged?.Invoke(--currentSentenceIndex);
            InitializeCharacterIcon();
        }

        public void SetNextSentenceIndex()
        {
            SentenceIndexChanged?.Invoke(++currentSentenceIndex);
            InitializeCharacterIcon();
        }

        private void OnChangeTutorialVideo()
        {
            var sentence = GetCurrentSentence();
            if (sentence == null)
            {
                Debug.LogWarning("Нет предложения.");
                return;
            }

            RuntimeImagePlacement temp = new RuntimeImagePlacement();
            if (RuntimeVideoManager.PickAndAssignVideo(temp))
            {
                sentence.TutorialVideoPath = temp.videoPath;
                Debug.Log($"Видео назначено: {temp.videoPath}");
            }
        }

        private void OnAddSentence()
        {
            if (lessonManager.CurrentLesson == null)
            {
                Debug.LogWarning("Урок не создан.");
                return;
            }

            RuntimeSentenceData newSentence = new RuntimeSentenceData
            {
                Text = "Новое предложение",
                CharacterPosition = new Vector3(353, -195, 0),
                Images = new List<RuntimeImagePlacement>(),
                InteractableImages = new List<RuntimeInteractablePlacement>(),
                IsBeforeTask = false,
                HideCharacter = false
            };

            lessonManager.CurrentLesson.Sentences.Add(newSentence);
            SentenceIndexChanged?.Invoke(--currentSentenceIndex);
            NewSentenceAdded?.Invoke();

            Debug.Log("Добавлено новое предложение.");
        }

        private void OnRemoveCurrentSentence()
        {
            if (lessonManager.CurrentLesson?.Sentences == null || lessonManager.CurrentLesson.Sentences.Count <= 1)
            {
                Debug.LogWarning("Нельзя удалить последнее предложение.");
                return;
            }

            lessonManager.CurrentLesson.Sentences.RemoveAt(currentSentenceIndex);
            currentSentenceIndex = Mathf.Clamp(currentSentenceIndex - 1, 0, lessonManager.CurrentLesson.Sentences.Count - 1);
            CurrentSentenceDeleted?.Invoke();

            Debug.Log("Предложение удалено.");
        }

        private void OnAddImage()
        {
            var sentence = GetCurrentSentence();
            if (sentence == null) return;

            var newPlacement = new RuntimeImagePlacement
            {
                position = Vector3.zero,
                size = Vector3.one * 100f,
                rotation = Quaternion.identity
            };

            sentence.Images ??= new List<RuntimeImagePlacement>();
            sentence.Images.Add(newPlacement);

            RefreshImageEditors(); // отрисовать все заново
        }

        private void OnLessonNameChange(string text)
        {
            lessonManager.CurrentLesson.TutorialName = text;
        }

        private void DetectSentenceChange(RuntimeSentenceData i, Sprite j, int k, int z) => RefreshImageEditors();
        
        private void OnDestroy()
        {
            if (RuntimeLessonEditorManager.Instance != null)
                RuntimeLessonEditorManager.Instance.SentenceChanged -= DetectSentenceChange;
        }
    }
}