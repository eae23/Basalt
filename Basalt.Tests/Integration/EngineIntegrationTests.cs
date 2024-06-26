﻿using Basalt.Common;
using Basalt.Common.Components;
using Basalt.Common.Entities;
using Basalt.Common.Events;
using Basalt.Common.Utils;
using Basalt.Core.Common.Abstractions.Engine;
using Basalt.Tests.Common;
using Moq;
using System.Numerics;

namespace Basalt.Tests.Integration
{
	[TestFixture]
	public class EngineIntegrationTests
	{
		[Test]
		public void EngineInitialize_WithRequiredComponents_ShouldRunSuccessfully()
		{
			// Arrange
			var logger = new Mock<ILogger>();

			var engine = new EngineBuilder()
				.AddComponent<IGraphicsEngine>(() => Mock.Of<IGraphicsEngine>(), true)
				.AddComponent<IEventBus, EventBus>()
				.AddComponent<TestEngineComponent>(() => new() { Value = 10 })
				.AddLogger(logger.Object)
				.Build();

			// Act
			engine.Initialize();

			// Assert
			Assert.IsTrue(engine.Running, "Engine did not change running bool");
			Assert.IsNotNull(engine.EntityManager, "Entity manager was null");
			Assert.IsNotNull(engine.GetEngineComponent<IGraphicsEngine>());
			Assert.IsNotNull(engine.GetEngineComponent<IEventBus>());
			Assert.IsNotNull(engine.GetEngineComponent<TestEngineComponent>());
			Assert.That(engine.GetEngineComponent<TestEngineComponent>()!.Value, Is.EqualTo(10));
			Assert.IsTrue(engine.GetEngineComponent<TestEngineComponent>()!.Initialized);
		}

		[Test]
		public void EngineInitialize_WhenMissingGraphicsEngine_ShouldNotInit()
		{
			// Arrange
			var loggerMock = new Mock<ILogger>();

			var engineBuilder = new EngineBuilder()
									.AddComponent<IEventBus>(() => Mock.Of<IEventBus>())
									.AddLogger(loggerMock.Object);
			var engine = engineBuilder.Build();

			// Act
			engine.Initialize();

			// Assert
			Assert.IsFalse(engine.Running, "Engine should not be running");
		}

		[Test]
		public void EngineInitialize_WhenMissingEventBus_ShouldNotInit()
		{
			// Arrange
			var loggerMock = new Mock<ILogger>();

			var engineBuilder = new EngineBuilder()
									.AddComponent<IGraphicsEngine>(() => Mock.Of<IGraphicsEngine>())
									.AddLogger(loggerMock.Object);
			var engine = engineBuilder.Build();

			// Act
			engine.Initialize();

			// Assert
			Assert.IsFalse(engine.Running, "Engine should not be running");
		}

		[Test]
		public void EngineShutdown_ShouldShutdownSuccessfuly()
		{
			// Arrange
			var engine = new EngineBuilder()
				.AddComponent<IGraphicsEngine>(() => Mock.Of<IGraphicsEngine>(), true)
				.AddComponent<IEventBus, EventBus>()
				.AddComponent<TestEngineComponent>(() => new() { Value = 10 })
				.Build();

			engine.Initialize();

			// Act
			engine.Shutdown();

			// Assert
			Assert.IsFalse(engine.Running, "Engine did not change running bool");
			Assert.IsFalse(engine.GetEngineComponent<TestEngineComponent>()!.Initialized);
		}

		[Test]
		public void EngineCreateEntity_WhenInitialized_ShouldCreateEntity()
		{
			// Arrange
			var entity = new Entity();

			entity.Id = "entity1";

			var engine = new EngineBuilder()
				.AddComponent<IGraphicsEngine>(() => Mock.Of<IGraphicsEngine>(), true)
				.AddComponent<IEventBus, EventBus>()
				.Build();


			engine.Initialize();


			// Act
			Engine.CreateEntity(entity);

			// Assert
			Assert.IsNotNull(entity, "Entity was null");
			Assert.That(engine.EntityManager.GetEntities().Count, Is.EqualTo(1));
			Assert.IsNotNull(engine.EntityManager.GetEntity("entity1"));
		}


		[Test]
		public void EngineEventBus_WhenHasObservers_ShouldNotify()
		{
			// Arrange
			var engine = new EngineBuilder()
				.AddComponent<IGraphicsEngine>(() => Mock.Of<IGraphicsEngine>(), true)
				.AddComponent<IEventBus, EventBus>()
				.Build();

			engine.Initialize();

			var entity = new Entity();
			entity.AddComponent(new TestComponent(entity));
			Engine.CreateEntity(entity);

			int physicsCalls = 10;
			int updareCalls = 12;
			int renderCalls = 12;

			var eventBus = engine.GetEngineComponent<IEventBus>();

			// Act
			for (int i = 0; i < physicsCalls; i++)
			{
				eventBus?.TriggerEvent(BasaltConstants.PhysicsUpdateEventKey);
			}

			for (int i = 0; i < updareCalls; i++)
			{
				eventBus?.TriggerEvent(BasaltConstants.UpdateEventKey);
			}

			for (int i = 0; i < renderCalls; i++)
			{
				eventBus?.TriggerEvent(BasaltConstants.RenderEventKey);
			}

			// Assert
			Assert.That(entity.GetComponent<TestComponent>()!.OnStartCount, Is.EqualTo(1));
			Assert.That(entity.GetComponent<TestComponent>()!.OnPhysicsUpdateCount, Is.EqualTo(physicsCalls));
			Assert.That(entity.GetComponent<TestComponent>()!.OnUpdateCount, Is.EqualTo(updareCalls));
			Assert.That(entity.GetComponent<TestComponent>()!.OnRenderCount, Is.EqualTo(renderCalls));
		}

		[Test]
		public void EngineCreateEntity_WhenNotInitialized_ShouldThrow()
		{
			// Arrange
			var entity = new Entity();

			entity.Id = "entity1";

			var engine = new EngineBuilder()
				.AddComponent<IGraphicsEngine>(() => Mock.Of<IGraphicsEngine>(), true)
				.AddComponent<IEventBus, EventBus>()
				.Build();

			// Act
			Assert.Throws<InvalidOperationException>(() => Engine.CreateEntity(entity));
		}

		[Test]
		public void EngineRemoveEntity_ShouldRemoveAnyReferences()
		{
			// Arrange
			var entity = new Entity();
			entity.Transform.Position = Vector3.Zero;
			entity.AddComponent(new Rigidbody(entity));
			IEqualityComparer<Vector3> comparer = new Vector3EqualityComparer();

			entity.Id = "entity1";

			var engine = new EngineBuilder()
				.UseMockPreset()
				.Build();

			engine.Initialize();

			Engine.CreateEntity(entity);

			// Act
			Engine.RemoveEntity(entity);
			engine.GetEngineComponent<IEventBus>()!.TriggerEvent(BasaltConstants.PhysicsUpdateEventKey);

			// Assert
			Assert.That(engine.EntityManager.GetEntities().Count, Is.EqualTo(0));
			Assert.IsNull(engine.EntityManager.GetEntity("entity1"));
			Assert.That(entity.Transform.Position, Is.EqualTo(Vector3.Zero).Using(comparer));
		}
	}
}
