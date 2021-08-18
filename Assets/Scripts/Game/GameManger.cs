using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public static bool hasTeddy = false;
    public static bool hasKeys = false;
    public static bool hasTourch = false;
    public static bool hasLightSaber = false;
    
    void Update()
    {
        
        if (Input.GetKey(KeyCode.Q))
        {
            // checks if items exists if it dose how far away if its close enough destory in world add to player sets the has bools to true
            if(keys != null)
            { 
                float keysDistance = (player.transform.position - keys.transform.position).magnitude;
                if (keysDistance < 5f)
                {
                    Destroy(keys);
                    Instantiate(Resources.Load<GameObject>("KeyPrefab"),KeysSlot.transform.position,KeysSlot.transform.rotation);
                    hasKeys = true;
                }
            }

            if (TeddyBear != null)
            {
                float teddyDistance = (player.transform.position - TeddyBear.transform.position).magnitude;
                if (teddyDistance < 5f)
                {
                    Destroy(TeddyBear);
                    Instantiate(Resources.Load<GameObject>("TeddyPrefab"),TeddySlot.transform.position,TeddySlot.transform.rotation);
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
                    Instantiate(Resources.Load<GameObject>("FlashlightPrefab"),TorchSlot.transform.position,TorchSlot.transform.rotation);
                    hasTourch = true;
                }
            }
            
            // debugs if nothing to interact with
            else
            {
                Debug.Log("nothing to interact with");
            }
        }
    }

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
    public void GameOver()
    {
       Debug.Log("game over Noob");
    }
}
