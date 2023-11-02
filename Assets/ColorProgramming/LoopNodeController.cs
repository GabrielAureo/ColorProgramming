using ColorProgramming.Core;
using ColorProgramming;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine.Events;

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
            if (
                PrefabStageUtility.GetPrefabStage(gameObject) != null
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
            for (var i = 0; i < ConcreteNode.TotalLoops; i++)
            {
                StartCoroutine(SpawnBattery(i));
            }
        }

        IEnumerator DestroyBattery(GameObject go)
        {
            yield return new WaitForSeconds(0);
            DestroyImmediate(go);
        }

        IEnumerator SpawnBattery(int i)
        {
            yield return new WaitForSeconds(0);
            Instantiate(
                batteryPrefab,
                spawnedBatteries.transform.position + Vector3.up * (.4f * i),
                batteryPrefab.transform.rotation,
                spawnedBatteries
            );
        }

        private void StartBuildMode()
        {
            GameManager.Instance.BoardController.ToggleLoopBuildMode(ConcreteNode);
        }
    }
}
