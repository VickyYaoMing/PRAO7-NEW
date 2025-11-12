using System;

public static class ActionExtensions
{
    /// <summary>
    /// Unsubscribes all listeners from <paramref name="action"/>. Useful to avoid memory leaks when destroying an object that other scripts may listen to.
    /// </summary>
    /// <param name="action"></param>
    public static void UnsubscribeAll(this Action action)
    {
        // not gonna lie, Action might do this automatically in destructor or smth but at least we make sure when using this
        // BECAUSE destructors aren't always called at the same time as game objects are deleted, but by the GC

        if (action == null) return;

        Delegate[] clientList = action.GetInvocationList();
        if (clientList == null) return;

        foreach (var d in clientList)
            action -= d as Action;
    }
}
