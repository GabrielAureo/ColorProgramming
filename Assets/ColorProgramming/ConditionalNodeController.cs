using ColorProgramming;
using ColorProgramming.Core;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace ColorProgramming
{
    public class ConditionalNodeController : NodeController, ITappable
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
                        GameManager.Instance.TouchController.TouchServiceManager.RegisterService(
                            new NodeConnectService(this, true)
                        );
                case "disconnect":
                    return () =>
                        GameManager.Instance.TouchController.TouchServiceManager.RegisterService(
                            new NodeConnectService(this, true)
                        );
                default:
                    return () => { };
            }
        }
    }
}
