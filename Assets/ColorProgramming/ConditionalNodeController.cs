using ColorProgramming;
using ColorProgramming.Core;
using System.Collections;
using UnityEngine;

namespace Assets.ColorProgramming
{
    public class ConditionalNodeController : NodeController<ConditionalNode>, ITappable
    {
        public void OnTap()
        {
            BuildMenu(
                new Vector3(transform.position.x, transform.position.y + 3f, transform.position.z)
            );
        }
    }
}
