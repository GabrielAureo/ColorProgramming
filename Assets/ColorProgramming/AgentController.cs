using ColorProgramming.Core;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ColorProgramming
{
    public class AgentController : BaseNodeController
    {
        public float speed = 5f;

        public AgentNode AgentNode;

        [Header("Components")]
        [SerializeField]
        private Material bodyMaterial;

        public override Node Node
        {
            get => AgentNode;
            set => AgentNode = (AgentNode)value;
        }

        private ElementsData elementsData;

        private void Awake()
        {
            AgentNode.OnChangeElement = UpdatePlayerElement;
        }

        public void WalkGraph(List<BaseNodeController> nodes)
        {
            StartCoroutine(DoWalk(nodes));
        }

        IEnumerator DoWalk(List<BaseNodeController> nodes)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                BaseNodeController nodeController = nodes[i];
                Vector3 startPosition = transform.position;
                Vector3 targetPosition = nodeController.transform.position;
                float t = 0f;

                // Perform interpolation from the current position to the target position
                while (t < 1f)
                {
                    t += Time.deltaTime * speed;
                    transform.position = Vector3.Lerp(startPosition, targetPosition, t);
                    yield return null;
                }

                if (nodeController is IEvaluatable evaluatableController)
                {
                    evaluatableController.Evaluate(this);
                }
            }
        }

        private void OnValidate()
        {
            UpdatePlayerElement();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private void UpdatePlayerElement()
        {
            if (elementsData == null)
            {
                elementsData = ValidateResources.LoadElementsData();
            }

            bodyMaterial.SetColor("_BaseColor", elementsData[AgentNode.CurrentElement].Color);
        }
    }
}
