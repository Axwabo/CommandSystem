﻿# Axwabo.CommandSystem

An SCP: Secret Laboratory plugin to improve moderation, command development and the Remote Admin GUI.

# Installation

## NW Plugin API

1. Run `p install Axwabo/CommandSystem` in the server console
2. Restart the server

## Manual

1. Install [Axwabo.Helpers](https://github.com/Axwabo/SCPSL-Helpers/) (NWAPI version) as a dependency
2. Install [Harmony](https://github.com/pardeike/Harmony) as a dependency, you need the **net4.8** version
3. Download the `Axwabo.CommandSystem.dll` file from the releases page
4. Place the file in the `plugins` folder: `%appdata%\SCP Secret Laboratory\PluginAPI\plugins\port\`
5. Restart the server

## Developer API

1. Download [Axwabo.Helpers](https://github.com/Axwabo/SCPSL-Helpers/) and add it as a reference
2. Download the `Axwabo.CommandSystem.dll` file from the releases page
3. Add the assembly as a reference to your project
4. Code away!

# Features

## Command Development System

The command system is a simple, yet powerful system that allows you to easily create commands for your plugin.

You **will** need to reference `CommandSystem.Core` if you want to work with players; ensure that you're not using anything from the simple `CommandSystem` namespace.

Call `Axwabo.CommandSystem.Registration.CommandRegistrationProcessor.RegisterAll(plugin)` in your plugin's load method
to register all commands from your assembly. Example:

```csharp
using PluginAPI.Core;
using Axwabo.CommandSystem.Registration;
using PluginAPI.Core.Attributes;

public sealed class MyPlugin
{
    [PluginEntryPoint("Name", "Version", "Description", "Author")]
    public void OnEnabled()
    {
        CommandRegistrationProcessor.RegisterAll(this);
        Log.Info("MyPlugin has been enabled.");
    }
}
```

To create your custom commands,

1. Extend the `Axwabo.CommandSystem.CommandBase` class.
2. Add the attribute `Axwabo.CommandSystem.Attributes.CommandProperties` to your class and specify the necessary properties.
3. Override the `Execute` method.

**Documentation can be found on the wiki.**

## Improved Player Selectors

The base-game player ID list parsing has been extended with a more powerful way to select players. It's integrated into the base game, meaning you can use vanilla commands with the selectors.

For example, if you want to forceclass yourself to Class-D, instead of `forceclass 2 1` you can do `forceclass @s 1`, where
**@s** stands for "self".

It also allows you to select a player based on a substring of their nickname, no more need to go to the GUI-based RA anymore 😉

**You can read more about player selectors on the wiki.**

## Remote Admin GUI Extensions

(WIP)

## Player Selection Stack

Everyone with Remote Admin access is able to use the player selection stack. You can push a list of players, pop, duplicate, clear. It's incredibly useful for commands that require multiple players.

Use `stackPush <players>` to push a list of players to the stack. This also works with player selectors.

Use `stackPop [index]` to discard the topmost player list from the stack.

When using a command that accepts player targets, you can use `@stack` to use the topmost player list from the stack.

You can also give it an index to use a specific player list from the stack, separated by one of `:>_- ` characters. For example, `@stack:1` will use the second player list from the stack.

Note: the most recently pushed player list is considered the top of the stack. To view the list, use `stackList` and list at index 0 is the topmost player list.
