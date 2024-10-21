using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 10f;
    private Transform _target;
    private int _wavePointIndex;

    private void Start()
    {
        _target = Waypoints.Points[0];
    }

    private void Update()
    {
        Vector3 dir = _target.position - transform.position;
        transform.Translate(dir.normalized * (speed * Time.deltaTime), Space.World);

        if (Vector3.Distance(transform.position, _target.position) <= 0.2f)
        {
            GetNextWayPoint();
        }

        void GetNextWayPoint()
        {
            if (_wavePointIndex >= Waypoints.Points.Length - 1)
            {
                Destroy(gameObject);
                return;
            }
            _wavePointIndex++;
            _target = Waypoints.Points[_wavePointIndex];
        }
    }
}