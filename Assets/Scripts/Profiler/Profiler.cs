using UnityEngine;
using UnityEngine.UI;

namespace Debugger.Profiler
{
    public abstract class Profiler : MonoBehaviour
    {
        public abstract string GetInfo();
    }
}
