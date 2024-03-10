using ColorProgramming;
using ColorProgramming.Core;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace ColorProgramming
{
    public class TargetNodeController : BaseNodeController
    {

        [SerializeField] private Transform elementTransform;
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
                UpdateElementsMaterials();
            };
        }

        private void UpdateElementsMaterials()
        {
            if (elementsData == null)
            {
                elementsData = ValidateResources.LoadElementsData();
            }

            AccentMaterial.SetColor("_BaseColor", elementsData[TargetNode.CurrentElement].Color);

            foreach (Transform child in elementTransform)
            {
                DestroyImmediate(child.gameObject);
            }

            var obj = Instantiate(elementsData[TargetNode.CurrentElement].Prefab, elementTransform);
            obj.transform.localPosition = Vector3.zero;
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
