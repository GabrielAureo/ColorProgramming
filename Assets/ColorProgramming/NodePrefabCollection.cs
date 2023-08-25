using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ColorProgramming
{
    [CreateAssetMenu(
        fileName = "NodePrefabCollection",
        menuName = "ColorProgramming/NodePrefabCollection"
    )]
    public class NodePrefabCollection : ScriptableObject
    {
        [SerializedDictionary("Node Name", "Controller Prefab")]
        public SerializedDictionary<string, GameObject> Data;
    }
}
