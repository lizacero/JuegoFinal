using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName ="InputManager")]
public class InputManagerSO : ScriptableObject
{
    Controls misControles;
    public event Action<Vector2> OnMover;
    public event Action OnDisparar;
    public event Action OnRecargar;
    public event Action OnInteractuar;

    private void OnEnable()
    {
        misControles = new Controls();
        misControles.Gameplay.Enable();
        misControles.Gameplay.Disparar.started += Disparar;
        misControles.Gameplay.Recargar.started += Recargar;
        misControles.Gameplay.Interactuar.started += Interactuar;
        misControles.Gameplay.Mover.performed += Mover;
        misControles.Gameplay.Mover.canceled += Mover;
    }

    private void Mover(InputAction.CallbackContext context)
    {
        OnMover?.Invoke(context.ReadValue<Vector2>());
    }

    private void Interactuar(InputAction.CallbackContext context)
    {
        OnInteractuar?.Invoke();
    }

    private void Recargar(InputAction.CallbackContext context)
    {
        OnRecargar?.Invoke();
    }

    private void Disparar(InputAction.CallbackContext context)
    {
        OnDisparar?.Invoke();
    }
}
