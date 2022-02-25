using UnityEngine;

public static class Constants
{
    public static class AnimationParameters
    {
        // Triggers
        public static readonly int JUMP_TRIG = Animator.StringToHash("Jump");
        public static readonly int LANDED_TRIG = Animator.StringToHash("Landed");
        public static readonly int SLIDE_TRIG = Animator.StringToHash("Slide");
        public static readonly int LANECHANGED_TRIG = Animator.StringToHash("LaneChanged");
        public static readonly int DEATH_TRIG = Animator.StringToHash("Death");
        public static readonly int SIDEHIT_TRIG = Animator.StringToHash("SideHit");

        // Integers
        public static readonly int SIDEMOVE_INT = Animator.StringToHash("SideMove");

        // Unused
        public static readonly int MOVELEFT_TRIG = Animator.StringToHash("MoveLeft");
        public static readonly int MOVERIGHT_TRIG = Animator.StringToHash("MoveRight");
    }

    public static class Tags
    {
        public static readonly int COIN_TAG = "Coin".GetHashCode();
        public static readonly int SIDEHIT_TAG = "Sidehit".GetHashCode();
        public static readonly int DEATH_TAG = "Death".GetHashCode();
        public static readonly int TRAP_TAG = "Trap".GetHashCode();
    }
}
