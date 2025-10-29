using UnityEngine;

//al poner enemmy donde monobehavior te permite heredar del otro script
public class MeleeEnemy : Enemy, IDamageable
//despues de poner de quien hereda, en este caso Enemy, ponemos todas las interfaces qeu queramos, se ven or tener una I mayus delante
{
    void Start()
    {
        Attack();
    }

    void Update()
    {

    }

    //con esto sobreescribes la funcion original para añadir cambios
    public override void Attack()
    {
        //El base.Attack llama a la funcion original
        base.Attack();
        Debug.Log("Ataque Melee");
    }

    void IDamageable.TakeDamage(float damage)
    {
        Debug.Log("Enemigo recibiendo daño");
    }
}
