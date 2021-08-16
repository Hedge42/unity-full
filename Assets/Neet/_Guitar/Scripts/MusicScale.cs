using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Scale
{
    private readonly int[] cMajor = new int[] { 0, 2, 4, 5, 7, 9, 11 };

    [Range(0, 11)] public int key;
    [Range(0, 6)] public int mode;

    public bool preferFlats;
    public int[] notes;

    public Scale(int key, int mode, bool preferFlats = true)
    {
        this.notes = BuildScale(key, mode);
        this.preferFlats = preferFlats;
    }

    public bool HasNote(int note)
    {
        return Scale.HasNote(this.notes, note);
    }
    public static bool HasNote(int[] scale, int note)
    {
        note %= 12;
        return IndexOf(scale, note) >= 0;
    }

    public static int IndexOf(int[] scale, int note)
    {
        note %= 12;
        for (int i = 0; i < scale.Length; i++)
        {
            if (note == scale[i])
                return i;
        }
        return -1;
    }


    public static string NoteToIntervalString(int[] scale, int note, bool preferFlats)
    {
        note %= 12;

        // interval included in scale
        int interval = IndexOf(scale, note);
        if (interval >= 0)
            return (interval + 1).ToString();

        // interval not in scale

        int tmp = note;
        int distance = 0;
        string s = "";

        // if flats, relative to next included interval
        if (preferFlats)
        {
            // find nearest next note value
            while (!HasNote(scale, tmp))
            {
                tmp = (tmp + 1) % 12;
                distance++;
            }

            interval = IndexOf(scale, tmp);

            for (int i = 0; i < distance; i++)
                s += "b";

            s += interval.ToString();
        }

        // if sharps, relative to previous included interval
        else
        {
            // find nearest previous note value
            while (!HasNote(scale, tmp))
            {
                tmp = (tmp - 1 + 12) % 12;
                distance++;
            }

            interval = IndexOf(scale, tmp);

            for (int i = 0; i < distance; i++)
                s += "#";

            s += interval.ToString();
        }

        return s;
    }

    private int[] BuildScale(int key, int mode)
    {
        var scale = (int[])cMajor.Clone();

        // use key and mode to calculate note in place
        for (int i = 0; i < scale.Length; i++)
        {
            var interval = (mode + i) % scale.Length;
            var note = (cMajor[interval] - cMajor[mode] + key + 12) % 12;
            scale[i] = note;
        }

        return scale;
    }

    public static string NoteValueToName(int value, bool preferFlats, bool pad)
    {
        value %= 12;

        if (value == 0)
            return (pad ? "C " : "C");
        else if (value == 1)
            return preferFlats ? "D" + "b" : "C" + "#";
        else if (value == 2)
            return pad ? "D " : "D";
        else if (value == 3)
            return preferFlats ? "E" + "b" : "D" + "#";
        else if (value == 4)
            return pad ? "E " : "E";
        else if (value == 5)
            return pad ? "F " : "F";
        else if (value == 6)
            return preferFlats ? "G" + "b" : "F" + "#";
        else if (value == 7)
            return pad ? "G " : "G";
        else if (value == 8)
            return preferFlats ? "A" + "b" : "G" + "#";
        else if (value == 9)
            return pad ? "A " : "A";
        else if (value == 10)
            return preferFlats ? "B" + "b" : "A" + "#";
        else if (value == 11)
            return pad ? "B " : "B";
        else
            return "?";
    }
    public static string ModeValueToName(int mode)
    {
        if (mode == 0)
            return "Ionian";
        else if (mode == 1)
            return "Dorian";
        else if (mode == 2)
            return "Phrygian";
        else if (mode == 3)
            return "Lydian";
        else if (mode == 4)
            return "MixoLydian";
        else if (mode == 5)
            return "Aeolian";
        else if (mode == 6)
            return "Locrian";
        else
            return "?";
    }

    public override string ToString()
    {
        var text = "";
        var scale = notes;

        for (var i = 0; i < scale.Length; i++)
            text += Scale.NoteValueToName(scale[i], preferFlats, false) + " ";

        return text;
    }
}