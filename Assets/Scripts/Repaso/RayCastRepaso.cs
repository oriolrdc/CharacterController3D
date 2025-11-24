using UnityEngine;
using UnityEngine.InputSystem;

public class RayCastRepaso : MonoBehaviour
{
    //Cuando hagas click en unos objetos que sepa que objeto estas clicando y si clicas uno que pasen cosas distintas a las que si clicas otra cosa.
    //Cambiar el look input, exactamente el del raton, de Delta[Mouse] a Position[Mouse]... Sino puedes crear un input nuevo, tu sabras.
    //para coprobar el clic usaremos el input de ataque.
    InputAction _clickAction;
    InputAction _positionAction;
    Vector2 _mousePosition; //necesario para leer el position action

    void Awake()
    {
        _clickAction = InputSystem.actions["Attack"];
        _positionAction = InputSystem.actions["MousePosition"]; //en caso de que sea el nuevo, sino pues con el de look
    }

    void Update()
    {
        _mousePosition = _positionAction.ReadValue<Vector2>(); //actualiza la variable diciendo donde esta el raton constantemente

        if(_clickAction.WasPressedThisFrame())
        {
            ShootRayCast();
        }
    }

    void ShootRayCast()
    {
        Ray ray = Camera.main.ScreenPointToRay(_mousePosition); //pilla la camera principal i mira donde esta el raton en relacion a esa camara, mandando un rayo hacia adelante de la camera.
        RaycastHit hit; //Variable donde se almacena lo que colisiona con el rayo
        if(Physics.Raycast(ray, out hit, Mathf.Infinity)) //esto basicamente tira el rayo palante en si, tiras el rayo de antes, ray, luego dices donde guardar la info, y despues la distancia del rayo (infinito es lo mejor).
        {
            if(hit.transform.gameObject.layer == 3) //forma 1 de ver con que ha chocado el rayo, por layer.
            {
                //pasan cosas
            }

            if(hit.transform.gameObject.tag == "Peruano") //forma 2 de ver con que ha chocado el rayo, por tags.
            {
                //pasan cosas
            }

            if(hit.transform.name == "Antonio") //forma 3 de ver con que ha chocado el rayo, por nombre del objeto.
            {
                //pasan cosas
            }
        }
    }
}
