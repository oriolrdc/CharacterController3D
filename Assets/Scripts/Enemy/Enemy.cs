using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float movementSpeed = 5;
    public float attackDamage = 10;

    public void Movement()
    {
        Debug.Log("Movimiento base");
    }

    //virtual hace que pueda decidir si es a melee o a distancia
    public virtual void Attack()
    {
        Debug.Log("Ataque base");
    }
    
}
