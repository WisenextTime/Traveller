﻿using ColdMint.scripts.character;
using ColdMint.scripts.debug;
using ColdMint.scripts.map.events;
using Godot;

namespace ColdMint.scripts.map;

/// <summary>
/// <para>Ai character generation point</para>
/// <para>Ai角色生成点</para>
/// </summary>
public partial class AiCharacterSpawn : Marker2D
{
    private PackedScene? _packedScene;

    public override void _Ready()
    {
        base._Ready();
        var resPath = GetMeta("ResPath", Name).AsString();
        if (!string.IsNullOrEmpty(resPath))
        {
            _packedScene = GD.Load<PackedScene>(resPath);
        }

        EventManager.AiCharacterGenerateEvent += OnAiCharacterGenerateEvent;
    }

    /// <summary>
    /// <para>When an event is triggered</para>
    /// <para>当触发事件时</para>
    /// </summary>
    /// <param name="aiCharacterGenerateEvent"></param>
    public void OnAiCharacterGenerateEvent(AiCharacterGenerateEvent aiCharacterGenerateEvent)
    {
        var node = _packedScene?.Instantiate();
        if (node is not AiCharacter aiCharacter)
        {
            return;
        }

        if (GameSceneNodeHolder.AICharacterContainer == null)
        {
            return;
        }

        GameSceneNodeHolder.AICharacterContainer.AddChild(aiCharacter);
        aiCharacter.Position = GlobalPosition;
    }

    public override void _ExitTree()
    {
        EventManager.AiCharacterGenerateEvent -= OnAiCharacterGenerateEvent;
    }
}