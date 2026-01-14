using UnityEngine;
public class Interactable : MonoBehaviour
{
    public Actor myActor { get; private set; }

    void Awake() 
    {
        myActor = GetComponent<Actor>();
    }

    public void InteractWithItem()
    {
        // Pickup Item
        Destroy(gameObject);
    }
}
