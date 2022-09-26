using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Profiling;
using UnityEngine.LowLevel;

namespace Debugger.Profiler
{
    public class PlayerProfiler : Profiler
    {
        private ProfilerRecorder mainThreadRecorder;

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

            PlayerLoopSubSystemAsText(playerLoop, ref sb);

            return sb.ToString();
        }

        private void PlayerLoopSubSystemAsText(PlayerLoopSystem system, ref StringBuilder playerLoopText)
        {
            if (system.subSystemList == null)
                return;

            foreach (PlayerLoopSystem subSystem in system.subSystemList)
            {
                playerLoopText.AppendLine($"\t{subSystem.ToString()}");
                PlayerLoopSubSystemAsText(subSystem, ref playerLoopText);
            }
        }
    }
}
