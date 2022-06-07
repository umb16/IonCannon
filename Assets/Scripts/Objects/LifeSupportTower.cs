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
    public record Circle(Vector2 pos, float radius)
    {
        public List<Vector2> Points;
        public float MedianDistance;
    }
    public class LifeSupportTower : MonoBehaviour
    {
        [SerializeField] float _supportDrainSpeed = .25f;
        [SerializeField] LineRenderer _lineRenderer;
        private List<Circle> _circles = new List<Circle>();
        private AsyncReactiveProperty<Player> _player;
        ComplexStat _stat;
        private List<Vector2> _points = new List<Vector2>();
        [Inject]
        private void Construct(AsyncReactiveProperty<Player> player, GameData gameData)
        {
            // _circles.Add(new Circle(Vector2.zero, 30));
            //_circles.Add(new Circle(new Vector2(0, 0), 25));
            //_circles.Add(new Circle(new Vector2(-10, 10), 25));
            gameData.GameStateChanged += GameStateChanged; ;
            _player = player;
            _player.Where(x => x != null).ForEachAsync(x =>
            {
                _stat = x.StatsCollection.GetStat(StatType.LifeSupport);
            });
            //Test();
            AddCircle(Vector3.zero, 30);
        }

        private void GameStateChanged(GameState state)
        {
            if (state == GameState.Restart)
                Reset();
        }

        private void Reset()
        {
            _circles.Clear();
            AddCircle(Vector3.zero, 30);
        }

        public void AddCircle(Vector2 center, float radius)
        {
            _circles.Add(new Circle(center, radius));
            UpdateVisual();
        }
        private void Test()
        {
            for (int i = 0; i < 50; i++)
            {
                _circles.Add(new Circle(new Vector2(100 * (Random.value - .5f), 100 * (Random.value - .5f)), Random.Range(10, 30)));
            }
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            UpdateVisual();
            stopwatch.Stop();
            Debug.Log("stopwatch "+stopwatch.ElapsedMilliseconds);
        }

        private void UpdateVisual()
        {
            _points.Clear();
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
            while (true)
            {
                //await UniTask.Delay(10);
                
                var currentPoint = currentCircle.Points[currentIndex % currentCircle.Points.Count];
                if ((prevPoint == null
                    || currentCircle.MedianDistance * 1.5f > Vector2.Distance(prevPoint.Value, currentPoint)))
                {
                    if (currentCircle == startCircle && currentIndex > 0 && currentIndex % currentCircle.Points.Count == 0)
                    {
                        Debug.Log("Full loop circle");
                        break;
                    }

                    prevPoint = currentPoint;
                    _lineRenderer.positionCount++;
                    _lineRenderer.SetPosition(lineRendererIndex, currentPoint);
                    currentIndex++;
                    lineRendererIndex++;
                }
                else
                {
                    (Circle circle, float distance, int index) bestCircle;
                    bestCircle.distance = float.PositiveInfinity;
                    bestCircle.circle = currentCircle;
                    bestCircle.index = 0;
                    foreach (var circle in _circles.Where(x=>x!=currentCircle))
                    {
                        for (int i = 0; i < circle.Points.Count; i++)
                        {
                            var distance = Vector2.Distance(prevPoint.Value, circle.Points[i]);
                            if (distance < bestCircle.distance)
                            {
                                bestCircle.distance = distance;
                                bestCircle.circle = circle;
                                bestCircle.index = i;
                            }
                        }
                    }
                    currentCircle = bestCircle.circle;
                    currentIndex = bestCircle.index;
                    prevPoint = null;
                    if (currentCircle == startCircle && currentIndex == 0)
                    {
                        Debug.Log("Full loop circle end index 0");
                        break;
                    }
                }
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
