# Axwabo.CommandSystem

An SCP: Secret Laboratory plugin to improve player selection, command development and the Remote Admin GUI.

> [!TIP]
> Check out the [wiki](https://github.com/Axwabo/CommandSystem/wiki) for more information.

# Installation

## LabAPI

1. Install [Axwabo.Helpers](https://github.com/Axwabo/SCPSL-Helpers/) (LabAPI version) as a dependency
2. Install [Harmony 2.2.2](https://github.com/pardeike/Harmony/releases/tag/v2.2.2.0) as a dependency, you need the **net4.8** version
3. Download the `Axwabo.CommandSystem.dll` file from the [releases page](https://github.com/Axwabo/CommandSystem/releases)
4. Place the file in the `plugins` folder:
    - Windows: `%appdata%\SCP Secret Laboratory\LabAPI-Beta\plugins\`
    - Linux: `.config/SCP Secret Laboratory/LabAPI-Beta/plugins/`
5. Restart the server

## Development

1. Download [Axwabo.Helpers](https://github.com/Axwabo/SCPSL-Helpers/) and add it as a reference
2. Download the `Axwabo.CommandSystem.dll` file from the [releases page](https://github.com/Axwabo/CommandSystem/releases)
3. Add the assembly as a reference to your project
4. Code away!

# Features

## Command Development System

The command system is a simple, yet powerful system that allows you to easily create commands for your plugin.

You **will** need to reference `CommandSystem.Core` if you want to work with players; ensure that you're not using anything from the simple `CommandSystem` namespace.

Call `Axwabo.CommandSystem.Registration.CommandRegistrationProcessor.RegisterAll(object)` in your plugin's load method
to register all commands from your assembly. Example:

```csharp
using System;
using Axwabo.CommandSystem.Registration;
using LabApi.Loader.Features.Plugins;

public sealed class MyPlugin
{
    public override string Name => "Name";
    public override string Description => "Description";
    public override string Author => "Author";
    public override Version Version => GetType().Assembly.GetName().Version;
    public override Version RequiredApiVersion { get; } = new(1, 0, 0);
    
    public override void Enable()
    {
        CommandRegistrationProcessor.RegisterAll(this);
    }
    
    public override void Disable()
    {
        CommandRegistrationProcessor.UnregisterAll(this);
    }
}
```

**To create your custom commands,**

1. Extend the `Axwabo.CommandSystem.CommandBase` class.
2. Add the attribute `Axwabo.CommandSystem.Attributes.CommandProperties` to your class and specify the necessary properties.
3. Override the `Execute` method.

> [!TIP]
> The `UnifiedTargetingCommand` and `SeparatedTargetingCommand` classes help to create commands that affect players.
> 
> Documentation can be found on the [wiki](https://github.com/Axwabo/CommandSystem/wiki/CommandBase)

## Improved Player Selectors

The base-game player ID list parsing has been extended with a more powerful way to select players.
It's integrated into the base game, meaning you can use vanilla commands with the selectors.

For example, if you want to forceclass yourself to Class-D, instead of `forceclass 2 1` you can do `forceclass @s 1`, where
**@s** stands for "self".

It also allows you to select a player based on a substring of their nickname, no more going to the GUI RA anymore 😉

> [!TIP]
> You can read more about player selectors on the [wiki](https://github.com/Axwabo/CommandSystem/wiki/Selectors)

## Remote Admin GUI Extensions

> [!NOTE]
> This feature is not currently compatible with [CedMod](https://github.com/CedModV2/CedMod)

The `Axwabo.CommandSystem.RemoteAdminExtensions.RemoteAdminOptionBase` class lets you add "player entries" to the Remote Admin GUI.
These can be interacted with through the buttons in the `Request Data` section.

For example, a simple counter can be made by making the `Request Data` subtract one, the `Request IP` button add one number and `Request Auth` would reset the counter.

To list, hide or show specific options in the GUI, use the `raOption` command.

The `Stack` option is built-in but hidden by default.
It allows you to use the [players on the stack](#player-selection-stack) in the Remote Admin GUI.
To enable it, execute the `raOpt show @stack` command.

> [!TIP]
> The [wiki](https://github.com/Axwabo/CommandSystem/wiki/Options) contains more details about RA Extensions.

## Player Selection Stack

> [!NOTE]
> This feature is not currently compatible with [CedMod](https://github.com/CedModV2/CedMod)

Everyone with Remote Admin access is able to use the player selection stack. You can push a list of players, pop, duplicate, clear.
It's incredibly useful for commands that require multiple players.

Use `stackPush <players>` to push a list of players to the stack. This also works with player selectors.

Use `stackPop [index]` to discard the topmost player list from the stack.

When using a command that accepts player targets, you can use `@stack` to use the topmost (at index 0) player list from the stack.

You can also give it an index to use a specific player list from the stack, separated by one of `:>_-` characters.
For example, `@stack:1` will use the second player list from the stack (indexes start with 0).

> [!NOTE]
> The most recently pushed player list is considered the top of the stack. To view the stack, use `stackList` and list at index 0 is the topmost player list.

## Config

There are some miscellaneous settings that can be specified in the plugin configuration.
Read about them on the [wiki](https://github.com/Axwabo/CommandSystem/wiki/Configuration)
