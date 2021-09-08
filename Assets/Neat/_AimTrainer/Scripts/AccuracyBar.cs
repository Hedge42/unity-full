using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Neat.Extensions;

namespace Neat.AimTrainer
{
    public class AccuracyBar : MonoBehaviour
    {
        public Image divider;
        public Image speed;
        public Image successPrefab;
        public Image failPrefab;

        private RectTransform dividerRect;
        private RectTransform speedRect;

        private float width;
        private float fadeTime = 2f;

        private MovementProfile profile;
        private Motor motor;
        private Canvas canvas;


        private void Start()
        {
            width = GetComponent<RectTransform>().sizeDelta.x;
            canvas = GetComponent<Canvas>();

            dividerRect = divider.GetComponent<RectTransform>();
            speedRect = speed.GetComponent<RectTransform>();

            successPrefab.gameObject.SetActive(false);
            failPrefab.gameObject.SetActive(false);

        }

        private void Update()
        {
            if (profile.useAccuracyRate)
            {
                var t = motor.Speed / profile.maxSpeed;
                UpdateSpeed(t);
            }
        }

        public void Setup(MovementProfile profile, Motor motor)
        {
            this.profile = profile;
            this.motor = motor;

            if (profile.useAccuracyRate)
            {
                SetDivider(profile.accuracyRate);
                UserEvent.SetStaticListener(Gun.SHOT_FIRED_KEY, Fire);
            }
            else
                gameObject.SetActive(false);
        }

        private void Fire()
        {
            var t = motor.Speed / profile.maxSpeed;
            SpawnResult(t, t <= profile.accuracyRate);
        }

        private void SpawnResult(float t, bool success)
        {
            var prefab = success ? successPrefab : failPrefab;

            var g = Instantiate(prefab, transform).GetComponent<Image>();
            g.gameObject.SetActive(true);
            g.GetComponent<RectTransform>().localPosition = Position(t);
            StartCoroutine(Fade(g));
        }

        private IEnumerator Fade(Image r)
        {
            var startOpacity = r.color.a;
            var startTime = Time.time;
            while (Time.time < startTime + fadeTime)
            {
                float ratio = (Time.time - startTime) / fadeTime;
                var a = Mathf.Lerp(startOpacity, 0, ratio);
                r.color = new Color(r.color.r, r.color.g, r.color.b, a);
                yield return null;
            }
            Destroy(r.gameObject);
        }

        private void SetDivider(float t)
        {
            dividerRect.localPosition = Position(t);
        }

        private void UpdateSpeed(float t)
        {
            speedRect.localPosition = Position(t);
        }

        private Vector3 Position(float t)
        {
            var x = (t - .5f) * width;
            return new Vector3(x, 0, 0);
        }
    }
}