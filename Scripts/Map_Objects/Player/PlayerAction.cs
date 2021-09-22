
public abstract class PlayerActionBase
{
    public PlayerCharacter Player { get; private set; }
    public abstract void DoPlayerAction();

    public PlayerActionBase(PlayerCharacter player)
    {
        Player = player;
    }
}

public class FindingPathAction : PlayerActionBase
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
    public FindingPathAction(PlayerCharacter p) : base(p) { }
}