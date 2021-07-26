using System.Collections.Generic;

namespace Neet.Input
{
    public class ActionMap
    {
        private Dictionary<GameAction, GamepadControl> map;

        public ActionMap()
        {
            map = GetDefault();
        }

        public GamepadControl Get(GameAction a)
        {
            return map[a];
        }
        public void Set(GameAction a, GamepadControl c)
        {
            map[a] = c;
        }

        public Dictionary<GameAction, GamepadControl> GetDefault()
        {
            return new Dictionary<GameAction, GamepadControl>()
            {
                { GameAction.Jump, GamepadControl.BumperLeft },
                { GameAction.Interact, GamepadControl.FaceBottom },
            };
        }
    }
}
