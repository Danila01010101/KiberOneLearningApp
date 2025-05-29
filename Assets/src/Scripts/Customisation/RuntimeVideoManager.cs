using System.IO;
using UnityEngine;
#if UNITY_EDITOR
	using SFB;
#endif

namespace KiberOneLearningApp
{
	public static class RuntimeVideoManager
	{
		public static bool PickAndAssignVideo(RuntimeImagePlacement placement)
		{
#if UNITY_EDITOR || UNITY_STANDALONE
			string[] paths = SFB.StandaloneFileBrowser.OpenFilePanel(
				"Выберите видео", "", new[] { new SFB.ExtensionFilter("Video", "mp4") }, false);

			if (paths == null || paths.Length == 0 || !File.Exists(paths[0])) return false;

			string fileName = Path.GetFileName(paths[0]);
			string folder = Path.Combine(Application.persistentDataPath, StaticStrings.VideoSavesFloulderName);
			Directory.CreateDirectory(folder);
			string destPath = Path.Combine(folder, fileName);

			if (!File.Exists(destPath))
				File.Copy(paths[0], destPath);

			placement.videoPath = $"{StaticStrings.VideoSavesFloulderName}/{fileName}";
			return true;
#else
    Debug.LogWarning("Выбор видео доступен только в редакторе или Standalone.");
    return false;
#endif
		}

		public static void RemoveVideo(RuntimeImagePlacement placement)
		{
			if (string.IsNullOrEmpty(placement.videoPath)) return;

			string fullPath = Path.Combine(Application.persistentDataPath, placement.videoPath);
			if (File.Exists(fullPath)) File.Delete(fullPath);

			placement.videoPath = string.Empty;
		}
	}
}