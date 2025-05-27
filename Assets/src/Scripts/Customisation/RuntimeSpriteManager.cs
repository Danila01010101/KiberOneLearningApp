using System.IO;
using UnityEngine;
#if UNITY_EDITOR
	using SFB;
#endif

namespace KiberOneLearningApp
{
	public static class RuntimeSpriteManager
	{
		public static Sprite LoadSprite(string fullPath)
		{
			if (!File.Exists(fullPath)) return null;

			byte[] imageBytes = File.ReadAllBytes(fullPath);
			Texture2D texture = new Texture2D(2, 2);
			if (!texture.LoadImage(imageBytes)) return null;

			return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
		}

		public static bool PickAndAssignSprite(RuntimeImagePlacement placement)
		{
#if UNITY_EDITOR || UNITY_STANDALONE
			var extensions = new[] { new ExtensionFilter("Image Files", "png", "jpg", "jpeg", "psd") };
			string[] paths = StandaloneFileBrowser.OpenFilePanel("Выбрать изображение", "", extensions, false);
			if (paths == null || paths.Length == 0 || !File.Exists(paths[0])) return false;

			string fileName = Path.GetFileName(paths[0]);
			string folder = Path.Combine(Application.persistentDataPath, StaticStrings.ImagesSavesFloulderName);
			Directory.CreateDirectory(folder);
			string destPath = Path.Combine(folder, fileName);

			if (!File.Exists(destPath))
				File.Copy(paths[0], destPath);

			placement.sprite = LoadSprite(destPath);
			placement.spritePath = $"{StaticStrings.ImagesSavesFloulderName}/{fileName}";

			return true;
#else
        return false;
#endif
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