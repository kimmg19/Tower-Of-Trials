using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Flame : MonoBehaviour
{
    Dragon dragon;
    ParticleSystem ps;
    //List<ParticleSystem.Particle> enter=new List<ParticleSystem.Particle>();
    // Start is called before the first frame update
    void Start()
    {
        dragon = GetComponentInParent<Dragon>();
        ps=GetComponent<ParticleSystem>();
        
    }
    private void OnParticleTrigger()
    {
        dragon.player.GetComponent<PlayerStatus>().TakeDamage(1);
        /*int number = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
        
        for (int i = 0; i < number; i++) {
            print(i + " 파리피플 충돌");
        }*/
    }

}
