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
                //TutorialVideo = LoadVideo(dto.TutorialVideoPath),
                CharacterPosition = dto.CharacterPosition.ToVector3(),
                IsBeforeTask = dto.IsBeforeTask,
                HideCharacter = dto.HideCharacter,
                TaskPrefab = LoadTaskPrefab(dto.TaskPrefabName),
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

        //private static VideoClip LoadVideo(string path)
        //{
        //    // VideoClip не может быть создан напрямую из файла в рантайме
        //    // Но можно использовать VideoPlayer.url с внешним файлом (mp4)
        //    return null; // можно вместо этого вернуть путь
        //}

        private static GameObject LoadTaskPrefab(string name)
        {
            if (string.IsNullOrEmpty(name)) return null;
            return Resources.Load<GameObject>($"Tasks/{name}");
        }
    }
}