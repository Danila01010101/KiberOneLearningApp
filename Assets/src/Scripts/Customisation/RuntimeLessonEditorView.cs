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
        [SerializeField] private Button changeCharacterButton;
        [SerializeField] private Button changeVideoButton;
        [SerializeField] private Button addSentenceButton;
        [SerializeField] private Button removeSentenceButton;
        [SerializeField] private Button addImageButton;
        
        [Header("Editor References")]
        [SerializeField] private RuntimeSpriteEditor characterEditor;
        [SerializeField] private RectTransform imageContainer;
        [SerializeField] private Canvas canvas;
        [SerializeField] private RuntimeSpriteEditor spriteEditorPrefab;
        [SerializeField] private TMP_InputField lessonNameInputField;

        private RuntimeLessonEditorManager lessonManager;
        private int currentSentenceIndex = 0;
        
        public static Action NewSentenceAdded;
        public static Action CurrentSentenceDeleted;
        public static Action<RuntimeImagePlacement> CurrentCharacterImageChanged;

        private void Start()
        {
            lessonManager = RuntimeLessonEditorManager.Instance;

            changeCharacterButton.onClick.AddListener(OnChangeCharacterIcon);
            changeVideoButton.onClick.AddListener(OnChangeTutorialVideo);
            addSentenceButton.onClick.AddListener(OnAddSentence);
            removeSentenceButton.onClick.AddListener(OnRemoveCurrentSentence);
            addImageButton.onClick.AddListener(OnAddImage);
            lessonNameInputField.onValueChanged.AddListener(OnLessonNameChange);
            lessonNameInputField.placeholder.GetComponent<TMP_Text>().text = lessonManager.CurrentLesson.TutorialName;
        }

        private RuntimeSentenceData GetCurrentSentence()
        {
            if (lessonManager.CurrentLesson == null || lessonManager.CurrentLesson.Sentences == null || lessonManager.CurrentLesson.Sentences.Count == 0)
                return null;

            currentSentenceIndex = Mathf.Clamp(currentSentenceIndex, 0, lessonManager.CurrentLesson.Sentences.Count - 1);
            return lessonManager.CurrentLesson.Sentences[currentSentenceIndex];
        }

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
                CurrentCharacterImageChanged?.Invoke(tempPlacement);
                Debug.Log("Иконка персонажа обновлена.");
            }
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
            currentSentenceIndex = lessonManager.CurrentLesson.Sentences.Count - 1;
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

            RuntimeSpriteEditor editor = Instantiate(spriteEditorPrefab, imageContainer);
            editor.Init(newPlacement, canvas);
        }

        private void OnLessonNameChange(string text)
        {
            lessonManager.CurrentLesson.TutorialName = text;
        }
    }
}