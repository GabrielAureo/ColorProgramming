/*==============================================================================
Copyright (c) 2021 PTC Inc. All Rights Reserved.

Vuforia is a trademark of PTC Inc., registered in the United States and other
countries.
==============================================================================*/

using TMPro;
using UnityEngine;
using Vuforia;

public class DevicePoseUI : MonoBehaviour
{
    public bool isTracked { get; private set; }
    public TextMeshProUGUI textUGUI;


    void Start()
    {
        VuforiaBehaviour.Instance.DevicePoseBehaviour.OnTargetStatusChanged += OnTargetStatusChanged;
    }

    void OnDestroy()
    {
        if (VuforiaBehaviour.Instance != null)
            VuforiaBehaviour.Instance.DevicePoseBehaviour.OnTargetStatusChanged -= OnTargetStatusChanged;
    }

    public void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus targetStatus)
    {
        isTracked = false;
        var statusMessage = "";

        switch (targetStatus.StatusInfo)
        {
            case StatusInfo.NORMAL:
                statusMessage = "";
                isTracked = true;
                break;
            case StatusInfo.UNKNOWN:
                statusMessage = "Status Limitado";
                break;
            case StatusInfo.INITIALIZING:
                statusMessage = "Mova-se para escanear";
                break;
            case StatusInfo.EXCESSIVE_MOTION:
                statusMessage = "Mova-se mais devagar";
                break;
            case StatusInfo.INSUFFICIENT_FEATURES:
            case StatusInfo.INSUFFICIENT_LIGHT:
                statusMessage = "Ambiente escuro demais";
                break;
            case StatusInfo.RELOCALIZING:
                // Display a relocalization message in the UI if:
                // * No AnchorBehaviours are being tracked
                // * None of the active/tracked AnchorBehaviours are in TRACKED status

                // Set the status message now and clear it if none of the conditions are met.
                statusMessage = "Move-se de volta para área anterior e reescaneie para relocalizar";

                var activeAnchors = FindObjectsOfType<AnchorBehaviour>();
                // Cycle through all of the active AnchorBehaviours.
                foreach (var anchor in activeAnchors)
                {
                    if (anchor.TargetStatus.Status == Status.TRACKED)
                    {
                        // If at least one of the AnchorBehaviours has Tracked status,
                        // then don't display the relocalization message.
                        isTracked = true;
                        statusMessage = "";
                        break;
                    }
                }
                break;
        }

        textUGUI.text = statusMessage;
    }
}
