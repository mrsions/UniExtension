#nullable enable

using UnityEngine;

namespace UniExtension
{
public static class Waits
{
    public static WaitForSeconds Wait(float duration)
    {
        return new WaitForSeconds(duration);
    }
}
}
