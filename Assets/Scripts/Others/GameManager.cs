using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playerObj;
    public BaldiAI baldiAiScript;

    void Awake()
    {
        Collector.playerObj = playerObj;
        Collector.baldiAiScript = baldiAiScript;
    }
}
