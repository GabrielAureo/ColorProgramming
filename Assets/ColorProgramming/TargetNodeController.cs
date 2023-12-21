using ColorProgramming;
using ColorProgramming.Core;
using UnityEngine;

namespace ColorProgramming
{
    public class TargetNodeController : BaseNodeController
    {
        public override Node Node
        {
            get => TargetNode;
            set => TargetNode = (TargetNode)value;
        }
        public TargetNode TargetNode;

        public Material AccentMaterial;

        private ElementsData elementsData;

        public bool IsCorrectElement(Element element) => element == TargetNode.CurrentElement;

        private void OnValidate()
        {
            if (elementsData == null)
            {
                elementsData = ValidateResources.LoadElementsData();
            }

            AccentMaterial.SetColor("_BaseColor", elementsData[TargetNode.CurrentElement].Color);
        }

        public override void OnAgentTouch(AgentController agent)
        {
            if (agent.AgentNode.CurrentElement == TargetNode.CurrentElement)
            {
                GameManager.Instance.StageController.CompleteStage();
            }
            else
            {
                GameManager.Instance.StageController.FailStage();
            }
        }
    }
}
