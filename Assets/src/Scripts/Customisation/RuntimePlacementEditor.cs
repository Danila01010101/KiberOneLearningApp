using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SFB;

namespace KiberOneLearningApp
{
    public class RuntimePlacementEditor : MonoBehaviour
    {
        [Header("UI References")]
        public TMP_Dropdown colliderTypeDropdown;
        public TMP_Dropdown keyCodeDropdown;

        public TMP_InputField posX, posY, posZ;
        public TMP_InputField sizeX, sizeY, sizeZ;
        public TMP_InputField rotX, rotY, rotZ;

        public TMP_InputField colPosX, colPosY, colPosZ;
        public TMP_InputField colSizeX, colSizeY, colSizeZ;

        public Button spriteSelectButton;
        public Image spritePreview;

        public Button saveButton;

        private Sprite selectedSprite;
        private string lastSelectedSpritePath = "";
        private string lastSelectedVideoPath = "";

        public Action<RuntimeInteractablePlacement> OnPlacementSaved;

        private void Start()
        {
            // Инициализация выпадающих списков
            colliderTypeDropdown.ClearOptions();
            colliderTypeDropdown.AddOptions(new List<string> { "rectangle", "circle" });

            keyCodeDropdown.ClearOptions();
            var names = Enum.GetNames(typeof(KeyCode));
            keyCodeDropdown.AddOptions(names.ToList());

            spriteSelectButton.onClick.AddListener(OpenSpritePicker);
            saveButton.onClick.AddListener(SavePlacement);
        }

        private void OpenSpritePicker()
        {
        #if UNITY_STANDALONE || UNITY_EDITOR
            var extensions = new[] { new ExtensionFilter("Image Files", "png", "jpg", "jpeg", "psd") };
            string[] paths = StandaloneFileBrowser.OpenFilePanel("Выберите изображение", "", extensions, false);

            if (paths != null && paths.Length > 0 && File.Exists(paths[0]))
            {
                string selectedPath = paths[0];
                string fileName = Path.GetFileName(selectedPath);
                string imageFolder = Path.Combine(Application.persistentDataPath, "UserImages");
                Directory.CreateDirectory(imageFolder);

                string targetPath = Path.Combine(imageFolder, fileName);

                // Если файла нет — копируем
                if (!File.Exists(targetPath))
                {
                    File.Copy(selectedPath, targetPath);
                    Debug.Log($"Картинка скопирована: {targetPath}");
                }
                else
                {
                    Debug.Log("Файл уже существует, повторно не копируем.");
                }

                // Загружаем превью и спрайт
                byte[] imageBytes = File.ReadAllBytes(targetPath);
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(imageBytes);

                selectedSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                spritePreview.sprite = selectedSprite;
                spritePreview.color = Color.white;

                // Сохраняем относительный путь для JSON
                lastSelectedSpritePath = $"UserImages/{fileName}";
            }
            else
            {
                Debug.LogWarning("Выбор файла отменён или файл не существует.");
            }
        #else
            Debug.LogWarning("File Picker работает только в Standalone билде или редакторе.");
        #endif
        }
        
        public void AssignSpriteToPlacement(RuntimeImagePlacement placement)
        {
#if UNITY_STANDALONE || UNITY_EDITOR
            var extensions = new[] { new ExtensionFilter("Image Files", "png", "jpg", "jpeg", "psd") };
            string[] paths = StandaloneFileBrowser.OpenFilePanel("Выберите изображение", "", extensions, false);

            if (paths != null && paths.Length > 0 && File.Exists(paths[0]))
            {
                string selectedPath = paths[0];
                string fileName = Path.GetFileName(selectedPath);
                string imageFolder = Path.Combine(Application.persistentDataPath, "UserImages");
                Directory.CreateDirectory(imageFolder);

                string targetPath = Path.Combine(imageFolder, fileName);

                if (!File.Exists(targetPath))
                {
                    File.Copy(selectedPath, targetPath);
                    Debug.Log($"Картинка скопирована: {targetPath}");
                }

                byte[] imageBytes = File.ReadAllBytes(targetPath);
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(imageBytes);

                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                placement.sprite = sprite;
                placement.spritePath = $"UserImages/{fileName}";

                Debug.Log($"Картинка назначена: {placement.spritePath}");
            }
            else
            {
                Debug.LogWarning("Выбор картинки отменён или путь не существует.");
            }
#else
    Debug.LogWarning("File Picker доступен только в Standalone или Editor.");
#endif
        }
        
        public void RemoveSpriteFromPlacement(RuntimeImagePlacement placement)
        {
            if (string.IsNullOrEmpty(placement.spritePath))
            {
                Debug.Log("Картинка не назначена.");
                return;
            }

            string fullPath = Path.Combine(Application.persistentDataPath, placement.spritePath);

            if (File.Exists(fullPath))
            {
                try
                {
                    File.Delete(fullPath);
                    Debug.Log($"Удалена картинка: {fullPath}");
                }
                catch (Exception e)
                {
                    Debug.LogError($"Ошибка при удалении картинки: {e.Message}");
                }
            }
            else
            {
                Debug.LogWarning("Файл картинки не найден. Возможно, он уже удалён.");
            }

            placement.sprite = null;
            placement.spritePath = string.Empty;
        }
        
        public void PickAndAssignVideo(RuntimeImagePlacement placement)
        {
        #if UNITY_EDITOR
            string path = UnityEditor.EditorUtility.OpenFilePanel("Выбрать видео", "", "mp4");
            if (string.IsNullOrEmpty(path))
                return;

            string fileName = Path.GetFileName(path);
            string destFolder = Path.Combine(Application.persistentDataPath, "UserVideos");

            if (!Directory.Exists(destFolder))
                Directory.CreateDirectory(destFolder);

            string destPath = Path.Combine(destFolder, fileName);

            // Копируем, если ещё не скопирован
            if (!File.Exists(destPath))
            {
                File.Copy(path, destPath);
                Debug.Log($"Видео скопировано в {destPath}");
            }

            // Сохраняем относительный путь
            placement.videoPath = $"UserVideos/{fileName}";
            Debug.Log($"Видео добавлено в placement: {placement.videoPath}");
        #else
            Debug.LogWarning("Загрузка видеофайлов доступна только в редакторе.");
        #endif
        }
        
        public void RemoveAssignedVideo(RuntimeImagePlacement placement)
        {
            if (string.IsNullOrEmpty(placement.videoPath))
            {
                Debug.Log("Видео не прикреплено.");
                return;
            }

            string fullPath = Path.Combine(Application.persistentDataPath, placement.videoPath);

            if (File.Exists(fullPath))
            {
                try
                {
                    File.Delete(fullPath);
                    Debug.Log($"Удалено видео: {fullPath}");
                }
                catch (Exception e)
                {
                    Debug.LogError($"Ошибка при удалении видео: {e.Message}");
                }
            }
            else
            {
                Debug.LogWarning("Файл видео не найден. Возможно, он уже удалён.");
            }

            placement.videoPath = string.Empty;
        }

        private void SavePlacement()
        {
            var placement = new RuntimeInteractablePlacement
            {
                imagePlacement = new RuntimeImagePlacement
                {
                    position = ParseVector3(posX, posY, posZ),
                    size = ParseVector3(sizeX, sizeY, sizeZ),
                    rotation = Quaternion.Euler(ParseVector3(rotX, rotY, rotZ)),
                    sprite = selectedSprite
                },
                colliderType = (ColliderType)colliderTypeDropdown.value,
                colliderPosition = ParseVector3(colPosX, colPosY, colPosZ),
                colliderSize = ParseVector3(colSizeX, colSizeY, colSizeZ),
                rotation = Quaternion.Euler(ParseVector3(rotX, rotY, rotZ)),
                
                keyCode = (KeyCode)Enum.Parse(typeof(KeyCode), keyCodeDropdown.options[keyCodeDropdown.value].text)
            };

            Debug.Log("Создан RuntimeInteractablePlacement");
            placement.imagePlacement.sprite = selectedSprite;
            placement.imagePlacement.spritePath = lastSelectedSpritePath;
            placement.imagePlacement.videoPath = lastSelectedVideoPath;
            OnPlacementSaved?.Invoke(placement);
        }

        private Vector3 ParseVector3(TMP_InputField x, TMP_InputField y, TMP_InputField z)
        {
            float fx = float.TryParse(x.text, out fx) ? fx : 0f;
            float fy = float.TryParse(y.text, out fy) ? fy : 0f;
            float fz = float.TryParse(z.text, out fz) ? fz : 0f;
            return new Vector3(fx, fy, fz);
        }
    }
}