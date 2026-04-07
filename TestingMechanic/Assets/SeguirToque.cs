using UnityEngine;
using UnityEngine.InputSystem;

public class SeguirToque : MonoBehaviour
{
    [SerializeField] private Transform objeto;
    [SerializeField] private Camera camaraPrincipal;

    private void Start()
    {
        camaraPrincipal = Camera.main;
    }
   
    private void Update()
    {
        if (!Touchscreen.current.primaryTouch.press.isPressed) { return; }
        
        Vector2 posicionToque = Touchscreen.current.primaryTouch.position.ReadValue();

        Vector3 posicionMundo = camaraPrincipal.ScreenToWorldPoint(posicionToque);

        posicionMundo.z = 0f;

        objeto.position = posicionMundo;
        
    }
}
