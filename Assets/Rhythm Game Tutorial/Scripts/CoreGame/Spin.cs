using UnityEngine;

public class Spin : MonoBehaviour
{
    public enum SpinDirection
    {
        Left,
        Right
    }

    public SpinDirection spinDirection; // Set the spin direction in the Inspector

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Disc"))
        {
            DiscController discController = other.GetComponent<DiscController>();

            if (discController != null)
            {
                Debug.Log("Has a controller!");


                // Check if disc's spinning direction matches SpinDirection
                if ((spinDirection == SpinDirection.Left && discController.isSpinningLeft) ||
                    (spinDirection == SpinDirection.Right && discController.isSpinningRight))
                {
                    Debug.Log("Is spinning!");


                    ScoreManager.instance.NoteHit(ScoreManager.instance.scorePerSpin);
                    Destroy(gameObject); 
                }
                else
                {
                    Debug.Log("Isn't spinning?");

                    ScoreManager.instance.NoteMissed();
                    Destroy(gameObject); 
                }
            }
        }
    }
}