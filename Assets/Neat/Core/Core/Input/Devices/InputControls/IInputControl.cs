using System;
using UnityEngine;

namespace Neat.Input
{
    public interface IInputControl
    {
        bool GetButtonDown();
        bool GetButton();
        bool GetButtonUp();
        float GetAxis();
    }
}
