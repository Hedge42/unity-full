using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterCommand
{
    public string name;
    public int button;
    public string directions;

    public FighterCommand(string _name, int _button, string _dir)
    {
        name = _name;
        button = _button;
        directions = _dir;
    }
}
