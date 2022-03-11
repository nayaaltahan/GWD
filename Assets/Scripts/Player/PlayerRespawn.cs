using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField]
    [Tooltip("When the playe reaches this y value they will respawn")]
    private float maxYDistanceFromLevel = -200f;


    //private string deathSound;

    //private EventInstance deathEI;

    //private static RespawnCheckpoint currentCheckpoint;
    //private static NetworkVariable<bool> isRespawning = new NetworkVariable<bool>();
    //private Rigidbody rb;
    //private Fade fade;
    //private PlayerController playerController;

    //private void Awake()
    //{
    //    deathEI = AudioManager.instance.CreateEventInstance(deathSound);
    //    playerController = GetComponent<PlayerController>();
    //    fade = FindObjectOfType<Fade>();
    //    fade.OnFadeOut += () =>
    //    {
    //        fade.In();
    //    };
    //    fade.OnFadeIn += () =>
    //    {
    //        rb.velocity = Vector3.zero;
    //        rb.position = currentCheckpoint.transform.position;
    //        playerController.canMove = true;
    //    };
    //}
    //// Start is called before the first frame update
    //void Start()
    //{
    //    if (IsServer)
    //        isRespawning.Value = false;
    //    rb = GetComponent<Rigidbody>();
    //}

    //private void Update()
    //{
    //    if (transform.position.y < maxYDistanceFromLevel)
    //    {
    //        RespawnPlayer();
    //    }
    //}

    //// Update is called once per frame
    //void UpdateServer()
    //{
    //    Debug.Log("Update server");
    //}

    //public void RespawnPlayer()
    //{
    //    if (isRespawning.Value)
    //        return;
    //    if (NetworkObjectId == 0)
    //    {
    //        deathEI.start();
    //        playerController.canMove = false;
    //        fade.Out();
    //        isRespawning.Value = false;
    //    }
    //    else
    //    {
    //        RespawnPlayerServerRpc();
    //    }
    //}

    //public void RespawnPlayerServerRpc()
    //{
    //    Debug.Log("Respawning server");
    //    rb.velocity = Vector3.zero;
    //    rb.position = currentCheckpoint.transform.position;
    //    isRespawning.Value = false;
    //}


    //public void UpdateCheckpoint(RespawnCheckpoint checkpoint)
    //{
    //    Debug.Log("Updated Checkpoint!");
    //    currentCheckpoint = checkpoint;
    //}
}
