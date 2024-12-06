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
                // Check if disc's spinning direction matches SpinDirection
                if ((spinDirection == SpinDirection.Left && discController.isSpinningLeft) ||
                    (spinDirection == SpinDirection.Right && discController.isSpinningRight))
                {
                    ScoreManager.instance.NoteHit(ScoreManager.instance.scorePerSpin);
                    Destroy(gameObject); 
                }
                else
                {
                    ScoreManager.instance.NoteMissed();
                    Destroy(gameObject); 
                }
            }
        }
    }
}