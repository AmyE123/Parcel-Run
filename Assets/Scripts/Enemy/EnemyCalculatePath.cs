using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCalculatePath : MonoBehaviour
{
    public Transform target;
    public PersonMovement movement;

    private ActivePath _currentPath;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("EnemyAdvanceMovement", 0f, 1f);

        if (target == null)
        {
            target = GameObject.Find("Player").transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentPath != null)
        {
            Vector3 direction = _currentPath.currentPosition - transform.position;
            direction.y = 0;

            movement.SetDesiredDirection(direction.normalized);

            if (direction.magnitude < 1.25f)
            {
                _currentPath.Advance(Time.deltaTime * movement.MaxSpeed);
            }
            _currentPath.DrawPath();
        }
    }

    void EnemyAdvanceMovement()
    {
        GeneratePathToPoint(target.position);
    }

    public void GeneratePathToPoint(Vector3 point)
    {
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, point, NavMesh.AllAreas, path);

        if (path != null && path.corners.Length > 0)
        {
            _currentPath = new ActivePath(path);
            _currentPath.Advance(1f);
        }
    }

    public class ActivePath
    {
        public Vector3[] points;
        public Vector3 currentPosition;

        private int _nextPoint = 1;
        private bool _isComplete = false;

        public bool IsComplete => _isComplete;

        private int NextPointIdx => Mathf.Clamp(_nextPoint, 0, points.Length - 1);

        private Vector3 NextPoint => points[NextPointIdx];

        public ActivePath(NavMeshPath path)
        {
            currentPosition = path.corners[0];
            points = path.corners;
        }

        public void DrawPath()
        {
            for (int i = 0; i < points.Length - 1; i++)
                Debug.DrawLine(points[i], points[i + 1], Color.blue);

            Debug.DrawLine(currentPosition, currentPosition + Vector3.up, Color.red);
        }

        public void Advance(float distanceRemaining)
        {
            if (_isComplete)
                return;

            // basically a while loop but safer because I don't have time to deal with unity crashing
            for (int i = 0; i < 128; i++)
            {
                if (_nextPoint > NextPointIdx)
                {
                    _isComplete = true;
                    break;
                }

                Vector3 vecToNext = NextPoint - currentPosition;

                if (vecToNext.magnitude < distanceRemaining)
                {
                    currentPosition = NextPoint;
                    distanceRemaining -= vecToNext.magnitude;
                    _nextPoint++;
                    continue;
                }

                Vector3 amountToMove = vecToNext.normalized * distanceRemaining;
                currentPosition += amountToMove;
                break;
            }
        }
    }
}
