#nullable enable

using System;

namespace UniExtension.Pools
{
    internal static class SharedPoolStatics
    {
        internal static readonly int PartitionCount = GetPartitionCount();
        internal static readonly int CapacityPerPartition = GetMaxObjectPerPartition();

        private static int GetPartitionCount()
        {
            return Environment.ProcessorCount;
        }
        private static int GetMaxObjectPerPartition()
        {
            return 50;
        }
    }
}
