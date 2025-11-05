using UnityEngine;

namespace Game.Controller.Gameplay
{
    public class BoardView : MonoBehaviour
    {
        public Transform board;
        public Transform ball;

        public void SetBoardTilt(Vector2 tilt, float maxDeg)
        {
            float ax = Mathf.Asin(Mathf.Clamp(tilt.x, -1f, 1f)) * Mathf.Rad2Deg;
            float ay = Mathf.Asin(Mathf.Clamp(tilt.y, -1f, 1f)) * Mathf.Rad2Deg;

            float rx = Mathf.Clamp(ay, -maxDeg, maxDeg);   // pitch
            float rz = Mathf.Clamp(-ax, -maxDeg, maxDeg);  // roll
            board.localRotation = Quaternion.Euler(rx, 0f, rz);
        }
    }
}