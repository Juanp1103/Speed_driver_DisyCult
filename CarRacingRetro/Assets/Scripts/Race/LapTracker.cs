using UnityEngine;
using System;

public class LapTracker : MonoBehaviour
{
    public int totalCheckpoints = 4; // incluye la línea de salida como índice 0
    public int totalLaps = 3;

    private int nextCheckpoint = 1; // tras la salida, el próximo esperado es el 1
    private int currentLap = 0;
    private bool started = false;

    public event Action<int> OnLapCompleted;
    public event Action OnRaceFinished;

    public void PassCheckpoint(int index)
    {
        
        if (index == 0)
        {
            if (!started)
            {
                // primera vez que cruzas la salida: arranca el conteo, no suma vuelta
                started = true;
                nextCheckpoint = 1;
                return;
            }

            // ya habías arrancado: para cerrar vuelta debiste pasar todos los intermedios
            if (nextCheckpoint == totalCheckpoints) // pasaste el último intermedio
            {
                currentLap++;
                OnLapCompleted?.Invoke(currentLap);
                nextCheckpoint = 1;

                if (currentLap >= totalLaps)
                    OnRaceFinished?.Invoke();
            }
            return;
        }

        // Checkpoints intermedios: deben pasarse en orden
        if (index == nextCheckpoint)
            nextCheckpoint++;
    }

    public int CurrentLap => currentLap;
}