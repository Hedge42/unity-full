using System;
using UnityEngine;

namespace Neat.GameManager
{
    public interface IInputControl
    {
        bool GetButtonDown();
        bool GetButton();
        bool GetButtonUp();
        float GetAxis();
    }
}
