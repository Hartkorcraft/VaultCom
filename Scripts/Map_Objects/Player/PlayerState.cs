using Godot;
using System;
using System.Collections.Generic;
using HartLib;
using static HartLib.Utils;

public abstract class PlayerStateBase
{
    public PlayerCharacter Player { get; private set; }

    public abstract void DoPlayerAction();

    public virtual void Start()
    {

    }

    public virtual void UpdateDisplay()
    {

    }

    public virtual void ClearDisplay()
    {
        Main.map.UpdateHightligthDisplay(null);
    }

    public PlayerStateBase(PlayerCharacter player)
    {
        Player = player;
        Start();
    }
}

// public class PlayerIdle : PlayerStateBase
// {
//     public override void DoPlayerAction()
//     {

//     }

//     public PlayerIdle(PlayerCharacter p) : base(p) { }
// }

public class PlayerPrimaryActionState : PlayerStateBase
{
    public override void DoPlayerAction()
    {

    }

    public override void UpdateDisplay()
    {
        Main.map?.UpdateHightligthDisplay(Player.posible_positions_tile_cache, TileType.Transparent_Orange);
    }

    public override void Start()
    {
        GD.Print("Changed to primary action");
    }

    public PlayerPrimaryActionState(PlayerCharacter p) : base(p) { }

}

public class PlayerFindingPathState : PlayerStateBase
{
    public override void DoPlayerAction()
    {
        //Pathfinding
        if (Main.game_Manager?.Mouseover.Count == 0 && Player.MovementPoints > 0 && (!Player.lastMouseGridPos.Equals(Main.Mouse_Grid_Pos) || Player.path_positions_cache.Count == 0))
        {
            Main.map?.PathfindingTiles.Clear();
            var path = Main.map?.PathFinding.FindPath(Player.GridPos, Main.Mouse_Grid_Pos, Player.blocking_movement, collider_layer: 1, diagonals: true, big: false);

            if (path != null && path.Count > 0 && Player.posible_positions_tile_cache.Contains(path[path.Count - 1].GridPos))
            {
                Player.path_positions_cache.Clear();
                var distance = 0;

                //*drawig path
                foreach (var pathTile in path)
                {
                    if (distance < Player.MovementPoints)
                    {
                        Main.map?.PathfindingTiles.SetCellv(pathTile.GridPos.Vec2(), (int)TileType.Green_Dot);
                        Player.path_positions_cache.Add(pathTile.GridPos);
                    }
                    else
                    {
                        Main.map?.PathfindingTiles.SetCellv(pathTile.GridPos.Vec2(), (int)TileType.Red_Dot);
                    }
                    distance++;
                }
            }
        }
    }

    public override void UpdateDisplay()
    {
        Main.map?.UpdateHightligthDisplay(Player.posible_positions_tile_cache);
    }

    public override void Start()
    {
        GD.Print("Changed to player finding path action");
    }

    public PlayerFindingPathState(PlayerCharacter p) : base(p) { }

}

