using UnityEngine;

public class AngryBird : MonoBehaviour
{

    // Sounds
    [SerializeField] private AudioClip _hitClip;
    private AudioSource _audioSource;

    // Global Variables
    private Rigidbody2D _rb;
    private CircleCollider2D _circleCollider;

    private bool _hasBeenLaunched; // default to false
    private bool _shouldFaceVelDirection;

    // Awake se inicializa lo primero, incluso antes que Start()
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        // La función GetComponent<T>() se utiliza para obtener una referencia a un componente específico que está adjunto al mismo GameObject (AngryBird) al que pertenece el script.
        _rb = GetComponent<Rigidbody2D>();
        _circleCollider = GetComponent<CircleCollider2D>();

    }

    private void Start()
    {
        _rb.bodyType = RigidbodyType2D.Kinematic;
        _circleCollider.enabled = false;
    }

    // runs 50 times per second
    private void FixedUpdate()
    {
        if (_hasBeenLaunched && _shouldFaceVelDirection)
        {
            transform.right = _rb.linearVelocity;
        }    
    }

    public void LaunchBird(Vector2 direction, float force)
    {
        _rb.bodyType = RigidbodyType2D.Dynamic; // devuelve gravedad
        _circleCollider.enabled = true;         // permite colisiones  

        // apply the force
        _rb.AddForce(direction * force, ForceMode2D.Impulse);

        _hasBeenLaunched = true;
        _shouldFaceVelDirection = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // cuando Red colisiona ya no queremos que mire en la dirección de trayectoria
        _shouldFaceVelDirection = false;

        SoundManager.instance.PlayClip(_hitClip, _audioSource); // sonido cuando colisiona Red

        // Intencionado: Destroy(this) destruye solo el componente (script), no el GameObject.
        // El pájaro permanece visible en la escena como escombro tras colisionar.
        Destroy(this);
    }

}
