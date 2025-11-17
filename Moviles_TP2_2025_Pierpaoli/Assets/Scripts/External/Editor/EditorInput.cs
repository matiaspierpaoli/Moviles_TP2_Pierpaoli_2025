using UnityEngine;
using Game.Core.ServicesCore;
using UnityEngine.InputSystem;

namespace Game.External
{
    public class EditorInput : IInputStrategy
    {
        private Vector2 currentTilt; 
        
        private readonly float moveSpeed; 
        private readonly float returnSpeed;

        public EditorInput(float moveSpeed = 2.0f, float returnSpeed = 3.0f)
        {
            this.moveSpeed = moveSpeed;
            this.returnSpeed = returnSpeed;
        }

        public Vector2 ReadTilt()
        {
            var k = Keyboard.current;
            
            float targetX = (k.rightArrowKey.isPressed ? 1f : 0f) - (k.leftArrowKey.isPressed ? 1f : 0f);
            float targetY = (k.upArrowKey.isPressed ? 1f : 0f) - (k.downArrowKey.isPressed ? 1f : 0f);
            
            float targetTiltX = -targetX;
            if (targetTiltX != 0)
                currentTilt.x = Mathf.MoveTowards(currentTilt.x, targetTiltX, moveSpeed * Time.deltaTime);
            else
                currentTilt.x = Mathf.MoveTowards(currentTilt.x, 0, returnSpeed * Time.deltaTime);
            
            float targetTiltY = targetY;
            if (targetTiltY != 0)
                currentTilt.y = Mathf.MoveTowards(currentTilt.y, targetTiltY, moveSpeed * Time.deltaTime);
            else
                currentTilt.y = Mathf.MoveTowards(currentTilt.y, 0, returnSpeed * Time.deltaTime);

            return currentTilt;
        }

        public Vector2 ReadRawTilt()
        {
            return currentTilt;
        }
    }
}