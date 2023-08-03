using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkLightning
{
    private Vector3 _startCoords;
    private IMob _firstTarget;
    private LineRenderer _lightningPath;

    public PerkLightning(Vector3 startCoords, IMob firstTarget)
    {
        _startCoords = startCoords;
        _firstTarget = firstTarget;
        _lightningPath = new LineRenderer();
    }

    public void CreateLightning()
    {
        _lightningPath.positionCount = 2;
        _lightningPath.SetPosition(0, _startCoords);
        _lightningPath.SetPosition(1, _firstTarget.Position);
    }
}
