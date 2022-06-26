using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SAnimator : MonoBehaviour
{
    [SerializeField] SpriteRenderer _spriteRenderer;
    public List<SAnimation> Animations;
    [SerializeField] SAnimation _currentAnimation;

    private float _nextFrameTime;
    private int _currentFrame;
    private bool _isStopped;
    private IDisposable _loop;
    private Dictionary<string, Action> _actions = new Dictionary<string, Action>();

    private void Start()
    {
        _loop = UniTaskAsyncEnumerable.EveryUpdate().Subscribe(_ =>
        {
            if (_nextFrameTime <= Time.time && !_isStopped)
                NextFrame();
        });
        if (_currentAnimation != null)
        {
            PlayAnim(_currentAnimation);
        }
    }

    public void PlayAnim(string name)
    {
        PlayAnim(Animations.First(x => x.name == name));
    }
    public void PlayAnim(SAnimation anim)
    {
        _isStopped = anim.Stopped;
        _currentAnimation = anim;
        if (_currentAnimation.RandomStart)
            _currentFrame = Random.Range(0, _currentAnimation.FrameCount);
        else
            _currentFrame = 0;
        ShowFrame(_currentFrame);
    }

    public void NextFrame()
    {
        _currentFrame++;
        if (_currentFrame >= _currentAnimation.FrameCount)
        {
            if (_currentAnimation.Loop)
            {
                _currentFrame = 0;
                ShowFrame(_currentFrame);
            }
            else
            {
                _isStopped = true;
            }
        }
        else
        {
            ShowFrame(_currentFrame);
        }
    }
    private void ShowFrame(int frameIndex)
    {
        var currentFrame = _currentAnimation.Frames[frameIndex];
        if (_actions.TryGetValue(currentFrame.Name, out var value))
            value?.Invoke();
        _spriteRenderer.sprite = currentFrame.Sprite;
        if(_currentAnimation.RandomStartTime)
            _nextFrameTime = Time.time + currentFrame.Duration / 1000f*Random.value;
        else
            _nextFrameTime = Time.time + currentFrame.Duration / 1000f;
    }

    public void AddAction(string name, Action action)
    {
        if(!_actions.ContainsKey(name))
            _actions.Add(name, null);
        _actions[name] += action;
    }

    private void OnDestroy()
    {
        _loop?.Dispose();
    }
}
