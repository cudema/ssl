using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    PlayerMovement movement;
    PlayerWeapon playerWeapon;
    PlayerInteraction playerInteraction;

    InputManager subscribedInputManager;
    bool setupRequested;
    bool isSubscribed;

    void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        playerWeapon = GetComponent<PlayerWeapon>();
        playerInteraction = GetComponent<PlayerInteraction>();
    }

    public void Setup()
    {
        setupRequested = true;
        SubscribeInput();
    }

    void OnEnable()
    {
        if (setupRequested)
        {
            SubscribeInput();
        }
    }

    void OnDisable()
    {
        UnsubscribeInput();
        movement.ResetInputState();
    }

    void SubscribeInput()
    {
        if (isSubscribed || !isActiveAndEnabled || InputManager.instance == null)
        {
            return;
        }

        subscribedInputManager = InputManager.instance;
        subscribedInputManager.move.performed += movement.ToPlayerMove;
        subscribedInputManager.move.canceled += movement.ToStap;
        subscribedInputManager.cameraAngle.performed += movement.ToMoveCameraAngle;
        subscribedInputManager.attack.performed += playerWeapon.Attack;
        subscribedInputManager.changeWeapon.performed += playerWeapon.ChangeWeapon;
        subscribedInputManager.skill.performed += playerWeapon.Skill;
        subscribedInputManager.desh.performed += playerWeapon.Desh;
        subscribedInputManager.interaction.performed += playerInteraction.OnInteraction;
        isSubscribed = true;
    }

    void UnsubscribeInput()
    {
        if (!isSubscribed || subscribedInputManager == null)
        {
            isSubscribed = false;
            subscribedInputManager = null;
            return;
        }

        subscribedInputManager.move.performed -= movement.ToPlayerMove;
        subscribedInputManager.move.canceled -= movement.ToStap;
        subscribedInputManager.cameraAngle.performed -= movement.ToMoveCameraAngle;
        subscribedInputManager.attack.performed -= playerWeapon.Attack;
        subscribedInputManager.changeWeapon.performed -= playerWeapon.ChangeWeapon;
        subscribedInputManager.skill.performed -= playerWeapon.Skill;
        subscribedInputManager.desh.performed -= playerWeapon.Desh;
        subscribedInputManager.interaction.performed -= playerInteraction.OnInteraction;
        isSubscribed = false;
        subscribedInputManager = null;
    }
}
