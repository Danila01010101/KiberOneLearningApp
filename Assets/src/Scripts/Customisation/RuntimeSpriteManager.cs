using System.IO;
using UnityEngine;
#if UNITY_EDITOR
	using SFB;
#endif

namespace KiberOneLearningApp
{
	public static class RuntimeSpriteManager
	{
		public static Sprite LoadSpriteFromPath(string spritePath)
		{
			string fullPath = Path.Combine(Application.persistentDataPath, spritePath);

			if (!File.Exists(fullPath))
			{
				Debug.LogError($"Файл не найден: {fullPath}");
				return null;
			}

			Texture2D texture = LoadTexture(fullPath);
			return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
		}


		public static bool PickAndAssignSprite(RuntimeImagePlacement placement)
		{
#if UNITY_EDITOR || UNITY_STANDALONE
			string[] paths = SFB.StandaloneFileBrowser.OpenFilePanel(
				"Выбрать изображение", "", new[] { new SFB.ExtensionFilter("Image Files", "png", "jpg", "jpeg", "psd") }, false);

			if (paths == null || paths.Length == 0 || !File.Exists(paths[0]))
				return false;

			string fileName = Path.GetFileName(paths[0]);
			string folder = Path.Combine(Application.persistentDataPath, "UserImages");
			Directory.CreateDirectory(folder);

			string destPath = Path.Combine(folder, fileName);

			if (!File.Exists(destPath))
				File.Copy(paths[0], destPath);

			// Сохраняем путь
			placement.spritePath = $"UserImages/{fileName}";

			// Загружаем в Texture2D
			Texture2D texture = LoadTexture(destPath);
			Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
			sprite.name = Path.GetFileNameWithoutExtension(fileName);
			placement.sprite = sprite;

			return true;
#else
    return false;
#endif
		}
		
		private static Texture2D LoadTexture(string path)
		{
			byte[] imageData = File.ReadAllBytes(path);
			Texture2D texture = new Texture2D(2, 2);
			texture.LoadImage(imageData);
			return texture;
		}

		public static void RemoveSprite(RuntimeImagePlacement placement)
		{
			if (string.IsNullOrEmpty(placement.spritePath)) return;

			string fullPath = Path.Combine(Application.persistentDataPath, placement.spritePath);
			if (File.Exists(fullPath)) File.Delete(fullPath);

			placement.sprite = null;
			placement.spritePath = string.Empty;
		}
	}
}