﻿using Basalt;
using Basalt.Common;
using Basalt.Common.Components;
using Basalt.Common.Entities;
using Basalt.Common.Events;
using Basalt.Common.Logging;
using Basalt.Common.Physics;
using Basalt.Common.Utils;
using Basalt.Core.Common.Abstractions.Engine;
using Basalt.Core.Common.Abstractions.Input;
using Basalt.Raylib.Components;
using Basalt.Raylib.Graphics;
using Basalt.Raylib.Input;
using Basalt.Raylib.Utils;
using Basalt.TestField;
using Basalt.TestField.Components;
using Basalt.Types;
using Raylib_cs;
using System.Numerics;


var initParams = new WindowInitParams
{
	Title = "Basalt Test Field",
	Width = 1920,
	Height = 1080,
	TargetFps = 120,
	Fullscreen = false,
	Borderless = true,
	MSAA4X = true,
	PostProcessing = false,
};
ResourceCache.Instance.LoadShader("lighting", @"resources/shaders/lighting.fs", @"resources/shaders/lighting.vs");
ResourceCache.Instance.LoadModel("robot", @"resources/robot.glb", "lighting");
var builder = new EngineBuilder();

builder.AddComponent<IGraphicsEngine, RaylibGraphicsEngine>(() => new RaylibGraphicsEngine(initParams) { LightingShaderCacheKey = "lighting" }, true);
builder.AddComponent<IPhysicsEngine, PhysicsEngine>(true);
builder.AddComponent<IEventBus, EventBus>();
builder.AddComponent<IInputSystem, RaylibInputSystem>();

builder.AddLogger(new ConsoleLogger());

var engine = builder.Build();

engine.Initialize();



var player = new Entity();
player.AddComponent(new CameraController(player));
player.Id = "entity.player";
Vector3 offset = Vector3.UnitY * -1;
player.Transform.Position = new Vector3(0, 5, 0);
player.AddComponent(new SphereRenderer(player) { Size = new Vector3(1f), Color = Color.Red, Offset = offset });
player.AddComponent(new BoxCollider(player) { Size = new Vector3(1, 2, 1), Offset = offset });
player.AddComponent(new Rigidbody(player) { IsKinematic = false, Mass = 25 });
player.AddComponent(new Basalt.TestField.Components.PlayerController(player));
player.AddComponent(new LightSource(player, "lighting") { Color = Color.Red, Type = LightType.Point });
//player.AddComponent(new TrailRenderer(player) { StartRadius = 0.5f, EndRadius = 0.1f, Color = Color.Red, TrailSegmentCount = 25, Offset = offset, TrailRefreshRate = 0.025f });
player.AddComponent(new Label(player) { Text = "This is a test label ", Pivot = UIPivot.TopCenter, Offset = new(100, 25)});
player.AddComponent(new Panel(player) { Size = new(100, 200), Color = Color.Blue, Pivot = UIPivot.TopRight, Offset = new(-100, 200), Rotation = 45f });

Engine.CreateEntity(player);

TestingUtils.SetupTestingScene();