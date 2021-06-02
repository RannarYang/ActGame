using UnityEngine;

public class ManualInput : MonoBehaviour
{
    private CharacterControl control;
    private void Awake()
    {
        control = this.GetComponent<CharacterControl>();
    }
}