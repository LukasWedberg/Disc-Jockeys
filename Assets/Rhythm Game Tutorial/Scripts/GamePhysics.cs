using UnityEngine;

public class GamePhysics : MonoBehaviour 
{
    void Awake()
    {
        // Adjust physics simulation angle
        Physics2D.gravity = Quaternion.Euler(transform.eulerAngles) * Physics2D.gravity;
        Physics2D.simulationMode = SimulationMode2D.Script;
    }

    void FixedUpdate()
    {
        Physics2D.Simulate(Time.fixedDeltaTime);
    }
}