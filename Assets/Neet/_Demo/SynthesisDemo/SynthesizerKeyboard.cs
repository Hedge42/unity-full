using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SynthesizerKeyboard : MonoBehaviour
{
    public Synthesizer synth;

    public GameObject whiteKey;
    public GameObject blackKey;

    public Transform bottomWhite;
    public Transform bottomBlack;
    public Transform topWhite;
    public Transform topBlack;

    public Dictionary<int, string> bottomWhiteKeys;
    public Dictionary<int, string> bottomBlackKeys;
    public Dictionary<int, string> topWhiteKeys;
    public Dictionary<int, string> topBlackKeys;

    private Dictionary<int, Image> images;

    private void Start()
    {
        whiteKey.SetActive(false);
        blackKey.SetActive(false);

        bottomBlackKeys = new Dictionary<int, string>();
        bottomBlackKeys.Add(44, "G#\n(a)"); // g#2
        bottomBlackKeys.Add(46, "A#\n(s)"); // a#2
        bottomBlackKeys.Add(-1, ""); // space
        bottomBlackKeys.Add(49, "C#\n(f)"); // c#3
        bottomBlackKeys.Add(51, "D#\n(g)"); // d#3
        bottomBlackKeys.Add(-2, ""); // space
        bottomBlackKeys.Add(54, "F#\n(j)"); // f#3
        bottomBlackKeys.Add(56, "G#\n(k)"); // g#3
        bottomBlackKeys.Add(58, "A#\n(l)"); // a#3
        bottomBlackKeys.Add(-3, ""); // space
        bottomBlackKeys.Add(61, "C#\n(')"); // c#4
        bottomBlackKeys.Add(63, "D#\n(re)"); // d#4

        bottomWhiteKeys = new Dictionary<int, string>();
        bottomWhiteKeys.Add(43, "G\n(ls)"); // g2
        bottomWhiteKeys.Add(45, "A\n(z)"); // a2
        bottomWhiteKeys.Add(47, "B\n(x)"); // b2
        bottomWhiteKeys.Add(48, "C\n(c)"); // c3
        bottomWhiteKeys.Add(50, "D\n(v)"); // d3
        bottomWhiteKeys.Add(52, "E\n(b)"); // e3
        bottomWhiteKeys.Add(53, "F\n(n)"); // f3
        bottomWhiteKeys.Add(55, "G\n(m)"); // g3
        bottomWhiteKeys.Add(57, "A\n(,)"); // a3
        bottomWhiteKeys.Add(59, "B\n(.)"); // b3
        bottomWhiteKeys.Add(60, "C\n(/)"); // c4
        bottomWhiteKeys.Add(62, "D\n(rs)"); // d4


        topBlackKeys = new Dictionary<int, string>();
        topBlackKeys.Add(-9, ""); // space
        topBlackKeys.Add(66, "F#\n(2)"); // f#4
        topBlackKeys.Add(68, "G#\n(3)"); // g#4
        topBlackKeys.Add(70, "A#\n(4)"); // a#4
        topBlackKeys.Add(-5, ""); // space
        topBlackKeys.Add(73, "C#\n(6)"); // c#5
        topBlackKeys.Add(75, "D#\n(7)"); // d#
        topBlackKeys.Add(-6, ""); // space
        topBlackKeys.Add(78, "F#\n(9)"); // f#5
        topBlackKeys.Add(80, "G#\n(0)"); // g#5
        topBlackKeys.Add(82, "A#\n(-)"); // a#5
        topBlackKeys.Add(-7, ""); // space
        topBlackKeys.Add(85, "C#\n(bs)"); // c#6

        topWhiteKeys = new Dictionary<int, string>();
        topWhiteKeys.Add(64, "E\n(tb)"); // e4
        topWhiteKeys.Add(65, "F\n(q)"); // f4
        topWhiteKeys.Add(67, "G\n(w)"); // g4
        topWhiteKeys.Add(69, "A\n(e)"); // a4
        topWhiteKeys.Add(71, "B\n(r)"); // b4
        topWhiteKeys.Add(72, "C\n(t)"); // c5
        topWhiteKeys.Add(74, "D\n(y)"); // d5
        topWhiteKeys.Add(76, "E\n(u)"); // e5
        topWhiteKeys.Add(77, "F\n(i)"); // f5
        topWhiteKeys.Add(79, "G\n(o)"); // g5
        topWhiteKeys.Add(81, "A\n(p)"); // a5
        topWhiteKeys.Add(83, "B\n([)"); // b5
        topWhiteKeys.Add(84, "C\n(])"); // c6
        topWhiteKeys.Add(86, "D\n(\\)"); // d6

        images = new Dictionary<int, Image>();
        foreach (KeyValuePair<int, string> entry in bottomWhiteKeys)
        {
            GameObject k = Instantiate(whiteKey, bottomWhite);
            k.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = entry.Value;
            k.SetActive(true);
            images.Add(entry.Key, k.GetComponent<Image>());
        }
        foreach (KeyValuePair<int, string> entry in topWhiteKeys)
        {
            GameObject k = Instantiate(whiteKey, topWhite);
            k.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = entry.Value;
            k.SetActive(true);
            images.Add(entry.Key, k.GetComponent<Image>());
        }
        foreach (KeyValuePair<int, string> entry in bottomBlackKeys)
        {
            GameObject k = Instantiate(blackKey, bottomBlack);
            k.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = entry.Value;
            if (!entry.Value.Equals(""))
                k.SetActive(true);
            images.Add(entry.Key, k.GetComponent<Image>());
        }
        foreach (KeyValuePair<int, string> entry in topBlackKeys)
        {
            GameObject k = Instantiate(blackKey, topBlack);
            k.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = entry.Value;
            if (!entry.Value.Equals(""))
                k.SetActive(true);
            images.Add(entry.Key, k.GetComponent<Image>());
        }

        bottomWhite.GetComponent<AxisSpacer>().FixSpacing();
        topWhite.GetComponent<AxisSpacer>().FixSpacing();
        bottomBlack.GetComponent<AxisSpacer>().FixSpacing();
        topBlack.GetComponent<AxisSpacer>().FixSpacing();
    }

    private void Update()
    {
        foreach (KeyValuePair<KeyCode, int> entry in synth.midiKeys)
        {
            if (Input.GetKeyDown(entry.Key))
            {
                images[entry.Value].color = Color.green;
            }
            else if (Input.GetKeyUp(entry.Key))
            {
                // detect if black or white key
                if (bottomBlackKeys.ContainsKey(entry.Value) || topBlackKeys.ContainsKey(entry.Value))
                    images[entry.Value].color = Color.black;
                else
                    images[entry.Value].color = Color.white;
            }
        }
    }
}
