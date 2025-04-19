using UnityEngine;

public class CharacterControllerBase : MonoBehaviour
{
    protected Rigidbody2D Rigidbody;
    protected Animator _anim;
    protected virtual void Start() {
        Rigidbody = GetComponent<Rigidbody2D>();
        _anim = GetComponentInChildren<Animator>();
    }
    protected virtual void Update() {}
    protected virtual void Jump() {}
}
