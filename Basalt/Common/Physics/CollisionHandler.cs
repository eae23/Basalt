﻿using Basalt.Common.Components;
using System.Numerics;

namespace Basalt.Common.Physics
{
	/// <summary>
	/// Handles collision between different types of colliders.
	/// </summary>
	public static class CollisionHandler
	{
		/// <summary>
		/// Delegate for collision handler methods.
		/// </summary>
		/// <param name="col1">The first collider.</param>
		/// <param name="col2">The second collider.</param>
		private delegate void CollisionHandlerDelegate(Collider col1, Collider col2);

		/// <summary>
		/// Dictionary to store collision handler delegates for different collider types.
		/// </summary>
		private readonly static Dictionary<(Type, Type), CollisionHandlerDelegate> handlers = new()
						{
							{(typeof(BoxCollider), typeof(BoxCollider)), BoxBoxCollision},
						};

		/// <summary>
		/// Handles collision between two colliders.
		/// </summary>
		/// <param name="col1">The first collider.</param>
		/// <param name="col2">The second collider.</param>
		public static void Handle(Collider col1, Collider col2)
		{
			if (handlers.TryGetValue((col1.GetType(), col2.GetType()), out CollisionHandlerDelegate handler))
			{
				handler(col1, col2);
			}
			else
			{
				Engine.Instance.Logger?.LogError($"No collision handler for {col1.GetType()} and {col2.GetType()}");
			}
		}

		/// <summary>
		/// Handles collision between two box colliders.
		/// </summary>
		/// <param name="col1">The first box collider.</param>
		/// <param name="col2">The second box collider.</param>
		private static void BoxBoxCollision(Collider col1, Collider col2)
		{
			BoxCollider box1 = (BoxCollider)col1;
			BoxCollider box2 = (BoxCollider)col2;

			Rigidbody? rb1 = box1.Entity.Rigidbody;
			Rigidbody? rb2 = box2.Entity.Rigidbody;

			Vector3 extents1 = box1.Size / 2f;
			Vector3 extents2 = box2.Size / 2f;

			// Calculate the min and max points of the two colliders along each axis
			Vector3 min1 = box1.Position - extents1;
			Vector3 max1 = box1.Position + extents1;
			Vector3 min2 = box2.Position - extents2;
			Vector3 max2 = box2.Position + extents2;

			// Calculate the overlap along each axis
			float overlapX = Math.Max(0, Math.Min(max1.X, max2.X) - Math.Max(min1.X, min2.X));
			float overlapY = Math.Max(0, Math.Min(max1.Y, max2.Y) - Math.Max(min1.Y, min2.Y));
			float overlapZ = Math.Max(0, Math.Min(max1.Z, max2.Z) - Math.Max(min1.Z, min2.Z));

			if (overlapX == 0 || overlapY == 0 || overlapZ == 0)
			{
				return; // No overlap, no need to perform separation or movement calculations
			}

			// Calculate the direction of least penetration
			Vector3 separationDirection = Vector3.Zero;
			float minOverlap = Math.Min(overlapX, Math.Min(overlapY, overlapZ));
			if (minOverlap == overlapX)
			{
				separationDirection = (box1.Position.X < box2.Position.X) ? -Vector3.UnitX : Vector3.UnitX;
			}
			else if (minOverlap == overlapY)
			{
				separationDirection = (box1.Position.Y < box2.Position.Y) ? -Vector3.UnitY : Vector3.UnitY;
			}
			else if (minOverlap == overlapZ)
			{
				separationDirection = (box1.Position.Z < box2.Position.Z) ? -Vector3.UnitZ : Vector3.UnitZ;
			}

			// Move the colliders to separate them along the direction of least penetration
			float separationDistance = Math.Abs(minOverlap);

			if (rb1 != null && rb1.IsKinematic)
			{
				box2.Entity.Transform.Position -= separationDirection * separationDistance;
				rb2.Velocity *= 0.5f;
			}
			else if (rb2 != null && rb2.IsKinematic)
			{
				box1.Entity.Transform.Position += separationDirection * separationDistance;
				rb1.Velocity *= 0.5f;
			}
			else
			{
				float totalMass = (rb1?.Mass ?? 0) + (rb2?.Mass ?? 0);
				float massRatio1 = (rb2?.Mass ?? 0) / totalMass;
				float massRatio2 = (rb1?.Mass ?? 0) / totalMass;

				Vector3 relativeVelocity = (rb2?.Velocity ?? Vector3.Zero) - (rb1?.Velocity ?? Vector3.Zero);
				Vector3 impulse = (1 + massRatio1) * massRatio2 * Vector3.Dot(relativeVelocity, separationDirection) * separationDirection;

				if (rb1 != null)
				{
					rb1.Velocity += impulse / rb1.Mass;
				}

				if (rb2 != null)
				{
					rb2.Velocity -= impulse / rb2.Mass;
				}

				box1.Entity.Transform.Position += separationDirection * separationDistance / 2f;
				box2.Entity.Transform.Position -= separationDirection * separationDistance / 2f;
			}
		}
	}
}
