using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectedObjController : MonoBehaviour
{
    PlayerManager playerManager;

    [SerializeField] Transform sphere;
    // Start is called before the first frame update
    void Start()
    {
        playerManager= GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<PlayerManager>();

        sphere = transform.GetChild(0);

        if(GetComponent<Rigidbody>() == null) {
            gameObject.AddComponent<Rigidbody>();

            Rigidbody rb = GetComponent<Rigidbody>();

            rb.useGravity=false;
            rb.constraints= RigidbodyConstraints.FreezeAll;

            GetComponent<Renderer>().material = playerManager.collectedObjMat;
        }
    }

   private void OnCollisionEnter(Collision other) {
       if(other.gameObject.CompareTag("CollectibleObj")) {
           if(!playerManager.collidedList.Contains(other.gameObject)) {
               other.gameObject.tag ="CollectedObj";
               other.transform.parent= playerManager.collectedPoolTransform;
               playerManager.collidedList.Add(other.gameObject);
               other.gameObject.AddComponent<CollectedObjController>();
           }
       }
       if(other.gameObject.CompareTag("Obstacle")) {
           DestroyTheObject();
       }
   }
   private void OnTriggerEnter(Collider other) {
              if(other.gameObject.CompareTag("CollectibleList")) {
                  print("trigger enter");
           other.transform.GetComponent<BoxCollider>().enabled=false;
           other.transform.parent= playerManager.collectedPoolTransform;

           foreach (Transform child in other.transform) {
               if(!playerManager.collidedList.Contains(child.gameObject)) {
                   playerManager.collidedList.Add(child.gameObject);
                   child.gameObject.tag="CollectedObj";
                   child.gameObject.AddComponent<CollectedObjController> ();
           }
           
               
           }
       }

       if(other.gameObject.CompareTag("FinishLine")) {
           if(playerManager.levelState != PlayerManager.LevelState.Finished) {
               playerManager.levelState = PlayerManager.LevelState.Finished;
                playerManager.CallMakeSphere();
           }
       }
   }
   void DestroyTheObject() {
       playerManager.collidedList.Remove(gameObject);
       Destroy(gameObject);

       Transform partcile= Instantiate(playerManager.partcilePrefab,transform.position,Quaternion.identity);
       partcile.GetComponent<ParticleSystem>().startColor = playerManager.collectedObjMat.color;
   }
    public void MakeSphere() {
        gameObject.GetComponent<BoxCollider>().enabled=false;
        gameObject.GetComponent<MeshRenderer>().enabled=false;

    sphere.gameObject.GetComponent<MeshRenderer>().enabled=true;
    sphere.gameObject.GetComponent<SphereCollider>().enabled=true;
    sphere.gameObject.GetComponent<SphereCollider>().isTrigger=true;

    sphere.gameObject.GetComponent<Renderer>().material = playerManager.collectedObjMat;


    }
   public void DropObj() {
       sphere.gameObject.layer=8;

       sphere.gameObject.GetComponent<SphereCollider>().isTrigger=false;
       sphere.gameObject.AddComponent<Rigidbody>();
       sphere.GetComponent<Rigidbody>().useGravity =true;
   }
}
