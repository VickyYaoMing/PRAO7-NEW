using System;
using UnityEngine;

public class TimerEasy
{
    public float totalTime { get; set; }
    public float currentTime { get; private set; }

    private bool hasBeenUpdatedAboutHalfway = false;
    private bool hasBeenUpdatedAboutAlmost = false;
    public float Progress
    {
        get
        {
            if (totalTime <= 0f) return 0f;
            return 1f - (currentTime / totalTime); 
        }
    }

    public enum TimerEnum
    {
        isNotDone,
        isHalfwayDone,
        isAlmostDone,
        isTimerDone
    }

    public TimerEnum currentState { get; private set; } = TimerEnum.isNotDone;

    public event Action OnHalfwayReached;
    public event Action OnAlmostDoneReached;

    public TimerEasy(float totalTime)
    {
        this.totalTime = totalTime;
        ResetTimer();
    }

    public void ResetTimer()
    {
        currentTime = totalTime;
        currentState = TimerEnum.isNotDone;
        hasBeenUpdatedAboutAlmost = false;
        hasBeenUpdatedAboutHalfway = false;
    }

    public void UpdateTimer(float deltaTime)
    {
        if (currentState == TimerEnum.isTimerDone) return;

        currentTime -= deltaTime;

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            currentState = TimerEnum.isTimerDone;
            return;
        }

        if (!hasBeenUpdatedAboutAlmost && currentTime <= totalTime * 0.33f)
        {
            hasBeenUpdatedAboutAlmost = true;
            currentState = TimerEnum.isAlmostDone;
            OnAlmostDoneReached?.Invoke();
        }
        else if (!hasBeenUpdatedAboutHalfway && currentTime <= totalTime * 0.5f)
        {
            hasBeenUpdatedAboutHalfway = true;
            currentState = TimerEnum.isHalfwayDone;
            OnHalfwayReached?.Invoke();
        }
    }
}




public class Timer : MonoBehaviour
{
    public float WaitTime { get; private set; }
    public float TimeLeft { get; private set; }
    public bool Paused { get; private set; }

    /// <summary>
    /// If OneShot is <c>true</c>, the timer doesn't restart on completion.
    /// </summary>
    public bool OneShot { get; private set; }

    /// <summary>
    /// If OneShot and DestroyOnTimeout are <c>true</c>, the timer is destroyed on completion.
    /// </summary>
    public bool DestroyOnTimeout { get; private set; }

    /// <summary>
    /// How much <see langword="TimeLeft"/> compared to <see langword="WaitTime"/>.
    /// If timer just started (or if <see langword="WaitTime"/> is <c>0</c>), <see langword="Ratio"/> will be <c>1</c>. If timer is finished, <see langword="Ratio"/> will be <c>0</c>.
    /// </summary>
    public float Ratio => WaitTime == 0f ? 1f : TimeLeft == 0f ? 0f : TimeLeft / WaitTime;

    /// <summary>
    /// Like ratio but reversed. Meaning, if timer just started (or if <see langword="WaitTime"/> is <c>0</c>), <see langword="Progress"/> will be <c>0</c>. If timer is finished, <see langword="Progress"/> will be <c>1</c>.
    /// </summary>
    public float Progress => 1f - Ratio;

    public event Action Timeout;

    [Obsolete("Use Timer.Create(float) instead.")] public Timer() { }

    /// <summary>
    /// Example usage:
    /// <code>Timer.Create(1f).Timeout += () => Debug.Log("1 second has passed!");</code>
    /// </summary>
    public static Timer Create(
        float waitTime,
        bool autoStart = true,
        bool oneShot = true,
        bool destroyOnTimeout = true,
        string name = nameof(Timer)
        )
    {
        GameObject gameObject = new() { name = name };
        Timer timer = gameObject.AddComponent<Timer>();
        timer.OneShot = oneShot;
        timer.DestroyOnTimeout = destroyOnTimeout;

        if (autoStart)
            timer.Begin(waitTime);

        return timer;
    }

    /// <summary>
    /// Resets and starts the timer. If no <paramref name="waitTime"/> is given, the previous known one will be used.
    /// </summary>
    /// <param name="waitTime"></param>
    public void Begin(float waitTime = 0)
    {
        if (waitTime > 0)
            WaitTime = waitTime;

        TimeLeft = WaitTime;
        Paused = false;
    }

    public void Pause()
    {
        Paused = true;
    }

    /// <summary>
    /// Does not restart the timer if it's already finished. See <see cref="Begin(float)"/>
    /// </summary>
    public void Resume()
    {
        if (TimeLeft > 0)
            Paused = false;
    }

    private void Update()
    {
        if (Paused) return;

        TimeLeft = Mathf.Max(TimeLeft - Time.unscaledDeltaTime, 0);
        if (TimeLeft == 0)
        {
            if (!OneShot)
                Begin(WaitTime);
            else if (DestroyOnTimeout)
                Destroy(gameObject);
            else
                Paused = true;

            Timeout?.Invoke();
        }
    }

    private void OnDestroy()
    {
        Timeout.UnsubscribeAll();

        if (gameObject != null)
            Destroy(gameObject);
    }

}








