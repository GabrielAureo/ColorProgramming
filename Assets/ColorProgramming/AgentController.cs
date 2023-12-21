using ColorProgramming.Core;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace ColorProgramming
{
    public class AgentController : BaseNodeController
    {
        public float speed = 5f;

        public AgentNode AgentNode;

        [Header("Components")]

        [SerializeField]
        private Material bodyMaterial;

        private Animator animator;

        public override Node Node
        {
            get => AgentNode;
            set => AgentNode = (AgentNode)value;
        }

        private ElementsData elementsData;
        private void Awake()
        {
            AgentNode.OnChangeElement = UpdatePlayerElement;
            animator = GetComponent<Animator>();
        }

        public void WalkGraph(AgentPath path)
        {
            StartCoroutine(DoWalk(path, path.RootPath));
        }

        IEnumerator DoWalk(AgentPath path, List<BaseNodeController> rootPath)
        {
            animator.SetBool("walking", true);
            var nodes = rootPath;

            for (int i = 0; i < nodes.Count; i++)
            {

                if (i == 0)
                {
                    transform.position = nodes[0].transform.position;
                    yield return null;
                }
                BaseNodeController nodeController = nodes[i];
                Vector3 startPosition = transform.position;
                Vector3 targetPosition = nodeController.transform.position;
                float t = 0f;

                Quaternion targetRotation = Quaternion.LookRotation(transform.position - targetPosition, Vector3.up);

                // Apply the rotation to your character's transform
                transform.rotation = targetRotation;
                // Perform interpolation from the current position to the target position
                while (t < 1f)
                {
                    t += Time.deltaTime * speed;
                    transform.position = Vector3.Lerp(startPosition, targetPosition, t);
                    yield return null;
                }
                nodeController.OnAgentTouch(this);
                if (nodeController is IEvaluatable evaluatableController)
                {
                    evaluatableController.Evaluate(this);
                }

                if (nodeController is CapsuleNodeController capsuleController)
                {
                    yield return StartCoroutine(DoWalk(path, path.SubPaths[capsuleController.ConcreteNode]));
                    transform.position = nodes[i].transform.position;
                    animator.SetBool("walking", true);

                }
            }
            animator.SetBool("walking", false);
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
