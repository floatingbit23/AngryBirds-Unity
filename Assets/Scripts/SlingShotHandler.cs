using System.Collections;
using UnityEngine; // default library
using UnityEngine.InputSystem; // detecting input library
using DG.Tweening; 


#region Declarations and References

public class SlingShotHandler : MonoBehaviour
{
    // Global Variables

    [Header("Line Renders")]
    // SerializedField allows us to modify these private variables in the Inspector
    [SerializeField] private LineRenderer _leftLineRenderer;
    [SerializeField] private LineRenderer _rightLineRenderer;

    [Header("Transform References")]
    [SerializeField] private Transform _leftStartPosition;
    [SerializeField] private Transform _rightStartPosition;
    [SerializeField] private Transform _elasticTransform;

    [Header("SlingShot Stats")]
    [SerializeField] private float _maxDistance = 3f;
    [SerializeField] private float _shotForce = 5f;
    [SerializeField] private Transform _centerPosition;
    [SerializeField] private Transform _idlePosition; // posicion de salida de la onda
    [SerializeField] private float _timeBetweenBirdRespawns = 2f;
    [SerializeField] private float _elasticDivider = 1.2f;
    [SerializeField] private AnimationCurve _elasticCurve;

    [Header("Scripts")]
    [SerializeField] private SlingShotArea _slingShotArea;
    [SerializeField] private CameraManager _cameraManager;

    [Header("Bird")]
    [SerializeField] private AngryBird _angryBirdPrefab;  // Reference to a GameObject prefab
    [SerializeField] private float _angryBirdPositionOffset = 0.3f;

    [Header("Sounds")]
    [SerializeField] private AudioClip _elasticPulledClip;
    [SerializeField] private AudioClip[] _elasticReleasedClips;
    private AudioSource _audioSource;

    private Vector2 _slingShotLinesPosition;
    private Vector2 _direction;
    private Vector2 _directionNormalized;

    private bool _clickedwithinArea; // false by default
    private bool _birdOnSlingshot;

    [SerializeField] private AngryBird _spawnedAngryBird;

    #endregion




    #region Awake & Update Functions

    // First thing called when clicking Play
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        _leftLineRenderer.enabled = false;
        _rightLineRenderer.enabled = false;

        SpawnAngryBird();
    }

    // Update is called once per frame
    private void Update()
    {

        if (InputManager.WasLeftMouseButtonPressed &&  _slingShotArea.isWithinSlingshotArea())
        {
            _clickedwithinArea = true;

            if(_birdOnSlingshot)
            {
                SoundManager.instance.PlayClip(_elasticPulledClip, _audioSource );
                _cameraManager.SwithToFollowCam(_spawnedAngryBird.transform);
            }
        }

        // Si hacemos click y lo hacemos dentro del área designada
        if (InputManager.IsLeftMousePressed && _clickedwithinArea && _birdOnSlingshot) 
        {
            DrawSlingShot(); // invoca a la acción
            PositionAndRotateAngryBird();
        }

        // Si soltamos el click para lanzar el pájaro
        if (InputManager.WasLeftMouseButtonReleased && _birdOnSlingshot && _clickedwithinArea && GameManager.instance.HasEnoughShots())
        {

            _clickedwithinArea = false;
            _birdOnSlingshot = false;

            // llamada a la función LaunchBird
            _spawnedAngryBird.LaunchBird(_direction, _shotForce);

            GameManager.instance.UseShot(); // uses 1 shot after launching the bird
            
            AnimateSlingShot();

            SoundManager.instance.PlayRandomClip(_elasticReleasedClips, _audioSource);

            if (GameManager.instance.HasEnoughShots())
            {
                // Co-routine
                StartCoroutine(SpawnAngryBirdAfterTime());
            }

        }

    }

    #endregion





    #region SlingShot Methods

    // Lee las coordenadas donde se ha clickado y llama a SetLines()
    private void DrawSlingShot()
    {

        // Local Variables
        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(InputManager.MousePosition);

        // limita el alcance de la honda 
        // 'touchPosition - _centerPosition.position' calcula un vector que apunta desde el centro hacia el lugar donde se tocó la pantalla, representando así la dirección y la distancia del toque respecto al centro del tirachinas
        // ClampMagnitude es una función que ajusta un vector para que tenga una magnitud (longitud) máxima especificada (_maxDistance) sin alterar su dirección.
        _slingShotLinesPosition = _centerPosition.position + Vector3.ClampMagnitude(touchPosition - _centerPosition.position, _maxDistance);
    
        SetLines(_slingShotLinesPosition); // invoca a la acción con parámetro 

        // Casting Vector3 into a Vector2
        _direction =  (Vector2)_centerPosition.position - _slingShotLinesPosition;

        // normalized significa que su magnitud estará entre 0 y 1
        _directionNormalized = _direction.normalized; 
    }

    // establece los puntos 0 (comienzo) y 1 (final) de las dos variables del componente LineRenderer
    private void SetLines(Vector2 position)
    {

        // para "despertar" a las líneas de la honda una vez se han calculado sus posiciones
        if (!_leftLineRenderer.enabled && !_rightLineRenderer.enabled)
        {
            _leftLineRenderer.enabled = true;
            _rightLineRenderer.enabled = true;
        }

        _leftLineRenderer.SetPosition(0, position); // donde se hace click comienza la onda
        _leftLineRenderer.SetPosition(1, _leftStartPosition.position); // donde se ha marcado mediante el Transform se termina la honda

        _rightLineRenderer.SetPosition(0, position);
        _rightLineRenderer.SetPosition(1, _rightStartPosition.position);
    }

    #endregion





    #region Angry Bird Methods

    private void SpawnAngryBird()
    {
        SetLines(_idlePosition.position); // establece cuerdas en posicion de salida de la honda

        Vector2 dir = (_centerPosition.position - _idlePosition.position).normalized;
        Vector2 spawnPosition = (Vector2)_idlePosition.position + dir * _angryBirdPositionOffset;

        // Spawnear al pájaro
        // Quaternion.identity <-> cero rotación del pájaro
        // nuevo GameObject local (clonado) al que se dará el movimiento
        _spawnedAngryBird = Instantiate(_angryBirdPrefab, spawnPosition, Quaternion.identity);

        // El pájaro mire en la dirección de lanzamiento
        _spawnedAngryBird.transform.right = dir;

        _birdOnSlingshot = true;

    }


    private void PositionAndRotateAngryBird()
    {
        _spawnedAngryBird.transform.position = _slingShotLinesPosition + _directionNormalized * _angryBirdPositionOffset;
        
        // El pájaro mire en la dirección de lanzamiento
        _spawnedAngryBird.transform.right = _directionNormalized;
    }


    // la función es una corutina
    // permiten pausar la ejecución de una función y luego continuar desde donde se dejó
    private IEnumerator SpawnAngryBirdAfterTime()
    {
        // espere _timeBetweenBirdRespawns segundos antes de continuar con la siguiente línea de código.
        yield return new WaitForSeconds(_timeBetweenBirdRespawns);

        SpawnAngryBird();
        _cameraManager.SwithToIdleCam();

    }


    #endregion



    #region Animate SlingShot

    private void AnimateSlingShot()
    {
        // _elasticTransform obtiene la posición donde se comienza la onda
        _elasticTransform.position = _leftLineRenderer.GetPosition(0);

        float dist = Vector2.Distance(_elasticTransform.position, _centerPosition.position);

        float time = dist / _elasticDivider; // dist / 1.2f

        // DOMove() is a Tweening function from DOTween library imported above
        // Ease.OutElastic suaviza con físicas de componente elástico
        _elasticTransform.DOMove(_centerPosition.position, time).SetEase(_elasticCurve);


        StartCoroutine(AnimateSlingShotLines(_elasticTransform, time));
    }

    // will be executed right after we release the mouse left button and release the bird
    private IEnumerator AnimateSlingShotLines(Transform trans, float time)
    {
        float elapsedTime = 0f; // tiempo transcurrido

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime; // iterates based on frame rate of each unique device
            
            SetLines(trans.position);

            yield return null; // waits for the next frame of the game
        }
    }

    #endregion


}
