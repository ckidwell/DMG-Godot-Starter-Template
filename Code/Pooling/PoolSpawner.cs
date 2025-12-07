using Godot;
using System;
using System.Collections.Generic;

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

        return GetStackBasedItem();

    }
    private Node2D GetStackBasedItem()
    {
        if (_pooledStack.Count > 5)
        {
            var repooled = _pooledStack.Pop();
            _activeItems.Add(repooled);
            
            repooled.SetProcess(true);
            var ipoolitem = repooled as IPooledItem;
            ipoolitem?.Activate();

            UpdateCounts();
            return repooled;
        }

        var spawned =  pooledScene.Instantiate() as Node2D;
        var pooledItem = spawned as IPooledItem;
        
        var myid = this.GetInstanceId();
        pooledItem?.SetPoolSpawner(myid);

        if (useOptionalAttachNode)
        {
            attachNode.AddChild(spawned);
        }
        else
        {
            entitiesLayer.AddChild(spawned);    
        }
        
        _activeItems.Add(spawned);
        UpdateCounts();
        return spawned;
    }
  

    private void UpdateCounts()
    {
        numSpawnedItems = _activeItems.Count;
        numPooledItems = _pooledStack.Count;
    }

    public void PoolItem(Node2D item)
    {
        item.SetProcess(false);
        _activeItems.Remove(item);
        _pooledStack.Push(item);

    }
}
