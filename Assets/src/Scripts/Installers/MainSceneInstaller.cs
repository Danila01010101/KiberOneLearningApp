using UnityEngine;
using Zenject;

namespace KiberOneLearningApp
{
    public class MainSceneInstaller : MonoInstaller
    {
        [SerializeField] private UIWindowManager mainMenuManager;

        public override void InstallBindings()
        {
            InstallMainMenu();
        }

        private void InstallMainMenu()
        {
            var menu = Instantiate(mainMenuManager);
            Container.Bind<UIWindowManager>().WithId("MainMenu").FromInstance(menu).AsSingle();
        }
    }
}