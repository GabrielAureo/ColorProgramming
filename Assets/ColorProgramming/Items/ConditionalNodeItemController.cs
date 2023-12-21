using ColorProgramming.Items;
using ColorProgramming;
using ColorProgramming.Core;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine.UI;

namespace ColorProgramming.Items
{
    public class ConditionalNodeItemController : ConcreteItemController<ConditionalNode>
    {

        [SerializeField]
        public Image TrueElementImage;
        [SerializeField]
        private Image FalseElementImage;
        [SerializeField]
        private Image CheckElementImage;

        private ElementsData ElementsData => Resources.Load<ElementsData>("ElementsData");


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
                UpdateElementsSprite();
            };
        }
        protected override void SetupConcreteNode(ConditionalNode concreteNode)
        {
            concreteNode.CheckedElement = ConcreteItem.ConcreteNode.CheckedElement;
            concreteNode.FalseElement = ConcreteItem.ConcreteNode.FalseElement;
            concreteNode.TrueElement = ConcreteItem.ConcreteNode.TrueElement;
            concreteNode.IsNot = ConcreteItem.ConcreteNode.IsNot;
        }

        private void Awake()
        {
            UpdateElementsSprite();
        }

        private void UpdateElementsSprite()
        {
            UpdateElementSprite(ConcreteItem.ConcreteNode.TrueElement, TrueElementImage);
            UpdateElementSprite(ConcreteItem.ConcreteNode.FalseElement, FalseElementImage);
            UpdateElementSprite(ConcreteItem.ConcreteNode.CheckedElement, CheckElementImage);
        }

        private void UpdateElementSprite(
           Element element,
           Image image
       )
        {
            var sprite = ElementsData[element].Sprite;
            image.sprite = sprite;

        }

        protected override void SetupConcreteController(ConcreteNodeController<ConditionalNode> nodeController)
        {
            var conditionalNodeController = nodeController as ConditionalNodeController;

            conditionalNodeController.UpdateElemenstMaterials();
        }
    }
}
