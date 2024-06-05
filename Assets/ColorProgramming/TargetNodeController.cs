﻿using ColorProgramming;
using ColorProgramming.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

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

        private void Awake()
        {
            UpdateElementsMaterials();
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
                UpdateElementsMaterials();
            };
#endif
        }

        private void UpdateElementsMaterials()
        {
            if (elementsData == null)
            {
                elementsData = ValidateResources.LoadElementsData();
            }

            AccentMaterial.SetColor("_BaseColor", elementsData[TargetNode.CurrentElement].Color);

            var elementImage = elementTransform.GetComponent<Image>();
            elementImage.sprite = elementsData[TargetNode.CurrentElement].Sprite;
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
