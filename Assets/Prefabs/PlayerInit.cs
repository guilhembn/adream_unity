using UnityEngine;
using UnityEngine.Networking;

public class PlayerInit : NetworkBehaviour {

public Behaviour[] componentsToDisable;
public GameObject[] componentsToNotRender;
public int notRenderedLayerId;

Camera sceneCamera;

void Start(){
  if (!isLocalPlayer){
    for (int i = 0; i < componentsToDisable.Length; i++){
       componentsToDisable[i].enabled = false;
    }
  } else {
       sceneCamera = Camera.main;
       if (sceneCamera != null){
         Camera.main.gameObject.SetActive(false);
       }
       for (int i=0; i < componentsToNotRender.Length; i++){
         componentsToNotRender[i].layer = notRenderedLayerId;
       }
  }
}

void OnDisable(){
  if (sceneCamera != null){
    sceneCamera.gameObject.SetActive(false);
  }
}

}
