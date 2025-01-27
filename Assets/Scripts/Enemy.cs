using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 10f;
    private Transform _target;
    private int _wavePointIndex;
    private int health = 100;
    
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
                PlayerStats.DecreaseLives(1);
                Destroy(gameObject);
                return;
            }
            _wavePointIndex++;
            _target = Waypoints.Points[_wavePointIndex];
        }
    }

    public void setHealth(int health)
    {
        this.health = health;
    }

    public void isHit(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            PlayerStats.IncreaseMoney(15);
            Destroy(gameObject);
        }
    }
}