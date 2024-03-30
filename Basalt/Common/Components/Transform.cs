﻿using Basalt.Common.Entities;
using Basalt.Common.Utils;
using Newtonsoft.Json;
using System.Numerics;

namespace Basalt.Common.Components
{
	public sealed class Transform : Component
	{
		private Vector3 position; 
		public Vector3 Position
		{
			get => position;
			set
			{
				var offset = value - position;
				position = value;
				foreach (var child in Entity.Children)
				{
					child.Transform.Position += offset;
				}
			}
		}

		private Quaternion rotation = Quaternion.Identity;
		public Quaternion Rotation
		{
			get => rotation;
			set => rotation = value;
			
		}

		[JsonIgnore]
		public Vector3 Forward => MathExtended.GetForwardVector(Rotation);

		[JsonIgnore]
		public Vector3 Right => MathExtended.GetRightVector(Rotation);
		public Transform(Entity entity) : base(entity)
		{
			Position = new Vector3();

			Engine.Instance.EventBus?.Subscribe(this);
		}


		public override void OnStart()
		{

		}

		public override void OnUpdate()
		{

		}
	}
}
