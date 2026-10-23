using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Параметры движения")]
    public float baseSpeed = 3.5f;   // Базовая скорость бега
    private float currentSpeed;      // Индивидуальная скорость этого врага
    private Transform player;        // Ссылка на игрока

    [Header("Параметры атаки")]
    public float attackRange = 2.0f;     // Дистанция, на которой враг останавливается и бьет
    public float attackDamage = 10f;     // Урон за один удар
    public float attackCooldown = 1.2f;  // Время между ударами (в секундах)
    private float nextAttackTime = 0f;   // Таймер для следующего удара

    void Start()
    {
        // Ищем игрока по тегу
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        // Задаем уникальную случайную скорость для каждого врага (база + разброс)
        currentSpeed = baseSpeed + Random.Range(-1.0f, 1.2f);
    }

    void Update()
    {
        if (player == null) return;

        // Всегда поворачиваемся лицом 
        transform.LookAt(player.position);

        // Считаем расстояние до игрока
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Если мы дальше дистанции атаки  бежим за игроком
        if (distanceToPlayer > attackRange)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, currentSpeed * Time.deltaTime);
        }
        else
        {
            // Если подошли вплотную  останавливаемся и бьем по кулдауну
            if (Time.time >= nextAttackTime)
            {
                AttackPlayer();
                nextAttackTime = Time.time + attackCooldown; // Устанавливаем задержку до следующего удара
            }
        }
    }

    void AttackPlayer()
    {
        // Проверяем компонент Health у игрока и наносим урон
        Health playerHealth = player.GetComponent<Health>();
        if (playerHealth != null)
        {
            Debug.Log("Враг нанес удар! Урон: " + attackDamage);
            playerHealth.TakeDamage(attackDamage);
        }
    }
}
