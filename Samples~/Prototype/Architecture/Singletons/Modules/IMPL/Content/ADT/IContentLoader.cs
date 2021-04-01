using InterVR.IF.Modules;
using Prototype.Defines;
using System;
using UniRx;

namespace Prototype.Modules
{
    public interface IContentLoader : IF_IModule
    {
        IObservable<Unit> EnsureLoadBaseAsync();
        IObservable<Unit> LoadContentAsync(ContentType type);
    }
}
