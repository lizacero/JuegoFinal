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
    public event Action OnEsc;

    //Crea y configura las entradas cuando el ScriptableObject se habilita.
    //Suscribe los eventos del mapa de controladores a los eventos públicos.
    private void OnEnable()
    {
        misControles = new Controls();
        misControles.Gameplay.Enable();
        misControles.Gameplay.Disparar.started += Disparar;
        misControles.Gameplay.Recargar.started += Recargar;
        misControles.Gameplay.Interactuar.started += Interactuar;
        misControles.Gameplay.Mover.performed += Mover;
        misControles.Gameplay.Mover.canceled += Mover;
        misControles.Gameplay.Esc.started += Esc;
    }

    //Los siguientes métodos notifican a los oyentes los diferentes eventos al detectar la entrada correspondiente.
    private void Esc(InputAction.CallbackContext context)
    {
        OnEsc?.Invoke();
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
