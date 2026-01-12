using System;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTrajectoryPlayer : MonoBehaviour
{
    [Serializable]
    public class Root
    {
        public List<Phase> phases;
    }

    [Serializable]
    public class Phase
    {
        public List<Waypoint> waypoints;
    }

    [Serializable]
    public class Waypoint
    {
        public float time;
        public Joints joints;
    }

    [Serializable]
    public class Joints
    {
        public float base_rotation;
        public float first_arm_rotation;
        public float second_arm_rotation;
        public float effector_base_rotation;
        public float effector_extension;
        public float spherical_yaw;
        public float spherical_pitch;
        public float needle_extension;
    }

    [Header("JSON")]
    public TextAsset trajectoryJson;

    [Header("Rotating joints")]
    public Transform rotating_cylinder;
    public Transform firstt_arm_joint;
    public Transform second_arm;
    public Transform effector_base_joint;
    public Transform spherical_middle;
    public Transform spherical_end;

    [Header("Sliding joints")]
    public Transform effector_extension;
    public Transform end_effector;

    private List<Waypoint> waypoints = new();
    private float time;

    void Start()
    {
        Root data = JsonUtility.FromJson<Root>(trajectoryJson.text);

        foreach (var p in data.phases)
            waypoints.AddRange(p.waypoints);

        time = waypoints[0].time;
    }

    void Update()
    {
        time += Time.deltaTime;

        for (int i = 0; i < waypoints.Count - 1; i++)
        {
            if (time >= waypoints[i].time && time <= waypoints[i + 1].time)
            {
                float t = Mathf.InverseLerp(
                    waypoints[i].time,
                    waypoints[i + 1].time,
                    time
                );

                Apply(
                    waypoints[i].joints,
                    waypoints[i + 1].joints,
                    t
                );
                break;
            }
        }
    }

    void Apply(Joints a, Joints b, float t)
    {
        // ROTATIONS (radians → degrees)
        rotating_cylinder.localRotation =
            Quaternion.Euler(0, Mathf.Lerp(a.base_rotation, b.base_rotation, t) * Mathf.Rad2Deg, 0);

        firstt_arm_joint.localRotation =
            Quaternion.Euler(Mathf.Lerp(a.first_arm_rotation, b.first_arm_rotation, t) * Mathf.Rad2Deg, 0, 0);

        second_arm.localRotation =
            Quaternion.Euler(Mathf.Lerp(a.second_arm_rotation, b.second_arm_rotation, t) * Mathf.Rad2Deg, 0, 0);

        effector_base_joint.localRotation =
            Quaternion.Euler(0, 0, Mathf.Lerp(a.effector_base_rotation, b.effector_base_rotation, t) * Mathf.Rad2Deg);

        spherical_middle.localRotation =
            Quaternion.Euler(0, Mathf.Lerp(a.spherical_yaw, b.spherical_yaw, t) * Mathf.Rad2Deg, 0);

        spherical_end.localRotation =
            Quaternion.Euler(Mathf.Lerp(a.spherical_pitch, b.spherical_pitch, t) * Mathf.Rad2Deg, 0, 0);

        // SLIDING (meters)
        effector_extension.localPosition =
            new Vector3(0, 0, Mathf.Lerp(a.effector_extension, b.effector_extension, t));

        end_effector.localPosition =
            new Vector3(0, 0, Mathf.Lerp(a.needle_extension, b.needle_extension, t));
    }
}
