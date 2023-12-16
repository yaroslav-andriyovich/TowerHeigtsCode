using UnityEngine;

namespace CodeBase.Gameplay.Obstacle
{
    public interface IObstacle
    {
        float Height { get; }
        float Width { get; }
        Transform transform { get; }
    }
}