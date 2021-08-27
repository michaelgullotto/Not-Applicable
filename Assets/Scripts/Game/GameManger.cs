using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class GameManger : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject keys;
    [SerializeField] private GameObject KeysSlot;
    [SerializeField] private GameObject TeddyBear;
    [SerializeField] private GameObject TeddySlot;
    //[SerializeField] private GameObject lightSaber;
    //[SerializeField] private GameObject lightSaberSlot;
    [SerializeField] private GameObject TorchSlot;
    [SerializeField] private GameObject Torch;
    public GameObject endDoor;
    public GameObject winTrigger;
    public GameObject winpannel;
    public GameObject GameOverPannel;
    public static bool hasTeddy = false;
    public static bool hasKeys = false;
    public static bool hasTourch = false;
    public static bool hasLightSaber = false;
    
    private Vector3 playerSpawn;
    private Vector3 keySpawn;
    Vector3 TeddySpawn;
    Vector3 torchSpawn;

    private void Start()
    {
        Time.timeScale = 1;
    }

    void Update()
    {
        float winDistance = (player.transform.position - winTrigger.transform.position).magnitude;
        if (winDistance < 5f)
        {
            WinGame();
        }
        
        if (Input.GetKey(KeyCode.Q))
        {
            // checks if items exists if it dose how far away if its close enough destory in world add to player sets the has bools to true
            if(keys != null)
            { 
                float keysDistance = (player.transform.position - keys.transform.position).magnitude;
                if (keysDistance < 5f)
                {
                    Destroy(keys);
                    Instantiate(Resources.Load<GameObject>("KeyPrefab"),KeysSlot.transform.position,KeysSlot.transform.rotation, KeysSlot.transform);
                    hasKeys = true;
                }
            }

            if (TeddyBear != null)
            {
                float teddyDistance = (player.transform.position - TeddyBear.transform.position).magnitude;
                if (teddyDistance < 5f)
                {
                    Destroy(TeddyBear);
                    Instantiate(Resources.Load<GameObject>("TeddyPrefab"),TeddySlot.transform.position,TeddySlot.transform.rotation, TeddySlot.transform);
                    hasTeddy = true;
                }
 
            }
            
            // if (lightSaber != null)
            // {
            //     float lightSaberDistance = (player.transform.position - lightSaber.transform.position).magnitude;
            //     if (lightSaberDistance < 5f)
            //    {
           //         Destroy(lightSaber);
          //          Instantiate(Resources.Load<GameObject>("lightSaber"),lightSaberSlot.transform.position,lightSaberSlot.transform.rotation);
           //         hasLightSaber = true;
          //      }
          //  }
          
            if (Torch != null)
            {
                float TorchDistance = (player.transform.position - Torch.transform.position).magnitude;
                if (TorchDistance < 5f)
                {
                    Destroy(Torch);
                    Instantiate(Resources.Load<GameObject>("FlashlightPrefab"),TorchSlot.transform.position,TorchSlot.transform.rotation, TorchSlot.transform);
                    hasTourch = true;
                }
            }
            // if have keys can destroy door to win area
            if(endDoor != null)
            {
                if (hasKeys)
                {
                    float doorDistance = (player.transform.position - endDoor.transform.position).magnitude;
                    if (doorDistance < 5f)
                    {
                        Destroy(endDoor);
                    }
                }
            }
            
            
            // debugs if nothing to interact with
            else
            {
                Debug.Log("nothing to interact with");
            }
        }
    }
 // if monster gets to you lose
    public void MonsterHit()
    {
        if (!hasTeddy)
        {
            GameOver();
        }
        if (hasTeddy)
        {
            Destroy(TeddySlot);
            hasTeddy = false;
        }
    }
    // triggers game over pannel
    public void GameOver()
    {
        GameOverPannel.SetActive(true);
        Time.timeScale = 0;
       Debug.Log("game over Noob");
    }
   // triggers game win pannel
    public void WinGame()
    {
        winpannel.SetActive(true);
        Time.timeScale = 0;
    }
}
