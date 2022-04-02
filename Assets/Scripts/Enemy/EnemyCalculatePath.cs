using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCalculatePath : MonoBehaviour
{
    public PersonMovement movement;
    private ActivePath _currentPath;

    public class ActivePath
    {
        public Vector3[] points;
        public Vector3 currentPosition;

        private int _nextPoint = 1;
        private bool _isComplete = false;

        public bool IsComplete => _isComplete;

        private int NextPointIdx => Mathf.Clamp(_nextPoint, 0, points.Length - 1);

        private Vector3 NextPoint => points[NextPointIdx];

        public Vector3 Destination => points[points.Length-1];

        public ActivePath(NavMeshPath path)
        {
            currentPosition = path.corners[0];
            points = path.corners;
        }

        public void DrawPath()
        {
            for (int i = 0; i < points.Length - 1; i++)
                Debug.DrawLine(points[i], points[i + 1], Color.gray);

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

    public void ClearPath()
    {
        _currentPath = null;
    }

    public bool ReachedEndOfPath()
    {
        if (_currentPath == null)
            return false;
            
        return _currentPath.IsComplete;
    }

    public bool ChasePlayer(Vector3 position)
    {
        if (_currentPath == null)
            GeneratePathToPoint(position);

        if (_currentPath == null)
            return false;

        Vector3 currentDestination = _currentPath.Destination;

        // If the path has become stale, regenerate it
        if (Vector3.Distance(currentDestination, position) > 2.5f)
        {
            GeneratePathToPoint(position);
        }

        if (_currentPath == null)
            return false;

        
        MoveAlongPath();
        return true;
    }

    public void GeneratePathToPoint(Vector3 point)
    {
        Debug.Log("Generating path!");
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, point, NavMesh.AllAreas, path);

        if (path != null && path.corners.Length > 0)
        {
            _currentPath = new ActivePath(path);
            _currentPath.Advance(1f);
        }
    }

    void MoveAlongPath()
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
