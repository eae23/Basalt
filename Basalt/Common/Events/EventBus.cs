﻿using Basalt.Core.Common.Abstractions.Engine;
namespace Basalt.Common.Events
{
	/// <summary>
	/// Represents an event bus that allows subscribing to and notifying observers of events.
	/// </summary>
	public class EventBus : IEventBus
	{
		private readonly List<IObserver> observers;
		private readonly object lockObject;

		/// <summary>
		/// Initializes a new instance of the <see cref="EventBus"/> class.
		/// </summary>
		public EventBus()
		{
			observers = new List<IObserver>();
			lockObject = new object();
		}

		/// <summary>
		/// Notifies all observers to render.
		/// </summary>
		public void NotifyRender()
		{
			lock (lockObject)
			{
				foreach (var observer in observers)
				{
					observer.OnRenderEvent();
				}
			}
		}

		/// <summary>
		/// Notifies all observers to start.
		/// </summary>
		public void NotifyStart()
		{
			foreach (var observer in observers)
			{
				observer.OnStartEvent();
			}
		}

		/// <summary>
		/// Notifies all observers to update.
		/// </summary>
		public void NotifyUpdate()
		{
			Task.Run(() =>
			{
				lock (lockObject)
				{
					foreach (var observer in observers)
					{
						observer.OnUpdateEvent();
					}
				}
			}).Wait();
		}

		/// <summary>
		/// Notifies all observers of a physics update.
		/// </summary>
		public void NotifyPhysicsUpdate()
		{
			Task.Run(() =>
			{
				lock (lockObject)
				{
					foreach (var observer in observers)
					{
						observer.OnPhysicsUpdateEvent();
					}
				}
			}).Wait();
		}

		/// <summary>
		/// Subscribes an observer to the event bus.
		/// </summary>
		/// <param name="observer">The observer to subscribe.</param>
		public void Subscribe(IObserver observer)
		{
			lock (lockObject)
			{
				observers.Add(observer);
				if (Engine.Instance.Running)
				{
					observer.OnStartEvent();
				}
			}
		}

		/// <summary>
		/// Unsubscribes an observer from the event bus.
		/// </summary>
		/// <param name="observer">The observer to unsubscribe.</param>
		public void Unsubscribe(IObserver observer)
		{
			lock (lockObject)
			{
				observers.Remove(observer);
			}
		}

		public bool IsSubscribed(IObserver observer)
		{
			lock (lockObject)
			{
				return observers.Contains(observer);
			}
		}

		public void Initialize()
		{

		}

		public void Shutdown()
		{

		}
	}
}
