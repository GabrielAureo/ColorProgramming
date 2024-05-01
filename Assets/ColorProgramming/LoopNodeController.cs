using ColorProgramming.Core;
using ColorProgramming;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ColorProgramming
{
    public class LoopNodeController : ConcreteNodeController<LoopNode>
    {
        [SerializeField]
        private GameObject batteryPrefab;

        [SerializeField]
        private Transform spawnedBatteries;

        public bool IsLoopClosed { get; private set; }

        protected override Dictionary<string, UnityAction> ActionSignalMap =>
            new()
            {
                { "connect", ConnectAction },
                { "disconnect", DisconnectAction },
                { "build-loop", StartBuildMode }
            };

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
            foreach (Transform child in spawnedBatteries)
            {
                StartCoroutine(DestroyBattery(child.gameObject));
            }
            SpawnBatteries();
#endif

        }

        IEnumerator DestroyBattery(GameObject go)
        {
            yield return new WaitForSeconds(0);
            DestroyImmediate(go);
        }

        public void SpawnBatteries()
        {
            for (var i = 0; i < ConcreteNode.TotalLoops; i++)
            {

                StartCoroutine(SpawnBattery(i));

            }
        }

        IEnumerator SpawnBattery(int i)
        {
            yield return new WaitForSeconds(0);
            var layoutGroup = spawnedBatteries.GetComponent<HorizontalLayoutGroup>();
            layoutGroup.childControlWidth = true;
            Instantiate(
                batteryPrefab,
                spawnedBatteries
            );
            yield return new WaitForSeconds(0);
            layoutGroup.childControlWidth = false;

        }

        private void StartBuildMode()
        {
            GameManager.Instance.BoardController.ToggleLoopBuildMode(ConcreteNode);
        }

        public override void OnAgentTouch(AgentController agent)
        {
            base.OnAgentTouch(agent);
            if (spawnedBatteries.childCount > 0)
            {

                StartCoroutine(DestroyBattery(spawnedBatteries.GetChild(0).gameObject));
            }

        }
    }
}
