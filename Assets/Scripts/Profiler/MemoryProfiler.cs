using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Profiling;
using UnityEngine;

namespace Debugger.Profiler
{
    public class MemoryProfiler : Profiler
    {
        private ProfilerRecorder totalUsedMemoryRecorder;
        private ProfilerRecorder totalReservedMemoryRecorder;
        private ProfilerRecorder gCUsedMemoryRecorder;
        private ProfilerRecorder gCReservedMemoryRecorder;
        private ProfilerRecorder gfxUsedMemoryRecorder;
        private ProfilerRecorder gfxReservedMemoryRecorder;
        private ProfilerRecorder audioUsedMemoryRecorder;
        private ProfilerRecorder audioReservedMemoryRecorder;
        private ProfilerRecorder videoUsedMemoryRecorder;
        private ProfilerRecorder videoReservedMemoryRecorder;
        private ProfilerRecorder profilerUsedMemoryRecorder;
        private ProfilerRecorder profilerReservedMemoryRecorder;
        private ProfilerRecorder systemUsedMemoryRecorder;
#if DEBUG
        private ProfilerRecorder textureRecorder;
        private ProfilerRecorder textureMemoryRecorder;
        private ProfilerRecorder meshRecorder;
        private ProfilerRecorder meshMemoryRecorder;
        private ProfilerRecorder materialRecorder;
        private ProfilerRecorder materialMemoryRecorder;
        private ProfilerRecorder animationClipRecorder;
        private ProfilerRecorder animationClipMemoryRecorder;
        private ProfilerRecorder assetRecorder;
        private ProfilerRecorder gameObjectRecorder;
        private ProfilerRecorder sceneRecorder;
        private ProfilerRecorder objectRecorder;
        private ProfilerRecorder gCAllocationRecorder;
        private ProfilerRecorder gCAllocatedRecorder;
#endif

        void OnEnable()
        {
            totalUsedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Total Used Memory");
            totalReservedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Total Reserved Memory");
            gCUsedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Used Memory");
            gCReservedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Reserved Memory");
            gfxUsedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Gfx Used Memory");
            gfxReservedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Gfx Reserved Memory");
            audioUsedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Audio Used Memory");
            audioReservedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Audio Reserved Memory");
            videoUsedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Video Used Memory");
            videoReservedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Video Reserved Memory");
            profilerUsedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Profiler Used Memory");
            profilerReservedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Profiler Reserved Memory");
            systemUsedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "System Used Memory");
#if DEBUG
            textureRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Texture Count");
            textureMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Texture Memory");
            meshRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, " Mesh Count");
            meshMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Mesh Memory");
            materialRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Material Count");
            materialMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Material Memory");
            animationClipRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "AnimationClip Count");
            animationClipMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "AnimationClip Memory");
            assetRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Asset Count");
            gameObjectRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GameObject Count");
            sceneRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Scene Object Count");
            objectRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Object Count");
            gCAllocationRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Allocation In Frame Count");
            gCAllocatedRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Allocated In Frame");
#endif
        }

        void OnDisable()
        {
            totalUsedMemoryRecorder.Dispose();
            totalReservedMemoryRecorder.Dispose();
            gCUsedMemoryRecorder.Dispose();
            gCReservedMemoryRecorder.Dispose();
            gfxUsedMemoryRecorder.Dispose();
            gfxReservedMemoryRecorder.Dispose();
            audioUsedMemoryRecorder.Dispose();
            audioReservedMemoryRecorder.Dispose();
            videoUsedMemoryRecorder.Dispose();
            videoReservedMemoryRecorder.Dispose();
            profilerUsedMemoryRecorder.Dispose();
            profilerReservedMemoryRecorder.Dispose();
            systemUsedMemoryRecorder.Dispose();
#if DEBUG
            textureRecorder.Dispose();
            textureMemoryRecorder.Dispose();
            meshRecorder.Dispose();
            meshMemoryRecorder.Dispose();
            materialRecorder.Dispose();
            materialMemoryRecorder.Dispose();
            animationClipRecorder.Dispose();
            animationClipMemoryRecorder.Dispose();
            assetRecorder.Dispose();
            gameObjectRecorder.Dispose();
            sceneRecorder.Dispose();
            objectRecorder.Dispose();
            gCAllocationRecorder.Dispose();
            gCAllocatedRecorder.Dispose();
#endif
        }

        public override string GetInfo()
        {
            var sb = new StringBuilder();
            sb.Append($"Total Used Memory: {totalUsedMemoryRecorder.LastValue / (1024 * 1024)} MB  / ");
            sb.Append($"GC: {gCUsedMemoryRecorder.LastValue / (1024 * 1024)} MB / ");
            sb.Append($"GFX: {gfxUsedMemoryRecorder.LastValue / (1024 * 1024)} MB / ");
            sb.Append($"Audio: {audioUsedMemoryRecorder.LastValue / (1024 * 1024)} MB / ");
            sb.Append($"Video: {videoUsedMemoryRecorder.LastValue / (1024 * 1024)} MB / ");
            sb.AppendLine($"Profiler: {profilerUsedMemoryRecorder.LastValue / (1024 * 1024)} MB");

            sb.Append($"Total Reserved Memory: {totalReservedMemoryRecorder.LastValue / (1024 * 1024)} MB / ");
            sb.Append($"GC: {gCReservedMemoryRecorder.LastValue / (1024 * 1024)} MB / ");
            sb.Append($"GFX: {gfxReservedMemoryRecorder.LastValue / (1024 * 1024)} MB / ");
            sb.Append($"Audio: {audioReservedMemoryRecorder.LastValue / (1024 * 1024)} MB / ");
            sb.Append($"Video: {videoReservedMemoryRecorder.LastValue / (1024 * 1024)} MB / ");
            sb.AppendLine($"Profiler: {profilerReservedMemoryRecorder.LastValue / (1024 * 1024)} MB");
            
            sb.AppendLine($"System: {systemUsedMemoryRecorder.LastValue / (1024 * 1024)} MB");
#if DEBUG
            sb.AppendLine();
            
            sb.AppendLine($"Textures: {textureRecorder.LastValue} / {textureMemoryRecorder.LastValue / (1024 * 1024)} MB");
            sb.AppendLine($"Meshes: {meshRecorder.LastValue} / {meshMemoryRecorder.LastValue / (1024 * 1024)} MB");
            sb.AppendLine($"Materials: {materialRecorder.LastValue} / {materialMemoryRecorder.LastValue / (1024 * 1024)} MB");
            sb.AppendLine($"AnimationClips: {animationClipRecorder.LastValue} / {animationClipMemoryRecorder.LastValue / (1024 * 1024)} MB");
            sb.AppendLine($"Asset Count: {assetRecorder.LastValue}");
            sb.AppendLine($"Game Object Count: {gameObjectRecorder.LastValue}");
            sb.AppendLine($"Scene Object Count: {sceneRecorder.LastValue}");
            sb.AppendLine($"Object Count: {objectRecorder.LastValue}");
            sb.AppendLine();

            sb.AppendLine($"GC Allocation In Frame: {gCAllocationRecorder.LastValue} / {gCAllocatedRecorder.LastValue / (1024 * 1024)} MB");
#endif
            return sb.ToString();
        }
    }
}
