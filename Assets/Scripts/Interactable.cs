using System;
using System.Threading;

public interface IInteractable
{
    event EventHandler<OnInteractedEventArgs> OnInteracted;
    void Interact();
}

public class OnInteractedEventArgs:EventArgs
{
    public int Id;
}