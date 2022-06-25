using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public int Enemies = 5;
    public Text EnemiesText;

    private void Awake()
    {
        EnemiesText.text = Enemies.ToString();
        Enemy.OnEnemyKilled += OnEnemyKilledAction;
    }

    private void OnEnemyKilledAction()
    {
        Enemies--;
        EnemiesText.text = Enemies.ToString();
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
