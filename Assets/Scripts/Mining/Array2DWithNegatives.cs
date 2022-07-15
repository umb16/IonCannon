public class Array2DWithNegatives<T>
{
    public int PX { get; private set; }
    public int PY { get; private set; }
    public int NX { get; private set; }
    public int NY { get; private set; }

    private T[,] _p;
    private T[,] _n;
    private T[,] _pn;
    private T[,] _np;

    public Array2DWithNegatives(int positiveX, int positiveY, int negativeX, int negativeY)
    {
        PX = positiveX;
        PY = positiveY;
        NX = -negativeX;
        NY = -negativeY;
        _p = new T[positiveX, positiveY];
        _pn = new T[positiveX, negativeY];
        _np = new T[negativeX, positiveY];
        _n = new T[negativeX, negativeY];
    }
    public bool InBounds(int x, int y)
    {
        return x >= NX && y >= NY && x < PX && y < PY;
    }

    public T this[int index1, int index2]
    {
        get
        {
            if (index1 >= 0 && index2 >= 0)
                return _p[index1, index2];
            if (index1 < 0 && index2 >= 0)
                return _np[-(index1 + 1), index2];
            if (index1 >= 0 && index2 < 0)
                return _pn[index1, -(index2 + 1)];
            return _n[-(index1 + 1), -(index2 + 1)];
        }
        set
        {

            if (index1 >= 0 && index2 >= 0)
                _p[index1, index2] = value;
            else if (index1 < 0 && index2 >= 0)
                _np[-(index1 + 1), index2] = value;
            else if (index1 >= 0 && index2 < 0)
                _pn[index1, -(index2 + 1)] = value;
            else
                _n[-(index1 + 1), -(index2 + 1)] = value;
        }
    }
}
