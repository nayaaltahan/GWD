using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class CheckpointManager : MonoBehaviour
{
    
    public static CheckpointManager instance { get; private set; }

    public delegate void RespawnPlayersDelegate();
    
    public RespawnPlayersDelegate OnRespawnPlayers;

    private Vector3 currentCheckpoint;

    [SerializeField]
    private PlayerStateController Onwell, Rani;

    [SerializeField] 
    private float fadeInTime = 1.0f;
    [SerializeField] 
    private float fadeOutTime = 1.0f;

    [SerializeField]
    private Image blackoutPanel;

    [SerializeField]
    private Color fadeInColor;
    [SerializeField]
    private Color fadeOutColor;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(this);
            Debug.LogWarning("More than one CheckpointManager in scene!");
        }


    }

    private void Start()
    {
        CharSelectManager.instance.OnPlayersConnected += () =>
        {
            if (GameManager.instance.allowSinglePlayer)
            {
                Rani = GameObject.Find("Rani").GetComponent<PlayerStateController>();
            }
            else
            {
                Onwell = CharSelectManager.instance.RobotPlayer.GetComponentInChildren<PlayerStateController>(); 
                Rani = CharSelectManager.instance.FrogPlayer.GetComponentInChildren<PlayerStateController>();
            }
        };
    }

    public void RespawnPlayers()
    {
        Debug.Log("Respawning players");
        OnRespawnPlayers?.Invoke();
        StartCoroutine(RespawnPlayersCoroutine());
    }

    private IEnumerator RespawnPlayersCoroutine()
    {
        blackoutPanel.gameObject.SetActive(true);
        
        // Fade in panel
        blackoutPanel.DOColor(fadeInColor, fadeInTime);
        yield return new WaitForSeconds(fadeInTime);
        // Disable movement
        Rani.SetCanMove(false);
        Onwell.SetCanMove(false);
        
        // Teleport players
        var transform1 = Onwell.transform;
        transform1.position = new Vector3(currentCheckpoint.x, currentCheckpoint.y, transform1.position.z);
        var transform2 = Rani.transform;
        transform2.position = new Vector3(currentCheckpoint.x, currentCheckpoint.y, transform2.position.z);
        
        // Fade out panel
        blackoutPanel.DOColor(fadeOutColor, fadeOutTime);
        yield return new WaitForSeconds(fadeOutTime);
        
        // Return movement to players
        Rani.SetCanMove(true);
        Onwell.SetCanMove(true);
        
        // Hide panel so it doesn't overlay other UI
        blackoutPanel.gameObject.SetActive(false);

    }

    public void SetNewCheckpoint(Transform transform)
    {
        currentCheckpoint = transform.position;
    }
    
    
}
