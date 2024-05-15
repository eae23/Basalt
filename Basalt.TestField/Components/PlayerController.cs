﻿using Basalt.Common;
using Basalt.Common.Components;
using Basalt.Common.Entities;
using Basalt.Common.Utils.Extensions;
using Basalt.Core.Common.Abstractions.Input;
using Basalt.Core.Common.Types;
using System.Numerics;

namespace Basalt.TestField.Components
{
	public class PlayerController : Component
	{
		public float MoveSpeed = 5;
		private IInputSystem inputSystem;
		public PlayerController(Entity entity) : base(entity)
		{

		}
		public override  void OnStart()
		{
			var input = Engine.Instance.GetEngineComponent<IInputSystem>();
			if (input != null)
			{
				inputSystem = input;


				inputSystem?.RegisterKeybind(new(
					InputKey.W,
					ActionType.Hold),
					() => Entity.Transform.Position += Entity.Transform.Forward.XZNormalized() * Time.DeltaTime * MoveSpeed);

				inputSystem?.RegisterKeybind(new(
					InputKey.S,
					ActionType.Hold),
					() => Entity.Transform.Position -= Entity.Transform.Forward.XZNormalized() * Time.DeltaTime * MoveSpeed);

				inputSystem?.RegisterKeybind(new(
					InputKey.A,
					ActionType.Hold),
					() => Entity.Transform.Position -= Entity.Transform.Right.XZNormalized() * Time.DeltaTime * MoveSpeed);

				inputSystem?.RegisterKeybind(new(
					InputKey.D,
					ActionType.Hold),
					() => Entity.Transform.Position += Entity.Transform.Right.XZNormalized() * Time.DeltaTime * MoveSpeed);

				inputSystem?.RegisterKeybind(new(
					InputKey.Space,
					ActionType.Press),
					() => Entity.Rigidbody.Velocity += Vector3.UnitY * MoveSpeed);
			}
		}



		public override void OnUpdate()
		{
		}


		public override void OnCollision(Collider other)
		{

		}
	}
}