using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Neet.Data;
using UnityEngine.Events;
using UnityEngine.UI;
using System;
using Neet.UI;

public static class UIHelpers
{
    public static void AddTooltip(GameObject prefab, Transform parent, string text = "")
    {
        GameObject tooltip = GameObject.Instantiate(prefab, parent);
        tooltip.SetActive(true);
        tooltip.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;

        if (text != "")
        {
            parent.GetComponent<Neet.UI.EventHandler>().onPointerClick += delegate
            {
                Neet.UI.ContextMenu.instance.Show(text);
            };
        }
    }

    /// <summary>
    /// Reusable method to validate float input for input field
    /// </summary>
    public static void SetInputValidation(TMP_InputField inp, GameObject warning,
        float min, float max, UnityAction endAction = null)
    {
        // show warning if input is invalid
        UnityAction<string> change = delegate (string s)
        {
            if (float.TryParse(s, out float result))
            {
                if (result < min || result > max)
                    warning.SetActive(true);
                else
                    warning.SetActive(false);
            }
            else
            {
                warning.SetActive(true);
            }
        };

        // fix input if necessary
        UnityAction<string> end = delegate (string s)
        {
            // set emptied text sets to min
            if (inp.text.Length == 0)
                inp.text = min.ToString();

            if (float.TryParse(s, out float result))  // parse-able float
            {
                // fit to range
                if (result < min)
                    result = min;
                else if (result > max)
                    result = max;

                inp.text = result.ToString();
                endAction?.Invoke();

                // set last valid to be able to revert
                inp.SetData(Neet.Keys.LAST_VALID, result);
            }

            // revert to last valid value
            else
            {
                inp.text = inp.GetData<float>(Neet.Keys.LAST_VALID).ToString();
            }

            warning.SetActive(false);
        };

        inp.onValueChanged.AddListener(change);
        inp.onSubmit.AddListener(end);
        inp.onEndEdit.AddListener(end);
        inp.onDeselect.AddListener(end);
    }

    /// <summary>
    /// Reusable method to validate int input for input field
    /// </summary>
    public static void SetInputValidation(TMP_InputField inp,
        GameObject warning, int min, int max, UnityAction endAction = null)
    {
        // show warning if input is invalid
        UnityAction<string> change = delegate (string s)
        {
            if (int.TryParse(s, out int result))
            {
                if (result < min || result > max)
                    warning.SetActive(true);
                else
                    warning.SetActive(false);
            }
            else
            {
                warning.SetActive(true);
            }
        };

        // fix input if necessary
        UnityAction<string> end = delegate (string s)
        {
            // set emptied text sets to min
            if (inp.text.Length == 0)
                inp.text = min.ToString();

            if (int.TryParse(s, out int result))  // parse-able float
            {
                // fit to range
                if (result < min)
                    result = min;
                else if (result > max)
                    result = max;

                inp.text = result.ToString();
                endAction?.Invoke();

                // set last valid to be able to revert
                inp.SetData(Neet.Keys.LAST_VALID, result);
            }

            // revert to last valid value
            else
            {
                inp.text = inp.GetData<int>(Neet.Keys.LAST_VALID).ToString();
            }

            warning.SetActive(false);
        };

        inp.onValueChanged.AddListener(change);
        inp.onSubmit.AddListener(end);
        inp.onEndEdit.AddListener(end);
        inp.onDeselect.AddListener(end);
    }

    public static void SetInputMinMaxValidation(TMP_InputField inpMin,
        TMP_InputField inpMax, GameObject limitWarning, GameObject minMaxWarning,
        float min, float max, UnityAction endAction = null)
    {
        // show warning if input is invalid
        UnityAction<string> change = delegate (string s)
        {
            // both have valid inputs
            if (float.TryParse(inpMin.text, out float minResult)
            && float.TryParse(inpMax.text, out float maxResult))
            {
                bool minInRange = minResult >= min && minResult <= max;
                bool maxInRange = maxResult >= min || maxResult <= max;

                // show if one not in limit
                limitWarning.SetActive(!(minInRange && maxInRange));

                // show if min > max
                minMaxWarning.SetActive(minResult > maxResult);
            }
            // one has invalid input, only show limit warning
            else
            {
                limitWarning.SetActive(true);
                minMaxWarning.SetActive(false);
            }
        };

        UnityAction<string> minEnd = delegate (string s)
        {
            // force valid float range input
            if (s.Length == 0)
                inpMin.text = min.ToString();

            if (float.TryParse(inpMin.text, out float minResult))
            {
                if (minResult < min)
                    minResult = min;
                else if (minResult > max)
                    minResult = max;
            }
            else
            {
                // set to last valid input
                inpMin.text = inpMin.GetData<float>(Neet.Keys.LAST_VALID).ToString();
            }

            // force < max
            if (float.TryParse(inpMax.text, out float maxResult))
            {
                if (minResult > maxResult)
                    minResult = maxResult;

                inpMin.text = minResult.ToString();
            }

            inpMin.SetData(Neet.Keys.LAST_VALID, minResult);
            endAction.Invoke();

            minMaxWarning.SetActive(false);
            limitWarning.SetActive(false);
        };

        UnityAction<string> maxEnd = delegate (string s)
        {
            // force valid float range input
            if (s.Length == 0)
                inpMax.text = min.ToString();

            if (float.TryParse(inpMax.text, out float maxResult))
            {
                if (maxResult < min)
                    maxResult = min;
                else if (maxResult > max)
                    maxResult = max;
            }
            else
            {
                // set to last valid input
                inpMax.text = inpMax.GetData<float>(Neet.Keys.LAST_VALID).ToString();
            }

            // force > min
            if (float.TryParse(inpMin.text, out float minResult))
            {
                if (minResult > maxResult)
                    maxResult = minResult;

                inpMax.text = maxResult.ToString();
            }

            inpMax.SetData(Neet.Keys.LAST_VALID, maxResult);
            endAction.Invoke();

            minMaxWarning.SetActive(false);
            limitWarning.SetActive(false);
        };

        inpMin.onValueChanged.AddListener(change);
        inpMax.onValueChanged.AddListener(change);

        inpMin.onSubmit.AddListener(minEnd);
        inpMin.onEndEdit.AddListener(minEnd);
        inpMin.onDeselect.AddListener(minEnd);

        inpMax.onSubmit.AddListener(maxEnd);
        inpMax.onEndEdit.AddListener(maxEnd);
        inpMax.onDeselect.AddListener(maxEnd);
    }

    public static void SetInputColorValidation(TMP_InputField f, Image preview,
        TMP_InputField r, TMP_InputField g, TMP_InputField b,
        GameObject limitWarning, UnityAction endAction, TMP_InputField a = null)
    {
        int MIN = 0;
        int MAX = 255;

        Func<Color> getColor = delegate
        {
            int _r = int.Parse(r.text);
            int _g = int.Parse(g.text);
            int _b = int.Parse(b.text);
            int _a = 255;

            if (a != null)
                _a = int.Parse(a.text);

            return new Color(_r / 255f, _g / 255f, _b / 255f, _a / 255f);
        };

        UnityAction<string> change = delegate (string s)
        {
            // set emptied text sets to 0 and select text for easier editing
            if (f.text.Length == 0)
            {
                f.text = MIN.ToString();
                f.selectionStringAnchorPosition = 0;
                f.selectionStringFocusPosition = 1;
            }

            // only accept parsable int
            if (!int.TryParse(s, out int result))
            {
                // reset to last valid
                f.text = f.GetData<int>(Neet.Keys.LAST_VALID).ToString();
                return; // I think this works?
            }
            else if (result >= MIN && result <= MAX)
            {
                // set last valid if in range
                f.SetData(Neet.Keys.LAST_VALID, result);
            }


            bool validR = int.TryParse(r.text, out int rResult);
            bool validG = int.TryParse(g.text, out int gResult);
            bool validB = int.TryParse(b.text, out int bResult);
            bool validA = true;

            int aResult = 255;

            if (a != null)
                validA = int.TryParse(a.text, out aResult);

            if (validR && validG && validB && validA)
            {
                // see if ALL values are in range [MIN, MAX]
                bool rInRange = rResult >= MIN && rResult <= MAX;
                bool gInRange = gResult >= MIN && gResult <= MAX;
                bool bInRange = bResult >= MIN && bResult <= MAX;
                bool aInRange = aResult >= MIN && aResult <= MAX;

                // update preview if all in range
                bool isRangeValid = rInRange && gInRange && bInRange && aInRange;
                if (isRangeValid)
                    preview.color = new Color
                    (rResult / 255f, gResult / 255f, bResult / 255f, aResult / 255f);

                // if one fails, show warning
                limitWarning.SetActive(!isRangeValid);
            }
        };

        UnityAction<string> end = delegate (string s)
        {
            // this really should happen all the time
            if (int.TryParse(s, out int result))
            {
                if (result < MIN)
                    result = MIN;
                else if (result > MAX)
                    result = MAX;

                f.text = result.ToString();
            }

            preview.color = getColor();
            endAction.Invoke();
        };

        f.onValueChanged.AddListener(change);

        f.onSubmit.AddListener(end);
        f.onDeselect.AddListener(end);
        f.onEndEdit.AddListener(end);
    }

    private static void OnlyAcceptInt(TMP_InputField f, int min)
    {
        UnityAction<string> change = delegate (string s)
        {
            // set emptied text sets to 0 and select text for easier editing
            if (f.text.Length == 0)
            {
                f.text = min.ToString();
                f.selectionStringAnchorPosition = 0;
                f.selectionStringFocusPosition = 1;
            }

            // only accept parsable int
            if (!int.TryParse(s, out int result))
            {
                f.text = f.GetData<int>(Neet.Keys.LAST_VALID).ToString();
            }
        };

        // clean up input
        UnityAction<string> end = delegate (string s)
        {
            f.text = s.ToString();
        };


        f.onValueChanged.AddListener(change);

        f.onEndEdit.AddListener(end);
        f.onDeselect.AddListener(end);
        f.onSubmit.AddListener(end);
    }

    public static GameObject CreateWarning(GameObject prefab, GameObject settingObj,
        Transform container, string text)
    {
        // find the sibling index of setting object parent
        Transform t = settingObj.transform;
        while (t.parent != container.transform)
            t = t.parent;
        int index = t.GetSiblingIndex();

        // instantiate and set sibling index
        GameObject warning = GameObject.Instantiate(prefab, container.transform);
        warning.transform.SetSiblingIndex(index + 1);

        // set text
        warning.GetComponentInChildren<TextMeshProUGUI>().text = text;

        return warning;
    }

    public static int GetContainerIndex(Transform obj, Transform container)
    {
        while (obj.parent != container)
            obj = obj.parent;

        return obj.GetSiblingIndex();
    }
}
