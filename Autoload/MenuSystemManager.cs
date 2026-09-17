using Godot;
using System.Collections.Generic;

namespace DMGStarterTemplate;

// Owns which menu is on screen. Two kinds of registration:
//
//   InitializeMenu(type, node)        Cached. The node is created once and kept alive for the
//                                     whole session; showing/hiding adds/removes it from the tree.
//                                     Right for menus, which are cheap to keep and should remember
//                                     their state (scroll position, last tab).
//
//   InitializeFreshMenu(type, scene)  Fresh. A new instance is created every time the type is
//                                     shown and freed when it is hidden. Right for gameplay: a
//                                     "Play" after "Quit to main" must start a new game, not
//                                     un-hide the old one with all its timers and enemies intact.
//
// Overlay operations (Toggle/Push/Pop) only apply to cached menus.
public partial class MenuSystemManager : Control
{
    private GameEvents _gameEvents;
    private MenuType _currentMenuType;

    private readonly Dictionary<MenuType, Node> _menuNodes = new();
    private readonly Dictionary<MenuType, PackedScene> _freshScenes = new();
    
    private Node _currentFreshInstance;

    [Signal]
    public delegate void SetMenuEventHandler(MenuTypeVariant mtv);

    // Which full-screen menu is currently shown (overlays such as PAUSE_QUIT do not change this).
    public MenuType CurrentMenu => _currentMenuType;

    public override void _Ready()
    {
        _gameEvents = GetNode<GameEvents>("/root/GameEvents");
    }

    public void InitializeMenu(MenuType mt, Node menuNode)
    {

        if (_menuNodes.ContainsKey(mt))
        {
            menuNode.QueueFree();
            return;
        }

        _menuNodes.Add(mt, menuNode);
    }

    public void InitializeFreshMenu(MenuType mt, PackedScene scene)
    {
        _freshScenes[mt] = scene;
    }

    public void SetCurrentMenu(MenuType mt)
    {
        HideCurrentMenu();

        _currentMenuType = mt;

        if (_freshScenes.TryGetValue(mt, out var scene))
        {
            _currentFreshInstance = scene.Instantiate();
            AddChild(_currentFreshInstance);
            return;
        }

        if (_menuNodes.TryGetValue(mt, out var cachedNode))
        {
            AddChild(cachedNode);
            return;
        }

        GD.PushError($"MenuSystemManager: no menu registered for {mt}. Call InitializeMenu or InitializeFreshMenu first.");
    }

    private void HideCurrentMenu()
    {
        if (_currentMenuType == MenuType.NONE) return;

        if (_currentFreshInstance != null)
        {

            RemoveChild(_currentFreshInstance);
            _currentFreshInstance.QueueFree();
            _currentFreshInstance = null;
            return;
        }

        if (_menuNodes.TryGetValue(_currentMenuType, out var cachedNode) && cachedNode.IsInsideTree())
        {
            RemoveChild(cachedNode);
        }
    }

    public void EmitSetMenu(MenuTypeVariant mtv)
    {
        EmitSignal(SignalName.SetMenu, mtv);
    }

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
        if (_menuNodes.TryGetValue(mt, out var menuNode) && !menuNode.IsInsideTree())
        {
            AddChild(menuNode);
        }
    }

    public void PopMenu(MenuType mt)
    {
        if (_menuNodes.TryGetValue(mt, out var menuNode) && menuNode.IsInsideTree())
        {
            RemoveChild(menuNode);
        }
    }
}
