using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerControl : BaseAnimatable
{
    [SerializeField] private Transform _snowPosition;
    private GameBalance GameBalance => GameBalance.Instance;
    private float Hortizontal => SimpleInput.GetAxis("Horizontal");

    private bool _canShoot = true;
    private float _reloadValue;
    
    void Start()
    {
        SetIdle();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        
        if(_canShoot)
            Shoot();
        else
            SetSnowBallReload();
    }

    private bool IsJoystickMoving()
    {
        return Math.Abs(Hortizontal) > 0.2;
    }

    private void Flip()
    {
        Vector3 newRot;
        
        if (Hortizontal > 0)
        {
            newRot = Vector3.up * 0;
        }
        else
        {
            newRot = Vector3.up * 180;
        }

        transform.eulerAngles = newRot;
    }
    
    private void Move()
    {
        if (IsJoystickMoving())
        {
            SetRun();
            Flip();
            
            var newPos = Vector2.right * (Hortizontal * GameBalance.playerSpeed * Time.deltaTime);
            transform.position += (Vector3)newPos;
        }
        else
        {
            SetIdle();
        }
    }

    private void Shoot()
    {
        if (SimpleInput.GetButtonDown("Fire"))
        {
            _canShoot = false;
            SetShoot();
            
            foreach (var snowBall in SnowBallsHolder.snowBalls)
            {
                if(snowBall.activeInHierarchy)
                    continue;
            
                StartCoroutine(ShootBall(snowBall));
                break;
            }
        }
    }

    private IEnumerator ShootBall(GameObject currentBall)
    {
        yield return new WaitForSeconds(GetShootingAnimLength() / 2);
        
        currentBall.SetActive(true);
        currentBall.transform.position = _snowPosition.position;
        SnowBallsHolder.Shoot(currentBall);
    }

    private void SetSnowBallReload()
    {
        _reloadValue += Time.deltaTime;
        SnowBallsHolder.ShowSnowBallReloadStatus(_reloadValue);
        
        if (_reloadValue < GameBalance.snowBallReloadSpeed)
            return;

        _canShoot = true;
        _reloadValue = 0;
    }
    
}
