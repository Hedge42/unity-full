using System.Collections.Generic;

namespace Neet.Input.DeviceMaps
{
    public interface IDeviceMap
    {
        Dictionary<GamepadControl, IInputControl> GetDefault(int index);

        IInputControl GetControl(GamepadControl c);
    }
}
