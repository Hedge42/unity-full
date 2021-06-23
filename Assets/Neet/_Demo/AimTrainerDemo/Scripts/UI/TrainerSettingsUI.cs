using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using Neet.Data;
using Neet.Audio;

public class TrainerSettingsUI : MonoBehaviour
{
    // static dictionary keys 
    private readonly string LAST_VALID_KEY = "lastValid";
    private readonly string PREVIEW_FIELDS_KEY = "rgb";

    private bool hasChanges;

    public GameObject areYouSure;

    public Slider masterVolume;
    public TextMeshProUGUI masterVolumeLabel;

    public TMP_InputField distance;
    public TMP_InputField dpi;

    public Image backgroundPreview;
    public TMP_InputField backgroundR;
    public TMP_InputField backgroundG;
    public TMP_InputField backgroundB;

    public Image targetPreview;
    public TMP_InputField targetR;
    public TMP_InputField targetG;
    public TMP_InputField targetB;
    public TMP_InputField targetA;

    public Image dummyPreview;
    public TMP_InputField dummyR;
    public TMP_InputField dummyG;
    public TMP_InputField dummyB;
    public TMP_InputField dummyA;

    public Image centerPreview;
    public TMP_InputField centerR;
    public TMP_InputField centerG;
    public TMP_InputField centerB;
    public TMP_InputField centerA;

    // needed for the are-you-sure stuff
    public UnityEvent returnEvent;

    private void Start()
    {
        SetupUI();

        LoadSavedSettings();
    }

    private void SetupUI()
    {
        // relates preview images with its RGBA input fields
        SetPreviewFields(backgroundPreview, backgroundR, backgroundG, backgroundB);
        SetPreviewFields(targetPreview, targetR, targetG, targetB, targetA);
        SetPreviewFields(dummyPreview, dummyR, dummyG, dummyB, dummyA);
        SetPreviewFields(centerPreview, centerR, centerG, centerB, centerA);

        // setting text-changed events for all RBGA input fields
        SetRgbChangedEvent(backgroundR, backgroundPreview);
        SetRgbChangedEvent(backgroundG, backgroundPreview);
        SetRgbChangedEvent(backgroundB, backgroundPreview);

        SetRgbChangedEvent(targetR, targetPreview);
        SetRgbChangedEvent(targetG, targetPreview);
        SetRgbChangedEvent(targetB, targetPreview);
        SetRgbChangedEvent(targetA, targetPreview);

        SetRgbChangedEvent(dummyR, dummyPreview);
        SetRgbChangedEvent(dummyG, dummyPreview);
        SetRgbChangedEvent(dummyB, dummyPreview);
        SetRgbChangedEvent(dummyA, dummyPreview);

        SetRgbChangedEvent(centerR, centerPreview);
        SetRgbChangedEvent(centerG, centerPreview);
        SetRgbChangedEvent(centerB, centerPreview);
        SetRgbChangedEvent(centerA, centerPreview);

        Set360DistanceChangedEvent(distance);
        SetDPIChangedEvent(dpi);
        SetVolumeChangedEvent(masterVolume);
    }

    // Automatically valdate fields
    private void SetRgbChangedEvent(TMP_InputField inp, Image preview)
    {
        // workaround using delegate method to include 
        // other parameters into the onValueChanged event
        UnityAction<string> s = delegate (string f)
        {
            // set emptied text sets to 0 and select text for easier editing
            if (inp.text.Length == 0)
            {
                inp.text = "0";
                inp.selectionStringAnchorPosition = 0;
                inp.selectionStringFocusPosition = 1;
            }

            if (int.TryParse(f, out int result)  // parse-able int
            && 0 <= result && result <= 255  // in range [0,255]
            && !(f.Length > 1 && f[0] == '0')) // isn't something like '04'
            {
                if (f != inp.GetData<int>(LAST_VALID_KEY).ToString())
                {
                    hasChanges = true;

                    // set last valid to be able to revert
                    inp.SetData(LAST_VALID_KEY, result);

                    // update preview color
                    UpdatePreview(preview);
                }

            }

            // revert to last valid value
            else
            {
                inp.text = inp.GetData<int>(LAST_VALID_KEY).ToString();
            }
        };

        inp.onValueChanged.AddListener(s);
    }
    private void Set360DistanceChangedEvent(TMP_InputField inp)
    {
        // workaround using delegate method to include 
        // other parameters into the onValueChanged event
        UnityAction<string> s = delegate (string f)
        {
            // set emptied text sets to 0 and select text for easier editing
            if (inp.text.Length == 0)
            {
                inp.text = "0";
                inp.selectionStringAnchorPosition = 0;
                inp.selectionStringFocusPosition = 1;
            }

            if (float.TryParse(f, out float result)  // parse-able int
            && 0 < result && result < 100  // in range (0, 100)
            && !(f.Length > 1 && f[0] == '0' && f[1] != '.')) // isn't something like '04.'
            {
                if (f != inp.GetData<int>(LAST_VALID_KEY).ToString())
                {
                    hasChanges = true;

                    // set last valid to be able to revert
                    inp.SetData(LAST_VALID_KEY, result);
                }

            }

            // revert to last valid value
            else
            {
                inp.text = inp.GetData<int>(LAST_VALID_KEY).ToString();
            }
        };

        inp.onValueChanged.AddListener(s);
    }
    private void SetDPIChangedEvent(TMP_InputField inp)
    {
        // workaround using delegate method to include 
        // other parameters into the onValueChanged event
        UnityAction<string> s = delegate (string f)
        {
            // set emptied text sets to 0 and select text for easier editing
            if (inp.text.Length == 0)
            {
                inp.text = "0";
                inp.selectionStringAnchorPosition = 0;
                inp.selectionStringFocusPosition = 1;
            }

            if (int.TryParse(f, out int result)  // parse-able int
            && 0 < result && result < 10000  // in range (0, 10,000)
            && !(f.Length > 1 && f[0] == '0')) // isn't something like '04.'
            {
                if (f != inp.GetData<int>(LAST_VALID_KEY).ToString())
                {
                    hasChanges = true;

                    // set last valid to be able to revert
                    inp.SetData(LAST_VALID_KEY, result);
                }

            }

            // revert to last valid value
            else
            {
                inp.text = inp.GetData<int>(LAST_VALID_KEY).ToString();
            }
        };

        inp.onValueChanged.AddListener(s);
    }
    private void SetVolumeChangedEvent(Slider s)
    {
        UnityAction<float> a = delegate
        {
            hasChanges = true;
            AudioManager.instance.UpdateMasterVolume(s.value / 100f);
            masterVolumeLabel.text = ((int)s.value).ToString();
        };

        s.onValueChanged.AddListener(a);
    }

    // Relates color previews to input fields
    private void SetPreviewFields(Image preview, TMP_InputField r, TMP_InputField g, TMP_InputField b, TMP_InputField a = null)
    {
        TMP_InputField[] arr = { r, g, b, a };
        preview.SetData(PREVIEW_FIELDS_KEY, arr);
    }

    // Uses stored input fields to update image color without extra params
    private void UpdatePreview(Image preview)
    {
        // get stored inputfield array and create float array to store RGBA
        TMP_InputField[] arr = preview.GetData<TMP_InputField[]>(PREVIEW_FIELDS_KEY);
        float[] rgba = new float[arr.Length];

        // parse float values from input fields
        for (int i = 0; i < arr.Length; i++)
        {
            // default to full (optional alpha)
            if (arr[i] == null)
                rgba[i] = 1f;

            else
            {
                // catches values missing on load
                if (int.TryParse(arr[i].text, out int result))
                    rgba[i] = result / 255f;
                else
                    rgba[i] = 1f;
            }
        }

        preview.color = new Color(rgba[0], rgba[1], rgba[2], rgba[3]);
    }

    // Need one for each settings object
    private ColorProfile ParseColorSettingsObject()
    {
        ColorProfile tso = new ColorProfile();

        // RBG values are automatically validated
        // prior to any of this running

        tso.backgroundR = int.Parse(backgroundR.text);
        tso.backgroundG = int.Parse(backgroundG.text);
        tso.backgroundB = int.Parse(backgroundB.text);

        tso.targetR = int.Parse(targetR.text);
        tso.targetG = int.Parse(targetG.text);
        tso.targetB = int.Parse(targetB.text);
        tso.targetA = int.Parse(targetA.text);

        tso.dummyR = int.Parse(dummyR.text);
        tso.dummyG = int.Parse(dummyG.text);
        tso.dummyB = int.Parse(dummyB.text);
        tso.dummyA = int.Parse(dummyA.text);

        tso.centerR = int.Parse(centerR.text);
        tso.centerG = int.Parse(centerG.text);
        tso.centerB = int.Parse(centerB.text);
        tso.centerA = int.Parse(centerA.text);

        return tso;
    }
    private ControlSetting ParseControlSettingsObject()
    {
        // values automatically validated previously

        ControlSetting cso = new ControlSetting();
        cso.dpi = int.Parse(dpi.text);
        cso.distance = float.Parse(distance.text);

        return cso;
    }
    private AudioSetting ParseAudioSettingsObject()
    {
        AudioSetting aso = new AudioSetting();
        aso.masterVolume = masterVolume.value / 100f;

        return aso;
    }

    // Need one for each settings object
    public void LoadUIColorValues(ColorProfile tso)
    {
        backgroundR.text = tso.backgroundR.ToString();
        backgroundG.text = tso.backgroundG.ToString();
        backgroundB.text = tso.backgroundB.ToString();
        backgroundPreview.color = new Color(tso.backgroundR / 255f, tso.backgroundG / 255f, tso.backgroundB / 255f);

        targetR.text = tso.targetR.ToString();
        targetG.text = tso.targetG.ToString();
        targetB.text = tso.targetB.ToString();
        targetA.text = tso.targetA.ToString();
        targetPreview.color = new Color(tso.targetR / 255f, tso.targetG / 255f, tso.targetB / 255f);

        dummyR.text = tso.dummyR.ToString();
        dummyG.text = tso.dummyG.ToString();
        dummyB.text = tso.dummyB.ToString();
        dummyA.text = tso.dummyA.ToString();
        dummyPreview.color = new Color(tso.dummyR / 255f, tso.dummyG / 255f, tso.dummyB / 255f);

        centerR.text = tso.centerR.ToString();
        centerG.text = tso.centerG.ToString();
        centerB.text = tso.centerB.ToString();
        centerA.text = tso.centerA.ToString();
        centerPreview.color = new Color(tso.centerR / 255f, tso.centerG / 255f, tso.centerB / 255f);
    }
    public void LoadUIControlValues(ControlSetting cso)
    {
        dpi.text = cso.dpi.ToString();
        distance.text = cso.distance.ToString();
    }
    public void LoadUIAudioValues(AudioSetting aso)
    {
        masterVolume.value = aso.masterVolume * 100f;
    }

    // References all settings objects
    public void RevertToDefaultSettings()
    {
        LoadUIColorValues(new ColorProfile());
        LoadUIControlValues(new ControlSetting());
        LoadUIAudioValues(new AudioSetting());

        hasChanges = true;
    }

    // References all settings objects
    public void LoadSavedSettings()
    {
        // LoadUIColorValues(ColorProfile.Load());
        // LoadUIControlValues(ControlSetting.Load());
        // LoadUIAudioValues(AudioSetting.Load());

        hasChanges = false;
    }

    // References all settings objects
    public void Save()
    {
        // ParseColorSettingsObject().Save();
        // ParseControlSettingsObject().Save();
        // ParseAudioSettingsObject().Save();

        hasChanges = false;
    }
    
    // "Are you sure?" stuff
    public void TryToReturn()
    {
        if (hasChanges)
        {
            areYouSure.SetActive(true);
        }
        else
        {
            returnEvent.Invoke();
        }
    }
    public void AreYouSureYes()
    {
        LoadSavedSettings();
        areYouSure.SetActive(false);
        returnEvent.Invoke();
    }
    public void AreYouSureNo()
    {
        areYouSure.SetActive(false);
    }
}
