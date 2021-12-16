using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Neat.InputHelpers;

public class Motor : MonoBehaviour
{
    public enum MotorType
    {
        // more??
        Free,
        Humanoid
    }
    public MotorType motorType;

    public MovementProfile p;


    [Range(0, 10f)]
    public float gravityScale = 1.0f;
    public static float globalGravity = -9.81f;

    /// <summary>
    /// Will a passive gravity be applied?
    /// </summary>
    public bool hasGravity;

    [Range(1, 50)]
    public float acceleration = 10f;
    [Range(1, 50)]
    public float deceleration = 10f;
    [Range(1, 50)]
    public float runSpeed = 10f;
    [Range(1, 50)]
    public float walkSpeed = 5f;
    [Range(1, 50)]
    public float jumpSpeed = 10f;

    /// <summary>
    /// Is the shift-modifier to run or to walk?
    /// </summary>
    public bool shiftToWalk;




    public Vector3 deltaPosition { get; private set; }
    private Vector3 lastPosition;

    private float deltaDistance;
    public float DeltaDistance
    {
        get { return deltaDistance; }
        private set { deltaDistance = value; }
    }

    [HideInInspector] public bool isPressing; // obsolete

    public event UnityAction<Transform> onTransformUpdate;

    

    private Animator anim;
    public Rigidbody rb;
    private Vector3 startPosition;
    private Quaternion startRotation;

    private MovementProfile profile;



    private Vector3 input;

    private bool _isInputActive;

    /// <summary>
    /// Returns the magnitude of the rigidbody's overall velocity
    /// </summary>
    public float Speed
    {
        get
        {
            // return rb.velocity.magnitude;
            return TransformSpeed;
        }
    }
    /// <summary>
    /// True: component will listen for input and apply it<br/>
    /// False: component will still apply passive gravity / deceleration
    /// </summary>
    public bool IsInputActive
    {
        get
        {
            return _isInputActive;
        }
        set
        {
            _isInputActive = value;

            //if (value)
            //{
            //    ProcessProfileRoutine();
            //}
            //else
            //{
            //    if (process != null)
            //    {
            //        StopCoroutine(process);
            //        process = null;
            //    }

            //}
        }
    }

    public float TransformSpeed => TransformVelocity.magnitude;

    [HideInInspector] public Vector3 TransformVelocity = Vector3.zero;

    private Keybinds _keybinds;
    public Keybinds keybinds => _keybinds ??= GetComponent<KeybindsComponent>().keybinds;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        deltaPosition = Vector3.zero;
        lastPosition = transform.position;

        SetTransformStart();
    }
    private void Update()
    {
        ReadInput();
        // ProcessProfile(profile);
    }
    private void FixedUpdate()
    {
        // ProcessProfile(profile);

        // onTransformUpdate?.Invoke(transform);

        if (hasGravity)
        {
            Vector3 gravity = globalGravity * gravityScale * Vector3.up;
            rb.AddForce(gravity, ForceMode.Acceleration);
        }
    }

    /// <summary>
    /// Set movement settings
    /// </summary>
    public void ReadMovementProfile(MovementProfile m)
    {
        profile = m;
    }

    /// <summary>
    /// Resets position and rotation to start values
    /// </summary>
    public void ResetTransform()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;
    }

    /// <summary>
    /// Sets current values as return-point for ResetTransform method
    /// </summary>
    public void SetTransformStart()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    /// <summary>
    /// True: Fully locks rigidbody contraints<br/>
    /// False: Typical X-Z rotation lock
    /// </summary>
    public void Freeze(bool value)
    {
        var locked = RigidbodyConstraints.FreezeAll;
        var unlockedNoY = RigidbodyConstraints.FreezePositionY |
            RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezeRotationZ;

        // this is the more general case, but not what I want right now.
        // var unlockedFull = RigidbodyConstraints.FreezeRotationX |
        //    RigidbodyConstraints.FreezeRotationZ;

        if (value)
        {
            rb.constraints = locked;
        }
        else
        {
            rb.constraints = unlockedNoY;
        }
    }

    public void Halt()
    {
        TransformVelocity = Vector3.zero;
        input = Vector3.zero;
    }

    // obsolete - use movement profile of some sort
    private void ProcessHumanoidMotor()
    {
        // get input and get relative forward
        Vector3 rawInput = ReadInput();
        Vector3 dirInput = rb.transform.rotation * rawInput;
        Vector3 dirLatInput = new Vector3(dirInput.x, 0, dirInput.z);

        Vector3 vAccel = dirLatInput * runSpeed * acceleration * Time.deltaTime;
        Vector3 vDeccel = rb.velocity * deceleration * Time.deltaTime;

        Vector3 v = rb.velocity + vAccel - vDeccel;

        //bool shiftPressed = Input.GetKey(KeyCode.LeftShift);
        bool shiftPressed = keybinds.FPS.WalkRun.IsPressed();

        bool isWalking = (shiftPressed && shiftToWalk) || (!shiftPressed && !shiftToWalk);

        float targetSpeed = isWalking ? walkSpeed : runSpeed;

        if (v.magnitude > targetSpeed)
            v = v.normalized * targetSpeed;
        rb.velocity = new Vector3(v.x, rb.velocity.y, v.z);

        if (keybinds.FPS.Jump.WasPerformedThisFrame())
            //if (Input.GetKeyDown(KeyCode.Space))
            rb.velocity += Vector3.up * jumpSpeed;

        Vector3 lateral = new Vector3(v.x, 0, v.z);
        // fixed nonrelative velocity in animator
        // https://answers.unity.com/questions/193398/velocity-relative-to-local.html
        var locVel = transform.InverseTransformDirection(lateral);

        if (anim != null)
        {
            //anim.SetFloat("Speed", rb.velocity.magnitude / maxSpeed);
            anim.SetBool("isWalking", lateral.magnitude > .1f);
            anim.SetFloat("ForwardSpeed", locVel.z / runSpeed);
            anim.SetFloat("LateralSpeed", locVel.x / runSpeed);
        }
    }
    private void ProcessFreeMotor()
    {
        Vector3 rawInput = ReadInput();
        Vector3 rotatedInput = rb.transform.rotation * rawInput;

        Vector3 v = rb.velocity + rotatedInput * runSpeed * acceleration * Time.deltaTime
            - rb.velocity * deceleration * Time.deltaTime;

        //bool shiftPressed = Input.GetKey(KeyCode.LeftShift);
        bool shiftPressed = keybinds.FPS.WalkRun.IsPressed();
        bool isWalking = (shiftPressed && shiftToWalk) || (!shiftPressed && !shiftToWalk);

        float targetSpeed = isWalking ? walkSpeed : runSpeed;

        if (v.magnitude > targetSpeed)
            v = v.normalized * targetSpeed;

        rb.velocity = new Vector3(v.x, v.y, v.z);
    }
    public void ProcessProfile(MovementProfile m)
    {
        var relativeInput = transform.rotation * input;
        var accelTick = profile.Acceleration * Time.deltaTime;

        Vector3 v;

        // decelerate
        if (relativeInput == Vector3.zero)
        {
            if (rb.velocity == Vector3.zero)
                return;

            //var t = Mathf.InverseLerp(profile.maxSpeed, 0, rb.velocity.magnitude);
            //t += Time.deltaTime / profile.accelRate;
            //var newSpeed = Mathf.Lerp(profile.maxSpeed, 0, t);
            //v = rb.velocity.normalized * newSpeed;

            v = rb.velocity - rb.velocity.normalized * accelTick;

            // prevent wobbling around 0
            if (v.magnitude > rb.velocity.magnitude)
                v = Vector3.zero;
        }
        // accelerate
        else
            v = rb.velocity + relativeInput.normalized * accelTick;



        //print(v);

        if (v.magnitude > profile.maxSpeed)
            v = v.normalized * profile.maxSpeed;

        rb.velocity = new Vector3(v.x, 0, v.z);


    }

    // call in fixed update
    public void ApplyTransform(MovementProfile m)
    {
        Vector3 dirInput = transform.rotation * input;
        var accelTick = m.Acceleration * Time.fixedDeltaTime;

        // find new velocity
        Vector3 newVelocity;

        if (dirInput == Vector3.zero)
        {
            if (TransformVelocity == Vector3.zero)
                return;

            // new velocity after decelerating
            newVelocity = TransformVelocity - TransformVelocity.normalized * accelTick;

            // prevent wobbling around 0
            if (newVelocity.magnitude > TransformVelocity.magnitude)
                newVelocity = Vector3.zero;
        }
        // accelerate
        else
            // new velocity after accelerating
            newVelocity = TransformVelocity + dirInput.normalized * accelTick;

        if (newVelocity.magnitude > profile.maxSpeed)
            newVelocity = newVelocity.normalized * profile.maxSpeed;

        TransformVelocity = new Vector3(newVelocity.x, 0, newVelocity.z);

        transform.position += TransformVelocity * Time.fixedDeltaTime;

        UpdateDelta();
    }


    private void UpdateDelta()
    {
        deltaPosition = transform.position - lastPosition;
        DeltaDistance = deltaPosition.magnitude;
        lastPosition = transform.position;
    }

    public bool IsAccurate()
    {
        //return rb.velocity.magnitude / profile.maxSpeed <= profile.accuracyRate;

        return !profile.useAccuracyRate
            || TransformSpeed / profile.maxSpeed <= profile.accuracyRate;
    }




    private Vector3 ReadInput()
    {
        if (!IsInputActive)
            return Vector3.zero;

        Vector3 input = keybinds.FPS.Move3D.ReadValue<Vector3>();
        isPressing = input.magnitude > 0;
        this.input = input;
        return input;
    }

    // experimental
    private Coroutine process;
    private Curve accelCurve;
    private Curve decelCurve;

    private bool _isInputIndependent;
    /// <summary>
    /// True:  component will accept and apply input independent of other scripts
    /// <br/>False: component can only be controlled by other components
    /// </summary>
    public bool IsInputIndependent
    {
        get { return _isInputIndependent; }
        set { _isInputIndependent = value; }
    }
    private void makeCurves(MovementProfile profile)
    {
        accelCurve = new Curve();
        Curve.SubCurve accel = new Curve.SubCurve();
        accel.easingType = Curve.EasingType.PowIn;
        accel.pow = profile.accelPow;
        accel.start = Vector2.zero;
        accel.end = new Vector2(profile.accelRate, profile.maxSpeed);
        accelCurve.curves = new List<Curve.SubCurve>() { accel };

        decelCurve = new Curve();
        Curve.SubCurve decel = new Curve.SubCurve();
        decel.easingType = Curve.EasingType.PowIn;
        decel.pow = profile.decelPow;
        decel.start = Vector2.zero;
        decel.end = new Vector2(profile.accelRate, profile.maxSpeed);
        decelCurve.curves = new List<Curve.SubCurve>() { decel };
    }
    private void Accel(Vector3 target, MovementProfile profile)
    {
        //if (rb.velocity != Vector3.zero)
        //    print("Velocity: " + rb.velocity.ToString());

        // accelerate or decelerate toward the target velocity
        // add a tiny float value to the existing velocity in the target direction
        // decelerate if the result has a smaller magnitude

        //bool decel;
        //if (Vector3.Equals(targetVelocity, Vector3.zero))
        //{
        //    decel = true;
        //}
        //else
        //{
        //    var testVec = targetVelocity.normalized * float.MinValue;
        //    var result = rb.velocity + testVec;
        //    decel = result.magnitude < rb.velocity.magnitude;
        //}

        //if (decel)
        //    Decel(targetVelocity, m);
        //else
        //    Accel(targetVelocity, m);

        float currentSpeed = rb.velocity.magnitude;
        float y = currentSpeed / profile.maxSpeed;

        float pow = profile.accelPow;

        // gets t [0,1]
        float t = Mathf.Pow(y, 1 / pow)
            + (Time.deltaTime / profile.accelRate);

        // should already be clamped
        float newSpeed = Mathf.Lerp(0, profile.maxSpeed, Mathf.Pow(t, pow));
        float velChange = newSpeed - currentSpeed;
        Vector3 newVel = rb.velocity + (target.normalized * velChange);
        if (newVel.magnitude > target.magnitude)
            newVel = newVel.normalized * target.magnitude;
        rb.velocity = newVel;
    }
    private void Decel(Vector3 target, MovementProfile profile)
    {
        float currentSpeed = rb.velocity.magnitude;
        float y = currentSpeed / profile.maxSpeed;

        float pow = profile.decelPow;

        // gets t [0,1]
        float t = Mathf.Pow(y, 1 / pow)
            + (Time.deltaTime / profile.accelRate);

        // should already be clamped
        float newSpeed = Mathf.Lerp(0, profile.maxSpeed, Mathf.Pow(t, pow));
        float velChange = newSpeed - currentSpeed;
        Vector3 newVel = rb.velocity + (target.normalized * velChange);
        if (newVel.magnitude > target.magnitude)
            newVel = newVel.normalized * target.magnitude;
        rb.velocity = newVel;
    }
    private float FindPowT(float pow, float scaledY)
    {
        return Mathf.Pow(scaledY, 1 / pow);
    }
    private void ShowAccuracy(KeyCode input)
    {
        IEnumerator routine()
        {

            float startTime = Time.time;
            while (IsAccurate())
                yield return new WaitForFixedUpdate();
            float timeTaken = Time.time - startTime;
            print("Took " + timeTaken + "s to become inaccurate with speed "
                + rb.velocity.magnitude.ToString());

            while (Input.GetKey(input))
                yield return null;

            startTime = Time.time;
            while (!IsAccurate())
                yield return new WaitForFixedUpdate();
            timeTaken = Time.time - startTime;
            print("Took " + timeTaken + "s to become accurate with speed "
                + rb.velocity.magnitude.ToString()); ;
        };

        StartCoroutine(routine());
    }
    private void ShowTiming(KeyCode input)
    {
        IEnumerator routine()
        {

            float startTime = Time.time;
            while (rb.velocity.magnitude < profile.maxSpeed)
                yield return null;
            float timeTaken = Time.time - startTime;
            print("Took " + timeTaken + "s to reach full speed");

            while (Input.GetKey(input))
                yield return null;

            startTime = Time.time;
            while (rb.velocity != Vector3.zero)
            {
                // print(rb.velocity.magnitude);
                yield return null;
            }
            timeTaken = Time.time - startTime;
            print("Took " + timeTaken + "s to stop");
        };

        StartCoroutine(routine());
    }
    private void ProcessProfileRoutine()
    {
        process = StartCoroutine(_ProcessProfile(profile));
    }
    private IEnumerator _ProcessProfile(MovementProfile m)
    {
        // TODO won't trigger event if input suddenly turned off
        while (IsInputActive)
        {
            var relativeInput = transform.rotation * ReadInput();
            var targetVelocity = relativeInput * m.maxSpeed;
            var targetSpeed = targetVelocity.magnitude;
            var accelTick = profile.Acceleration * Time.deltaTime;

            Vector3 v;
            // if decelerate
            if (relativeInput == Vector3.zero)
            {
                if (rb.velocity == Vector3.zero)
                {
                    yield return new WaitForFixedUpdate();
                    continue;
                }

                //var t = Mathf.InverseLerp(profile.maxSpeed, 0, rb.velocity.magnitude);
                //t += Time.deltaTime / profile.accelRate;
                //var newSpeed = Mathf.Lerp(profile.maxSpeed, 0, t);
                //v = rb.velocity.normalized * newSpeed;

                print("here");
                v = rb.velocity - rb.velocity.normalized * accelTick;
                if (v.magnitude > rb.velocity.magnitude)
                    v = Vector3.zero;
            }
            // accelerate
            else
                v = rb.velocity + relativeInput.normalized * accelTick;



            //print(v);

            if (v.magnitude > profile.maxSpeed)
                v = v.normalized * profile.maxSpeed;

            rb.velocity = new Vector3(v.x, 0, v.z);

            yield return new WaitForFixedUpdate();
            deltaPosition = transform.position - lastPosition;
            lastPosition = transform.position;
            onTransformUpdate?.Invoke(transform);
        }
    }

}
