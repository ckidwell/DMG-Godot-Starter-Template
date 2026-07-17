using Godot;
using System.Collections.Generic;

namespace DMGStarterTemplate;

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
        // This manager is an autoload, so it outlives the Main scene. If Main is ever reloaded
        // and re-registers menus, Dictionary.Add would throw on the duplicate key — keep the
        // originally registered instance and free the redundant one instead.
        if (_menuNodes.ContainsKey(mt))
        {
            menuNode.QueueFree();
            return;
        }

        _menuNodes.Add(mt, menuNode);
    }

    public void SetCurrentMenu(MenuType mt)
    {
        // On the first call _currentMenuType is NONE (nothing shown yet), so there is nothing to remove.
        if (_currentMenuType != MenuType.NONE &&
            _menuNodes.TryGetValue(_currentMenuType, out var removeNode) &&
            removeNode.IsInsideTree())
        {
            RemoveChild(removeNode);
        }

        _currentMenuType = mt;

        if (_menuNodes.TryGetValue(mt, out var addNode))
        {
            AddChild(addNode);
        }
    }
    
    public void EmitSetMenu(MenuTypeVariant mtv)
    {
        EmitSignal(SignalName.SetMenu, mtv);
    }


    // Show the menu if it is hidden, hide it if it is shown. Used for overlay menus like pause.
    public void ToggleMenu(MenuType mt)
    {
        if (!_menuNodes.TryGetValue(mt, out var menuNode)) return;

        if (menuNode.IsInsideTree())
        {
            RemoveChild(menuNode);
        }
        else
        {
            AddChild(menuNode);
        }
    }

    public void PushMenu(MenuType mt)
    {
        if (_menuNodes.TryGetValue(mt, out var menuNode))
        {
            AddChild(menuNode);
        }
    }

    public void PopMenu(MenuType mt)
    {
        if (_menuNodes.TryGetValue(mt, out var menuNode))
        {
            RemoveChild(menuNode);
        }
    }
}
