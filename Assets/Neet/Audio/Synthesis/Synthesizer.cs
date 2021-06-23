using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Synthesizer : MonoBehaviour
{
    public const float A4_freq = 440;
    public const int A4_midi = 69; // nice

    // must be added as a child gameobject with an audiosource
    public Oscillator oscPrefab;

    public Dictionary<KeyCode, int> midiKeys;
    public Dictionary<KeyCode, Oscillator> oscillators;

    private Dictionary<KeyCode, Coroutine> attacks;
    private Dictionary<KeyCode, Coroutine> releases;

    [Range(0, 1)]
    public float sinAmp;
    [Range(0, 1)]
    public float squareAmp;
    [Range(0, 1)]
    public float triAmp;

    public float attack;
    public float release;

    public float volume = .1f;

    private void Awake()
    {
        oscPrefab.gameObject.SetActive(false);

        midiKeys = new Dictionary<KeyCode, int>();
        midiKeys.Add(KeyCode.LeftShift, 43); // g2
        midiKeys.Add(KeyCode.A, 44); // g#2
        midiKeys.Add(KeyCode.Z, 45); // a2
        midiKeys.Add(KeyCode.S, 46); // a#2
        midiKeys.Add(KeyCode.X, 47); // b2
        midiKeys.Add(KeyCode.C, 48); // c3
        midiKeys.Add(KeyCode.F, 49); // c#3
        midiKeys.Add(KeyCode.V, 50); // d3
        midiKeys.Add(KeyCode.G, 51); // d#3
        midiKeys.Add(KeyCode.B, 52); // e3
        midiKeys.Add(KeyCode.N, 53); // f3
        midiKeys.Add(KeyCode.J, 54); // f#3
        midiKeys.Add(KeyCode.M, 55); // g3
        midiKeys.Add(KeyCode.K, 56); // g#3
        midiKeys.Add(KeyCode.Comma, 57); // a3
        midiKeys.Add(KeyCode.L, 58); // a#3
        midiKeys.Add(KeyCode.Period, 59); // b3
        midiKeys.Add(KeyCode.Slash, 60); // c4
        midiKeys.Add(KeyCode.Quote, 61); // c#4
        midiKeys.Add(KeyCode.RightShift, 62); // d4
        midiKeys.Add(KeyCode.Return, 63); // d#4

        midiKeys.Add(KeyCode.Tab, 64); // e4
        midiKeys.Add(KeyCode.Q, 65); // f4
        midiKeys.Add(KeyCode.Alpha2, 66); // f#4
        midiKeys.Add(KeyCode.W, 67); // g4
        midiKeys.Add(KeyCode.Alpha3, 68); // g#4
        midiKeys.Add(KeyCode.E, 69); // a4
        midiKeys.Add(KeyCode.Alpha4, 70); // a#4
        midiKeys.Add(KeyCode.R, 71); // b4
        midiKeys.Add(KeyCode.T, 72); // c5
        midiKeys.Add(KeyCode.Alpha6, 73); // c#5
        midiKeys.Add(KeyCode.Y, 74); // d5
        midiKeys.Add(KeyCode.Alpha7, 75); // d#5
        midiKeys.Add(KeyCode.U, 76); // e5
        midiKeys.Add(KeyCode.I, 77); // f5
        midiKeys.Add(KeyCode.Alpha9, 78); // f#5
        midiKeys.Add(KeyCode.O, 79); // g5
        midiKeys.Add(KeyCode.Alpha0, 80); // g#5
        midiKeys.Add(KeyCode.P, 81); // a5
        midiKeys.Add(KeyCode.Minus, 82); // a#5
        midiKeys.Add(KeyCode.LeftBracket, 83); // b5
        midiKeys.Add(KeyCode.RightBracket, 84); // c6
        midiKeys.Add(KeyCode.Backspace, 85); // c#6
        midiKeys.Add(KeyCode.Backslash, 86); // d6

        oscillators = new Dictionary<KeyCode, Oscillator>();
        attacks = new Dictionary<KeyCode, Coroutine>();
        releases = new Dictionary<KeyCode, Coroutine>();
    }

    private void Update()
    {
        foreach (KeyValuePair<KeyCode, int> entry in midiKeys)
        {
            if (Input.GetKeyDown(entry.Key))
            {
                ClearAttackAndRelease(entry.Key);

                attacks.Add(entry.Key, StartCoroutine(StartOscillation(entry)));
            }
            else if (Input.GetKeyUp(entry.Key))
            {
                ClearAttackAndRelease(entry.Key);

                releases.Add(entry.Key, StartCoroutine(EndOscillation(entry)));
            }
        }
    }

    private void ClearAttackAndRelease(KeyCode key)
    {
        if (attacks.ContainsKey(key))
        {
            StopCoroutine(attacks[key]);
            attacks.Remove(key);
        }
        if (releases.ContainsKey(key))
        {
            StopCoroutine(releases[key]);
            releases.Remove(key);
        }
    }

    public float MidiFreq(float note)
    {
        return RelativeFrequency(note - A4_midi);
    }
    public float RelativeFrequency(float stepsFromA4)
    {
        return Mathf.Pow(2, (float)(stepsFromA4 / 12)) * A4_freq;
    }
    public double SemiTonesFromA4(float fn)
    {
        return 12 * Mathf.Log(fn / A4_freq, 2f);
    }

    private IEnumerator StartOscillation(KeyValuePair<KeyCode, int> entry)
    {
        // check first to see if the oscillator already exists
        Oscillator osc;

        float startGain;
        if (oscillators.ContainsKey(entry.Key))
        {
            osc = oscillators[entry.Key];
            startGain = osc.gain;
        }
        else
        {
            //osc = gameObject.AddComponent<Oscillator>();

            osc = Instantiate(oscPrefab.gameObject, transform).GetComponent<Oscillator>();
            osc.gameObject.SetActive(true);

            oscillators.Add(entry.Key, osc);
            startGain = 0;
        }

        osc.frequency = MidiFreq(entry.Value);
        osc.sinAmp = sinAmp;
        osc.squareAmp = squareAmp;
        osc.triAmp = triAmp;

        float startTime = Time.time;
        while (Time.time < startTime + attack)
        {
            osc.gain = Mathf.Lerp(startGain, volume, (Time.time - startTime) / attack);
            yield return null;
        }

        osc.gain = volume;

        attacks.Remove(entry.Key);
    }

    private IEnumerator EndOscillation(KeyValuePair<KeyCode, int> entry)
    {
        var osc = oscillators[entry.Key];
        float vol = osc.gain;

        float startTime = Time.time;
        while (Time.time < startTime + release)
        {
            osc.gain = Mathf.Lerp(vol, 0, (Time.time - startTime) / release);
            yield return null;
        }

        osc.gain = 0;

        Destroy(oscillators[entry.Key].gameObject);
        oscillators.Remove(entry.Key);

        releases.Remove(entry.Key);
    }
}
