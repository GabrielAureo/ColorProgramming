using System.Collections;
using UnityEngine;

namespace ColorProgramming
{
    [ExecuteInEditMode]
    public class ValidateResources
    {
        public static ElementsData LoadElementsData()
        {
            return Resources.Load<ElementsData>("ElementsData");
        }
    }
}
