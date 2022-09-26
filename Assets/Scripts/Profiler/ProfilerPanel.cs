using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Profiling;
using Unity.Profiling.LowLevel.Unsafe;
using System.Text;
using TMPro;
using UnityEngine.LowLevel;

namespace Debugger.Profiler
{
    public class ProfilerPanel : MonoBehaviour
    {
        [SerializeField] private Profiler defaultProfiler;
        [SerializeField] private TextMeshProUGUI statsText;

        Profiler currentProfiler;

        private void Awake()
        {
            currentProfiler = defaultProfiler;
        }

        public void ShowProfiler(Profiler profiler)
        {
            currentProfiler = profiler;
        }

        void Update()
        {
            statsText.text = currentProfiler.GetInfo();
        }
    }
}
