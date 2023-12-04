using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torpedoes : MonoBehaviour
{
    private ParticleSystem particleSystem;
    private List<ParticleCollisionEvent> particleCollisionEvents;
    private string targetTag;
    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        particleCollisionEvents = new List<ParticleCollisionEvent>();
    }

    void OnParticleCollision(GameObject other)
    {
        ParticlePhysicsExtensions.GetCollisionEvents(particleSystem, other, particleCollisionEvents);
        for(int i = 0; i<particleCollisionEvents.Count; i++)
        {
            var collider = particleCollisionEvents[i].colliderComponent;
            if(collider.CompareTag(targetTag))
            {
                print("HIT");
                score.curr_score += 1;
            }
        }
    }

    public void SetTargetTag(string targetTag)
    {
        this.targetTag = targetTag;
    }

    public void Fire()
    {
        particleSystem.Emit(1);
    }
}
