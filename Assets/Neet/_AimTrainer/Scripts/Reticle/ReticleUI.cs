using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Neat.UI;
using UnityEngine.Events;
using System;

public class ReticleUI : MonoBehaviour, ISettingUI<ReticleProfile>
{
    public Reticle reticle;
    public Transform container;
    public GameObject warningPrefab;

    public Image colorPreview;
    public TMP_InputField r;
    public TMP_InputField g;
    public TMP_InputField b;
    public TMP_InputField a;

    public Image bgPreview;
    public TMP_InputField bgR;
    public TMP_InputField bgG;
    public TMP_InputField bgB;

    public Slider dotSlider;
    public TextMeshProUGUI dotText;

    public Slider offsetSlider;
    public TextMeshProUGUI offsetText;

    public Slider widthSlider;
    public TextMeshProUGUI widthText;

    public Slider lengthSlider;
    public TextMeshProUGUI lengthText;

    public Button saveButton;
    public Button returnButton;

    private ReticlePreviewBackgroundProfile bgProfile;
    private GameObject colorLimitWarning;
    private GameObject bgLimitWarning;

    private bool hasChanges;

    private void Start()
    {
        SetButtonEvents();

        bgProfile = ReticlePreviewBackgroundProfile.Load();
        reticle.profile = ReticleProfile.Load();

        CreateWarnings(warningPrefab, container);
        LoadFields(reticle.profile);
        SetUIValidation();
        ForceValidate();
        hasChanges = false;
    }

    public void AddAllTooltips(Transform container, GameObject prefab)
    {
        throw new System.NotImplementedException();
    }

    public void AddTooltip(Transform obj, Transform container, string text, GameObject prefab)
    {
        throw new System.NotImplementedException();
    }

    public void Apply(ref ReticleProfile profile)
    {
        profile.dotSize = (int)dotSlider.value;
        profile.lineLength = (int)lengthSlider.value;
        profile.lineWidth = (int)widthSlider.value;
        profile.lineOffset = (int)offsetSlider.value;

        profile.r = int.Parse(r.text);
        profile.g = int.Parse(g.text);
        profile.b = int.Parse(b.text);
        profile.a = int.Parse(a.text);
    }

    public void CreateWarnings(GameObject warningPrefab, Transform container)
    {
        colorLimitWarning = UIHelpers.CreateWarning(warningPrefab, r.gameObject,
            container, "Color fields must be in range [0, 255]");

        bgLimitWarning = UIHelpers.CreateWarning(warningPrefab, bgR.gameObject,
            container, "Color fields must be in range [0, 255]");
    }

    public void LoadFields(ReticleProfile profile)
    {
        dotSlider.value = profile.dotSize;
        lengthSlider.value = profile.lineLength;
        widthSlider.value = profile.lineWidth;
        offsetSlider.value = profile.lineOffset;

        r.text = profile.r.ToString();
        g.text = profile.g.ToString();
        b.text = profile.b.ToString();
        a.text = profile.a.ToString();

        bgR.text = bgProfile.r.ToString();
        bgG.text = bgProfile.g.ToString();
        bgB.text = bgProfile.b.ToString();
    }

    public void ForceValidate()
    {
        dotSlider.onValueChanged?.Invoke(dotSlider.value);
        lengthSlider.onValueChanged?.Invoke(lengthSlider.value);
        widthSlider.onValueChanged?.Invoke(widthSlider.value);
        offsetSlider.onValueChanged?.Invoke(offsetSlider.value);

        r.onEndEdit?.Invoke(r.text);
        g.onEndEdit?.Invoke(g.text);
        b.onEndEdit?.Invoke(b.text);
        a.onEndEdit?.Invoke(a.text);

        bgR.onEndEdit?.Invoke(bgR.text);
        bgG.onEndEdit?.Invoke(bgG.text);
        bgB.onEndEdit?.Invoke(bgB.text);
    }

    public void SetContextTexts()
    {
        throw new System.NotImplementedException();
    }

    public void SetUIValidation(UnityAction endAction = null)
    {
        UnityAction change = delegate
        {
            Apply(ref reticle.profile);
            reticle.UpdateReticle();
            hasChanges = true;
            endAction?.Invoke();
        };

        UnityAction bgChange = delegate
        {
            hasChanges = true;

            bgProfile.r = int.Parse(bgR.text);
            bgProfile.g = int.Parse(bgG.text);
            bgProfile.b = int.Parse(bgB.text);

            endAction?.Invoke();
        };

        UIHelpers.SetInputColorValidation(r, colorPreview, r, g, b,
            colorLimitWarning, change, a);
        UIHelpers.SetInputColorValidation(g, colorPreview, r, g, b,
            colorLimitWarning, change, a);
        UIHelpers.SetInputColorValidation(b, colorPreview, r, g, b,
            colorLimitWarning, change, a);
        UIHelpers.SetInputColorValidation(a, colorPreview, r, g, b,
            colorLimitWarning, change, a);

        UIHelpers.SetInputColorValidation(bgR, bgPreview, bgR, bgG, bgB,
            bgLimitWarning, bgChange);
        UIHelpers.SetInputColorValidation(bgG, bgPreview, bgR, bgG, bgB,
            bgLimitWarning, bgChange);
        UIHelpers.SetInputColorValidation(bgB, bgPreview, bgR, bgG, bgB,
            bgLimitWarning, bgChange);


        dotSlider.maxValue = ReticleProfile.DOT_MAX;
        dotSlider.onValueChanged.AddListener(delegate
        {
            dotText.text = dotSlider.value.ToString();
            change.Invoke();
        });

        offsetSlider.maxValue = ReticleProfile.OFFSET_MAX;
        offsetSlider.onValueChanged.AddListener(delegate
        {
            offsetText.text = offsetSlider.value.ToString();
            change.Invoke();
        });

        lengthSlider.maxValue = ReticleProfile.LENGTH_MAX;
        lengthSlider.onValueChanged.AddListener(delegate
        {
            lengthText.text = lengthSlider.value.ToString();
            change.Invoke();
        });

        widthSlider.maxValue = ReticleProfile.WIDTH_MAX;
        widthSlider.onValueChanged.AddListener(delegate
        {
            widthText.text = widthSlider.value.ToString();
            change.Invoke();
        });
    }


    public void SetButtonEvents()
    {
        Func<bool> shouldShow = delegate { return hasChanges; };

        ContextPrompt save = new ContextPrompt();
        save.infoText = "Are you sure you want to overwrite previous settings?";
        save.yesText = "Yes, overwrite and save";
        save.noText = "Nevermind";
        save.shouldShow = shouldShow;
        save.onYes = delegate
        {
            Apply(ref reticle.profile);
            reticle.profile.Save();
            bgProfile.Save();
            hasChanges = false;
        };

        ContextPrompt back = new ContextPrompt();
        back.infoText = "You have unsaved changes.";
        back.yesText = "Discard changes and return to main";
        back.noText = "Continue editing";
        back.shouldShow = shouldShow;
        back.onYes = delegate
        {
            SceneSwitcher2.instance.SwitchTo(0);
        };


        saveButton.onClick.AddListener(delegate
        {
            Neat.UI.ContextMenu.instance.Process(save);
        });

        returnButton.onClick.AddListener(delegate
        {
            Neat.UI.ContextMenu.instance.Process(back);
        });
    }
}
