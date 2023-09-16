using AYellowpaper.SerializedCollections;
using ColorProgramming.Items;
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
        public SerializedDictionary<string, ItemController> Data;
    }
}
