using UnityEngine;
using UnityEngine.Events;

public class MovableController : MonoBehaviour{
    public Movable currentMovable;
    public bool isHolding;
    HingeJoint hinge;
    public Transform previewer;

    private BaseSocket currentTargetSocket;

    private void Awake(){
        hinge = GetComponent<HingeJoint>();
        var touchController = GetComponent<ARTouchController>();
        SetupController(touchController);
    }

    
    public void SetupController(ARTouchController touchController){
        isHolding = false;
        isTargeting = false;
        //touchController.onTouch.AddListener(Touch);
        touchController.OnHold.AddListener(Grab);
        touchController.OnRelease.AddListener(Release);
        previewer = new GameObject("Previewer", typeof(MeshFilter), typeof(MeshRenderer)).transform;
    }   

    private void Grab(ARTouchData touchData){
        if(!(touchData.selectedInteractable is BaseSocket)) return;
        var socket = touchData.selectedInteractable as BaseSocket;
        isHolding = true;

        var takenMovable = socket.TryTake(touchData);
        if (takenMovable)
        {
            currentMovable = takenMovable;
            ConnectToHinge(takenMovable);
        }

    }

    private void Update()
    {
        CheckTarget(ARTouchController.touchData);
    }

    private bool isTargeting;    
    void CheckTarget(ARTouchData touchData)
    {
        if (!isHolding) return;
        RaycastHit[] hits = new RaycastHit[10];
        var hitSize = Physics.RaycastNonAlloc(touchData.ray, hits);
        if (hitSize <= 0)
        {

             for (var i = 0; i < hitSize; i++)
            {
                var hit = hits[i];
                var isSocket = hit.transform.GetComponent<BaseSocket>();
                if (currentTargetSocket != null)
                {
                    isTargeting = true;
                    break;
                }
            }

        }
    
    }

    // private void SetPreviewer(MovablePlacementPose pose)
    // {
    //     previewer.position = pose.position;
    //     previewer.rotation = pose.rotation;
    //     previewer.localScale = pose.scale;

    //     var filter = previewer.GetComponent<MeshFilter>();
    //     filter.sharedMesh = currentMovable.mesh;

    //     previewer.gameObject.SetActive(true);
        
    // }

    void Release(ARTouchData touchData)
    {
        if (touchData.lastStatus != ARTouchData.Status.HOLDING) return;


        if(!(touchData.selectedInteractable is BaseSocket)) return;
        BaseSocket lastSocket = (BaseSocket)touchData.selectedInteractable;

        BaseSocket target = null;

        RaycastHit[] hits;
        hits = Physics.RaycastAll(touchData.ray);

        if(hits.Length > 0){
            foreach(var hit in hits){
                target = hit.transform.GetComponent<BaseSocket>();
                if(target != null) break;
            }
        }

        if (target == null) return;

        var movable = lastSocket.GetMovable(touchData);
        var didPlace =  target.TryPlaceObject(touchData, movable);

        if (didPlace)
        {
            hinge.connectedBody = null;
            movable.rigidBody.isKinematic = true;
        }
        isHolding = false;

    }

    public void ConnectToHinge(Movable movable){
        //currentMovable = movable;
        movable.transform.parent = null;
        movable.rigidBody.isKinematic = false;
        hinge.connectedBody = movable.rigidBody;
    }





}