using UnityEngine;

namespace Game.Core.ServicesCore
{
    public interface IInputStrategy
    {
        Vector2 ReadTilt();
        Vector2 ReadRawTilt();
    }
}