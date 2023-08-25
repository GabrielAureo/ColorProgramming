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

        private ElementsData elementsData;

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

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private void UpdateElemenstMaterials()
        {
            if (elementsData == null)
            {
                elementsData = ValidateResources.LoadElementsData();
            }
            UpdateElementRenderer(ConcreteNode.TrueElement, TrueElementRenderer, elementsData);
            UpdateElementRenderer(ConcreteNode.FalseElement, FalseElementRenderer, elementsData);
            UpdateElementRenderer(ConcreteNode.CheckedElement, CheckElementRenderer, elementsData);
        }

        private void UpdateElementRenderer(
            Element element,
            Renderer renderer,
            ElementsData elementsData
        )
        {
            if (renderer == null || renderer.sharedMaterial == null)
                return;
            var material = new Material(renderer.sharedMaterial);
            material.SetTexture("_BaseMap", elementsData.Data[element].Texture);
            renderer.material = material;
        }
    }
}
