using System;
using System.Linq;
using System.Reflection;
using KiberOneLearningApp;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class SceneConstantsValidator : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;

    private static readonly Type ConstantsClass = typeof(SceneNaming);

    public void OnPreprocessBuild(BuildReport report)
    {
        // Получаем список всех сцен из Build Settings
        var buildScenes = EditorBuildSettings.scenes
            .Where(scene => scene.enabled)
            .Select(scene => System.IO.Path.GetFileNameWithoutExtension(scene.path))
            .ToList();

        // Получаем все константы из указанного класса
        var constants = ConstantsClass.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(field => field.IsLiteral && !field.IsInitOnly && field.FieldType == typeof(string))
            .Select(field => field.GetValue(null)?.ToString())
            .ToList();

        // Проверяем, все ли константы используются в Build Settings
        foreach (var constant in constants)
        {   
            if (!buildScenes.Contains(constant))
            {
                throw new BuildFailedException($"Ошибка сборки: сцена с именем '{constant}' (из класса {ConstantsClass.Name}) отсутствует в Build Settings!");
            }
        }

        Debug.Log("[SceneConstantsValidator] Все константы из класса проверены, ошибок не обнаружено.");
    }
}