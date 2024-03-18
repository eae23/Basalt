﻿using Basalt.Common.Entities;
using Basalt.Core.Common.Abstractions;
using Basalt.Core.Common.Abstractions.Sound;

namespace Basalt
{
	public class Engine : IEngine
	{
		public bool HasStarted { get; private set; } = false;

		private static Engine? _instance;
		private readonly IGraphicsEngine? _graphicsEngine;
		public readonly ISoundSystem? SoundSystem;
		private readonly IPhysicsEngine? _physicsEngine;
		internal ILogger? logger;
		private readonly IEventBus? _eventBus;
		private bool _exceptionOccurred = false;
		public readonly EntityManager EntityManager = new();

		public Action<Entity> OnCreateEntity;

		private Thread graphicsThread, physicsThread;
		private Engine(IGraphicsEngine? graphicsEngine, ISoundSystem? soundSystem, IPhysicsEngine? physicsEngine, IEventBus? eventBus = null)
		{
			_graphicsEngine = graphicsEngine;
			SoundSystem = soundSystem;
			_physicsEngine = physicsEngine;
			_eventBus = eventBus;
		}

		public static Engine Instance
		{
			get
			{
				if (_instance == null)
				{
					throw new InvalidOperationException("Engine has not been initialized.");
				}
				return _instance;
			}
		}

		public static void Initialize(IGraphicsEngine? graphicsEngine, ISoundSystem? soundSystem, IPhysicsEngine? physicsEngine, IEventBus? eventBus = null)
		{
			if (_instance != null)
			{
				throw new InvalidOperationException("Engine has already been initialized.");
			}
			_instance = new Engine(graphicsEngine, soundSystem, physicsEngine, eventBus);
		}

		public void Run()
		{
			HasStarted = true;
			EventBus?.NotifyStart();
			logger?.LogInformation("Engine Initializing");
			if (_graphicsEngine == null)
			{
				logger?.LogFatal("Graphics engine not specified! Cannogivet run engine.");
				return;
			}
			if (SoundSystem == null)
			{
				logger?.LogWarning("Sound system not specified! Engine will run without sound.");
			}

			if (_physicsEngine == null)
			{
				logger?.LogWarning("Physics engine not specified! Engine will run without physics.");
			}

			SoundSystem?.Initialize();

			physicsThread = new Thread(() => SafeInitialize(_physicsEngine));
			physicsThread.Start();

			graphicsThread = new Thread(() => SafeInitialize(_graphicsEngine));
			graphicsThread.Start();

			physicsThread.Join();
			graphicsThread.Join();

			if (_exceptionOccurred)
			{
				Shutdown();
				return;
			}
		}

		public void Shutdown()
		{
			logger?.LogWarning("Engine shutting down");
			if (physicsThread != null && physicsThread.IsAlive)
			{
				_physicsEngine?.Shutdown();
			}

			if (graphicsThread != null && graphicsThread.IsAlive)
			{
				_graphicsEngine?.Shutdown();
			}

			SoundSystem?.Shutdown();
			logger?.LogInformation("Engine shut down");
		}

		public static void CreateEntity(Entity entity)
		{
			Instance.EntityManager.AddEntity(entity);
			Instance.OnCreateEntity?.Invoke(entity);

			Console.WriteLine(string.Join(", ", Instance.EntityManager.GetEntities().Select(e => e.Transform.Position)));
		}

		public static void RemoveEntity(Entity entity)
		{
			Instance.EntityManager.RemoveEntity(entity);
		}

		private void SafeInitialize(IEngineComponent? component)
		{
			try
			{
				component?.Initialize();
			}
			catch (Exception e)
			{
				_exceptionOccurred = true;
				logger?.LogFatal($"EXCEPTION OCURRED AT {component?.GetType().Name}: {e.Message}");
				Shutdown();
				return;
			}
		}

		public IEventBus? EventBus => _eventBus;
	}
}
