using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playerObj;
    public BaldiAI baldiAiScript;
    public Settings settings;

    void Awake()
    {
        Collector.playerObj = playerObj;
        Collector.baldiAiScript = baldiAiScript;
        Collector.settings = settings;
    }
}
