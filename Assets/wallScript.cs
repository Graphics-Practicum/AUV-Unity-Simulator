using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class wallScript : MonoBehaviour
{
  void OnCollisionEnter(Collision collision)
  {
    bool collided = false;
    foreach (ContactPoint contact in collision.contacts)
    {
      if(!collided)
      {
        score.curr_score -= 1;
        collided = true;
      }
      else
      {
        return;
      }
      //print(contact.thisCollider.name + " hit " + contact.otherCollider.name);
      // Visualize the contact point
      //Debug.DrawRay(contact.point, contact.normal, Color.red);
    }
  }
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }
}
