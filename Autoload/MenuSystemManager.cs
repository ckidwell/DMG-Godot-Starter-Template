using Godot;
using System.Collections.Generic;

public partial class MenuSystemManager : Control
{
    
    private GameEvents _gameEvents;
    private MenuType _currentMenuType;
    
    private Dictionary<MenuType, Node> _menuNodes = new Dictionary<MenuType, Node>();

    [Signal]
    public delegate void SetMenuEventHandler(MenuTypeVariant mtv);
    
    public override void _Ready()
    {
        _gameEvents = GetNode<GameEvents>("/root/GameEvents");
    }

    public void InitializeMenu(MenuType mt, Node menuNode)
    {
        _menuNodes.Add(mt, menuNode);
    }
    
    public void SetCurrentMenu(MenuType mt)
    {

        var removeNode = _menuNodes[_currentMenuType];
        
        if (removeNode.IsInsideTree())
        {
            RemoveChild(removeNode); 
        }
        
        _currentMenuType = mt;

        var addNode = _menuNodes[mt];
        
        AddChild(addNode);
        
    }
    
    public void EmitSetMenu(MenuTypeVariant mtv)
    {
        EmitSignal(SignalName.SetMenu, mtv);
    }


    public void PushMenu(MenuType mt)
    {
        AddChild(_menuNodes[mt]);
    }

    public void PopMenu(MenuType mt)
    {
        RemoveChild(_menuNodes[mt]);
    }
}
