using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Profiling;
using UnityEngine.LowLevel;
using UnityEngine.Profiling;

namespace Debugger.Profiler
{
    public class PlayerProfiler : Profiler
    {
        private ProfilerRecorder mainThreadRecorder;

        private List<int> parentsCacheList = new List<int>();
        private List<int> childrenCacheList = new List<int>();

        void OnEnable()
        {
            mainThreadRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Main Thread", 15);
        }

        private void OnDisable()
        {
            mainThreadRecorder.Dispose();
        }

        public override string GetInfo()
        {
            Sampler s = Sampler.Get("PlayerLoop");
            Recorder rec = s.GetRecorder();
            var sb = new StringBuilder();
            sb.AppendLine($"Frame Time: {GetRecorderAverage(mainThreadRecorder) * (1e-6f):F1} ms");
            //sb.AppendLine(GetPlayerLoop());
            return sb.ToString();
        }

        private double GetRecorderAverage(ProfilerRecorder recorder)
        {
            if (recorder.Capacity == 0)
                return 0;

            List<ProfilerRecorderSample> samples = new List<ProfilerRecorderSample>(recorder.Capacity);
            recorder.CopyTo(samples);

            double total = 0;
            foreach (ProfilerRecorderSample sample in samples)
            {
                total += sample.Value;
            }

            return total / recorder.Capacity;
        }

        private string GetPlayerLoop()
        {
            PlayerLoopSystem playerLoop = PlayerLoop.GetCurrentPlayerLoop();

            var sb = new StringBuilder();

            PlayerLoopSubSystemAsText(playerLoop, sb);

            return sb.ToString();
        }

        private void PlayerLoopSubSystemAsText(PlayerLoopSystem system, StringBuilder playerLoopText)
        {
            if (system.subSystemList == null)
                return;

            foreach (PlayerLoopSystem subSystem in system.subSystemList)
            {
                Sampler sampler = Sampler.Get(subSystem.type.FullName);
                playerLoopText.AppendLine($"\t{subSystem.type.Name}");
                PlayerLoopSubSystemAsText(subSystem, playerLoopText);
            }
        }
    }
}
