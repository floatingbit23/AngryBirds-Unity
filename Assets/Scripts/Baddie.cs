using UnityEngine;

public class Baddie : MonoBehaviour
{
    // Control Health of the Pig

    [SerializeField] private float _maxHealth = 3f;
    [SerializeField] private float _damageThreshold = 0.2f;
    [SerializeField] private GameObject _baddieDeathParticle;

    // Pop sound
    [SerializeField] private AudioClip _deathClip;

    private float _currentHealth;

    private void Awake()
    {
        _currentHealth = _maxHealth; // al iniciar el juego, la salud actual es igual a la máxima salud posible
    }

    // damageAmount == impactVelocity 
    public void DamageBaddie(float damageAmount)
    {
        _currentHealth -= damageAmount; // le quitamos salud en base a la cantidad de daño

        if (_currentHealth <= 0)
        {
            Die();
        }    
    }

    private void Die()
    {
        Instantiate(_baddieDeathParticle, transform.position, Quaternion.identity);

        AudioSource.PlayClipAtPoint(_deathClip, transform.position); // Pop sound

        GameManager.instance.RemoveBaddie(this); // primero elimina el cerdo de la lista _baddies

        Destroy(gameObject); // luego desaparece el cerdo de la escena


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float impactVelocity = collision.relativeVelocity.magnitude; // velocidad a la que Baddie es impactado

        // si se supera el umbral de daño se llamará a la función
        if(impactVelocity > _damageThreshold)
        {
            DamageBaddie(impactVelocity);
        }
    }

}
