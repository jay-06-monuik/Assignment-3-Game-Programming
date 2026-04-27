using UnityEngine;

public abstract class NPC : MonoBehaviour
    {
        protected virtual void Move()
        {
            Debug.Log("I am Moving");
        }

        protected virtual void Interact()
        {
            Debug.Log("I am Interacting");
        }

        protected virtual void Damage(float damageValue, GameObject dropObject)
        {
            Debug.Log("Taking Damage");
        }
    }