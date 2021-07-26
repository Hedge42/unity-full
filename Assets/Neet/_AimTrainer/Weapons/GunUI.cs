using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunUI : MonoBehaviour
{
    public TextMeshProUGUI ammoText;
    public GameObject statsTextTemplate;
    public TrailRenderer tr;
    public float trWidth;

    public bool fireRate;
    public bool capacity;
    public bool damage;
    public bool reloadTime;
    public bool wallPen;

    private List<GameObject> texts;

    private void OnValidate()
    {
        Init();
    }

    private void Init()
    {
        if (tr == null)
        {
            var g = GameObject.Find("ReticleTrail");

            if (g != null)
            {
                tr = g.GetComponent<TrailRenderer>();
                tr.startWidth = trWidth;
            }
        }
    }

    private void Awake()
    {
        Init();
        if (statsTextTemplate != null)
        {
            texts = new List<GameObject>();
            statsTextTemplate.SetActive(false);
            DestroyTexts();

            if (fireRate)
                CreateStatsText("Fire rate:");
            if (capacity)
                CreateStatsText("Capacity:");
            if (damage)
                CreateStatsText("Damage:");
            if (reloadTime)
                CreateStatsText("Reload time:");
            if (wallPen)
                CreateStatsText("Wall pen:");
        }
    }

    public void UpdateUI(Gun g)
    {
        ammoText.text = g.currentClip + "/" + g.currentAmmo;
    }

    private GameObject CreateStatsText(string text)
    {
        GameObject go = Instantiate(statsTextTemplate, statsTextTemplate.transform.parent);
        go.SetActive(true);
        go.GetComponent<TextMeshProUGUI>().text = text;
        texts.Add(go);
        return go;
    }

    private void DestroyTexts()
    {
        while (texts.Count > 0)
        {
            Destroy(texts[0]);
            texts.Remove(texts[0]);
        }
    }
}
