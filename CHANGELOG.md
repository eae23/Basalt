# Change Log

All notable changes to this project will be documented in this file. See [versionize](https://github.com/versionize/versionize) for commit guidelines.

<a name="1.7.0"></a>
## [1.7.0](https://www.github.com/thiagomvas/Basalt/releases/tag/v1.7.0) (2024-06-03)

### Features

* Add Audio support to resource cache ([fc3e3ac](https://www.github.com/thiagomvas/Basalt/commit/fc3e3ac6b8f217125063f7b8d6174e297aca6b50))
* Added Trigger functionality to Colliders ([3bc6781](https://www.github.com/thiagomvas/Basalt/commit/3bc678166a2ef9ba7da7594a34f1e16ee2ef9b76))
* Updated RaylibSoundSystem to use ResourceCache and automatically subscribe to update event ([d1288dd](https://www.github.com/thiagomvas/Basalt/commit/d1288dd619b60c1dca78ca042bb155528c253834))

### Bug Fixes

* ChainLink and FixedLink throws when null anchor (closes [#21](https://www.github.com/thiagomvas/Basalt/issues/21)) ([9dba6b6](https://www.github.com/thiagomvas/Basalt/commit/9dba6b692973e1dae99a2a265f2dc6b68ae77cac))
* ModelRenderer now updates rendered model when model key updates ([f849bf8](https://www.github.com/thiagomvas/Basalt/commit/f849bf8d30f422a21b5c6da50b6112762a8f4223))
* OnDestroy no longer is called twice (closes [#19](https://www.github.com/thiagomvas/Basalt/issues/19)) ([b44a0dc](https://www.github.com/thiagomvas/Basalt/commit/b44a0dce571c4ed99c4cbe9a414e968ce34f951d))
* PhysicsEngine and Grid would crash due to parallelism with distanced enough entities. ([36b9fe5](https://www.github.com/thiagomvas/Basalt/commit/36b9fe5e1a81e3586b869474968d186f4b37809b))
* ResourceCache keys are now trimmed and changed to lowercase. ([d98ba73](https://www.github.com/thiagomvas/Basalt/commit/d98ba730dc22a2fd7021dc34c16b54af22f3d4f3))
* ResourceCache no longer crashes when trying to get a resource of different type. Returns null instead ([a8bca62](https://www.github.com/thiagomvas/Basalt/commit/a8bca62118da7e31fcffd6404cbfc7b2bc12f58e))

<a name="1.6.2"></a>
## [1.6.2](https://www.github.com/thiagomvas/Basalt/releases/tag/v1.6.2) (2024-05-29)

<a name="1.6.1"></a>
## [1.6.1](https://www.github.com/thiagomvas/Basalt/releases/tag/v1.6.1) (2024-05-29)

### Bug Fixes

* Removed pointless fields and constructors ([c9cb7cf](https://www.github.com/thiagomvas/Basalt/commit/c9cb7cf0ca6dd2a4a1c17b622eb6c5b9fdc63dc3))

<a name="1.6.0"></a>
## [1.6.0](https://www.github.com/thiagomvas/Basalt/releases/tag/v1.6.0) (2024-05-28)

### Features

* Add UI System ([ac91489](https://www.github.com/thiagomvas/Basalt/commit/ac914891b82a865cb1c4b79329f9e64dbaa6fdb6))
* Add Panel UI Component ([28fd0b6](https://github.com/thiagomvas/Basalt/commit/28fd0b6d38277fcbcbae6d5dd62f9dfa56d811ee))
* Add Label UI Component ([59095ba](https://github.com/thiagomvas/Basalt/commit/59095baaf841e88becc4abdab344fac7e0fb0f69))
* Add Image UI Component ([b482269](https://github.com/thiagomvas/Basalt/commit/b48226907b962d49140709eb9519974a159b7ffd))
* Add Progress Bar UI Component ([d1e34dc](https://github.com/thiagomvas/Basalt/commit/d1e34dc49cec92d2524ac909cdf2719cb580dd23))
* Add Button UI Component ([74742dc](https://github.com/thiagomvas/Basalt/commit/74742dcd8fd74a0c1de900bc04be742a77cc6af8))
* Add abstract UIComponent class to support custom UI elements([de7f029](https://github.com/thiagomvas/Basalt/commit/de7f02949251efcc93c208cfb2e4c797db7de6d5))
* Add Texture support to ResourceCache([0cc8f73](https://github.com/thiagomvas/Basalt/commit/0cc8f7341ec05798e50c4d6be2ac83baa46c786c))


<a name="1.5.0"></a>
## [1.5.0](https://www.github.com/thiagomvas/Basalt/releases/tag/v1.5.0) (2024-05-23)

### Features

* Event Bus now supports custom events ([c7715a5](https://www.github.com/thiagomvas/Basalt/commit/c7715a5d5238e5b5c24be67f77d9dafc7e44cacd))
* Rewrite event system altogether to optimize event notifications. ([9d38f94](https://www.github.com/thiagomvas/Basalt/commit/9d38f9403ce2db650608ea1cebb4d36fb69f402b))

<a name="1.4.0"></a>
## [1.4.0](https://www.github.com/thiagomvas/Basalt/releases/tag/v1.4.0) (2024-05-23)

### Features

* RaylibCache reworked to be extensions method of ResourceCache instead ([b5f105d](https://www.github.com/thiagomvas/Basalt/commit/b5f105da13b7034eaee618b5d6246536a658a6e9))
* Reworked ResourceCache to store objects instead of strings ([16aa5ba](https://www.github.com/thiagomvas/Basalt/commit/16aa5bacaaa534c9e20c6c7e8c4903bfbd517a10))

### Bug Fixes

* Primitive models are now affected by lighting ([48a314c](https://www.github.com/thiagomvas/Basalt/commit/48a314c519abcb44e56268f5278139d2112e59e1))

<a name="1.3.2"></a>
## [1.3.2](https://www.github.com/thiagomvas/Basalt/releases/tag/v1.3.2) (2024-05-22)

### Bug Fixes

* EventBus now calls a method that isn't the overridable method for events. ([1639db6](https://www.github.com/thiagomvas/Basalt/commit/1639db64d3bbfad02a95cd1b59ff0383a0272675))
* Renderers rendered when parent was disabled ([783638a](https://www.github.com/thiagomvas/Basalt/commit/783638a8e80c41db7f147cacc5f7dd1d64fcb68e))

<a name="1.3.1"></a>
## [1.3.1](https://www.github.com/thiagomvas/Basalt/releases/tag/v1.3.1) (2024-05-18)

### Bug Fixes

* Improve grid performance due to Octree simply not working ([fc8e224](https://www.github.com/thiagomvas/Basalt/commit/fc8e22444cc4cd2d627af194989a81ec72d7efc4))

<a name="1.3.0"></a>
## [1.3.0](https://www.github.com/thiagomvas/Basalt/releases/tag/v1.3.0) (2024-05-17)

### Features

* Add PerlinNoise generator ([6723287](https://www.github.com/thiagomvas/Basalt/commit/6723287d88d0f99752000ea60a993706453b50f8))
* Migrate MathExtended and add BasaltMath ([b73d175](https://www.github.com/thiagomvas/Basalt/commit/b73d175fa9ce18d4d96327d1c03d0cd0796f6dfd))

### Bug Fixes

* XML documentation wasn't included in package ([8f5192c](https://www.github.com/thiagomvas/Basalt/commit/8f5192ce75b156f8dea0c7059692ffd9e93596bc))

<a name="1.2.0"></a>
## [1.2.0](https://www.github.com/thiagomvas/Basalt/releases/tag/v1.2.0) (2024-05-17)

### Features

* Add Octree ([258cc31](https://www.github.com/thiagomvas/Basalt/commit/258cc3181e654f0a0a91f75d1fba774060fd55b7))

<a name="1.1.0"></a>
## [1.1.0](https://www.github.com/thiagomvas/Basalt/releases/tag/v1.1.0) (2024-05-16)

### Features

* Add extension methods for EngineBuilder to add raylib presets ([5b85754](https://www.github.com/thiagomvas/Basalt/commit/5b85754f5ffa07e817d5ec6a991522617444484d))

### Bug Fixes

* Engine would not fully remove any references from component when removing entity ([b144dd4](https://www.github.com/thiagomvas/Basalt/commit/b144dd4f917ebfe7587e9c73ac5252519b77cdf4))
* Grid would non-stop add entities. ([e7c806a](https://www.github.com/thiagomvas/Basalt/commit/e7c806a83916dd8731b0d7f30089488aea0e300f))
* RaylibSoundSystem missing parameterless constructor ([05125e3](https://www.github.com/thiagomvas/Basalt/commit/05125e362373abbf09bab9374ea32eae161b343a))

<a name="1.0.1"></a>
## [1.0.1](https://www.github.com/thiagomvas/Basalt/releases/tag/v1.0.1) (2024-05-15)

### Bug Fixes
* No longer throws random exceptions for tightly timed entity instantiation ([e0f1ce0](https://www.github.com/thiagomvas/Basalt/commit/e0f1ce08e3b544ffd07b10e1809656eb94e1a11b))
* Physics Delta Time was not consistent ([e793eba](https://www.github.com/thiagomvas/Basalt/commit/e793eba484f1c517217dfaff2528fdf089544006))
* Raylib Graphics Engine always rendered info boxes ([d03d650](https://www.github.com/thiagomvas/Basalt/commit/d03d6502fa1896d2712af0b4c05991eebfe9e6ef))
* Raylib Graphics Engine logger was always null ([8cdd55a](https://www.github.com/thiagomvas/Basalt/commit/8cdd55acbb13ca378500a70ea55acfac2396618d))