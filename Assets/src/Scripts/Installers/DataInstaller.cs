using UnityEngine;
using Zenject;

namespace KiberOneLearningApp
{
    public class DataInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            new GameSaver();
        }
    }
}
