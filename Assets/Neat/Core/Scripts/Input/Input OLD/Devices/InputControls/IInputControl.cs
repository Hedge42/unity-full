using System;
using UnityEngine;

namespace Neat.InputManagement
{
    public interface IInputControl
    {
        bool GetButtonDown();
        bool GetButton();
        bool GetButtonUp();
        float GetAxis();
    }
}
