using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace KiberOneLearningApp
{
    public class RuntimeLessonEditorView : MonoBehaviour
    {
        [Header("UI Buttons")]
        [SerializeField] private Button changeVideoButton;
        [SerializeField] private Button continueButton;
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
        [SerializeField] private RuntimeTextEditorView runtimeTextEditorView;
        [SerializeField] private EditorTaskWindowsCreator editTaskWindowsCreator;
        [SerializeField] private RuntimeSpriteEditor characterEditor;
        [SerializeField] private TMP_InputField newTaskName;
        [SerializeField] private TMP_InputField newTaskSentenceNumber;
        [SerializeField] private RectTransform imageContainer;
        [SerializeField] private Canvas canvas;
        [SerializeField] private RuntimeSpriteEditor spriteEditorPrefab;
        [SerializeField] private TMP_InputField lessonNameInputField;

        private RuntimeLessonEditorManager lessonManager;
        public RuntimeTutorialData currentData => isTaskWindow ? lessonManager.CurrentTask : lessonManager.CurrentLesson;
        private bool isTaskWindow;
        private int currentSentenceIndex = 0;
        private readonly List<RuntimeSpriteEditor> spawnedImageEditors = new();
        
        public static Action NewSentenceAdded;
        public static Action CurrentSentenceDeleted;
        public static Action<int, bool> SentenceIndexChanged;
        public static Action TaskCreated;

        private void Awake()
        {
            Initialize();
        }

        public void InitializeAsTask()
        {
            isTaskWindow = true;
            Initialize();
        }

        private void Initialize()
        {
            lessonManager = RuntimeLessonEditorManager.Instance;

            lessonNameInputField.onValueChanged.RemoveAllListeners();
            lessonNameInputField.onValueChanged.AddListener(OnLessonNameChange);
            InitializeButton(changeVideoButton, OnChangeTutorialVideo);
            InitializeButton(addSentenceButton, OnAddSentence);
            InitializeButton(removeSentenceButton, OnRemoveCurrentSentence);
            InitializeButton(addImageButton, OnAddImage);
            InitializeButton(nextButton, SetNextSentenceIndex);
            InitializeButton(previousButton, SetPreviousSentenceIndex);
            InitializeButton(addNewTaskButton, OnAddNewTask);
            InitializeButton(addInteractableObjectButton, OnAddInteractableObject);
            
            if (lessonManager.CurrentLesson.Tasks != null && lessonManager.CurrentLesson.Tasks.Count > 0)
                lessonNameInputField.placeholder.GetComponent<TMP_Text>().text = currentData.TutorialName;
            
            RuntimeSpriteEditor.ImageRemoved += RemoveImage;
            RuntimeInteractablePlacementEditor.InteractableRemoved += RemoveInteractable;
            RuntimeLessonEditorManager.Instance.SentenceChanged += DetectSentenceChange;
            InitializeCharacterIcon();
            visualElementsManager.RefreshVisuals(GetCurrentSentence());
        }

        private void InitializeButton(Button buttonToInitialize, UnityAction clickAction)
        {
            buttonToInitialize.onClick.RemoveAllListeners();
            buttonToInitialize.onClick.AddListener(clickAction);
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
            if (currentData == null || currentData.Sentences == null || currentData.Sentences.Count == 0)
                return null;

            currentSentenceIndex = Mathf.Clamp(currentSentenceIndex, 0, currentData.Sentences.Count - 1);
            return currentData.Sentences[currentSentenceIndex];
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
            if (currentSentenceIndex <= 0)
                return;
            
            SentenceIndexChanged?.Invoke(--currentSentenceIndex, isTaskWindow);
            newTaskSentenceNumber.placeholder.GetComponent<TMP_Text>().text = (currentSentenceIndex+1).ToString();
            newTaskSentenceNumber.text = (currentSentenceIndex+1).ToString();
            InitializeCharacterIcon();

            if (currentSentenceIndex == currentData.Sentences.Count - 2)
            {
                continueButton.onClick.RemoveAllListeners();
                InitializeButton(continueButton, SetNextSentenceIndex);
            }
        }

        public void SetNextSentenceIndex()
        {
            SentenceIndexChanged?.Invoke(++currentSentenceIndex, isTaskWindow);
            newTaskSentenceNumber.placeholder.GetComponent<TMP_Text>().text = (currentSentenceIndex+1).ToString();
            newTaskSentenceNumber.text = (currentSentenceIndex+1).ToString();
            InitializeCharacterIcon();

            if (currentSentenceIndex == currentData.Sentences.Count - 1)
            {
                if (isTaskWindow)
                {
                    continueButton.onClick.RemoveAllListeners();
                    continueButton.onClick.AddListener(delegate
                    {
                        UIWindowManager.ShowLast();
                    });
                }
                else
                {
                    continueButton.onClick.RemoveAllListeners();
                    continueButton.onClick.AddListener(delegate { SceneManager.LoadScene(StaticStrings.StartSceneName); });
                }
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
            if (currentData == null)
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

            
            currentData.Sentences.Insert(currentSentenceIndex + 1, newSentence);
            SetNextSentenceIndex();
            NewSentenceAdded?.Invoke();

            Debug.Log("Добавлено новое предложение.");
        }
        
        private void OnAddNewTask()
        {
            if (currentData == null)
            {
                Debug.LogWarning("Урок не инициализирован.");
                return;
            }

            // Создание нового задания
            RuntimeTutorialData newTask = new RuntimeTutorialData
            {
                TutorialName = newTaskName.text == "" ? "Новое задание" : newTaskName.text,
                ThemeName = currentData.ThemeName,
                Sentences = new List<RuntimeSentenceData>
                {
                    new RuntimeSentenceData
                    {
                        Text = "Текст первого предложения",
                        CharacterPosition = new Vector3(353, -195, 0),
                        Images = new List<RuntimeImagePlacement>(),
                        InteractableImages = new List<RuntimeInteractablePlacement>(),
                        IsBeforeTask = false,
                        HideCharacter = false
                    }
                }
            };

            // Добавление в список заданий
            if (currentData.Tasks == null)
                currentData.Tasks = new List<RuntimeTutorialData>();
            
            if (editTaskWindowsCreator == null)
            {
                //Зачем это вообще здесь если только при том что есть скрипт настраиваемого окна должно быть добавлено задание? Ну, работает - не трожь (:
                
                currentData.Tasks.Add(newTask);
                lessonManager.CurrentLesson.Sentences[int.Parse(newTaskSentenceNumber.text)].IsBeforeTask = true;
                taskWindowsCreator.SetRuntimeTasks(lessonManager.CurrentLesson.Tasks, GetComponent<LessonWindow>());
                taskWindowsCreator.OpenTaskWindow(currentSentenceIndex);
            }
            else
            {
                currentData.Tasks = InsertTaskInList(lessonManager.CurrentLesson, (newTask, int.Parse(newTaskSentenceNumber.text)));
                editTaskWindowsCreator.SetRuntimeLessons(lessonManager.CurrentLesson.Tasks);
                editTaskWindowsCreator.SpawnLessonEditors();
                editTaskWindowsCreator.OpenEditorWindow(currentSentenceIndex);
            }

            TaskCreated?.Invoke();
            Debug.Log("Новое задание добавлено.");
        }

        private void OnRemoveCurrentSentence()
        {
            if (currentData?.Sentences == null || currentData.Sentences.Count <= 1)
            {
                Debug.LogWarning("Нельзя удалить последнее предложение.");
                return;
            }

            currentData.Sentences.RemoveAt(currentSentenceIndex);
            runtimeTextEditorView.RefreshText();
            
            if (currentSentenceIndex >= currentData.Sentences.Count)
                currentSentenceIndex = currentData.Sentences.Count - 1;
            
            SentenceIndexChanged?.Invoke(currentSentenceIndex, isTaskWindow);
        }

        private List<RuntimeTutorialData> InsertTaskInList(RuntimeTutorialData currentSentence, (RuntimeTutorialData task, int taskSentenceIndex) newTaskVariable)
        {
            if (currentSentence.Tasks == null || currentSentence.Tasks.Count == 0)
                return new List<RuntimeTutorialData>() { newTaskVariable.task };
            
            List<RuntimeTutorialData> result = currentSentence.Tasks;
            List<RuntimeSentenceData> sentences = currentSentence.Sentences;
            List<(RuntimeTutorialData, int)> indexes = new List<(RuntimeTutorialData, int)>();
            int currentTaskIndex = 0;

            for (int i = 0; i < sentences.Count - 1; i++)
            {
                if (sentences[i].IsBeforeTask == false)
                    continue;
                
                indexes.Add((result[currentTaskIndex], i));
                currentTaskIndex += 1;
            }
            
            lessonManager.CurrentLesson.Sentences[int.Parse(newTaskSentenceNumber.text)].IsBeforeTask = true;

            for (int i = 0; i < indexes.Count - 1; i++)
            {
                if (i + 1 > indexes.Count || 
                    (indexes[i].Item2 > newTaskVariable.taskSentenceIndex)) 
                /* && indexes[i + 1].Item2 < newTaskVariable.taskSentenceIndex)*/
                {
                    result.Insert(i, newTaskVariable.task);
                }
            }
            
            return result;
        }

        private void OnAddImage()
        {
            var sentence = GetCurrentSentence();
            if (sentence == null) return;

            var newPlacement = new RuntimeImagePlacement
            {
                position = Vector3.zero,
                size = Vector3.one * 300f,
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
                    size = Vector3.one * 300f,
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
            currentData.TutorialName = text;
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