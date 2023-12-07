using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
 
   String target; //Trigger的目標

   void Awake() {
      target = "Ground";   
   }

   void OnTriggerEnter2D(Collider2D other) {

    if (other.tag == target)
    {
         Destroy(other.gameObject);
    }   
   }
}
