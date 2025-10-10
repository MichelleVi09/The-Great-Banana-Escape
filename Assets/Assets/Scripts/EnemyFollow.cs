using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    protected GameObject _player;
    [SerializeField] protected float _speed = 1.0f;

    public virtual void Init()
    {
        //sets the enemy's target to the player.
        SetTarget();
    }

    protected virtual void Start()
    {
        Init();
    }

    protected virtual void Update()
    {
        //updates the enemy's position to chase the player.
        Chase();
    }

    protected virtual void SetTarget()
    {
        //finds the player GameObject
        if (GameObject.FindWithTag("Player") != null)
        {
            _player = GameObject.FindWithTag("Player");
        }
        else
        {
            Debug.LogError("Player was not found!!!");
        }
    }

    protected virtual void Chase()
    {
        if (_player == null) return;

        //makes the enemy face the player
        transform.right = _player.transform.position - transform.position;

        //moves toward the player
        transform.position += transform.right * _speed * Time.deltaTime;
    }
}

