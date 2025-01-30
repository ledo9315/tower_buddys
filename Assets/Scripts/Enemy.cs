using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float originalSpeed = 4;
    private float currSpeed = 0;
    private Transform _target;
    private int _wavePointIndex;
    private float origHealth;
    private float health = 100;
    private float slowedTimer = 0;
    
    [SerializeField] private GameObject[] stages;
    [SerializeField] private Collider collider;

    private void Start()
    {
        _target = Waypoints.Points[0];
        currSpeed = originalSpeed;
    }

    private void Update()
    {
        Vector3 dir = _target.position - transform.position;
        transform.Translate(dir.normalized * (currSpeed * Time.deltaTime), Space.World);

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

        if (slowedTimer > 0)
        {
            slowedTimer -= Time.deltaTime;
        }
        else
        {
            slowedTimer = 0;
            currSpeed = originalSpeed;
        }
    }

    public void setHealth(float health)
    {
        this.health = health;
        this.origHealth = health;
    }

    public void setSpeedMultiplier(float speedMultiplier)
    {
        originalSpeed *= speedMultiplier;
    }

    public void isHit(float damage)
    {
        health -= damage;
        float healthPercent = (float)health / origHealth;
        if (healthPercent > 0.5f)
        {
            stages[0].SetActive(true);
        } else if (healthPercent > 0f)
        {
            stages[0].SetActive(false);
            stages[1].SetActive(true);
        }
        else
        {
            PlayerStats.IncreaseMoney(6);
            stages[1].SetActive(false);
            stages[2].SetActive(true);
            this.tag = "Untagged";
            collider.enabled = false;
        }
    }

    public void isSlowed(float effect)
    {
        slowedTimer = 4;
        Debug.Log(effect);
        currSpeed = originalSpeed / (effect / 4f);
    }
}