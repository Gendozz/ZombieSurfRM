using System.Collections;
using System.Collections.Generic;
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

        // Integers
        public static readonly int SIDEMOVE_INT = Animator.StringToHash("SideMove");

        // Unused
        public static readonly int MOVELEFT_TRIG = Animator.StringToHash("MoveLeft");
        public static readonly int MOVERIGHT_TRIG = Animator.StringToHash("MoveRight");
    }
}
