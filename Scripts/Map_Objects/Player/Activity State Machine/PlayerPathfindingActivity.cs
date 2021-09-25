using Godot;
using System;
using System.Collections.Generic;
using HartLib;
using static HartLib.Utils;

public class PlayerPathfindingActivity : PlayerActivityBase
{
    public override void DoPlayerActionProcess()
    {
        //Pathfinding
        if (Main.game_Manager?.Mouseover.Count == 0 && player_character.MovementPoints > 0 && (!player_character.lastMouseGridPos.Equals(Main.Mouse_Grid_Pos) || player_character.path_positions_cache.Count == 0))
        {
            Main.map?.PathfindingTiles.Clear();
            var path = Main.map?.PathFinding.FindPath(player_character.GridPos, Main.Mouse_Grid_Pos, player_character.blocking_movement, collider_layer: 1, diagonals: true, big: false);

            if (path != null && path.Count > 0 && positions_cache.Contains(path[path.Count - 1].GridPos))
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

    public override void ShowCurrentDisplay()
    {
        Main.map?.Update_Higthlight_Display(positions_cache);
    }

    public override void UpdateCalculations()
    {
        base.UpdateCalculations();
        positions_cache = Main.map?.PathFinding.FindPossibleSpaces(player_character.GridPos, player_character.MovementPoints, player_character.blocking_movement, 1);
        //player_character.CalculatePossiblePositions();
    }
    public override void Start()
    {
        GD.Print("Changed to player finding path action");
    }

    public PlayerPathfindingActivity(PlayerCharacter p) : base(p) { }

}

