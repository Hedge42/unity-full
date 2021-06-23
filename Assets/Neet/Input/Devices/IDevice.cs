using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neet.Input
{
    interface IDevice
    {
        bool GetButton(GamepadControl g);
        bool GetButtonDown(GamepadControl g);
        bool GetButtonUp(GamepadControl g);
        float GetAxis(GamepadControl g);
    }
}
