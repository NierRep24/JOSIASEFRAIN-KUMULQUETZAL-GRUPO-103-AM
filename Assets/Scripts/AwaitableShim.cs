using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Shim sencillo para poder usar Awaitable.WaitForSecondsAsync(...)
/// en versiones de Unity donde Awaitable aún no existe.
/// No bloquea el hilo principal, solo usa Task.Delay.
/// </summary>
public static class Awaitable
{
    public static async Task WaitForSecondsAsync(float seconds)
    {
        int ms = Mathf.Max(0, Mathf.RoundToInt(seconds * 1000f));
        await Task.Delay(ms);
    }
}
