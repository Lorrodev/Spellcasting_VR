using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastInfo
{
    private CastPoint castPoint;
    private Rune rune;
    private float scaleFactor;
    private float speedFactor;
    private float delta;

    public void SetCastPoint(CastPoint cp)
    {
        castPoint = cp;
    }

    public void SetRune(Rune r)
    {
        rune = r;
    }

    public void SetScaleFactor(float scf)
    {
        scaleFactor = scf;
    }

    public void SetSpeedFactor(float spf)
    {
        speedFactor = spf;
    }

    public void SetDelta(float d)
    {
        delta = d;
    }

    public CastPoint GetCastPoint()
    {
        return castPoint;
    }

    public Rune GetRune()
    {
        return rune;
    }

    public float GetScaleFactor()
    {
        return scaleFactor;
    }

    public float GetSpeedFactor()
    {
        return speedFactor;
    }

    public float GetDelta()
    {
        return delta;
    }
}
