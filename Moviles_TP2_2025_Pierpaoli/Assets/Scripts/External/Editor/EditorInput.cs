using UnityEngine;
using Game.Core.ServicesCore;
using UnityEngine.InputSystem;

namespace Game.External
{
    public class EditorInput : IInputStrategy
    {
        public Vector2 ReadTilt()
        {
            var k = Keyboard.current;
            float x = (k.rightArrowKey.isPressed ? 1f : 0f) - (k.leftArrowKey.isPressed ? 1f : 0f);
            float y = (k.upArrowKey.isPressed ? 1f : 0f) - (k.downArrowKey.isPressed ? 1f : 0f);
            return new Vector2(x, y) * 0.3f; // ganancia para simular tilt
        }
    }
}