using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace KiberOneLearningApp
{
	public class RuntimeLessonEditorManager
	{
		public static RuntimeLessonEditorManager Instance { get; private set; }
		public RuntimeTutorialData CurrentLesson { get; private set; }
		public RuntimeTutorialData CurrentTask { get; set; }

        public int CurrentSentenceIndex { get; private set; } = 0;

        public event Action<RuntimeSentenceData, Sprite, int, int> SentenceChanged;
        
        private static bool isTaskSelected = false;

        public RuntimeLessonEditorManager()
        {
            if (Instance != null)
                return;
            
            Instance = this;
            RuntimeLessonEditorView.SentenceIndexChanged += DetectIndexChange;
        }

        public List<string> GetAvailableLessonFiles()
        {
            List<string> files = new();
            
            string persistentDataPathFolder = Path.Combine(Application.persistentDataPath, StaticStrings.LessonSavesFloulderName);
            files.AddRange(GetFilesInFolder(persistentDataPathFolder));  
            
            string streamingAssetsPathFolder = Path.Combine(Application.streamingAssetsPath, StaticStrings.LessonSavesFloulderName);
            files.AddRange(GetFilesInFolder(streamingAssetsPathFolder));  
            
            return files;
        }
        
        private void DetectIndexChange(int newIndex, bool isTask)
        {
            CurrentSentenceIndex = newIndex;
            isTaskSelected = isTask;
            Debug.Log(newIndex);
            TriggerSentenceChanged();
        }

        private List<string> GetFilesInFolder(string folder)
        {
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            List<string> files = new();
            foreach (string path in Directory.GetFiles(folder, "*.json"))
            {
                files.Add(Path.GetFileName(path));
            }
            
            return files;
        }
        
        public bool SelectChangeLesson(string filename)
        {
            string userFolder = Path.Combine(Application.persistentDataPath, StaticStrings.LessonSavesFloulderName);
            string streamingFolder = Path.Combine(Application.streamingAssetsPath, StaticStrings.LessonSavesFloulderName);

            string userPath = Path.Combine(userFolder, filename);
            string streamingPath = Path.Combine(streamingFolder, filename);

            string targetPath = null;

            if (File.Exists(userPath))
            {
                targetPath = userPath;
            }
            else if (File.Exists(streamingPath))
            {
                targetPath = streamingPath;
            }
            else
            {
                Debug.LogWarning($"Файл не найден ни в пользовательской, ни в заготовленной папке: {filename}");
                return false;
            }

            var dto = JsonIO.LoadFromJson<TutorialDataDTO>(targetPath);
            if (dto == null)
            {
                Debug.LogError($"Ошибка загрузки DTO из {targetPath}");
                return false;
            }

            CurrentLesson = TutorialRuntimeBuilder.FromDTO(dto);
            CurrentTask = CurrentLesson.Tasks[0];
            CurrentSentenceIndex = 0;
            TriggerSentenceChanged();

            return true;
        }
        
        public void CreateNewLesson(string theme, string lessonName)
        {
            CurrentLesson = new RuntimeTutorialData
            {
                ThemeName = theme,
                TutorialName = lessonName,
                LessonNumber = 0,
                Sentences = new List<RuntimeSentenceData>(),
                Tasks = new List<RuntimeTutorialData>()
            };

            CurrentLesson.Sentences.Add(new RuntimeSentenceData());
            CurrentSentenceIndex = 0;
            TriggerSentenceChanged();
        }
        
        public static bool DeleteLesson(RuntimeTutorialData lessonToDelete)
        {
            if (lessonToDelete == null || string.IsNullOrEmpty(lessonToDelete.TutorialName))
            {
                Debug.LogWarning("Удаление урока: данные урока не заданы.");
                return false;
            }

            string userFolder = Path.Combine(Application.persistentDataPath, StaticStrings.LessonSavesFloulderName);
            if (!Directory.Exists(userFolder))
            {
                Debug.LogWarning("Папка с уроками не существует.");
                return false;
            }

            foreach (var file in Directory.GetFiles(userFolder, "*.json"))
            {
                var dto = JsonIO.LoadFromJson<TutorialDataDTO>(file);
                if (dto != null && dto.TutorialName == lessonToDelete.TutorialName)
                {
                    try
                    {
                        File.Delete(file);
                        Debug.Log($"Урок \"{dto.TutorialName}\" удалён. Путь: {file}");
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Ошибка при удалении урока: {ex.Message}");
                        return false;
                    }
                }
            }

            Debug.LogWarning($"Файл урока \"{lessonToDelete.TutorialName}\" не найден для удаления или является предустановленным.");
            return false;
        }

        public bool LoadLesson(string filename)
        {
            string path = Path.Combine(Application.persistentDataPath, StaticStrings.LessonSavesFloulderName, filename);
            if (!File.Exists(path)) return false;

            var dto = JsonIO.LoadFromJson<TutorialDataDTO>(path);
            if (dto == null) return false;

            CurrentLesson = TutorialRuntimeBuilder.FromDTO(dto);
            CurrentSentenceIndex = 0;
            TriggerSentenceChanged();

            return true;
        }

        public void SaveCurrentLesson()
        {
            if (CurrentLesson == null) return;

            string folder = Path.Combine(Application.persistentDataPath, StaticStrings.LessonSavesFloulderName);
            Directory.CreateDirectory(folder);

            string fileName = $"{CurrentLesson.TutorialName.Replace(" ", "_")}.json";
            string path = Path.Combine(folder, fileName);

            var dto = TutorialConverter.ToDTO(CurrentLesson);
            File.WriteAllText(path, JsonUtility.ToJson(dto, true));
            TriggerSentenceChanged();
        }

        private void TriggerSentenceChanged()
        {
            if (CurrentLesson == null || CurrentLesson.Sentences.Count == 0 || CurrentSentenceIndex >= CurrentLesson.Sentences.Count - 1)
                return;

            var sentence = isTaskSelected ? CurrentTask.Sentences[CurrentSentenceIndex] : CurrentLesson.Sentences[CurrentSentenceIndex];
            var defaultBackground = isTaskSelected ? CurrentTask.DefaultBackground : CurrentLesson.DefaultBackground;
            var lenght = isTaskSelected ? CurrentTask.Sentences.Count : CurrentLesson.Sentences.Count;

            SentenceChanged?.Invoke(
                sentence,
                defaultBackground,
                CurrentSentenceIndex,
                lenght
            );
        }
	}
}