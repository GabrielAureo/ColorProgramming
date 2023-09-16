using ColorProgramming.Core;
using UnityEditor;
using UnityEngine;

namespace ColorProgramming
{
    public class ConditionalNodeController : ConcreteNodeController<ConditionalNode>, IEvaluatable
    {
        [SerializeField]
        private Renderer TrueElementRenderer;

        [SerializeField]
        private Renderer FalseElementRenderer;

        [SerializeField]
        private Renderer CheckElementRenderer;
        [SerializeField]
        private Material ElementDisplayMaterial;

        private ElementsData ElementsData => GameManager.Instance.ElementsData;

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
            UpdateElemenstMaterials();
        }

        private void Awake()
        {
            UpdateElemenstMaterials();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private void UpdateElemenstMaterials()
        {
   
            UpdateElementRenderer(ConcreteNode.TrueElement, TrueElementRenderer, ElementsData);
            UpdateElementRenderer(ConcreteNode.FalseElement, FalseElementRenderer, ElementsData);
            UpdateElementRenderer(ConcreteNode.CheckedElement, CheckElementRenderer, ElementsData);
        }

        private void UpdateElementRenderer(
            Element element,
            Renderer renderer,
            ElementsData elementsData
        )
        {
            if (renderer == null)
                return;
            var material = new Material(ElementDisplayMaterial);
            material.SetTexture("_BaseMap", elementsData.Data[element].Texture);
            renderer.material = material;
        }
    }
}
