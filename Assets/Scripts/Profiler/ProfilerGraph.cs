using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;

namespace Debugger.Profiler
{
    public class ProfilerGraph : Graphic
    {
        private ProfilerRecorder mainThreadRecorder;

        private int numberOfSamples = 100;
        private int maxMsToShow = 100;

        private Queue<float> frames = new Queue<float>();

        protected override void OnEnable()
        {
            base.OnEnable();
            mainThreadRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Main Thread", 15);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            mainThreadRecorder.Dispose();
        }

        private void Update()
        {
            if (!Application.isPlaying)
                return;

            SetVerticesDirty();    
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            if (!Application.isPlaying)
                return;

            float currentMs = GetRecorderAverage(mainThreadRecorder) * 1e-6f;
            float percent = currentMs / maxMsToShow;

            float sampleWidth = rectTransform.rect.width / numberOfSamples;

            float currentX = 0f;

            vh.Clear();

            if (frames.Count == numberOfSamples)
                frames.Dequeue();

            foreach (float frame in frames)
            {
                percent = frame / maxMsToShow;
                vh.AddUIVertexQuad(CreateVertexQuad(sampleWidth, percent, currentX));
                currentX += sampleWidth;
            }
            percent = currentMs / maxMsToShow;
            vh.AddUIVertexQuad(CreateVertexQuad(sampleWidth, percent, currentX));

            frames.Enqueue(currentMs);
        }

        UIVertex[] CreateVertexQuad(float sampleWwidth, double percent, float currentX)
        {
            UIVertex[] vertices = new UIVertex[4];
            vertices[0] = UIVertex.simpleVert;
            vertices[0].position = new Vector3(rectTransform.rect.x + currentX, rectTransform.rect.y, 0f);
            vertices[1] = UIVertex.simpleVert;
            vertices[1].position = new Vector3(rectTransform.rect.x + currentX, rectTransform.rect.y + rectTransform.rect.height * (float)percent, 0f);
            vertices[2] = UIVertex.simpleVert;
            vertices[2].position = new Vector3(rectTransform.rect.x + currentX + sampleWwidth, rectTransform.rect.y + rectTransform.rect.height * (float)percent, 0f);
            vertices[3] = UIVertex.simpleVert;
            vertices[3].position = new Vector3(rectTransform.rect.x + currentX + sampleWwidth, rectTransform.rect.y, 0f);

            return vertices;
        }

        private float GetRecorderAverage(ProfilerRecorder recorder)
        {
            if (recorder.Capacity == 0)
                return 0;

            List<ProfilerRecorderSample> samples = new List<ProfilerRecorderSample>(recorder.Capacity);
            recorder.CopyTo(samples);

            float total = 0;
            foreach (ProfilerRecorderSample sample in samples)
            {
                total += sample.Value;
            }

            return total / recorder.Capacity;
        }
    }
}
