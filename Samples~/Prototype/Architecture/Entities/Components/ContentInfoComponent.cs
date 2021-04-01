using EcsRx.Components;
using EcsRx.Entities;
using EcsRx.Extensions;
using EcsRx.UnityEditor.MonoBehaviours;
using Prototype.Defines;

namespace Prototype.Components
{
    public class ContentInfo : IComponent
    {
        public ContentType Type { get; set; }
    }

    public class ContentInfoComponent : RegisterAsEntity
    {
        public ContentType Type;

        public override void Convert(IEntity entity)
        {
            var component = new ContentInfo();

            component.Type = Type;

            entity.AddComponentSafe(component);

            Destroy(this);
        }
    }
}