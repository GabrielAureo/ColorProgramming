using ColorProgramming;
using ColorProgramming.Core;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace ColorProgramming
{
    public class ConditionalNodeController : NodeController<ConditionalNode>, ITappable
    {
        public void OnTap()
        {
            BuildMenu(
                new Vector3(transform.position.x, transform.position.y + 3f, transform.position.z)
            );
        }

        protected override UnityAction ParseActionSignal(string actionSignal)
        {
            switch (actionSignal)
            {
                case "connect":
                    return () =>
                        GameManager.Instance.TouchController.RegisterService(
                            new NodeConnectService(true)
                        );
                case "disconnect":
                    return () =>
                        GameManager.Instance.TouchController.RegisterService(
                            new NodeConnectService(true)
                        );
                default:
                    return () => { };
            }
        }
    }
}
