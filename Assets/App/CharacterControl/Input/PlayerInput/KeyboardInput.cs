using UnityEngine;

public class KeyboardInput : MonoBehaviour {
    [Header("===== Key settings =====")]
    public KeyCode keyLeft;
    public KeyCode keyRight;
    public KeyCode keyTurbo;

    public KeyCode keyUp;
    public KeyCode keyDown;

    [Header("===== Output signals =====")]
    public float Dright;
    public bool Turbo;
    public bool MoveUp;
    public bool MoveDown;

    private MyButton buttonKeyLeft;
    private MyButton buttonKeyRight;
    private MyButton buttonKeyTurbo;
    private MyButton buttonKeyUp;
    private MyButton buttonKeyDown;
    
    // 中间变量
	private float targetDright;
    private float velocityDright;

    private void Update()
    {
        buttonKeyLeft.Tick(Input.GetKey(keyLeft));
        buttonKeyRight.Tick(Input.GetKey(keyRight));
        buttonKeyUp.Tick(Input.GetKey(keyUp));
        buttonKeyDown.Tick(Input.GetKey(keyDown));
        buttonKeyTurbo.Tick(Input.GetKey(keyTurbo));

        targetDright = (buttonKeyRight.IsPressing ? 1 : 0) - (buttonKeyLeft.IsPressing ? 1 : 0);
        Dright = Mathf.SmoothDamp(Dright, targetDright, ref velocityDright, 0.1f);

        Turbo = (buttonKeyTurbo.IsPressing && !buttonKeyTurbo.IsDelaying) || buttonKeyTurbo.IsExtending;
    }

}