#nullable enable

using UnityEngine;
using UnityEngine.Profiling;

namespace UniExtension
{
    public enum MemoryPressure
    {
        Low,
        Medium,
        High
    }

    public static class Settings
    {
    private const double MediumThreshold = 0.60;
    private const double HighThreshold = 0.80;
    private const long MobileMemoryCapBytes = 1610612736L;

    public static MemoryPressure GetMemoryPressure()
    {
        long totalMemoryBytes = (long)SystemInfo.systemMemorySize * 1024L * 1024L;
        if (Application.isMobilePlatform && totalMemoryBytes > 0)
        {
            totalMemoryBytes = totalMemoryBytes < MobileMemoryCapBytes ? totalMemoryBytes : MobileMemoryCapBytes;
        }
        if (totalMemoryBytes <= 0)
        {
            return MemoryPressure.Low;
        }

        long allocatedBytes = Profiler.GetTotalAllocatedMemoryLong();
        double ratio = (double)allocatedBytes / totalMemoryBytes;

        if (ratio >= HighThreshold) return MemoryPressure.High;
        if (ratio >= MediumThreshold) return MemoryPressure.Medium;
        return MemoryPressure.Low;
    }
    }
}
