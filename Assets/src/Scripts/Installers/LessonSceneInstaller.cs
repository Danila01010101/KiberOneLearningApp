using UnityEngine;
using Zenject;

namespace KiberOneLearningApp
{
	public class LessonSceneInstaller : MonoInstaller
	{
		[SerializeField] private LessonWindow lessonWindow;

		public override void InstallBindings()
		{
			Container.BindInstance(lessonWindow).AsSingle()
				.IfNotBound();
		}
	}
}