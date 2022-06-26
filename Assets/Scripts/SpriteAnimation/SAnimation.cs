using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SAnimation", menuName = "ScriptableObjects/SAnimation")]
public class SAnimation : ScriptableObject
{
    public List<SFrame> Frames = new List<SFrame>();
    public bool RandomStart;
    public bool RandomStartTime;
    public bool Loop;
    public bool Stopped;
    public int FrameCount => Frames.Count;
}
