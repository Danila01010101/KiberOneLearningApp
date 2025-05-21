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
            var extensions = new[] { new ExtensionFilter("Image Files", "png", "jpg", "jpeg") };
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