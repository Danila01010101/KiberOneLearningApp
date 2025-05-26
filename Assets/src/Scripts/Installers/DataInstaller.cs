using OfficeOpenXml;
using UnityEngine;
using Zenject;

namespace KiberOneLearningApp
{
    public class DataInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            InstallGlobalValueInstaller();
            InstallGameSaver();
            SetupLessonsData();
            SetupRuntimePlacementEditor();
        }

        private void InstallGlobalValueInstaller()
        {
            ExcelPackage.License.SetNonCommercialOrganization("DanilaDev");
            var valuesSetter = new GameObject("GlobalValueSetter").AddComponent<GlobalValueSetter>();
            DontDestroyOnLoad(valuesSetter);
        }
        
        private void InstallGameSaver() => new GameSaver();

        private void SetupLessonsData() => new LessonsLoader();
        
        private void SetupRuntimePlacementEditor() => new RuntimePlacementEditor();
    }
}
