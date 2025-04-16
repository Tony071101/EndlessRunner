using UnityEngine;

public class CharacterControllerBase : MonoBehaviour
{
    protected Rigidbody2D Rigidbody;
    protected virtual void Start() {
        Rigidbody = GetComponent<Rigidbody2D>();
    }
    protected virtual void Update() {}
    protected virtual void Jump() {}
}
