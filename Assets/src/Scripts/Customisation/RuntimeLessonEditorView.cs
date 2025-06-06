using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace KiberOneLearningApp
{
    public class RuntimeLessonEditorView : MonoBehaviour
    {
        [Header("UI Buttons")]
        [SerializeField] private Button changeVideoButton;
        [SerializeField] private Button addNewTaskButton;
        [SerializeField] private Button addSentenceButton;
        [SerializeField] private Button removeSentenceButton;
        [SerializeField] private Button addImageButton;
        [FormerlySerializedAs("addTaskButton")] [SerializeField] private Button addInteractableObjectButton;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button previousButton;
        
        [Header("Editor References")]
        [SerializeField] private RuntimeVisualElementsManager visualElementsManager;
        [SerializeField] private TaskWindowsCreator taskWindowsCreator;
        [SerializeField] private RuntimeSpriteEditor characterEditor;
        [SerializeField] private TMP_InputField newTaskName;
        [SerializeField] private TMP_InputField newTaskSentenceNumber;
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
            addNewTaskButton.onClick.AddListener(OnAddNewTask);
            addInteractableObjectButton.onClick.AddListener(OnAddInteractableObject);
            lessonNameInputField.placeholder.GetComponent<TMP_Text>().text = lessonManager.CurrentLesson.TutorialName;
            RuntimeSpriteEditor.ImageRemoved += RemoveImage;
            RuntimeInteractablePlacementEditor.InteractableRemoved += RemoveInteractable;
            RuntimeLessonEditorManager.Instance.SentenceChanged += DetectSentenceChange;
            InitializeCharacterIcon();
            visualElementsManager.RefreshVisuals(GetCurrentSentence());
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
            newTaskSentenceNumber.placeholder.GetComponent<TMP_Text>().text = (currentSentenceIndex+1).ToString();
        }

        public RuntimeSentenceData GetCurrentSentence()
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
            newTaskSentenceNumber.placeholder.GetComponent<TMP_Text>().text = (currentSentenceIndex+1).ToString();
            InitializeCharacterIcon();
        }

        public void SetNextSentenceIndex()
        {
            SentenceIndexChanged?.Invoke(++currentSentenceIndex);
            newTaskSentenceNumber.placeholder.GetComponent<TMP_Text>().text = (currentSentenceIndex+1).ToString();
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
            SentenceIndexChanged?.Invoke(++currentSentenceIndex);
            NewSentenceAdded?.Invoke();

            Debug.Log("Добавлено новое предложение.");
        }
        
        private void OnAddNewTask()
        {
            if (lessonManager.CurrentLesson == null)
            {
                Debug.LogWarning("Урок не инициализирован.");
                return;
            }

            // Создание нового задания
            RuntimeTutorialData newTask = new RuntimeTutorialData
            {
                TutorialName = $"Новое задание {lessonManager.CurrentLesson.Tasks?.Count + 1 ?? 1}",
                ThemeName = lessonManager.CurrentLesson.ThemeName,
                Sentences = new List<RuntimeSentenceData>
                {
                    new RuntimeSentenceData
                    {
                        Text = newTaskName.text,
                        CharacterPosition = new Vector3(353, -195, 0),
                        Images = new List<RuntimeImagePlacement>(),
                        InteractableImages = new List<RuntimeInteractablePlacement>(),
                        IsBeforeTask = false,
                        HideCharacter = false
                    }
                }
            };
            
            lessonManager.CurrentLesson.Sentences[currentSentenceIndex].IsBeforeTask = true;

            // Добавление в список заданий
            if (lessonManager.CurrentLesson.Tasks == null)
                lessonManager.CurrentLesson.Tasks = new List<RuntimeTutorialData>();

            lessonManager.CurrentLesson.Tasks.Add(newTask);
            taskWindowsCreator.SetRuntimeTasks(lessonManager.CurrentLesson.Tasks, GetComponent<LessonWindow>());
            taskWindowsCreator.OpenTaskWindow(currentSentenceIndex);

            Debug.Log("Новое задание добавлено.");
        }

        private void OnRemoveCurrentSentence()
        {
            if (lessonManager.CurrentLesson?.Sentences == null || lessonManager.CurrentLesson.Sentences.Count <= 1)
            {
                Debug.LogWarning("Нельзя удалить последнее предложение.");
                return;
            }

            lessonManager.CurrentLesson.Sentences.RemoveAt(currentSentenceIndex);
            currentSentenceIndex = Mathf.Clamp(currentSentenceIndex, 0, lessonManager.CurrentLesson.Sentences.Count - 1);
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

            visualElementsManager.RefreshVisuals(GetCurrentSentence());
        }
        
        public void RemoveImage(RuntimeImagePlacement image)
        {
            var sentence = GetCurrentSentence();
            if (sentence == null || sentence.Images == null) return;

            sentence.Images.Remove(image);
            visualElementsManager.RefreshVisuals(sentence);
        }
        
        private void OnAddInteractableObject()
        {
            var sentence = GetCurrentSentence();
            if (sentence == null) return;

            var newPlacement = new RuntimeInteractablePlacement
            {
                imagePlacement = new RuntimeImagePlacement
                {
                    position = Vector3.zero,
                    size = Vector3.one * 100f,
                    rotation = Quaternion.identity
                },
                colliderType = ColliderType.rectangle,
                colliderPosition = Vector3.zero,
                colliderSize = new Vector3(100f, 100f, 0f),
                rotation = Quaternion.identity,
                keyCode = KeyCode.Mouse0
            };

            sentence.InteractableImages ??= new List<RuntimeInteractablePlacement>();
            sentence.InteractableImages.Add(newPlacement);

            visualElementsManager.RefreshVisuals(GetCurrentSentence());
        }
        
        public void RemoveInteractable(RuntimeInteractablePlacement interactable)
        {
            var sentence = GetCurrentSentence();
            if (sentence == null || sentence.InteractableImages == null) return;

            sentence.InteractableImages.Remove(interactable);
            visualElementsManager.RefreshVisuals(sentence);
        }

        private void OnLessonNameChange(string text)
        {
            lessonManager.CurrentLesson.TutorialName = text;
        }

        private void DetectSentenceChange(RuntimeSentenceData i, Sprite j, int k, int z) => visualElementsManager.RefreshVisuals(GetCurrentSentence());
        
        private void OnDestroy()
        {
            RuntimeSpriteEditor.ImageRemoved -= RemoveImage;
            RuntimeInteractablePlacementEditor.InteractableRemoved -= RemoveInteractable;
            
            if (RuntimeLessonEditorManager.Instance != null)
                RuntimeLessonEditorManager.Instance.SentenceChanged -= DetectSentenceChange;
        }
    }
}