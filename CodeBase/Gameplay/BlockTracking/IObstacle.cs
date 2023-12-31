using UnityEngine;

namespace CodeBase.Gameplay.BlockTracking
{
    public interface IObstacle
    {
        float Height { get; }
        float Width { get; }
        Transform transform { get; }
    }
}