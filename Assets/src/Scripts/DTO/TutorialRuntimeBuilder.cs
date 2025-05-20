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
                TutorialVideoPath = dto.TutorialVideoPath,
                CharacterPosition = dto.CharacterPosition.ToVector3(),
                IsBeforeTask = dto.IsBeforeTask,
                HideCharacter = dto.HideCharacter,
                InteractableImages = dto.InteractableImages?.Select(i => new RuntimeInteractablePlacement()
                {
                    colliderPosition = i.colliderPosition.ToVector3(),
                    colliderType = i.colliderType,
                    colliderSize = i.colliderSize.ToVector3(),
                    rotation = i.rotation.ToQuaternion(),
                    imagePlacement = new RuntimeImagePlacement ()
                    {
                        position = i.imagePlacement.position.ToVector3(),
                        size = i.imagePlacement.size.ToVector3(),
                        rotation = i.imagePlacement.rotation.ToQuaternion(),
                        sprite = LoadSprite(i.imagePlacement.spritePath)
                    },
                }).ToList(),
                Text = dto.Text,
                Images = dto.Images?.Select(i => new RuntimeImagePlacement
                {
                    position = i.position.ToVector3(),
                    size = i.size.ToVector3(),
                    rotation = i.rotation.ToQuaternion(),
                    sprite = LoadSprite(i.spritePath)
                }).ToList()
            };
        }

        private static Sprite LoadSprite(string path)
        {
            if (string.IsNullOrEmpty(path)) return null;

            string fullPath = Path.Combine(Application.persistentDataPath, path);
            if (!File.Exists(fullPath)) return null;

            byte[] data = File.ReadAllBytes(fullPath);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(data);
            return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        }

        private static GameObject LoadTaskPrefab(string name)
        {
            if (string.IsNullOrEmpty(name)) return null;
            return Resources.Load<GameObject>($"Tasks/{name}");
        }
    }
}