using Godot;
using System;
using System.Collections.Generic;
using HartLib;
using static HartLib.Utils;

public abstract class PlayerActivityBase
{
    protected PlayerCharacter player_character;

    public abstract void DoPlayerAction();

    public virtual void Start(PlayerCharacter p)
    {
        player_character = p;
    }

    public virtual void Exit() { }

    public virtual void UpdateDisplay() { }

    public virtual void ClearDisplay()
    {
        Main.map?.UpdateHightligthDisplay(null);
    }

    public PlayerActivityBase() { }
}

public class PlayerIdleActivity : PlayerActivityBase
{
    public override void DoPlayerAction()
    {

    }

    public override void Start(PlayerCharacter p)
    {
        base.Start(p);
        GD.Print("Changed to idle action");
    }

    public PlayerIdleActivity() : base() { }
}

public class PlayerPrimaryActivity : PlayerActivityBase
{
    public override void DoPlayerAction()
    {

    }

    public override void UpdateDisplay()
    {
        Main.map?.UpdateHightligthDisplay(player_character.posible_positions_tile_cache, TileType.Transparent_Orange);
    }

    public override void Start(PlayerCharacter p)
    {
        base.Start(p);
        GD.Print("Changed to primary action");
    }

    public PlayerPrimaryActivity() : base() { }
}

public class PlayerUseActivity : PlayerActivityBase
{
    public override void DoPlayerAction()
    {

    }

    public override void UpdateDisplay()
    {
        Main.map?.UpdateHightligthDisplay(player_character.posible_positions_tile_cache, TileType.Red_Dot);
    }

    public override void Start(PlayerCharacter p)
    {
        base.Start(p);
        GD.Print("Changed to use action");
    }

    public PlayerUseActivity() : base() { }
}


public class PlayerFindingPathActivity : PlayerActivityBase
{
    public override void DoPlayerAction()
    {
        //Pathfinding
        if (Main.game_Manager?.Mouseover.Count == 0 && player_character.MovementPoints > 0 && (!player_character.lastMouseGridPos.Equals(Main.Mouse_Grid_Pos) || player_character.path_positions_cache.Count == 0))
        {
            Main.map?.PathfindingTiles.Clear();
            var path = Main.map?.PathFinding.FindPath(player_character.GridPos, Main.Mouse_Grid_Pos, player_character.blocking_movement, collider_layer: 1, diagonals: true, big: false);

            if (path != null && path.Count > 0 && player_character.posible_positions_tile_cache.Contains(path[path.Count - 1].GridPos))
            {
                player_character.path_positions_cache.Clear();
                var distance = 0;

                //*drawig path
                foreach (var pathTile in path)
                {
                    if (distance < player_character.MovementPoints)
                    {
                        Main.map?.PathfindingTiles.SetCellv(pathTile.GridPos.Vec2(), (int)TileType.Green_Dot);
                        player_character.path_positions_cache.Add(pathTile.GridPos);
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
        Main.map?.UpdateHightligthDisplay(player_character.posible_positions_tile_cache);
    }

    public override void Start(PlayerCharacter p)
    {
        base.Start(p);
        GD.Print("Changed to player finding path action");
    }

    public PlayerFindingPathActivity() : base() { }

}

