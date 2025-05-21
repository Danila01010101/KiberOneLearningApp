
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using System.IO;

namespace KiberOneLearningApp
{
	public class CopyAssetsOnBuild : IPreprocessBuildWithReport
	{
		public int callbackOrder => 0;

		private const string CopyToPath = "Assets/StreamingAssets/UserImages/";

		public void OnPreprocessBuild(BuildReport report)
		{
			Debug.Log("Запущена копия ассетов в StreamingAssets перед сборкой...");

			// Создание папки, если нужно
			if (!Directory.Exists(CopyToPath))
				Directory.CreateDirectory(CopyToPath);

			// Очистим старое
			foreach (var file in Directory.GetFiles(CopyToPath))
			{
				File.Delete(file);
			}

			// Копируем ассеты
			CopySprites();
			CopyVideos();

			AssetDatabase.Refresh();
			Debug.Log("Копия ассетов завершена.");
		}

		private void CopySprites()
		{
			string[] spriteGuids = AssetDatabase.FindAssets("t:Sprite");

			foreach (string guid in spriteGuids)
			{
				string assetPath = AssetDatabase.GUIDToAssetPath(guid);
				string filename = Path.GetFileName(assetPath);

				string destPath = Path.Combine(CopyToPath, filename);
				File.Copy(assetPath, destPath, true);
			}
		}

		private void CopyVideos()
		{
			string[] videoGuids = AssetDatabase.FindAssets("t:VideoClip");

			foreach (string guid in videoGuids)
			{
				string assetPath = AssetDatabase.GUIDToAssetPath(guid);
				string filename = Path.GetFileName(assetPath);

				string destPath = Path.Combine(CopyToPath, filename);
				File.Copy(assetPath, destPath, true);
			}
		}
	}
}