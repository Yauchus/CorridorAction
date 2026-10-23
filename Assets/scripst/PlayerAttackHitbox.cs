using UnityEngine;

public class PlayerAttackHitbox : MonoBehaviour
{
    public float damage = 35f;

    void OnTriggerEnter(Collider other)
    {
        // Не реагируем на самого игрока 
        if (other.CompareTag("Player") || other.gameObject.name == "Player")
            return;

        Debug.Log("Хитбокс задел: " + other.gameObject.name + " (тег: " + other.tag + ")");

        // Проверяем, враг ли это
        if (other.CompareTag("Enemy"))
        {
            Health enemyHealth = other.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
                Debug.Log("урон врагу нанесен");
            }
            else
            {
                Debug.Log("Враг найден, но у него нет компонента Health");
            }
        }
    }
}