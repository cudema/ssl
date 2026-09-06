using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    PlayerInput input;
    InputActionMap playerMoveMap;

    public InputAction move;
    public InputAction cameraAngle;
    public InputAction attack;
    public InputAction changeWeapon;
    public InputAction skill;
    public InputAction desh;
    public InputAction interaction;

    InputActionMap currentActinoMap;

    bool isInputable;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        Application.targetFrameRate = 120;

        input = GetComponent<PlayerInput>();

        playerMoveMap = input.actions.FindActionMap("Move", true);
        move = playerMoveMap.FindAction("move", true);
        cameraAngle = playerMoveMap.FindAction("cameraAngle", true);
        attack = playerMoveMap.FindAction("Attack", true);
        changeWeapon = playerMoveMap.FindAction("ChangeWeapon", true);
        skill = playerMoveMap.FindAction("Skill", true);
        desh = playerMoveMap.FindAction("Desh", true);
        interaction = playerMoveMap.FindAction("Interaction", true);
    }

    public void StopControll()
    {
        isInputable = false;
        playerMoveMap.Disable();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        //Player.instance.movement.movement.Controller.enabled = false;
    }

    public void StartControll()
    {
        //Debug.Log("OnInput");
        isInputable = true;
        playerMoveMap.Enable();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Player.instance.movement.movement.Controller.enabled = true;
    }

    public bool GetInputUseable()
    {
        return isInputable;
    }

    void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}
