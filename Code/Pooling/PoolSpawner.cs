using Godot;
using System;
using System.Collections.Generic;

namespace DMGStarterTemplate;

public partial class PoolSpawner : Node
{
     [Export] public PackedScene pooledScene;
    public Node2D attachNode;
    private bool useOptionalAttachNode;
    
    private List<Node2D> _activeItems = new List<Node2D>();
    private Stack<Node2D> _pooledStack = new Stack<Node2D>();

    [Export] public int numSpawnedItems { get; private set; }
    [Export] public int numPooledItems { get; private set; }

    private Node entitiesLayer;
    private GameEvents _gameEvents;
    public override void _Ready()
    {
        _gameEvents = GetNode<GameEvents>("/root/GameEvents");
        _gameEvents.RePoolMe += OnRepoolMe;
        entitiesLayer =  GetTree().GetFirstNodeInGroup("entities_layer");
    }

    public override void _ExitTree()
    {
        if (_gameEvents == null) return;
        _gameEvents.RePoolMe -= OnRepoolMe;
    }


    public void SetAttachNode(Node2D node2D)
    {
        useOptionalAttachNode = true;
        attachNode = node2D;
    }
    private void OnRepoolMe(ulong myspawner, Node2D item)
    {
        if (myspawner != this.GetInstanceId())
        {
            return;
        }

        PoolItem(item);
    }
    
    public Node2D GetItem()
    {
        // Reuse a pooled item whenever one is available; only instantiate when the pool is empty.
        if (_pooledStack.Count > 0)
        {
            var reused = _pooledStack.Pop();

            GetSpawnParent().AddChild(reused);
            reused.SetProcess(true);
            reused.SetPhysicsProcess(true);
            reused.Visible = true;
            (reused as IPooledItem)?.Activate();

            _activeItems.Add(reused);
            UpdateCounts();
            return reused;
        }

        var spawned = pooledScene.Instantiate<Node2D>();
        (spawned as IPooledItem)?.SetPoolSpawner(GetInstanceId());

        GetSpawnParent().AddChild(spawned);
        (spawned as IPooledItem)?.Activate();

        _activeItems.Add(spawned);
        UpdateCounts();
        return spawned;
    }

    private Node GetSpawnParent()
    {
        return useOptionalAttachNode ? attachNode : entitiesLayer;
    }

    private void UpdateCounts()
    {
        numSpawnedItems = _activeItems.Count;
        numPooledItems = _pooledStack.Count;
    }

    public void PoolItem(Node2D item)
    {
        // Guard against pooling the same item twice, which would later hand one node to two callers.
        if (_pooledStack.Contains(item)) return;

        _activeItems.Remove(item);

        // Let the item reset its own state (disable collision, stop effects, etc.).
        (item as IPooledItem)?.DeSpawn();

        item.SetProcess(false);
        item.SetPhysicsProcess(false);

        // Remove from the tree so a pooled item no longer renders, processes, or collides.
        item.GetParent()?.RemoveChild(item);

        _pooledStack.Push(item);
        UpdateCounts();
    }
}
