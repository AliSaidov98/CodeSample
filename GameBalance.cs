using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameBalance",menuName = "GameBalance", order = 1)]
public class GameBalance : ScriptableObject
{
    public GameEvents gameEvents;
    [Header("Speed")]
    public float playerSpeed;
    public float enemySpeed;
    
    [Header("Points")]
    public int scoreToWin;
    public int pointFromLowEnemy;
    public int pointFromMidEnemy;
    public int pointFromHighEnemy;
    
    [Header("SnowBall")]
    public float snowBallReloadSpeed;
    public float enemyShootDelay;


    private static GameBalance _instance;

    public static GameBalance Instance
    {
        get
        {
            if (_instance == null)
            {
                GameBalance[] assets = Resources.LoadAll<GameBalance>("");
                
                if (assets == null || assets.Length < 1)
                    throw new Exception("Could not find GameBalance");
                else if(assets.Length > 1)
                    Debug.LogWarning("There is more GameBalance assets than 1");
                
                _instance = assets[0];
            }
            
            return _instance;
        }
    }

}
