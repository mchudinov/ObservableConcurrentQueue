namespace ObservableConcurrentQueue
{
    /// <summary>
    /// Observable Concurrent queue changed event handler
    /// </summary>
    /// <typeparam name="T">
    /// The concurrent queue elements type
    /// </typeparam>
    /// <param name="sender">
    /// The sender.
    /// </param>
    /// <param name="args">
    /// The <see cref="NotifyConcurrentQueueChangedEventArgs{T}"/> instance containing the event data.
    /// </param>
    public delegate void ConcurrentQueueChangedEventHandler<T>(object sender, NotifyConcurrentQueueChangedEventArgs<T> args);
}