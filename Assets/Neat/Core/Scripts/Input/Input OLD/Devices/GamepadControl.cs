using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neat.GameManager
{
    public enum GamepadControl
    {
        StickLeft2D,
        StickLeftX,
        StickLeftY,

        StickRight2D,
        StickRightX,
        StickRightY,

        StickLeftXLeft,
        StickLeftXRight,
        StickLeftYUp,
        StickLeftYDown,

        StickRightXLeft,
        StickRightXRight,
        StickRightYUp,
        StickRightYDown,

        StickRightClick,
        StickLeftClick,

        Dpad2D,
        DpadX,
        DpadY,
        DpadLeft,
        DpadRight,
        DpadUp,
        DpadDown,

        FaceBottom,
        FaceLeft,
        FaceRight,
        FaceTop,
        BumperRight,
        BumperLeft,
        TriggerRight,
        TriggerLeft,
        Start,
        Menu,
    }
}
