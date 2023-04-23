using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;
using System;
using Assets.ColorProgramming;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;
using System.Linq;

namespace ColorProgramming
{
    public class ARTouchController : MonoBehaviour
    {
        public float timer;
        public float holdThreshold = 0.2f;

        [HideInInspector]
        public Ray ray;

        public static ARTouchData touchData;

        public UnityEvent<ARTouchData> OnTouch;
        public UnityEvent<ARTouchData> OnHold;
        public UnityEvent<ARTouchData> OnRelease;

        private IEnumerable<IARInteractable> hitInteractables;

        void Awake()
        {
            touchData = new ARTouchData();
            touchData.currentStatus = ARTouchData.Status.NO_TOUCH;
        }

        private bool IsOverUI()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
    var position = Input.GetTouch(0).position;
#else
            var position = Input.mousePosition;
#endif
            var eventDataCurrentPosition = new PointerEventData(EventSystem.current)
            {
                position = position
            };
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }

        void Update()
        {
            HandleInput();
        }

        public void HandleInput()
        {
            if (IsOverUI())
                return;

#if UNITY_ANDROID && !UNITY_EDITOR
            
            if(Input.touchCount > 0){
                ray = CameraRay();
                touchData.ray = ray;
                InputStateMachine();

                if(Input.touches[0].phase == TouchPhase.Ended){
                    Release();
                }
            }
            return;
#endif

            ray = CameraRay();
            touchData.ray = ray;
            var input = Input.GetMouseButton(0);
            if (input)
            {
                InputStateMachine();
            }
            if (Input.GetMouseButtonUp(0))
            {
                Release();
            }
        }

        private void InputStateMachine()
        {
            RaycastHit[] hits;
            if (touchData.currentStatus == ARTouchData.Status.NO_TOUCH)
            {
                hits = Physics.RaycastAll(
                    ray,
                    Mathf.Infinity,
                    1 << LayerMask.NameToLayer("Default")
                );
                if (hits.Length > 0)
                {
                    hitInteractables = hits.Select(
                            hit => hit.transform.GetComponent<IARInteractable>()
                        )
                        .Where(interactable => interactable != null)
                        .ToArray();

                    OnTouch.Invoke(touchData);
                }
                ChangeStatus(ARTouchData.Status.WAITING);
            }
            if (touchData.currentStatus == ARTouchData.Status.HOLDING)
            {
                if (touchData.lastStatus == ARTouchData.Status.WAITING)
                {
                    try
                    {
                        var holdable = (IHoldable)
                            hitInteractables.FirstOrDefault((hit) => hit is IHoldable);
                        if (holdable != null)
                        {
                            holdable.OnHold();
                            touchData.selectedInteractable = holdable;
                        }
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError(
                            "ARTouchController: "
                                + e.GetType().ToString()
                                + " caught on "
                                + touchData.selectedInteractable?.ToString()
                                + " Hold Event"
                        );
                    }
                    OnHold.Invoke(touchData);
                    ChangeStatus(ARTouchData.Status.HOLDING);
                }
            }

            if (touchData.currentStatus == ARTouchData.Status.WAITING)
            {
                timer += Time.deltaTime;
                if (timer >= holdThreshold)
                {
                    ChangeStatus(ARTouchData.Status.HOLDING);
                }
            }
        }

        private void Release()
        {
            timer = 0.0f;

            RaycastHit[] hits;
            hits = Physics.RaycastAll(
                touchData.ray,
                Mathf.Infinity,
                1 << LayerMask.NameToLayer("Default")
            );

            if (hits.Length > 0)
            {
                IARInteractable selectedInteractable = null;
                foreach (var hit in hits)
                {
                    selectedInteractable = hit.transform.GetComponent<IARInteractable>();
                    if (selectedInteractable != null)
                    {
                        touchData.hit = hit;
                        break;
                    }
                }
            }

            OnRelease.Invoke(touchData);

            if (touchData.currentStatus == ARTouchData.Status.WAITING)
            {
                try
                {
                    //touchData.selectedInteractable?.OnTap();
                }
                catch (Exception e)
                {
                    Debug.LogError(
                        "ARTouchController: "
                            + e.GetType().ToString()
                            + " caught on "
                            + touchData.selectedInteractable?.ToString()
                            + " Tap Event"
                    );
                }
            }

            //Catch Exceptions so the controller doesn't get stuck in the Holding or Waiting status
            try
            {
                //touchData.selectedInteractable?.OnRelease();
            }
            catch (Exception e)
            {
                Debug.LogError(
                    "ARTouchController: "
                        + e.GetType().ToString()
                        + " caught on "
                        + touchData.selectedInteractable?.ToString()
                        + " Release Event"
                );
            }

            touchData.selectedInteractable = null;
            ChangeStatus(ARTouchData.Status.NO_TOUCH);
        }

        private void ChangeStatus(ARTouchData.Status newStatus)
        {
            touchData.lastStatus = touchData.currentStatus;
            touchData.currentStatus = newStatus;
        }

        private Ray CameraRay()
        {
            Vector2 inputPosition = Input.mousePosition;
#if UNITY_ANDROID && !UNITY_EDITOR
            inputPosition = Input.touches[0].position;
#endif

            var wrldPos = Camera.main.ScreenToWorldPoint(
                new Vector3(inputPosition.x, inputPosition.y, 1.35f)
            );

            transform.position = new Vector3(wrldPos.x, wrldPos.y, wrldPos.z);
            Ray ray = Camera.main.ScreenPointToRay(inputPosition);
            return ray;
        }

        void OnDrawGizmos()
        {
            if (touchData == null)
                return;
            Gizmos.DrawLine(
                touchData.ray.origin,
                touchData.ray.origin + touchData.ray.direction * 1000f
            );
            Gizmos.DrawWireSphere(touchData.hit.point, 5f);
        }
    }
}
