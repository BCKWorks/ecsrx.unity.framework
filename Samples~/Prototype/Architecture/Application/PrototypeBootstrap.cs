using EcsRx.Infrastructure.Extensions;
using Prototype.Installer;
using InterVR.IF;
using Prototype.Modules;
using UniRx;
using InterVR.IF.Events;

namespace Prototype
{
    public class PrototypeBootstrap : IF_ApplicationBehaviour
    {
        protected override void BindSystems()
        {
            base.BindSystems();
        }

        protected override void LoadModules()
        {
            base.LoadModules();

            Container.LoadModule<ContentModules>();
        }

        protected override void ApplicationStarted()
        {
            var settings = Container.Resolve<PrototypeInstaller.Settings>();
            var contentLoader = Container.Resolve<IContentLoader>();
            contentLoader.EnsureLoadBaseAsync().Subscribe(x =>
            {
                contentLoader.LoadContentAsync(settings.StartContent).Subscribe(y =>
                {
                    this.Started = true;

                    EventSystem.Publish(new IF_ApplicationStartedEvent()
                    {
                    });
                });
            });
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        private void OnApplicationPause(bool pause)
        {
            if (pause == false)
            {
            }
        }

        private void OnApplicationFocus(bool focus)
        {

        }
    }
}
