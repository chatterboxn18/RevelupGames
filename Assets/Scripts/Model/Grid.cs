namespace SGChess
{
	public class Grid<TGridObject> where TGridObject : new()
	{
		public TGridObject[,] GridObjects = new TGridObject[,] { };
		private int _width;
		private int _height;
		private int _cellSize;

		public Grid(int width, int height, int cellSize)
		{
			_width = width;
			_height = height;
			_cellSize = cellSize;
			GridObjects = new TGridObject[width, height];

			for (var i = 0; i < GridObjects.GetLength(0); i++)
			{
				for (var j = 0; j < GridObjects.GetLength(1); j++)
				{
					SetValue(i,j,new TGridObject());
				}
			}
		}

		public void SetValue(int x, int y, TGridObject value)
		{
			if (x < _width && y < _height)
			{
				GridObjects[x, y] = value;
			}
		}
	}
}