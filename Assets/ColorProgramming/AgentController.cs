using ColorProgramming.Core;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UI;
namespace ColorProgramming
{
    public class AgentController : BaseNodeController
    {
        public float speed = 5f;

        public AgentNode AgentNode;

        public Element InitialElement;

        [SerializeField]
        Transform elementTransform;

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


                BaseNodeController nodeController = nodes[i];
                Vector3 startPosition = transform.position;
                Vector3 targetPosition = nodeController.transform.position;
                float t = 0f;


                if (path.CurvedPaths.TryGetValue(i, out var points))
                {

                    for (int j = 0; j < points.Length - 1; j++)
                    {
                        Vector3 curveStart = points[j];
                        Vector3 curveEnd = points[j + 1];

                        Quaternion targetRotation = Quaternion.LookRotation(transform.position - curveEnd, Vector3.up);
                        transform.rotation = targetRotation;

                        t = 0f;
                        while (t < 1f)
                        {
                            t += Time.deltaTime * speed * 50f;
                            transform.position = Vector3.Lerp(curveStart, curveEnd, Mathf.SmoothStep(0f, 1f, t));
                            yield return null;
                        }
                        yield return null;

                    }

                }
                else
                {
                    Quaternion targetRotation = Quaternion.LookRotation(transform.position - targetPosition, Vector3.up);
                    transform.rotation = targetRotation;

                    while (t < 1f)
                    {
                        t += Time.deltaTime * speed;
                        transform.position = Vector3.Lerp(startPosition, targetPosition, t);
                        yield return null;
                    }
                }

                nodeController.OnAgentTouch(this);
                if (nodeController is IEvaluatable evaluatableController)
                {
                    evaluatableController.Evaluate(this);
                }


            }
            animator.SetBool("walking", false);
        }

        private void OnValidate()
        {
#if UNITY_EDITOR
            if (
                UnityEditor.SceneManagement.PrefabStageUtility.GetPrefabStage(gameObject) != null
                || EditorUtility.IsPersistent(this)
                || EditorApplication.isPlayingOrWillChangePlaymode
            )
            {
                return;
            }
            EditorApplication.delayCall += () =>
            {
                AgentNode.SetElement(InitialElement);
                UpdatePlayerElement();
            };

#endif
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private void UpdatePlayerElement()
        {
            if (elementsData == null)
            {
                elementsData = ValidateResources.LoadElementsData();
            }

            bodyMaterial.SetColor("_BaseColor", elementsData[AgentNode.CurrentElement].Color);

            var elementImage = elementTransform.GetComponent<Image>();
            elementImage.sprite = elementsData[AgentNode.CurrentElement].Sprite;

        }
    }
}
