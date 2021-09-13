using EcsRx.Unity.Framework;
using System.Collections;
using UnityEngine;

namespace BCKWorks.Framework.Libraries.Sample
{
    public class SampleInterface : ISampleInterface
    {
        public bool Ready { get; set; }

        public IEnumerator Initialize()
        {
            yield return null;

            Ready = true;
        }

        public void Shutdown()
        {
            Ready = false;
        }

        public void Print()
        {
            Debug.Log("Print Sample Interface in Sample Module");
        }
    }
}