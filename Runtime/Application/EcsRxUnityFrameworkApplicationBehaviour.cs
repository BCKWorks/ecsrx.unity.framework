using EcsRx.Infrastructure.Extensions;
using EcsRx.Zenject;
using System.Collections;
using UnityEngine;

namespace EcsRx.Unity.Framework
{
    [DefaultExecutionOrder(-20000)]
    public abstract class EcsRxUnityFrameworkApplicationBehaviour : EcsRxApplicationBehaviour
    {
        protected override void BindSystems()
        {
            base.BindSystems();

            Container.BindApplicableSystems(
                "EcsRx.Unity.Framework.Systems",
                "EcsRx.Unity.Framework.ViewResolvers");
        }

        protected override void LoadModules()
        {
            base.LoadModules();

            Container.LoadModule<ToolModules>();
            Container.LoadModule<PluginModules>();
            Container.LoadModule<SceneModules>();
        }

        protected override IEnumerator ApplicationStartedAsync()
        {
            yield return Container.InitializeModules();
            ApplicationStarted();
        }

        protected override void ApplicationStarted()
        {
            Started = true;
            EventSystem.Publish(new EcsRxUnityFrameworkApplicationStartedEvent() { });
        }

        protected virtual void OnDestroy()
        {
            Container.UnloadModules();
            StopAndUnbindAllSystems();
        }
    }
}