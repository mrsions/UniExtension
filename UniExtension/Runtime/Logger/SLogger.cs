#nullable enable

using UnityEngine;

namespace UniExtension
{
public static class SLogger
{
    public static bool IsDebug => Debug.isDebugBuild;
}
}
