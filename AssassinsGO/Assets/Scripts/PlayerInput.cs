using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float H { get; private set; }
    public float V { get; private set; }
    public bool InputEnabled { get; set; } = false;

    public void GetKeyInput()
    {
        if (InputEnabled)
        {
            H = Input.GetAxisRaw("Horizontal");
            V = Input.GetAxisRaw("Vertical");
        }
        else
        {
            H = 0f;
            V = 0f;
        }
    }
}
