using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

// for storing preset settings
// serialized object stored in a list
[System.Serializable]
public class PresetProfile
{
    public static PresetProfile current;

    // default values
    public string name = "default preset";

    public ColorProfile colorProfile;
    public ChallengeProfile challengeProfile;
    public TrackingProfile trackingProfile;
    public AimProfile aimProfile;
    public TimingProfile timingProfile;
    public MovementProfile movementProfile;

    public List<ScoreProfile> scores;

    public PresetProfile()
    {
        scores = new List<ScoreProfile>();

        colorProfile = new ColorProfile();
        challengeProfile = new ChallengeProfile();
        trackingProfile = new TrackingProfile();
        aimProfile = new AimProfile();
        timingProfile = new TimingProfile();
        movementProfile = new MovementProfile();
    }

    public PresetProfile Clone()
    {
        // https://levelup.gitconnected.com/5-ways-to-clone-an-object-in-c-d1374ec28efa
        using (var ms = new MemoryStream())
        {
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(ms, this);
            ms.Seek(0, SeekOrigin.Begin);
            return (PresetProfile)formatter.Deserialize(ms);
        }
    }
}
