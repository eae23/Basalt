﻿using Basalt.Common.Components;
using Basalt.Core.Common.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;
namespace Basalt.Common.Entities
{
	public class ComponentDto
	{
		public required Type Type { get; set; }
		public required Component Data { get; set; }
	}

	/// <summary>
	/// Represents an entity in the game world, containing components and children entities.
	/// </summary>
	public class Entity
	{
		[JsonProperty("Components")]
		private List<ComponentDto> componentDtos = new();
		private List<Component> components = new();

		[JsonProperty("Id")]
		public string Id { get; set; } = System.Guid.NewGuid().ToString();
		/// <summary>
		/// The transform component of the entity.
		/// </summary>
		[JsonIgnore]
		public Transform Transform;

		/// <summary>
		/// The rigidbody component of the entity.
		/// </summary>
		[JsonIgnore]
		public Rigidbody? Rigidbody;

		[JsonIgnore]
		public Collider? Collider;

		[JsonIgnore]
		public Entity? Parent { get; private set; }

		/// <summary>
		/// The children entities of the entity.
		/// </summary>
		public List<Entity> Children { get; set; } = new();

		private bool enabled = true;
		/// <summary>
		/// Whether the entity is enabled or not.
		/// </summary>
		public bool Enabled
		{
			get => enabled;
			set
			{
				enabled = value;
				foreach (var child in Children)
				{
					child.Enabled = value;
				}
			}
		}

		public Entity()
		{
			Transform = new Transform(this);
			AddComponent(Transform);
		}

		/// <summary>
		/// Serializes the entity to a JSON string.
		/// </summary>
		/// <returns>The entity and it's data in JSON format</returns>
		public string SerializeToJson()
		{
			componentDtos = new();
			foreach (var component in components)
			{
				componentDtos.Add(new ComponentDto
				{
					Type = component.GetType(),
					Data = component
				});
			}

			List<JObject> childrenObjects = new List<JObject>();

			foreach (var child in Children)
			{
				JObject childObject = JObject.Parse(child.SerializeToJson());
				childrenObjects.Add(childObject);
			}

			foreach (var child in childrenObjects)
			{
				Console.WriteLine(child.ToString(Formatting.Indented));
			}
			var entityJson = new JObject();
			entityJson["Components"] = JArray.FromObject(componentDtos);
			entityJson["Children"] = new JArray(childrenObjects.ToArray()); // Use the parsed child objects
			entityJson["Id"] = Id;

			return entityJson.ToString(Formatting.Indented);
		}

		/// <summary>
		/// Deserializes an entity from a JSON string.
		/// </summary>
		/// <param name="json">The json string to deserialize from</param>
		/// <returns>An entity instance from the JSON string</returns>
		public static Entity DeserializeFromJson(string json)
		{
			JObject jObject = JObject.Parse(json);

			var target = new Entity();
			target.Id = jObject["Id"]?.Value<string>() ?? Guid.NewGuid().ToString();

			if (jObject["Components"] == null)
				return target;

			foreach (var component in jObject["Components"]!)
			{
				var type = ByName(component["Type"]?.Value<string>()?.Split(',').First() ?? string.Empty);

				if (type == null)
				{
					continue;
				}

				ConstructorInfo constructor = type.GetConstructor(new[] { typeof(Entity) })!;


				var typeProps = type.GetProperties();
				var instance = constructor.Invoke([target]);

				if (component["Data"] == null)
				{
					continue;
				}

#pragma warning disable CS8602 // Dereference of a possibly null reference.
				foreach (var prop in component["Data"] as JObject) // CS8602: Dereference of a possibly null reference.
				{
					if (typeProps.Any(p => p.Name == prop.Key))
					{
						var propInfo = typeProps.First(p => p.Name == prop.Key);
						if (propInfo.CanWrite)
						{
							propInfo.SetValue(instance, prop.Value.ToObject(propInfo.PropertyType));
						}
					}
				}


				target.ForceAddComponent((Component)instance);
			}

			foreach (var child in jObject["Children"]) // CS8602: Dereference of a possibly null reference.
			{
				var childEntity = DeserializeFromJson(child.ToString());
				Console.WriteLine($"Deserialized entity with id {childEntity.Id}");
				target.AddChildren(childEntity);
			}

			return target;

#pragma warning restore CS8602 // Dereference of a possibly null reference.
		}

		/// <summary>
		/// Adds a component to the entity.
		/// </summary>
		/// <param name="component">The component to add</param>
		public void AddComponent(Component component)
		{
			// Check for singleton attribute
			if (components.Any(c => c.GetType() == component.GetType()) && component.GetType().GetCustomAttribute<SingletonComponentAttribute>() != null)
				return;

			components.Add(component);
			switch (component)
			{
				case Rigidbody rb when Rigidbody == null:
					Rigidbody = rb;
					break;

				case Transform t:
					Transform = t;
					break;

				case Collider c when Collider == null:
					Collider = c;
					break;

				default:
					// Handle other cases if necessary
					break;
			}

		}

		private void ForceAddComponent(Component component)
		{

			// Check for singleton attribute
			if (components.Any(c => c.GetType() == component.GetType()) && component.GetType().GetCustomAttribute<SingletonComponentAttribute>() != null)
			{
				// Replace the existing component
				components.Remove(components.First(c => c.GetType() == component.GetType()));
			}


			components.Add(component);
			if (Rigidbody == null && component is Rigidbody rb)
			{
				Rigidbody = rb;
			}

			else if (component is Transform t)
			{
				Transform = t;
			}
		}

		/// <summary>
		/// Removes a component from the entity.
		/// </summary>
		/// <param name="component">The component reference to remove</param>
		public void RemoveComponent(Component component)
		{
			components.Remove(component);
			component.onDestroy();
			if (component.GetType() == typeof(Rigidbody))
				Rigidbody = null;

		}

		/// <summary>
		/// Gets a component of the specified type from the entity.
		/// </summary>
		/// <typeparam name="T">The type of the component</typeparam>
		/// <returns>The first instance of a component of type <typeparamref name="T"/></returns>
		public T? GetComponent<T>() where T : Component
		{
			if (typeof(T) == typeof(Transform))
				return Transform as T;
			if (typeof(T) == typeof(Rigidbody))
				return Rigidbody as T;
			if (typeof(T) == typeof(Collider))
				return Collider as T;

			foreach (var component in components)
			{
				if (component is T match)
				{
					return match;
				}
			}

			return null;
		}


		/// <summary>
		/// Gets all components of the entity.
		/// </summary>
		/// <returns>A list of all components in this entity</returns>
		public List<Component> GetComponents()
		{
			return new List<Component>(components);
		}

		/// <summary>
		/// Adds a child entity to the entity.
		/// </summary>
		/// <param name="child">The child entity to add</param>
		public void AddChildren(Entity child)
		{
			Children.Add(child);
			child.Parent = this;
		}

		/// <summary>
		/// Removes a child entity from the entity.
		/// </summary>
		/// <param name="child">The child entity reference to remove</param>
		public void RemoveChildren(Entity child)
		{
			Children.Remove(child);
		}

		/// <summary>
		/// Destroys the entity and all of its children.
		/// </summary>
		public void Destroy()
		{
			Engine.RemoveEntity(this);
			foreach (var child in Children)
			{
				child.Destroy();
			}

			foreach (var component in components)
			{
				component.onDestroy();
			}
		}


		internal void CallOnCollision(Collider other)
		{
			foreach (var component in components)
				component.OnCollision(other);
		}

		private static Type? ByName(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
				return null;

			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies().Reverse())
			{
				var tt = assembly.GetType(name);
				if (tt != null)
				{
					return tt;
				}
			}

			return null;
		}

		internal void CallStart()
		{
			foreach (var component in components)
			{
				if (!component.started)
				{
					component.OnStartEvent(this, EventArgs.Empty);
				}
			}
		}
	}
}
