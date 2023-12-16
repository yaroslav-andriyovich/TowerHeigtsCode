using CodeBase.Gameplay.TowerLogic;
using UnityEditor;
using UnityEngine;

namespace CodeBase.Editor
{
    [CustomEditor(typeof(Tower))]
    public class TowerEditor : UnityEditor.Editor
    {
        [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
        public static void RenderCustomGizmo(Tower tower, GizmoType gizmo)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawCube(tower.transform.position, new Vector3(0.5f, 0.8f * 2f, 0.5f));
        }
    }
}