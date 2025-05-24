using Zenject;

namespace KiberOneLearningApp
{
	public class LessonOpenerInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			var data = LessonDataHolder.SelectedLesson;
			Container.Bind<RuntimeTutorialData>().FromInstance(data).AsSingle();
		}
	}
}