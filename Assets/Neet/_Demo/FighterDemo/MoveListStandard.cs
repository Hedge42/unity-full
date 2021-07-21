using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neet.Fighter
{
    public class MoveListStandard
    {
        public Command[] commands;

        public MoveListStandard()
        {
            commands = new Command[]
            {
                new Command("DP", "6*,5?,2,3,6?p"),
                new Command("Grab", "pk"),
                new Command("Low parry", "g*k"),
                new Command("High parry", "g*p"),
                new Command("bfbfK", "147*,258?,369,258?,147,258?,369*k")
            };
        }
    }
}