using CodeBase.Logic.EnemySpawners;
using CodeBase.StaticData;
using UnityEditor;
using UnityEngine;

namespace CodeBase.Editor
{
    [CustomEditor(typeof(SpawnMarker))]
    public class SpawnedMarkerEditor : UnityEditor.Editor
    {
        [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
        public static void RenderCustomGizmo(SpawnMarker spawnMarker, GizmoType gizmoType)
        {
            switch (spawnMarker.MonsterTypeId)
            {
                case MonsterTypeId.SimplePink:
                    Gizmos.color = new Color(1f, 0.96f, 0.14f);
                    break;
                case MonsterTypeId.MediumPink:
                    Gizmos.color = new Color(1f, 0.36f, 0f);
                    break;
                case MonsterTypeId.HardPink:
                    Gizmos.color = new Color(1f, 0.01f, 0.04f);
                    break;
                
                case MonsterTypeId.SimpleRed:
                    Gizmos.color = new Color(1f, 0.96f, 0.14f);
                    break;
                case MonsterTypeId.MediumRed:
                    Gizmos.color = new Color(1f, 0.36f, 0f);
                    break;
                case MonsterTypeId.HardRed:
                    Gizmos.color = new Color(1f, 0.01f, 0.04f);
                    break;
            }
            Gizmos.DrawSphere(spawnMarker.transform.position, 0.5f);
        }
    }
}