using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace KiberOneLearningApp
{ 
    public static class TutorialRuntimeBuilder
    {
        public static RuntimeTutorialData FromDTO(TutorialDataDTO dto)
        {
            return new RuntimeTutorialData
            {
                ThemeName = dto.ThemeName,
                TutorialName = dto.TutorialName,
                DefaultBackground = LoadSprite(dto.DefaultBackgroundPath),
                DefaultText = LoadSprite(dto.DefaultTextPath),
                Tasks = dto.Tasks?.Select(FromDTO).ToList(),
                Sentences = dto.Sentences?.Select(FromSentenceDTO).ToList()
            };
        }

        private static RuntimeSentenceData FromSentenceDTO(SentenceDataDTO dto)
        {
            return new RuntimeSentenceData
            {
                Background = LoadSprite(dto.BackgroundPath),
                CharacterIcon = LoadSprite(dto.CharacterIconPath),
                CharacterPosition = dto.CharacterPosition.ToVector3(),
                CharacterSize = dto.CharacterSize.ToVector3(),
                IsBeforeTask = dto.IsBeforeTask,
                HideCharacter = dto.HideCharacter,
                TutorialVideoPath = dto.TutorialVideoPath,
                Text = dto.Text,

                Images = dto.Images?.Select(i => new RuntimeImagePlacement
                {
                    position = i.position.ToVector3(),
                    size = i.size.ToVector3(),
                    rotation = i.rotation.ToQuaternion(),
                    sprite = LoadSprite(i.spritePath),
                    spritePath = i.spritePath
                }).ToList(),

                InteractableImages = dto.InteractableImages?.Select(i => new RuntimeInteractablePlacement
                {
                    colliderPosition = i.colliderPosition.ToVector3(),
                    colliderSize = i.colliderSize.ToVector3(),
                    colliderType = i.colliderType,
                    rotation = i.rotation.ToQuaternion(),
                    keyCode = Enum.TryParse<KeyCode>(i.keyCode, out var parsedKey) ? parsedKey : KeyCode.Mouse0,
                    imagePlacement = new RuntimeImagePlacement
                    {
                        position = i.imagePlacement.position.ToVector3(),
                        size = i.imagePlacement.size.ToVector3(),
                        rotation = i.imagePlacement.rotation.ToQuaternion(),
                        sprite = LoadSprite(i.imagePlacement.spritePath),
                        spritePath = i.imagePlacement.spritePath
                    }
                }).ToList()
            };
        }
        
        private static Sprite LoadSprite(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath)) return null;

            string fullPath = relativePath;
            
#if UNITY_EDITOR
            string sesondPartOfPath = "D:\\Unity\\UnityProjects\\KiberOneLearningApp\\Assets\\";
            fullPath = sesondPartOfPath + fullPath;
            if (!File.Exists(fullPath)) return null;
#else
            fullPath = Path.Combine(Application.persistentDataPath, relativePath);
            if (!File.Exists(fullPath)) return null;
#endif


            byte[] data = File.ReadAllBytes(fullPath);
            Texture2D texture = new Texture2D(2, 2);
            if (!texture.LoadImage(data)) return null;
            
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }

        private static GameObject LoadTaskPrefab(string name)
        {
            if (string.IsNullOrEmpty(name)) return null;
            return Resources.Load<GameObject>($"Tasks/{name}");
        }
    }
}