using Godot;
using System;
using System.Collections.Generic;
using HartLib;
using static HartLib.Utils;

public class PlayerPathfindingActivity : PlayerActivityBase
{

    public override void Start()
    {
        if (LogChangesInGodot) { GD.Print("Changed to player finding path action"); }
    }

    #region Helpers 
    bool NoMouseable => Main.game_Manager?.Mouseover.Count == 0;
    bool DiffrentLastMousePos => !player_character.lastMouseGridPos.Equals(Main.Mouse_Grid_Pos);
    bool EmptyPathCache => player_character.path_positions_cache.Count == 0;
    bool PathNotEmpty(List<PathFindingCell<TileType>> path) => (path != null && path.Count > 0);
    bool PossiblePositionsContainPath(List<PathFindingCell<TileType>> path) => positions_cache.Contains(path[path.Count - 1].GridPos);
    #endregion

    public override void DoPlayerActionProcess()
    {
        //Pathfinding
        if (NoMouseable && player_character.MovementPoints > 0 && (DiffrentLastMousePos || EmptyPathCache))
        {
            Main.map?.PathfindingTiles.Clear();
            var path = Main.map?.PathFinding.FindPath(
                startPos: player_character.GridPos,
                endPos: Main.Mouse_Grid_Pos,
                blockingTiles: player_character.blocking_movement,
                collider_layer: 1,
                diagonals: true,
                big: false);

            if (PathNotEmpty(path) && PossiblePositionsContainPath(path))
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

    public override void UpdateCalculations()
    {
        positions_cache = Main.map?.PathFinding.FindPossibleSpaces(player_character.GridPos, player_character.MovementPoints, player_character.blocking_movement, 1);
    }

    public override void ShowCurrentDisplay()
    {
        Main.map?.Update_Higthlight_Display(positions_cache);
    }

    public PlayerPathfindingActivity(PlayerCharacter p) : base(p) { }

}

