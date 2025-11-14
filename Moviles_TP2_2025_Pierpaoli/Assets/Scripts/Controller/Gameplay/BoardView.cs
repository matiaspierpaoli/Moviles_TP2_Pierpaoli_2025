using UnityEngine;

namespace Game.Controller.Gameplay
{
    public class BoardView : MonoBehaviour
    {
        [Header("Refs")]
        public Rigidbody boardRb;
        public Transform ball;

        [Header("Pitch degrees")]
        public float maxPitchDownDeg = 30f;
        public float maxPitchUpDeg   = 15f;
        [Header("Roll degrees")]
        public float maxRollDeg = 20f;

        [Header("Suavizado física")]
        [Range(0f, 30f)] public float rotSmoothSpeed = 12f;

        Quaternion _targetRot;

        void Awake()
        {
            if (!boardRb) boardRb = GetComponent<Rigidbody>();
            if (boardRb) { boardRb.isKinematic = true; boardRb.interpolation = RigidbodyInterpolation.Interpolate; }
        }

        void Start()
        {
            var brb = ball.GetComponent<Rigidbody>();
            if (brb) { brb.maxAngularVelocity = 50f; brb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic; brb.interpolation = RigidbodyInterpolation.Interpolate; }
            _targetRot = boardRb ? boardRb.rotation : transform.rotation;
        }

        public void SetBoardTilt(Vector2 tilt)
        {
            float pitch = tilt.y >= 0 ?  tilt.y *  maxPitchDownDeg
                                      :  tilt.y *  maxPitchUpDeg;

            float roll  = tilt.x * maxRollDeg;

            float rx = (roll + pitch);
            float rz = (roll - pitch);

            _targetRot = Quaternion.Euler(rx, 0f, rz);
        }

        void FixedUpdate()
        {
            if (!boardRb) return;

            var current = boardRb.rotation;
            var next = Quaternion.Slerp(current, _targetRot, 1f - Mathf.Exp(-rotSmoothSpeed * Time.fixedDeltaTime));

            boardRb.MoveRotation(next);
        }
    }
}
