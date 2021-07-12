using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBallsHolder : MonoBehaviour
{
    [SerializeField] private ProgressBarPro _snowBallStatus;

    [SerializeField] private GameObject _snowBall;
    [SerializeField] private int _numOfSnowBalls;
    
    public static ForceMeter forceMeter;
    
    public static List<GameObject> snowBalls = new List<GameObject>();

    public static Vector3 initBallPosition = Vector3.one * 200;
    
    private SnowBallsHolder _instance;

    private static float _snowBallForce = 1000;
    private static float _snowBallStatusValue;
    
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
            InitParams();
        }
        else
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        _snowBallStatus.Value = _snowBallStatusValue / GameBalance.Instance.snowBallReloadSpeed;
    }

    private void InitParams()
    {
        _snowBallStatusValue = GameBalance.Instance.snowBallReloadSpeed;
        forceMeter = FindObjectOfType<ForceMeter>();
        CreateSnowBalls();
    }

    private void CreateSnowBalls()
    {
        for (int i = 0; i < _numOfSnowBalls; i++)
        {
            var snow = Instantiate(_snowBall, initBallPosition, Quaternion.identity);
            snow.transform.SetParent(transform);
            snow.SetActive(false);
            snowBalls.Add(snow);
        }
    }

    public static void Shoot(GameObject currentBall)
    {
        var rb = currentBall.GetComponent<Rigidbody2D>();
        rb.gravityScale = 1;
        rb.AddForce(Vector2.up * (forceMeter.GetProgressBarValue() * _snowBallForce));
    }

    public static void ShowSnowBallReloadStatus(float timeValue)
    {
        _snowBallStatusValue = timeValue;
    }
}
