using UnityEngine;

namespace crass
{
    public class SmoothFollow : MonoBehaviour
    {
        public Transform Target;

        public Vector3 PositionOffsetFromTarget, RotationOffsetFromTarget;

        [Tooltip("How many seconds it takes for this object to reach the target's position plus the offset")]
        public float MoveDelay = 0.3f;

        [Tooltip("How many seconds it takes for this object's rotation to reach the target's rotation")]
        public float RotationDelay = 0.3f;

        Vector3 positionvelocity = Vector3.zero;
        Quaternion rotationVelocity = Quaternion.identity;

        void Start ()
        {
            if (transform.parent != null)
            {
                transform.parent = null;
            }
        }

        void Update ()
        {
            if (Target == null) return;

            Vector3 followPosition = Target.TransformPoint(PositionOffsetFromTarget);
            transform.position = Vector3.SmoothDamp(transform.position, followPosition, ref positionvelocity, MoveDelay);

            Quaternion followRotation = Target.rotation * Quaternion.Euler(RotationOffsetFromTarget);
            transform.rotation = transform.rotation.SmoothDampTo(followRotation, ref rotationVelocity, RotationDelay);
        }
    }
}
