
using System;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

public abstract class PID
{
    private float kp, ki, kd, max_i, last_error, cum_error;
    public PID(float kp, float ki, float kd, float max_i)
    {
        this.kp = kp;
        this.ki = ki;
        this.kd = kd;
        this.max_i = max_i;
        this.last_error = 0;
        this.cum_error = 0;
    }

    protected abstract float Wrap(float input);
    public float Step(float current, float desire)
    {
        float error = this.Wrap(desire - current);
        float time_delta = Time.deltaTime;

        // integrate error, clip to prevent windup
        this.cum_error += error * time_delta;
        Debug.Log(Time.deltaTime);
        this.cum_error = Math.Min(Math.Max(this.cum_error, -max_i), max_i);

        // evaluate PID
        float comp_p = this.kp * error;
        float comp_i = this.ki * this.cum_error;
        float comp_d = this.kd * (error - this.last_error) / Math.Max(time_delta, 0.01f);

        this.last_error = error;

        // return control output
        return comp_p + comp_i + comp_d;
    }
}

public class PIDLateral : PID
{
    public PIDLateral(float kp, float ki, float kd, float max_i) : base(kp, ki, kd, max_i)
    {
    }

    protected override float Wrap(float input)
    {
        return input;
    }
}

public class PIDRotation : PID
{
    public PIDRotation(float kp, float ki, float kd, float max_i) : base(kp, ki, kd, max_i)
    {
    }

    protected override float Wrap(float input)
    {
        float thing = input + 180;
        while (thing < 0) thing += 360;
        while (thing >= 360) thing -= 360;
        return thing - 180;
    }
}