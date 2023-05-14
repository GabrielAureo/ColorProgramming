using ColorProgramming;
using ColorProgramming.Core;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace ColorProgramming
{
    public class ConditionalNodeController : NodeController
    {
        protected override UnityAction ParseActionSignal(string actionSignal)
        {
            return actionSignal switch
            {
                "connect"
                    => () =>
                        GameManager.Instance.TouchController.TouchServiceManager.RegisterService(
                            new NodeConnectService(this, true)
                        ),
                "disconnect"
                    => () =>
                        GameManager.Instance.TouchController.TouchServiceManager.RegisterService(
                            new NodeConnectService(this, true)
                        ),
                _ => () => { },
            };
        }
    }
}
