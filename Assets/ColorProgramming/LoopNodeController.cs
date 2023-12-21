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
            SpawnBatteries();

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
            var battery = Instantiate(
                batteryPrefab,
                spawnedBatteries.transform.position + Vector3.up * (.4f * i),
                Quaternion.identity,
                spawnedBatteries
            );

            battery.transform.localRotation = Quaternion.Euler(Vector3.left * 90f);

        }

        private void StartBuildMode()
        {
            GameManager.Instance.BoardController.ToggleLoopBuildMode(ConcreteNode);
        }
    }
}
