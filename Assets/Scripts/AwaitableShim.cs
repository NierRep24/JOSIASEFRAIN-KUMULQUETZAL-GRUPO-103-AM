using System.Threading.Tasks;
using UnityEngine;

public static class Awaitable
{
    public static async Task WaitForSecondsAsync(float seconds)
    {
        int ms = Mathf.Max(0, Mathf.RoundToInt(seconds * 1000f));
        await Task.Delay(ms);
    }
}
