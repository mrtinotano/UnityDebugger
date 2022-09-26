using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;

namespace Debugger.Profiler
{
    public class RenderingProfiler : Profiler
    {
        private ProfilerRecorder setPassRecorder;
        private ProfilerRecorder drawCallsRecorder;
        private ProfilerRecorder toalBatchesRecorder;
        private ProfilerRecorder trianglesRecorder;
        private ProfilerRecorder verticesRecorder;

        private ProfilerRecorder dynamicDrawCallsRecorder;
        private ProfilerRecorder dynamicBatchesRecorder;
        private ProfilerRecorder dynamicTrianglesRecorder;
        private ProfilerRecorder dynamicVerticesRecorder;
        private ProfilerRecorder dynamicTimeRecorder;

        private ProfilerRecorder staticDrawCallsRecorder;
        private ProfilerRecorder staticBatchesRecorder;
        private ProfilerRecorder staticTrianglesRecorder;
        private ProfilerRecorder staticVerticesRecorder;

        private ProfilerRecorder instancingDrawCallsRecorder;
        private ProfilerRecorder instancingBatchesRecorder;
        private ProfilerRecorder instancingTrianglesRecorder;
        private ProfilerRecorder instancingVerticesRecorder;

        private ProfilerRecorder usedTexturesRecorder;
        private ProfilerRecorder usedTexturesBytesRecorder;
        private ProfilerRecorder renderTexturesRecorder;
        private ProfilerRecorder renderTexturesBytesRecorder;
        private ProfilerRecorder renderTexturesChangesRecorder;
        private ProfilerRecorder usedBuffersRecorder;
        private ProfilerRecorder usedBuffersBytesRecorder;
        private ProfilerRecorder vertexBufferUploadInFrameRecorder;
        private ProfilerRecorder vertexBufferUploadInFrameBytesRecorder;
        private ProfilerRecorder indexBufferUploadInFrameRecorder;
        private ProfilerRecorder indexBufferUploadInFrameBytesRecorder;
        private ProfilerRecorder shadowCastersRecorder;


        void OnEnable()
        {
            setPassRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "SetPass Calls Count");
            drawCallsRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Draw Calls Count");
            toalBatchesRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Total Batches Count");
            trianglesRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Triangles Count");
            verticesRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Vertices Count");
#if DEBUG
            dynamicDrawCallsRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Dynamic Batched Draw Calls Count");
            dynamicBatchesRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Dynamic Batches Count");
            dynamicTrianglesRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Dynamic Batched Triangles Count");
            dynamicVerticesRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Dynamic Batched Vertices Count");
            dynamicTimeRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Dynamic Batching Time");

            staticDrawCallsRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Static Batched Draw Calls Count");
            staticBatchesRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Static Batches Count");
            staticTrianglesRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Static Batched Triangles Count");
            staticVerticesRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Static Batched Vertices Count");

            instancingDrawCallsRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Instanced Batched Draw Calls Count");
            instancingBatchesRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Instanced Batches Count");
            instancingTrianglesRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Instanced Batched Triangles Count");
            instancingVerticesRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Instanced Batched Vertices Count");

            usedTexturesRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Used Textures Count");
            usedTexturesBytesRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Used Textures Bytes");
#endif
            renderTexturesRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Render Textures Count");
            renderTexturesBytesRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Render Textures Bytes");
            renderTexturesChangesRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Render Textures Changes Count");
            usedBuffersRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Used Buffers Count");
            usedBuffersBytesRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Used Buffers Bytes");
            vertexBufferUploadInFrameRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Vertex Buffer Upload In Frame Count");
            vertexBufferUploadInFrameBytesRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Vertex Buffer Upload In Frame Bytes");
            indexBufferUploadInFrameRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Index Buffer Upload In Frame Count");
            indexBufferUploadInFrameBytesRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Index Buffer Upload In Frame Bytes");
            shadowCastersRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Shadow Casters Count");
        }

        void OnDisable()
        {
            setPassRecorder.Dispose();
            drawCallsRecorder.Dispose();
            toalBatchesRecorder.Dispose();
            trianglesRecorder.Dispose();
            verticesRecorder.Dispose();
#if DEBUG
            dynamicDrawCallsRecorder.Dispose();
            dynamicBatchesRecorder.Dispose();
            dynamicTrianglesRecorder.Dispose();
            dynamicVerticesRecorder.Dispose();
            dynamicTimeRecorder.Dispose();

            staticDrawCallsRecorder.Dispose();
            staticBatchesRecorder.Dispose();
            staticTrianglesRecorder.Dispose();
            staticVerticesRecorder.Dispose();

            instancingDrawCallsRecorder.Dispose();
            instancingBatchesRecorder.Dispose();
            instancingTrianglesRecorder.Dispose();
            instancingVerticesRecorder.Dispose();

            usedTexturesRecorder.Dispose();
            usedTexturesBytesRecorder.Dispose();
#endif
            renderTexturesRecorder.Dispose();
            renderTexturesBytesRecorder.Dispose();
            renderTexturesChangesRecorder.Dispose();
            usedBuffersRecorder.Dispose();
            usedBuffersBytesRecorder.Dispose();
            vertexBufferUploadInFrameRecorder.Dispose();
            vertexBufferUploadInFrameBytesRecorder.Dispose();
            indexBufferUploadInFrameRecorder.Dispose();
            indexBufferUploadInFrameBytesRecorder.Dispose();
            shadowCastersRecorder.Dispose();
        }

        public override string GetInfo()
        {
            var sb = new StringBuilder();
            sb.Append($"SetPass Calls: {setPassRecorder.LastValue} / ");
            sb.Append($"Draw Calls: {drawCallsRecorder.LastValue} / ");
            sb.Append($"Total Batches: {toalBatchesRecorder.LastValue} / ");
            sb.Append($"Triangles: {trianglesRecorder.LastValue} / ");
            sb.AppendLine($"Vertices: {verticesRecorder.LastValue}");
#if DEBUG
            sb.AppendLine("Dynamic Batching: ");
            sb.Append($"\tDraw Calls: {dynamicDrawCallsRecorder.LastValue} / ");
            sb.Append($"Total Batches: {dynamicBatchesRecorder.LastValue} / ");
            sb.Append($"Triangles: {dynamicTrianglesRecorder.LastValue} / ");
            sb.Append($"Vertices: {dynamicVerticesRecorder.LastValue} / ");
            sb.AppendLine($"Time: {dynamicTimeRecorder.LastValue}");

            sb.AppendLine("Static Batching: ");
            sb.Append($"\tDraw Calls: {staticDrawCallsRecorder.LastValue} / ");
            sb.Append($"Total Batches: {staticBatchesRecorder.LastValue} / ");
            sb.Append($"Triangles: {staticTrianglesRecorder.LastValue} / ");
            sb.AppendLine($"Vertices: {staticVerticesRecorder.LastValue}");

            sb.AppendLine("Instancing Batching: ");
            sb.Append($"\tDraw Calls: {instancingDrawCallsRecorder.LastValue} / ");
            sb.Append($"Total Batches: {instancingBatchesRecorder.LastValue} / ");
            sb.Append($"Triangles: {instancingTrianglesRecorder.LastValue} / ");
            sb.AppendLine($"Vertices: {instancingVerticesRecorder.LastValue} / ");
            
            sb.AppendLine($"Used Textures: {usedTexturesRecorder.LastValue} / {usedTexturesBytesRecorder.LastValue / (1024 * 1024)} MB");
#endif
            sb.AppendLine($"Render Textures: {renderTexturesRecorder.LastValue} / {renderTexturesBytesRecorder.LastValue / (1024 * 1024)} MB");
            sb.AppendLine($"Render Textures Changes: {renderTexturesChangesRecorder.LastValue}");
            sb.AppendLine($"Buffers Total: {usedBuffersRecorder.LastValue} / {usedBuffersBytesRecorder.LastValue / (1024 * 1024)} MB");
            sb.AppendLine($"Vertex Buffers Upload: {vertexBufferUploadInFrameRecorder.LastValue} / {vertexBufferUploadInFrameBytesRecorder.LastValue / (1024 * 1024)} MB");
            sb.AppendLine($"Index Buffers Upload: {indexBufferUploadInFrameRecorder.LastValue} / {indexBufferUploadInFrameBytesRecorder.LastValue / (1024 * 1024)} MB");
            sb.Append($"Shadow Casters: {shadowCastersRecorder.LastValue}");
            return sb.ToString();
        }
    }
}

