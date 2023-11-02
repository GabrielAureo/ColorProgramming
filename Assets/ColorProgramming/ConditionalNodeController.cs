using System.Collections;
using System.Collections.Generic;
using ColorProgramming.Core;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace ColorProgramming
{
    public class ConditionalNodeController : ConcreteNodeController<ConditionalNode>, IEvaluatable
    {
        [SerializeField]
        private Transform TrueElementTransform;

        [SerializeField]
        private Transform FalseElementTransform;

        [SerializeField]
        private Transform CheckElementTransform;


        private ElementsData ElementsData => Resources.Load<ElementsData>("ElementsData");

        public void Evaluate(AgentController playerController)
        {
            var newElement =
                playerController.AgentNode.CurrentElement == ConcreteNode.CheckedElement
                && !ConcreteNode.IsNot
                    ? ConcreteNode.TrueElement
                    : ConcreteNode.FalseElement;

            playerController.AgentNode.SetElement(newElement);
        }

        private void OnValidate()
        {
            if (
                PrefabStageUtility.GetPrefabStage(gameObject) != null
                || EditorUtility.IsPersistent(this)
                || EditorApplication.isPlayingOrWillChangePlaymode
            )
            {
                return;
            }
            EditorApplication.delayCall += () =>
            {
                UpdateElemenstMaterials();
            };
        }

        private void Awake()
        {
            UpdateElemenstMaterials();
        }

        private void UpdateElemenstMaterials()
        {
            UpdateElementObject(ConcreteNode.TrueElement, TrueElementTransform, ElementsData);
            UpdateElementObject(ConcreteNode.FalseElement, FalseElementTransform, ElementsData);
            UpdateElementObject(ConcreteNode.CheckedElement, CheckElementTransform, ElementsData);
        }

        private void UpdateElementObject(
            Element element,
            Transform transform,
            ElementsData elementsData
        )
        {
            foreach (Transform child in transform)
            {
                DestroyImmediate(child.gameObject);
            }
            var obj = Instantiate(elementsData[element].Prefab, transform);
            obj.transform.localPosition = Vector3.zero;

        }
    }
}
