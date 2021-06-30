using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neet.UI;
using UnityEngine.Events;
using TMPro;
using Neet.Events;

public class _TargetMenu : MonoBehaviour
{
    public _TargetSpawner spawner;
    public _TargetScoreboard scoreboard;

    public TextMeshProUGUI title;

    public AccuracyBar accBar;

    private Canvas canvas;
    private Player player;
    private Motor motor;
    private MouseRotator rotator;

    private UnityAction onPause;
    private UnityAction onResume;

    private PresetProfile profile => PresetProfile.current;

    private void Awake()
    {
        if (PresetProfile.current == null)
            PresetCollection.Load();

        canvas = GetComponent<Canvas>();
        canvas.enabled = true;
    }
    private void Start()
    {
        player = Player.main;

        rotator = player.GetComponent<MouseRotator>();

        SetupMotor();
        scoreboard.ShowResults = ShowResults;


        accBar.Setup(profile.movementProfile, motor);


        // maybe there should be a separation between "resume" and "start"
        onPause = delegate
        {
            PauseListener.isListeningForKey = false;
            canvas.enabled = true;

            // reset transforms
            rotator.ResetRotation();
            rotator.Toggle(false);
            player.ResetTransform();
            spawner.ResetTransform();

            motor.IsInputActive = false;
            motor.ResetTransform();
            motor.Halt();
            spawner.Stop();
            scoreboard.Stop();

            player.GetComponent<WeaponHandler>().gun.enabled = false;
        };

        onResume = delegate
        {
            PauseListener.isListeningForKey = true;
            canvas.enabled = false;

            // give FPS control to player
            rotator.Toggle(true);

            if (profile.movementProfile.canMove)
            {
                motor.IsInputActive = profile.movementProfile.canMove;
                motor.Freeze(false);
            }

            player.GetComponent<WeaponHandler>().gun.enabled = false;
        };

        PauseListener.onPause.AddListener(onPause);
        PauseListener.onResume.AddListener(onResume);

        title.text = PresetProfile.current.name;

        PauseListener.Pause();
    }

    private void OnDestroy()
    {
        PauseListener.onPause.RemoveListener(onPause);
        PauseListener.onResume.RemoveListener(onResume);
    }

    private void SetupMotor()
    {
        motor = player.GetComponent<Motor>();
        motor.ReadMovementProfile(profile.movementProfile);
        motor.SetDefaultKeys();
        motor.runSpeed = 500f;
        motor.shiftToWalk = true;
    }

    public void Play(bool challenge)
    {
        PauseListener.Resume();

        spawner.Play(challenge);
    }

    public void ShowResults()
    {
        GetComponent<UISwitcher>().SwitchTo(1);
    }

    public void ReturnToMenu()
    {
        SceneSwitcher2.instance.SwitchTo(1);
    }
}
