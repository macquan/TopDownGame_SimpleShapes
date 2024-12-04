using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2.0f;  
    private Transform player;
    public int sessionId;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        sessionId = GameManager.Instance != null ? GameManager.Instance.gameSessionId : -1;
    }

    void Update()
    {
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
    }
    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.IncreaseScore(sessionId);
        }
    }
}
