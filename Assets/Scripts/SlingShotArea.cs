using UnityEngine;
using UnityEngine.InputSystem;

public class SlingShotArea : MonoBehaviour
{

    // Global ariables
    [SerializeField] private LayerMask _slingshotAreaMask;

    public bool isWithinSlingshotArea()
    {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(InputManager.MousePosition);

        // si nuestro ratón está tocando el collider específico de la layer 'SlinShotArea'
        if (Physics2D.OverlapPoint(worldPosition, _slingshotAreaMask))
        {
            return true;
        }
        else
        {
            return false;
        }

    }

   
}
