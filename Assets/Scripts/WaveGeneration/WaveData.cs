using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WaveData
{
    private int[] _units;
    private int _currentIndex;
    private float _startTime = 0;
    public bool TimeIsOver => WaveTime < Time.time - _startTime;
    public float WaveTime { get; private set; } = 0;
    public int UnitsLeft => _units.Length - _currentIndex;
    public int UnitsCount => _units.Length;
    public bool IsEnd { get; private set; }
    public Vector2? SpawnDelay { get; private set; }

    public WaveData((int id, int count)[] units, bool shuffle = true, Vector2? spawnDelay = null)
    {
        //WaveTime = waveTime;
        SpawnDelay = spawnDelay;
        System.Random rnd = new System.Random();
        List<int> newUnitsList = new List<int>();
        foreach (var unit in units)
        {
            newUnitsList.AddRange(Enumerable.Repeat(unit.id, unit.count));
        }
        if (shuffle)
            _units = newUnitsList.OrderBy(x => rnd.Next()).ToArray();
        else
            _units = newUnitsList.ToArray();
    }
    public void Reset()
    {
        _currentIndex = 0;
        IsEnd = false;
    }

    public int GetNext()
    {
        if (_currentIndex == 0)
            _startTime = Time.time;
        if (IsEnd)
            return 0;
        int result = _units[_currentIndex];
        _currentIndex++;
        if (_currentIndex >= _units.Length)
            IsEnd = true;
        return result;
    }
}
