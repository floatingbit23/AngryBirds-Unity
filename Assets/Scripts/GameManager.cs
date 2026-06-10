using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // libreria para manejar escenas
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    // Static (using Singleton Pattern)
    public static GameManager instance;

    // Public variable
    public int MaxNumberOfShots = 3;

    // Private
    private int _usedNumberOfShots;
    private IconHandler _iconHandler;

    // Lista de Baddies (cerdos)
    private readonly List<Baddie> _baddies = new List<Baddie>();

    [SerializeField] private float _secondsToWaitBeforeDeathCheck = 5f;
    [SerializeField] private GameObject _restartScreenObject;
    [SerializeField] private SlingShotHandler _slingShotHandler;
    [SerializeField] private Image _nextLevelImage;

    private void Awake()
    {
        // Singleton Pattern
        if (instance == null)
        {
            // instance será referenciado como este script
            instance = this;
        }

        _iconHandler = GameObject.FindAnyObjectByType<IconHandler>();

        // Averiguar cuantos Baddies (cerdos) tenemos al empezar el nivel
        // Array de tipo 'Baddie' llamado baddies [] busca todos los objetos de tipo 'Baddie' y los añade al array
        Baddie[] baddies = FindObjectsByType<Baddie>();

        for (int i = 0; i < baddies.Length; i++)
        {
            _baddies.Add(baddies[i]); // añade un cerdo a la lista _baddies
        }

        if (_nextLevelImage != null)
        {
            _nextLevelImage.enabled = false;
        }
    }

    public void UseShot()
    {
        _usedNumberOfShots++; // pos-incremento
        _iconHandler.UseShot(_usedNumberOfShots); // gestiona el color de los iconos

        CheckForLastShot(); // comprobamos si es el último lanzamiento
    }

    public bool HasEnoughShots()
    {
        if (_usedNumberOfShots < MaxNumberOfShots )
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void CheckForLastShot()
    {
        // si es el último lanzamiento se ejecuta la Co-Rutina
        if(_usedNumberOfShots == MaxNumberOfShots)
        {
            StartCoroutine(CheckAfterWaitTime());
        }
    }

    private IEnumerator CheckAfterWaitTime()
    {
        // espera 4 segundos y ejecuta código 
        yield return new WaitForSeconds( _secondsToWaitBeforeDeathCheck );

        // si no quedan cerdos hemos ganado, en caso contrario hemos perdido (tras el último shot)
        if (_baddies.Count == 0 )
        {
            WinGame();
        }
        else
        {
            RestartGame();
        }

    }

    public void RemoveBaddie(Baddie baddie)
    {
        _baddies.Remove(baddie); // elimina un cerdo de la lista _baddies

        CheckForAllDeadBaddies(); // comprueba si ya están todos eliminados
    }

    private void CheckForAllDeadBaddies()
    {
        // los arrays usan .length, las listas usan .Count
        if(_baddies.Count == 0) // si no quedan cerdos vivos
        {
            WinGame();
        }
    }

    #region Win/Restart/NextLevel functions

    private void WinGame()
    {
        _restartScreenObject.SetActive(true);
        _slingShotHandler.enabled = false;

        // do we have any more levels to load ?
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex; // returns current scene index
        int maxLevels = SceneManager.sceneCountInBuildSettings; // return the count of scenes (addition)

        if (currentSceneIndex + 1 < maxLevels && _nextLevelImage != null)
        {
            _nextLevelImage.enabled = true;
        }

    }
    


    public void RestartGame()
    {
        // la escena nº0 es la primera (y única en este caso) que tenemos
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // reinicia el nivel actual
    }
     

    public void NextLevel()
    {
        // carga la próxima escena
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }


    #endregion

}
