using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 10f;
    private Transform _target;
    private int _wavePointIndex;
    private int origHealth;
    private int health = 100;

    [SerializeField] private GameObject[] stages;
    [SerializeField] private Collider collider;

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
                if(health > 0) PlayerStats.DecreaseLives(1);
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
        this.origHealth = health;
    }

    public void isHit(int damage)
    {
        health -= damage;
        float healthPercent = (float)health / origHealth;
        if (healthPercent > 0.66f)
        {
            stages[0].SetActive(true);
        } else if (healthPercent > 0.33f)
        {
            stages[0].SetActive(false);
            stages[1].SetActive(true);
        }
        else
        {
            stages[1].SetActive(false);
            stages[2].SetActive(true);
            this.tag = "Untagged";
            collider.enabled = false;
        }
    }
}