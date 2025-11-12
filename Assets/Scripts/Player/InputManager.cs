using UnityEngine;

public class InputManager : MonoBehaviour
{
    private InputSystem_Actions inputSystem;
    private InputSystem_Actions.PlayerActions playerActions;
    private PlayerMovement playerMovement;
    private InteractionManager interactionManager;
    void Awake()
    {
        inputSystem = new InputSystem_Actions();
        playerActions = inputSystem.Player;
        playerMovement = GetComponent<PlayerMovement>();
        interactionManager = GetComponent<InteractionManager>();
        playerActions.Interact.performed += ctx => interactionManager.OnInteract();

    }

    void Update()
    {
        playerMovement.ProcessMove(playerActions.Move.ReadValue<Vector2>());
    }

    void OnEnable()
    {
        playerActions.Enable();
    }

    void OnDisable()
    {
        playerActions.Disable();
    }
}
