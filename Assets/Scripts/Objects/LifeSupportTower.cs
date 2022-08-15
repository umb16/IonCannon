using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Umb16.Extensions;
using UnityEngine;
using Zenject;
using Debug = UnityEngine.Debug;

namespace SPVD.LifeSupport
{
    public class LifeSupportTower : MonoBehaviour
    {
        [SerializeField] float _supportDrainSpeed = .25f;
        [SerializeField] LineRenderer _lineRenderer;
        private List<Circle> _circles = new List<Circle>();
        private AsyncReactiveProperty<Player> _player;
        ComplexStat _stat;
        [Inject]
        private void Construct(AsyncReactiveProperty<Player> player, GameData gameData)
        {
            // _circles.Add(new Circle(Vector2.zero, 30));
            //_circles.Add(new Circle(new Vector2(0, 0), 25));
            //_circles.Add(new Circle(new Vector2(-10, 10), 25));
            gameData.GameStateChanged += GameStateChanged;
            gameData.GameStarted += GameStarted;
            _player = player;
            _player.Where(x => x != null).ForEachAsync(x =>
            {
                _stat = x.StatsCollection.GetStat(StatType.LifeSupport);
            });
            //Test();
            //AddCircle(Vector3.zero, 30);
        }

        private void GameStarted()
        {
            Reset();
        }

        private void GameStateChanged(GameState state)
        {
            if (state == GameState.Restart)
            {
                _circles.Clear();
                _lineRenderer.positionCount = 0;
            }
        }
        [EditorButton]
        public void AddRandomCircle()
        {
            for (int i = 0; i < 1; i++)
            {
                AddCircle(GetRandomPointOnEdge(), 20);
            }

        }
        [EditorButton]
        public void AddRandomCircle10()
        {
            for (int i = 0; i < 10; i++)
            {
                AddCircle(GetRandomPointOnEdge(), 20);
            }

        }
        [EditorButton]
        private void Reset()
        {
            _circles.Clear();
            AddCircle(Vector3.zero, 30);
        }

        public Vector2 GetNerestPoint(Vector2 point)
        {
            float bestDistance = float.PositiveInfinity;
            Vector2 bestPoint = Vector2.zero;
            foreach (var circle in _circles)
            {
                foreach (var point2 in circle.Points)
                {
                    var distance = (point - point2).sqrMagnitude;
                    if (distance < bestDistance)
                    {
                        bestDistance = distance;
                        bestPoint = point2;
                    }
                }
            }
            return bestPoint;
        }

        public bool InRadius(Vector2 point)
        {
            return _circles.Any(x => (point - x.pos).sqrMagnitude < x.radius * x.radius);
        }

        public Vector2 GetRandomPoint()
        {
            var currentCircle = _circles[Random.Range(0, _circles.Count)];
            return currentCircle.pos + new Vector2(Random.value - .5f, Random.value - .5f).normalized * currentCircle.radius * Random.value;
        }
        public Vector2 GetRandomPointOnEdge()
        {
            var circles = _circles.Where(x => x.Points.Count > 0).ToArray();
            var currentCircle = circles[Random.Range(0, circles.Length)];
            return currentCircle.pos + new Vector2(Random.value - .5f, Random.value - .5f).normalized * currentCircle.radius;
        }
        public void AddCircle(Vector2 center, float radius)
        {
            _circles.Add(new Circle(center, radius, _circles.Count));
            UpdateVisual();
        }
        private void Test()
        {
            for (int i = 0; i < 50; i++)
            {
                _circles.Add(new Circle(new Vector2(100 * (Random.value - .5f), 100 * (Random.value - .5f)), Random.Range(10, 30), i));
            }
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            UpdateVisual();
            stopwatch.Stop();
            Debug.Log("stopwatch " + stopwatch.ElapsedMilliseconds);
        }

        private void UpdateVisual()
        {
            foreach (var circle in _circles)
            {
                var points = GetCirclePoints(circle, 100);
                circle.Points = RemovePointsInCircles(_circles.Where(x => x != circle), points);
                for (int i = 1; i < circle.Points.Count; i++)
                {
                    var currentPoint = circle.Points[i];
                    var prevPoint = circle.Points[i - 1];
                    //Debug.DrawLine(currentPoint, prevPoint, Color.yellow * .5f, 30);
                }
                if (circle.Points.Count < 4)
                {
                    circle.Points.Clear();
                    continue;
                }
                var ordered = circle.Points.Zip(points.Skip(1), (x, y) => Vector2.Distance(x, y)).OrderBy(x => x);
                var median = (ordered.ElementAt((circle.Points.Count - 1) / 2) + ordered.ElementAt((circle.Points.Count - 2) / 2)) / 2;
                circle.MedianDistance = median;
            }

            UpdateCircles2();
        }


        private List<Vector2> GetCirclePoints(Circle circle, int steps)
        {
            List<Vector2> result = new List<Vector2>();
            for (int i = 0; i < steps; i++)
            {
                var newPoint = (Vector2)Vector3Extensions.DiamondToNormalVectorXY(4.0f / steps * i);
                newPoint *= circle.radius;
                newPoint += circle.pos;
                result.Add(newPoint);
            }
            return result;
        }

        private List<Vector2> RemovePointsInCircles(IEnumerable<Circle> circles, List<Vector2> points)
        {
            foreach (var circle in circles)
            {
                for (int i = 0; i < points.Count; i++)
                {
                    var point = points[i];
                    if ((point - circle.pos).SqrMagnitude() < circle.radius * circle.radius)
                    {
                        points.RemoveAt(i);
                        i--;
                    }
                }
            }
            return points;
        }

        private void UpdateCircles2()
        {
            _lineRenderer.positionCount = 0;
            Circle startCircle = _circles.First(x => x.Points.Count > 0);
            var currentCircle = startCircle;
            Vector2? prevPoint = null;
            int currentIndex = 0;
            int lineRendererIndex = 0;

            int index = 0;
            HashSet<int> set = new HashSet<int>();
            while (true)
            {
                //await UniTask.Delay(10);
                var normalIndex = currentIndex % currentCircle.Points.Count;


                var currentPoint = currentCircle.Points[normalIndex];
                if ((prevPoint == null
                    || currentCircle.MedianDistance * 1.5f > Vector2.Distance(prevPoint.Value, currentPoint)))
                {
                    if (set.Contains(normalIndex + currentCircle.ID * 1000000))
                    {
                        Debug.Log("Loop error " + normalIndex + " " + currentCircle.ID);
                        break;
                    }
                    /*if (currentCircle == startCircle && (normalIndex == 0 && currentIndex > 0))
                    {
                        Debug.Log("Full loop circle");
                        break;
                    }*/

                    prevPoint = currentPoint;
                    _lineRenderer.positionCount++;
                    _lineRenderer.SetPosition(lineRendererIndex, currentPoint);

                    set.Add(normalIndex + currentCircle.ID * 1000000);
                    currentIndex++;
                    /*if (currentIndex >= currentCircle.Points.Count)
                        currentIndex = 0;*/
                    lineRendererIndex++;
                }
                else
                {
                    (Circle circle, float distance, int index) bestCircle;
                    bestCircle.distance = float.PositiveInfinity;
                    bestCircle.circle = startCircle;
                    bestCircle.index = 0;
                    foreach (var circle in _circles.Where(x => x != currentCircle))
                    {
                        for (int i = 0; i < circle.Points.Count; i++)
                        {
                            var distance = Vector2.Distance(prevPoint.Value, circle.Points[i]);
                            if (distance < bestCircle.distance)
                            {
                                if (bestCircle.distance == float.PositiveInfinity || !set.Contains(i + circle.ID * 1000000))
                                {
                                    bestCircle.distance = distance;
                                    bestCircle.circle = circle;
                                    bestCircle.index = i;
                                }
                                else
                                {
                                   // Debug.Log("Loop in loop " + normalIndex + " " + currentCircle.ID);
                                }
                            }
                        }
                    }
                    currentCircle = bestCircle.circle;
                    currentIndex = bestCircle.index;
                    prevPoint = null;
                    /* if (currentCircle == startCircle)
                         Debug.Log("to mach return to start " + currentIndex);*/
                    if (currentCircle == startCircle && currentIndex == 0)
                    {
                        Debug.Log("Full loop circle end index 0");
                        break;
                    }
                }
                if (index > 10000)
                {

                    Debug.Log("to mach " + startCircle.Points.Count());
                    break;
                }
                index++;
                //Debug.Log(lineRendererIndex);
            }



        }



        void Update()
        {
            if (_player.Value == null)
                return;
            if (_circles.Any(x => (_player.Value.Position - (Vector3)x.pos).SqrMagnetudeXY() < x.radius * x.radius))
            {
                _stat.AddBaseValue(_supportDrainSpeed * Time.deltaTime * .1f);
            }
            else
            {
                _stat.AddBaseValue(-_supportDrainSpeed * Time.deltaTime);
            }
        }
    }
}
