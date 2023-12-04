
using System;
using UnityEngine;
public abstract class BaseLoop
{
    private PID loop_x, loop_y, loop_z;
    private float clampVal;
    public BaseLoop(PID loop_x, PID loop_y, PID loop_z, float clampVal)
    {
        this.loop_x = loop_x;
        this.loop_y = loop_y;
        this.loop_z = loop_z;
        this.clampVal = clampVal;
    }

    public Vector3 Step(Vector3 current, Vector3 desire, Vector3 passives)
    {
        Vector3 u = new Vector3(
            this.loop_x.Step(current.x, desire.x),
            this.loop_y.Step(current.y, desire.y),
            this.loop_z.Step(current.z, desire.z)
        );

        // fight against passive forces
        u -= passives;


        u.x = Math.Min(this.clampVal, Math.Max(-this.clampVal, u.x));
        u.y = Math.Min(this.clampVal, Math.Max(-this.clampVal, u.y));
        u.z = Math.Min(this.clampVal, Math.Max(-this.clampVal, u.z));

        return u;
    }
}

public class PositionLoop : BaseLoop
{
    public PositionLoop(PIDLateral loop_x, PIDLateral loop_y, PIDLateral loop_z, float clampVal) : base(loop_x, loop_y, loop_z, clampVal)
    {

    }
}

public class RotationLoop : BaseLoop
{
    public RotationLoop(PIDRotation loop_x, PIDRotation loop_y, PIDRotation loop_z, float clampVal) : base(loop_x, loop_y, loop_z, clampVal)
    {
    }
}