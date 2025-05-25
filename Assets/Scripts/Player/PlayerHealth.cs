using Assets.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    private int health = 3;
    private int maxHealth = 3;


    private StatsManager statsManager;

    [SerializeField] private Collider2D playerCollider;
    public int Health
    {
        get { return health; }

        set { health = Mathf.Clamp(value, 0, maxHealth); } //No dejo que sea menor que 0
    }
    void Start()
    {

        statsManager = GetComponent<StatsManager>(); //LLamo al StatsManager para que reciba actualizaciones.
                                                     //En este script, solo recibira de vida
    }

    private void Update()
    {
        if (health <= 0)
        {
            SceneManager.LoadScene("DefeatScene");
            Destroy(gameObject);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage();
        }
    }
    public void TakeDamage()
    {
        statsManager.NewHealth(-1);
    }


}


