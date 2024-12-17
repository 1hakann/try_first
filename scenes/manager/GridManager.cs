using System.Collections.Generic;
using Godot;

namespace Game.Manager;

public partial class GridManager : Node
{
	private HashSet<Vector2> occupiedCells = new();

	[Export]
	private TileMapLayer highlightTilemapLayer;

	[Export]
	private TileMapLayer baseTerrainTilemapLayer;

	public bool IsTilePositionValid(Vector2I tilePosition)
	{
		var tilePOsitionInt = new Vector2I((int) tilePosition.X, (int) tilePosition.Y);
		var customData = baseTerrainTilemapLayer.GetCellTileData(tilePOsitionInt);
		if(customData == null) return false;
		if(!(bool) customData.GetCustomData("buildable")) return false;

		return !occupiedCells.Contains(tilePosition);
	}

	public void MarkTileAsOccupied(Vector2I tilePosition)
	{
		occupiedCells.Add(tilePosition);
	}

	public void HighlightValidTilesInRadius(Vector2I rootCell, int radius)
	{
		ClearHighlightedTiles();

		for (var x = rootCell.X - radius; x <= rootCell.X + radius; x++)
		{
			for (var y = rootCell.Y - radius; y <= rootCell.Y + radius; y++)
			{
				var tilePosition = new Vector2I(x, y);
				if (!IsTilePositionValid(tilePosition)) continue;
				highlightTilemapLayer.SetCell(tilePosition, 0, Vector2I.Zero);
			}
		}
	}

	public void ClearHighlightedTiles()
	{
		highlightTilemapLayer.Clear();
	}

	public Vector2I GetMouseGridCellPosition()
	{
		var mousePosition = highlightTilemapLayer.GetGlobalMousePosition();
		var gridPosition = mousePosition / 64;
		gridPosition = gridPosition.Floor();
		return new Vector2I((int) gridPosition.X, (int) gridPosition.Y);
	}
}
