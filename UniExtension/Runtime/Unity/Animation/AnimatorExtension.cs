namespace UnityEngine
{
    public static class AnimatorExtension
    {
        public static bool IsPlaying(this Animator anim)
        {
            return anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1 || anim.IsInTransition(0);
        }
    }
}