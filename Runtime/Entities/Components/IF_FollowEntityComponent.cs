using InterVR.IF.Defines;
using EcsRx.Components;
using EcsRx.Entities;
using UnityEngine;

namespace InterVR.IF.Components
{
    public class IF_FollowEntity : IComponent
    {
        public IF_UpdateMomentType UpdateMoment { get; set; }
        public bool FollowPosition { get; set; }
        public bool FollowRotation { get; set; }
        public bool SmoothPosition { get; set; }
        public bool SmoothRotation { get; set; }
        public Vector3 OffsetPosition { get; set; }
        public Vector3 OffsetRotation { get; set; }
        public IEntity FollowTargetEntity { get; set; }
        public IEntity FollowSourceEntity { get; set; }
        public float MaxDistanceDeltaPerFrame { get; set; }

        public Vector3 CalculatedPosition;
        public Vector3 CalculatedRotation;
    }
}