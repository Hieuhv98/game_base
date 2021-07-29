using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game_Base.Editor
{
    public class LevelMap : MonoBehaviour
    {
        void OnDrawGizmos()
        {
            float verticalHeightSeen = Camera.main.orthographicSize * 2.0f;
            float verticalWidthSeen = verticalHeightSeen * Camera.main.aspect;

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, new Vector3(verticalWidthSeen, verticalHeightSeen, 0));
        }
    }
}

