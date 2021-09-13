using EcsRx.Components;
using EcsRx.Extensions;
using EcsRx.Entities;
using EcsRx.Unity.MonoBehaviours;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BCKWorks.Framework.Components
{
    public class TestCube : IComponent
    {
        public float Timer { get; set; }
        public int Interval { get; set; }
    }

    public class TestCubeComponent : RegisterAsEntity
    {
        public override void Convert(IEntity entity)
        {
            base.Convert(entity);

            entity.AddComponent(new TestCube() { Interval = 5 });
        }
    }
}