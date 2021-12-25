using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using System.Linq;
using Neat.Tools;

public class IKController : MonoBehaviour
{
    public RigBuilder rig;

    private Dictionary<IKJointType, Transform> effectors;
    private Dictionary<IKJointType, Transform> targets;
    private Dictionary<IKJointType, RigLayer> layers;

    private void Start()
    {
        effectors = new Dictionary<IKJointType, Transform>();
        targets = new Dictionary<IKJointType, Transform>();
        layers = new Dictionary<IKJointType, RigLayer>();
        ProcessLayers();
        ProcessEffectors();
    }
    void Update()
    {
        UpdateJoint(IKJointType.Head);
        UpdateJoint(IKJointType.LeftHand);
        UpdateJoint(IKJointType.RightHand);
    }

    void ProcessEffectors()
    {
        var headRig = layers[IKJointType.Head];
        var lookSource = (headRig.constraints[0] as MultiAimConstraint).data.sourceObjects[0].transform;
        effectors.Add(IKJointType.Head, lookSource);

        var leftHandRig = layers[IKJointType.LeftHand];
        var lhSource = (leftHandRig.constraints[0] as TwoBoneIKConstraint).data.target.transform;
        effectors.Add(IKJointType.LeftHand, lhSource);

        var rightHandRig = layers[IKJointType.RightHand];
        var rhSource = (rightHandRig.constraints[0] as TwoBoneIKConstraint).data.target.transform;
        effectors.Add(IKJointType.RightHand, rhSource);
    }
    void ProcessLayers()
    {
        layers[IKJointType.Head] = rig.layers[0];
        layers[IKJointType.LeftHand] = rig.layers[1];
        layers[IKJointType.RightHand] = rig.layers[2];
    }
    public void Equip(IKTargetGroup controller)
    {
        foreach (var x in controller.targets)
        {
            SetTarget(x.targetJoint, x.transform);
        }
    }
    void SetTarget(IKJointType joint, Transform target)
    {
        if (target != null)
        {
            targets[joint] = target;

        }
    }
    void UpdateJoint(IKJointType joint)
    {
        if (targets.TryGetValue(joint, out Transform target))
        {
            if (target != null)
            {
                UpdateTransform(effectors[joint], targets[joint]);
                layers[joint].rig.weight = 1f;
            }
            else
            {
                UnhookJoint(joint);
                layers[joint].rig.weight = 0f;
            }
        }
    }
    void UnhookJoint(IKJointType joint)
    {
        targets.Remove(joint);
    }

    void UpdateTransform(Transform source, Transform target)
    {
        source.position = target.position;
        source.rotation = target.rotation;
    }
}
