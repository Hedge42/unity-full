using System;
using UnityEngine;

namespace Neet.Input
{
    public interface IInputControl
    {
        bool GetButtonDown();
        bool GetButton();
        bool GetButtonUp();
        float GetAxis();
    }
}
