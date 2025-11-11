using System;
using UnityEngine;

namespace Game.Controller.Gameplay
{
    public class BoardView : MonoBehaviour
    {
        public Transform board;
        public Transform ball;
        public Transform ballInitialTransform;
        public Transform mazeInitialTransform;
        [Header("Pitch degrees")]
        public float maxPitchDownDeg = 30f;
        public float maxPitchUpDeg   = 15f;

        [Header("Roll degrees")]
        public float maxRollDeg = 20f;

        private void Start()
        {
            ball.GetComponent<Rigidbody>().maxAngularVelocity = 50f;
            ball.transform.position = ballInitialTransform.position;
            ball.transform.rotation = ballInitialTransform.rotation;

            board.position = mazeInitialTransform.position;
            board.rotation = mazeInitialTransform.rotation;
        }

        public void SetBoardTilt(Vector2 tilt)
        {
            float pitch; 
            if (tilt.y > 0)
                pitch = tilt.y * maxPitchDownDeg;
            else
                pitch = tilt.y * maxPitchUpDeg;
            
            float roll = tilt.x * maxRollDeg;

            float rx = (roll + pitch);
            float rz = (roll - pitch);

            board.localRotation = Quaternion.Euler(rx, 0f, rz);
        }
    }
}