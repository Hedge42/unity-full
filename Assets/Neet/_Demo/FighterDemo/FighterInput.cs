using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterInput : MonoBehaviour
{


    private const int dirBuffer = 60;
    private const int btnBuffer = 5;

    public string dirInput;
    private List<int> btnInput;

    private FighterCommand[] cmdList;

    private void Start()
    {
        Time.fixedDeltaTime = 1f / 60f;

        dirInput = "";
        btnInput = new List<int>();
        for (int i = 0; i < dirBuffer; i++)
        {
            dirInput += "5";
            btnInput.Add(0);
        }
        CreateList();
    }

    private void FixedUpdate()
    {
        HandleInput();

        ParseCommandList();
    }

    private void HandleInput()
    {
        Vector2 vec = GetDirectionalVector();
        string str = GetDirectionString(vec);
        UpdateDirInput(str);
        UpdateButtonInput();
    }

    // directional
    private void UpdateDirInput(string dir)
    {
        dirInput = dirInput.Remove(0, 1) + dir;
    }
    private Vector2 GetDirectionalVector()
    {
        Vector2 input = Vector2.zero;

        if (Input.GetKey(KeyCode.Space))
            input.y += 1;
        if (Input.GetKey(KeyCode.S))
            input.y -= 1;
        if (Input.GetKey(KeyCode.D))
            input.x += 1;
        if (Input.GetKey(KeyCode.A))
            input.x -= 1;

        return input;
    }
    private string GetDirectionString(Vector3 dir)
    {
        int i = 5; // neutral

        // x
        if (dir.x > 0)
            i += 1;
        else if (dir.x < 0)
            i -= 1;

        // y
        if (dir.y > 0)
            i += 3;
        else if (dir.y < 0)
            i -= 3;

        return i.ToString();
    }

    private void UpdateButtonInput()
    {
        int i = 0;

        if (Input.GetKey(KeyCode.J))
            i += 1;
        if (Input.GetKey(KeyCode.K))
            i += 2;
        if (Input.GetKey(KeyCode.L))
            i += 4;
        if (Input.GetKey(KeyCode.Semicolon))
            i += 8;

        // string s = Convert.ToString(i, 2);
        // PrintReadable(i);

        btnInput.Add(i);
        btnInput.RemoveAt(0);
    }

    private void ParseCommandList()
    {
        if (btnInput[btnInput.Count - btnBuffer] != 0)
        {
            foreach (var cmd in cmdList)
            {
                if (IsValidButton(cmd) && IsValidDirection(cmd))
                {
                    print(cmd.name);
                    return;
                }
            }
        }
    }

    private void PrintReadable(int input)
    {
        // https://stackoverflow.com/questions/34946568/finding-whether-bit-position-is-set-in-c-sharp

        string output = "----";

        for (int i = 0; i < 4; i++)
        {
            if (IsBitOn(input, i))
            {
                var c = output.ToCharArray(0, 4);
                c[i] = 'o';
                output = new string(c);
            }
        }

        print(output);
    }

    private bool IsBitOn(int value, int pos)
    {
        return (value & (1 << pos)) != 0;
    }


    private void CreateList()
    {
        cmdList = new FighterCommand[]
        {
            new FighterCommand("DP", 1, "623"),
            new FighterCommand("Grab", 1 + 2, "")
        };
    }
    private bool IsValidDirection(FighterCommand cmd)
    {
        // require directions in sequence, ignore neutral
        int position = cmd.directions.Length - 1;
        var searchChar = '-';
        for (int i = dirInput.Length - 1; i >= 0; i--)
        {
            // all positions have been validated
            if (position < 0)
                return true;

            // skip neutral and repeated inputs
            while (i > 0 && (dirInput[i] == searchChar || dirInput[i] == '5'))
                i--;

            // validate char at cmd position
            searchChar = cmd.directions[position];
            if (dirInput[i] == searchChar)
                position--;
        }
        return false;
    }
    private bool IsValidButton(FighterCommand cmd)
    {
        // find most significant digit of command button
        int digit = (int)Mathf.Log(cmd.button);
        for (int i = 0; i <= digit; i++)
        {
            // fail if command requires input not being pressed
            if (IsBitOn(cmd.button, i) &&
                !IsBitOn(btnInput[btnInput.Count - 1], i))
                return false;
        }
        return true;
    }
}
