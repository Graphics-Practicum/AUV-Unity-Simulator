using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buoyScript : MonoBehaviour
{
  void OnCollisionStay(Collision collision)
  {
    foreach (ContactPoint contact in collision.contacts)
    {
      print(contact.thisCollider.name + " hit " + contact.otherCollider.name);
      // Visualize the contact point
      print("HIT");
      Debug.DrawRay(contact.point, contact.normal, Color.red);
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
