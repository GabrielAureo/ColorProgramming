using AYellowpaper.SerializedCollections;
using ColorProgramming.Core;
using System;
using UnityEditor;
using UnityEngine;

namespace ColorProgramming
{
    [CreateAssetMenu(fileName = "ElementsData", menuName = "ColorProgramming/ElementData")]
    public class ElementsData : ScriptableObject
    {
        [SerializedDictionary("Element", "Data")]
        public SerializedDictionary<Element, ElementData> Data;

        public ElementData this[Element key]
        {
            get { return (Data[key]); }
            private set { }
        }
    }

    [Serializable]
    public class ElementData
    {
        [SerializeField]
        public Texture2D Texture;

        [SerializeField]
        public Color Color;
    }
}
