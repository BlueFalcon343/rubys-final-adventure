using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CogCollectible : MonoBehaviour
{
    public AudioClip collectedSound;

    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if(controller != null)
        {
            controller.PlaySound(collectedSound);
            controller.cogCount = controller.cogCount + 3;
            controller.SetCogText();
            Destroy(gameObject);
        }
    }
       
}
