using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neat.Audio;
using Neat.UI;
using Neat.Extensions;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using Neat.FileManagement;
using UnityEngine.Events;

namespace Neat.AimTrainer
{
    public class TargetSpawner : MonoBehaviour
    {
        // component references
        public GameObject targetPrefab;

        public CurvedZone minLines;

        // zone preview
        public Image zoneTop;
        public Image zoneBottom;
        public Image zoneLeft;
        public Image zoneRight;
        public Image zoneFill;

        public AccuracyBar accBar;
        public TurnIndicator turnIndicator;

        public TargetScoreboard scoreboard;

        // quick references
        private Player player => Player.main;
        private Motor motor => player.GetComponent<Motor>();
        private PresetProfile profile => PresetProfile.current;
        private AimProfile aim => profile.aimProfile;
        private TrackingProfile tracking => profile.trackingProfile;
        private ColorProfile colors => profile.colorProfile;
        private TimingProfile timing => profile.timingProfile;

        // data
        private Camera cam;
        private bool waitingForFirstHit = true;
        private int targetNum = 0;
        private GameObject currentTarget;
        private Coroutine currentSpawnWait;

        private bool isChallenge;

        private Vector3 startPos;
        private List<GameObject> animations;

        // private const float targetDistance = 26.48f; // ???

        private Vector3 localPlayerPos => transform.InverseTransformPoint(cam.transform.position);
        private Vector3 localSpawnOrigin =>
            localPlayerPos + Vector3.forward * aim.distMin;

        // initialization
        private void Awake()
        {
            UserEvent.SetStaticListener(Gun.SHOT_FIRED_KEY, ProcessGunClick);
            UserEvent.SetStaticListener(Gun.ACTIVE_RAYCAST_KEY, ProcessGunTrack);
        }
        private void OnDestroy()
        {
            UserEvent.RemoveStaticListener(Gun.SHOT_FIRED_KEY, ProcessGunClick);
            UserEvent.RemoveStaticListener(Gun.ACTIVE_RAYCAST_KEY, ProcessGunTrack);
        }
        private void Start()
        {
            cam = Camera.main;

            ApplyColorSettings();
            startPos = transform.position;

            animations = new List<GameObject>();

            minLines.DrawLines(aim.yMax, aim.xMax);

            // maxLines code
            {
                //if (aim.useDistRange)
                //{
                //    maxLines.enabled = true;
                //    var farAngles = MaxDistAngles();
                //    maxLines.DrawLines(farAngles.y, farAngles.x);
                //}
                //else
                //{
                //    maxLines.enabled = false;
                //}
            }
        }
        private void FixedUpdate()
        {
            motor.ApplyTransform(profile.movementProfile);
            LogPlayerMovement();
            UpdateTransform();
        }
        private void ApplyColorSettings()
        {
            Camera.main.backgroundColor = colors.backgroundColor;
        }
        private void SetZoneTransform()
        {
            // var parentPos = zoneBottom.transform.parent.position;
            var dist = Vector3.Distance(cam.transform.position, transform.position);

            var x = profile.aimProfile.xMax;
            var y = profile.aimProfile.yMax;

            // determine length
            // z = height
            // x = width
            var xTan = Mathf.Tan(profile.aimProfile.xMax * Mathf.Deg2Rad);
            var width = xTan * dist;

            var yTan = Mathf.Tan(profile.aimProfile.yMax * Mathf.Deg2Rad);
            var height = yTan * dist;

            var bottomRect = zoneBottom.GetComponent<RectTransform>();
            var topRect = zoneTop.GetComponent<RectTransform>();
            var leftRect = zoneLeft.GetComponent<RectTransform>();
            var rightRect = zoneRight.GetComponent<RectTransform>();

            bottomRect.sizeDelta = topRect.sizeDelta = new Vector2(width * 2, 1);
            leftRect.sizeDelta = rightRect.sizeDelta = new Vector2(height * 2, 1);

            topRect.anchoredPosition = new Vector2(0, height);
            bottomRect.anchoredPosition = new Vector2(0, -height);
            leftRect.anchoredPosition = new Vector2(-width, 0);
            rightRect.anchoredPosition = new Vector2(width, 0);

            zoneFill.GetComponent<RectTransform>().sizeDelta
                = new Vector2(width * 2 - 1, height * 2 - 1);

            // zoneMask.transform.localScale = new Vector3(width * 2 - 1, 1, height * 2 - 1);
        }
        private Vector2 MaxDistAngles()
        {
            // draw lines forward from max angles at min distances
            // TODO possible to do without separating x and y
            var distRange = aim.distMax - aim.distMin;

            var horizontalX = Mathf.Sin(aim.xMax * Mathf.Deg2Rad);
            var horizontalZ = Mathf.Cos(aim.xMax * Mathf.Deg2Rad);
            var xMinPos = new Vector3(horizontalX, 0, horizontalZ).normalized * aim.distMin;
            var xMaxPos = xMinPos + transform.forward * distRange;
            var xFarAngle = Mathf.Atan2(xMaxPos.x, xMaxPos.z);

            var verticalY = Mathf.Sin(aim.yMax * Mathf.Deg2Rad);
            var verticalZ = Mathf.Cos(aim.yMax * Mathf.Deg2Rad);
            var yMinPos = new Vector3(0, verticalY, verticalZ);
            var yMaxPos = yMinPos + transform.forward * distRange;
            var yFarAngle = Mathf.Atan2(yMaxPos.y, yMaxPos.z);

            return new Vector2(xFarAngle, yFarAngle);
        }

        public void ResetTransform()
        {
            this.transform.rotation = Quaternion.identity;
            this.transform.position = startPos;
        }
        private void UpdateTransform()
        {
            KeepDistance(startPos.z, cam.transform);
            LookAway(cam.transform);
        }
        private void KeepDistance(float distance, Transform from)
        {
            // https://answers.unity.com/questions/292084/keeping-distance-between-two-gameobjects.html
            transform.position = (transform.position - from.position)
                .normalized * distance + from.position;
        }
        private void LookAway(Transform from)
        {
            // https://answers.unity.com/questions/43001/looking-away-from-a-target.html
            Vector3 direction = transform.position - from.position;
            transform.rotation = Quaternion.LookRotation(direction);
        }
        private void MoveSpawner()
        {
            if (profile.aimProfile.canSpawnRotate)
            {
                var angle = Random.Range(aim.spawnRotateMin, aim.spawnRotateMax);
                var LR = Random.Range(0f, 1f) < .5f ? -1 : 1;
                angle *= LR;
                transform.RotateAround(cam.transform.position, Vector3.up, angle);
            }
        }

        public void Play(bool challenge)
        {
            turnIndicator.enabled = true;
            isChallenge = challenge;
            waitingForFirstHit = true;
            SpawnTarget();
        }
        public void Stop()
        {
            turnIndicator.enabled = false;
            StopAllCoroutines();
            DestroyAnimations();
            currentSpawnWait = null;
            if (currentTarget != null)
                Destroy(currentTarget);
            waitingForFirstHit = true;
        }

        private IEnumerator AnimateTargetKill(GameObject _target, bool success)
        {
            var anim = Instantiate(targetPrefab, transform);
            anim.SetActive(true);
            anim.transform.localScale = Vector3.one;
            var color = profile.colorProfile.targetColor;
            anim.SetColor(color);
            anim.transform.position = _target.transform.position;
            animations.Add(anim);

            float animTime = .2f;
            float startTime = Time.time;
            float startScale = anim.transform.localScale.magnitude;
            float endScale = success ? 1.5f : 0;
            while (Time.time < startTime + animTime)
            {
                var t = (Time.time - startTime) / animTime;
                float alpha = Mathf.Lerp(1, 0, t);
                float scale = Mathf.Lerp(startScale, startScale * endScale, t);

                anim.SetColor(new Color(color.r, color.g, color.b, alpha));
                anim.transform.localScale = Vector3.one.normalized * scale;

                // print(target.GetColor() + "\n" + target.transform.localScale);

                yield return null;
            }

            // print("aa");
            animations.Remove(anim);
            Destroy(anim);
        }

        private void DestroyAnimations()
        {
            foreach (GameObject g in animations)
                Destroy(g);

            animations.Clear();
        }

        // target spawning
        private void NextTarget(GameObject target, bool success)
        {
            StartCoroutine(AnimateTargetKill(target, success));
            target.RemoveData();
            DestroyImmediate(target);
            currentTarget = null;
            WaitForSpawn();
        }
        private void SpawnTarget()
        {
            // to prevent a double spawn
            if (currentTarget != null)
                return;

            // instantiate with scale and color
            var goTarget = Instantiate(targetPrefab, transform);
            goTarget.SetActive(true);
            goTarget.transform.localScale = Vector3.one;
            goTarget.SetColor(colors.targetColor);

            // give data container
            Target t = new Target();
            goTarget.SetData(t);

            // set position and spawner data
            SetTargetPosition(goTarget);
            currentTarget = goTarget;
            turnIndicator.Target = goTarget.transform;

            // for start-target
            if (waitingForFirstHit)
                return;

            // set target data
            t.id = targetNum++;
            t.spawnTime = Time.time;
            t.isTracking = false;

            // handle insta-move and insta-track
            if (tracking.isMoveInstant)
            {
                StartMoving(goTarget);
                if (tracking.isTrackInstant)
                    StartTracking(goTarget);
            }

            // handle click timeout
            if (timing.canClickTimeout && !waitingForFirstHit)
                WaitForClickTimeout(goTarget);
        }
        private void SetTargetPosition(GameObject goTarget)
        {
            if (waitingForFirstHit)
            {
                goTarget.transform.position =
                    cam.transform.position + Vector3.forward * aim.distMin;

                // instead of vector3.forward
                // get the vector to point to the spawner
            }
            else
            {
                // get horizontal angle
                var hRandAngle = Random.Range(profile.aimProfile.xMin, profile.aimProfile.xMax);
                var hRandRad = hRandAngle * Mathf.Deg2Rad;

                // get vertical angle
                var vRandAngle = Random.Range(profile.aimProfile.yMin, profile.aimProfile.yMax);
                var vRandRad = vRandAngle * Mathf.Deg2Rad;

                // get horizontal coords
                int LR = Random.Range(0f, 1f) < .5f ? -1 : 1;
                var hRandZ = Mathf.Cos(hRandRad);
                var hRandX = Mathf.Sin(hRandRad);

                // get vertical coords
                int UD = Random.Range(0f, 1f) < .5f ? -1 : 1;
                var vRandZ = Mathf.Cos(vRandRad);
                var vRandX = Mathf.Sin(vRandRad);

                // get unit vector for decided angle
                var randUnit = new Vector3(hRandX * LR, vRandX * UD, hRandZ).normalized;
                randUnit = transform.rotation * randUnit;

                // get target position at minimum distance
                var minRelativePos = randUnit * aim.distMin;
                var minAbsolutePos = cam.transform.position + minRelativePos;

                // adjust for random distance
                var distDelta = Random.Range(aim.distMin, aim.distMax) - aim.distMin;
                var pos = minAbsolutePos + transform.forward * distDelta;
                var playerDist = Vector3.Distance(cam.transform.position, pos);

                goTarget.transform.position = pos;

                goTarget.GetData<Target>().distance = playerDist;
            }
        }

        // spawns target after profile-defined delay
        private Coroutine WaitForSpawn()
        {
            // to prevent this coroutine occasionally overlapping itself
            if (currentSpawnWait == null)
            {
                MoveSpawner();
                var routine = StartCoroutine(_WaitForSpawn());
                currentSpawnWait = routine;
                return routine;
            }
            else
            {
                return null;
            }
        }
        private IEnumerator _WaitForSpawn()
        {
            if (timing.canDelay)
            {
                float delay = Random.Range(timing.delayMin, timing.delayMax);
                yield return new WaitForSeconds(delay);
            }
            SpawnTarget();
            currentSpawnWait = null; // reset field once complete
        }

        // determine if the goal is click or track to fire attempt
        private void ProcessGunClick(object sender, object receiver)
        {
            if (currentTarget == null || currentTarget.GetData<Target>().isTracking)
                return;

            GameExtensions.Cast(sender, receiver, out Gun g, out GameObject target);

            if (waitingForFirstHit)
            {
                if (Target.IsTarget(target))
                {
                    waitingForFirstHit = false;
                    scoreboard.sounds.Play("click");
                    scoreboard.StartScoring(isChallenge);
                    NextTarget(target, true);
                }
            }
            else
                ClickAttempt(target);
        }
        private void ProcessGunTrack(object sender, object receiver)
        {
            if (currentTarget != null && currentTarget.GetData<Target>().isTracking)
            {
                GameExtensions.Cast(sender, receiver, out Gun g, out GameObject target);
                TrackAttempt(target);
            }
        }
        private void TrackAttempt(GameObject target)
        {
            // successful track
            if (Target.IsTarget(target) && motor.IsAccurate())
            {
                target.GetData<Target>().isTrackingSuccessful = true;
            }
            else //  if (currentTarget != null) // current target should never be null here
            {
                // other stuff happening elsewhere
                currentTarget.GetData<Target>().isTrackingSuccessful = false;
            }
        }
        private void ClickAttempt(GameObject target)
        {
            if (Target.IsTarget(target, out Target _target) && motor.IsAccurate())
            {
                scoreboard.ClickFired(true, _target);

                // only click-destroy on this condition
                if (!tracking.canTrack)
                {
                    scoreboard.ClickDestroyed(target);
                    NextTarget(target, true);
                }

                else
                {
                    if (!tracking.isMoveInstant)
                        StartMoving(target);

                    if (!tracking.isTrackInstant)
                        StartTracking(target);
                }

            }
            else
            {
                scoreboard.ClickFired(false, _target);

                if (aim.failTargetOnMissClick)
                {
                    scoreboard.ClickFail();
                    NextTarget(currentTarget, false);
                }
            }
        }

        private void LogPlayerMovement()
        {
            if (profile.movementProfile.canMove
                && Target.IsTarget(currentTarget, out Target target))
            {
                target.playerDistanceMoved += motor.DeltaDistance;
            }
        }

        // only elapses tracking time
        private Coroutine StartTracking(GameObject target)
        {
            StartCoroutine(_StartTracking(target));

            if (tracking.canTrackTimeout)
                return WaitForTrackTimeout(target);
            else
                return null;
        }
        private IEnumerator _StartTracking(GameObject target)
        {
            Target t = target.GetData<Target>();
            t.trackStartTime = Time.time;
            t.trackSuccessTime = 0f;
            t.isTracking = true;
            t.isTrackingSuccessful = false;

            yield return null; // to not add an unneccesary Time.deltaTime
            while (target != null)
            {
                // every tick while target-tracking active
                scoreboard.TrackElapsed(Time.deltaTime);
                t.trackAttemptTime += Time.deltaTime;

                // during successful track
                if (t.isTrackingSuccessful)
                {
                    t.trackSuccessTime += Time.deltaTime;
                    var timeTracked = t.trackSuccessTime;
                    target.SetColor(colors.trackingColor);
                    scoreboard.TrackSuccess(Time.deltaTime);

                    // target fully-finished tracking?
                    if (tracking.canTrackDestroy && timeTracked >= tracking.timeToDestroy)
                    {
                        scoreboard.TrackDestroyed(target);
                        NextTarget(target, true);
                    }
                }

                // unsuccessful track
                else
                {
                    target.SetColor(colors.targetColor);
                }

                // because it couldn't understand how to deal with 
                // releasing click while tracking
                // there's a better way
                t.isTrackingSuccessful = false;

                yield return null;
            }
        }

        // only moves target while it exists
        private Coroutine StartMoving(GameObject target)
        {
            return StartCoroutine(_StartMoving(target));
        }
        private IEnumerator _StartMoving(GameObject target)
        {
            var rb = target.GetComponent<Rigidbody>();
            var t = target.GetData<Target>();

            float timeSinceTickStart = 0f;
            Vector3 accelVec = GetAccelerationVector(target);
            while (target != null)
            {
                if (timeSinceTickStart > tracking.tickRate)
                {
                    accelVec = GetAccelerationVector(target);
                    timeSinceTickStart = 0f;
                }

                // add accelleration, fix velocity
                rb.AddRelativeForce(accelVec, ForceMode.Acceleration);
                if (rb.velocity.magnitude > tracking.speedMax)
                    rb.velocity = rb.velocity.normalized * tracking.speedMax;
                else if (rb.velocity.magnitude < tracking.speedMin)
                    rb.velocity = rb.velocity.normalized * tracking.speedMin;
                var relativeVelocity = target.transform.InverseTransformVector(rb.velocity);

                // fix transform and direction
                target.transform.localPosition = localPlayerPos +
                    (target.transform.localPosition - localPlayerPos).normalized
                    * t.distance;
                target.transform.LookAt(cam.transform);
                rb.velocity = target.transform.TransformVector(relativeVelocity);


                timeSinceTickStart += Time.fixedDeltaTime;

                yield return new WaitForFixedUpdate();
            }
        }

        // polling for and handling timeouts
        private Coroutine WaitForClickTimeout(GameObject target)
        {
            return StartCoroutine(_WaitForClickTimeout(target));
        }
        private IEnumerator _WaitForClickTimeout(GameObject target)
        {
            var t = target.GetData<Target>();

            // wait for time to elapse
            var startTime = Time.time;
            while (Time.time < startTime + profile.timingProfile.timeout)
                yield return null;
            yield return null; // this fixed a bug or something idk

            // timeout target if it still exists and didn't switch to tracking
            // also detects null
            if (target != null && !t.isTracking)
            {
                scoreboard.ClickTimeout();
                NextTarget(target, false);
            }
        }
        private Coroutine WaitForTrackTimeout(GameObject target)
        {
            return StartCoroutine(_WaitForTrackTimeout(target));
        }
        private IEnumerator _WaitForTrackTimeout(GameObject target)
        {
            // assume target is moving
            var startTime = Time.time;
            while (Time.time < startTime + tracking.trackTimeout)
                yield return null;
            yield return null; // TODO necessary? I don't remember why

            if (target != null)
            {
                scoreboard.TrackTimeout();
                NextTarget(target, false);
            }
        }

        // movement methods
        private bool isOutOfBounds(GameObject target)
        {
            target.transform.LookAt(cam.transform);

            var forward = transform.position - cam.transform.position;

            // check if target is outside of the bounding box
            var relativePos = target.transform.position - cam.transform.position;
            // relativePos = transform.rotation * relativePos;

            relativePos = target.transform.localPosition - localPlayerPos;

            var xAngle = 90 - (Mathf.Atan2(relativePos.z, relativePos.x) * Mathf.Rad2Deg);
            var yAngle = 90 - (Mathf.Atan2(relativePos.z, relativePos.y) * Mathf.Rad2Deg);
            xAngle = Mathf.Abs(xAngle);
            yAngle = Mathf.Abs(yAngle);

            //print(xAngle + ", " + yAngle);

            bool oob = xAngle > profile.aimProfile.xMax || yAngle > profile.aimProfile.yMax;
            return oob;
        }
        private Vector3 GetAccelerationVector(GameObject target)
        {
            // get local direction vector
            Vector3 dir = default;
            float accel = 0f;
            var t = target.GetData<Target>();
            if (isOutOfBounds(target))
            {
                dir = localSpawnOrigin - target.transform.localPosition;

                var localOrigin = cam.transform.position +
                    (transform.position - cam.transform.position).normalized *
                    t.distance;

                dir = (localOrigin - target.transform.position).normalized;

                // since target is facing the opposite direction
                // its left/rights are inverted
                dir = target.transform.localRotation * dir;
                accel = profile.trackingProfile.accelMax;
            }
            else
            {
                // randomize direction according to bounding box
                var x = Random.Range(-profile.aimProfile.xMax, profile.aimProfile.xMax);
                var y = Random.Range(-profile.aimProfile.yMax, profile.aimProfile.yMax);
                dir = new Vector3(x, y);
                accel = Random.Range(profile.trackingProfile.accelMin, profile.trackingProfile.accelMax);
            }

            // apply acceleration to direction vector
            dir = dir.normalized;
            var accel_vec = dir * accel;

            return accel_vec;
        }


    }
}