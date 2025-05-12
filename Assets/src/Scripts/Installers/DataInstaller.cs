using OfficeOpenXml;
using UnityEngine;
using Zenject;

namespace KiberOneLearningApp
{
    public class DataInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            ExcelPackage.License.SetNonCommercialOrganization("DanilaDev");
            var valuesSetter = new GameObject("GlobalValueSetter").AddComponent<GlobalValueSetter>();
            DontDestroyOnLoad(valuesSetter);
            new GameSaver();
        }
    }
}
