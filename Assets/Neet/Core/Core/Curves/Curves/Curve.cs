using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Curve : ScriptableObject
{
    [System.Serializable]
    public class SubCurve
    {
        [Range(1, 10)]
        public float pow;
        [Range(-1, 1.5f)]
        public float endY;
        [Range(.01f, 1)]
        public float relativeLength;

        [HideInInspector]
        public float normalizedLength;
        [HideInInspector]
        public Vector2 end;
        [HideInInspector]
        public Vector2 start;
        public EasingType easingType;


        // https://docs.microsoft.com/en-us/dotnet/api/system.func-2?view=netcore-3.1
        public delegate float EasingFunction(float p, float t);
        public EasingFunction interpolator;

        public SubCurve Complement(SubCurve last, Vector2 v)
        {
            bool invert = false;
            // last.end.y > end.y;

            EasingFunction e = null;

            // error conditions
            // cannot make complement from positive slope to lesser value
            if (invert && last.Slope(1) > Mathf.Epsilon)
                throw new System.ArgumentOutOfRangeException();
            // similarly, cannot make complement from negative slope to higher value
            else if (!invert && last.Slope(1) < -Mathf.Epsilon)
                throw new System.ArgumentOutOfRangeException();


            // easeIn -> easeOut
            // easeOut -> easeIn
            // e = EaseOut(inverted)
            if (invert)
            {
                if (last.interpolator == PowInInverted)
                    e = PowOutInverted;
                else if (last.interpolator == PowOut || last.interpolator == PowOutInverted)
                    e = PowInInverted;
            }
            else
            {
                if (last.interpolator == PowOutInverted || last.interpolator == PowOut)
                    e = PowIn;
                else if (last.interpolator == PowIn)
                    e = PowOut;
            }

            if (e == null)
                throw new Exception();

            //return new SubCurve(e, last.end, end, );    
            return null;
        }

        public float Interpolate(float t)
        {
            // fixed_t translates to [0, 1]
            float dist = end.x - start.x;
            float elapsed = t - start.x;
            float fixed_t = elapsed / dist;

            float height = end.y - start.y;

            // fixed_t scales x
            // height scales y
            // start.y translates
            return height * interpolator.Invoke(pow, fixed_t) + start.y;
        }

        private float Slope(float t)
        {
            if (interpolator == PowIn)
                return EaseInPrime(pow, t);
            else if (interpolator == PowInInverted)
                return EaseInPrime(pow, t) * -1;

            else if (interpolator == PowOut)
                return EaseOutPrime(pow, t);
            else if (interpolator == PowOutInverted)
                return EaseOutPrime(pow, t) * -1;

            else
                return 0f;
        }
    }

    public List<SubCurve> curves;

    public enum EasingType
    {
        PowIn,
        PowOut,
        SpringIn,
        SpringOut
    }

    private void OnValidate()
    {
        UpdateCurves();
    }

    public void UpdateCurves()
    {
        if (curves != null)
        {
            float totalRelativeLength = 0;
            foreach (SubCurve c in curves)
                totalRelativeLength += c.relativeLength;

            Vector2 lastPos = Vector2.zero;
            foreach (SubCurve sc in curves)
            {
                sc.normalizedLength = sc.relativeLength * (1 / totalRelativeLength);
                sc.start = lastPos;
                sc.end = new Vector2(lastPos.x + sc.normalizedLength, sc.endY);
                lastPos = sc.end;


                bool inverted = sc.end.y < sc.start.y;

                if (sc.easingType == EasingType.PowIn)
                {
                    if (inverted)
                        sc.interpolator = PowInInverted;
                    else
                        sc.interpolator = PowIn;
                }
                else if (sc.easingType == EasingType.PowOut)
                {
                    //if (inverted)
                    //    sc.interpolator = PowOutInverted;
                    //else
                    //    sc.interpolator = PowOut;
                    sc.interpolator = PowOut;
                }
                else if (sc.easingType == EasingType.SpringIn)
                {
                    if (inverted)
                        sc.interpolator = EaseInElasticInverted;
                    else
                        sc.interpolator = EaseInElastic;
                }
                else if (sc.easingType == EasingType.SpringOut)
                {
                    if (inverted)
                        sc.interpolator = EaseOutElasticInverted;
                    else
                        sc.interpolator = EaseOutElastic;
                }

            }
        }
    }

    public static float PowIn(float p, float t)
    {
        return Mathf.Pow(t, p);
    }
    public static float PowOut(float p, float t)
    {
        return 1 - Mathf.Pow(1 - t, p);
    }

    public static float PowInInverted(float p, float t)
    {
        return PowIn(p, t) * -1;
    }
    public static float PowOutInverted(float p, float t)
    {
        return PowOut(p, t) * -1;
    }

    public static float EaseInPrime(float p, float t)
    {
        return p * Mathf.Pow(t, p - 1);
    }
    public static float EaseOutPrime(float p, float t)
    {
        return p * Mathf.Pow(1 - t, p - 1);
    }

    public static float EaseOutElastic(float p, float t)
    {
        float c4 = (2 * Mathf.PI) / 3;

        return Mathf.Pow(p, -10 * t) * Mathf.Sin((t * 10f - 0.75f) * c4) + 1;
    }
    public static float EaseInElastic(float p, float t)
    {
        return 1 - EaseOutElastic(p, 1 - t);
    }
    public static float EaseOutElasticInverted(float p, float t)
    {
        return EaseOutElastic(p, t) * -1;
    }
    public static float EaseInElasticInverted(float p, float t)
    {
        return EaseInElastic(p, t) * -1;
    }


    private SubCurve SubCurveAtTime(float t)
    {
        SubCurve current = null;
        foreach (SubCurve s in curves)
            if (t >= s.start.x)
                current = s;
        return current;
    }
    public float Ferp(float t)
    {
        SubCurve current = SubCurveAtTime(t);

        if (current != null)
            return current.Interpolate(t);
        else
            return 0;
    }
    public Vector3 Verp(Vector3 a, Vector3 b, float t)
    {
        return a + (b - a) * Ferp(t);
    }

    public Quaternion Qerp(Quaternion a, Quaternion b, float t)
    {
        // this doesn't work lol
        return Quaternion.Euler(Verp(a.eulerAngles, b.eulerAngles, t));
    }
}
