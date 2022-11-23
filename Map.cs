using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions; 
using SplashKitSDK;

namespace PathFinder
{
    public class Map
    {        
        private int _xBound;
        private int _yBound;
        private Cell _root;
        private List<Cell> _goals;
        private Cell[,] _cells;
        private int _nCells;

        public Map(StreamReader filePath)
        {

            // Create cells   
            string[] boundary = filePath.ReadLine().Split("x");       
            _xBound = Int32.Parse(boundary[1]);
            _yBound = Int32.Parse(boundary[0]);
            _nCells = _xBound * _yBound;    
            _cells = new Cell[_xBound, _yBound];     
            int y = 0;
            while (y < _yBound)
            {
                int x = 0;
                while (x < _xBound)
                {
                    _cells[x, y] = new Cell(x, y);
                    x++;
                }
                y++;
            }
            Set_neighbours();

            // set up inital location state
            string[] initialLocation = Regex.Split(filePath.ReadLine(), @"\D");
            int init_x = Int32.Parse(initialLocation[1]);
            int init_y = Int32.Parse(initialLocation[2]);
            _cells[init_x, init_y].Type = Type.ROOT;
            _root = _cells[init_x, init_y];

            // Set up goal states                     
            _goals = new List<Cell>();
            Set_goals(filePath.ReadLine());

            // Set up wall states
            string line;
            List<string> walls = new List<string>();
            while ((line = filePath.ReadLine()) != null)
            {
                walls.Add(line);
            }                                             
            for (int i = 0; i < walls.Count(); i++)
            {
                Set_Walls(walls[i]);
            }
        }     

        // Properties
        public Cell Root 
        {
            get 
            {
                return _root;
            }
        }   
        public List<Cell> Goals 
        {
            get 
            {
                return _goals;
            }
        }
        public int xbound
        {
            get 
            {
                return _xBound;
            }
        }
        public int ybound
        {
            get
            {
                return _yBound;
            }
        }
        public int nCells
        {
            get 
            {
                return _nCells;
            }
        }
        
        // Methods
        public List<Cell> List_cells()
        {
            List<Cell> cells = new List<Cell>();
            for (int i = 0; i < _xBound; i++)
            {
                for (int j = 0; j < _yBound; j++)
                {
                    cells.Add(_cells[i,j]);
                }
            }
            return cells;
        }
        private void Set_Walls(string wall)
        {
            // fileLine wall format (position X, position Y, wide, High)                 
            string[] wall_info = Regex.Split(wall, @"\D+");
            int x_start = Int32.Parse(wall_info[1]);
            int y_start = Int32.Parse(wall_info[2]);
            int wide = Int32.Parse(wall_info[3]);
            int high = Int32.Parse(wall_info[4]);

            int x_end = x_start + wide;
            int y_end = y_start + high;
            for (int i = x_start; i < x_end; i++) // horizontal
            {
                for (int j = y_start; j < y_end; j++) // vertcal
                {
                    _cells[i, j].Type = Type.WALL;
                }
            }            
        }
        private void Set_goals(string str_goals)
        {
            if (!str_goals.Contains("|"))
            {   
                // single goal
                string[] goals = Regex.Split(str_goals, @"\D+");                
                string x = goals[1].ToString().Trim();
                string y = goals[2].ToString().Trim();
                _cells[Int32.Parse(x), Int32.Parse(y)].Type = Type.GOAL;
                _goals.Add(_cells[Int32.Parse(x), Int32.Parse(y)]);
            }
            else
            {
                // multiple goals separated with '|'
                string[] goals = str_goals.Split("|");
                foreach (string g in goals)
                {
                    string[] goal = Regex.Split(g, @"\D+");
                    int x = Int32.Parse(goal[1]);
                    int y = Int32.Parse(goal[2]);
                    _cells[x, y].Type = Type.GOAL;
                    _goals.Add(_cells[x,y]);
                }
            }
        }
        private void Set_neighbours()
        {
            int y_index = 0;
            while (y_index < _yBound)
            {
                int x_index = 0;
                while (x_index < _xBound)
                {
                    // by order of expansion UP DOWN LEFT RIGHT
                    Cell cells = _cells[x_index, y_index];
                    if (y_index != 0) // up
                    {
                        cells.AddNeighbour(_cells[x_index, y_index - 1]);
                    }
                    if (x_index != 0) // left
                    {
                        cells.AddNeighbour(_cells[x_index - 1, y_index]);
                    }                   
                    if (y_index != _yBound - 1) // down
                    {
                        cells.AddNeighbour(_cells[x_index, y_index + 1]);
                    }
                    if (x_index != _xBound - 1) // right
                    {
                        cells.AddNeighbour(_cells[x_index + 1, y_index]);
                    }
                                        
                    x_index += 1;
                }
                y_index += 1;
            }

        }
        public Cell Get_neighbour(Cell c, string direction)
        {
            int x = 0;
            int y = 0;        
            
                        
            if (direction == "LEFT")
            {
                if (c.X != 0) // not most left side
                {
                    x = c.X - 1;
                    y = c.Y;
                    return _cells[x,y];
                }
                
            }                   
            if (direction == "RIGHT")
            {
                if (!(c.X == _xBound - 1)) // not most right cell
                {
                    x = c.X + 1;
                    y = c.Y;
                    return _cells[x,y];
                }

            }
            if (direction == "UP")
            {
                if (c.Y != 0) // not most top cell
                {
                    x = c.X;
                    y = c.Y - 1;
                    return _cells[x,y];
                }
                
            }
            if (direction == "DOWN")
            {
                if (!(c.Y == _yBound - 1)) // not most bottom cell
                {
                    x = c.X;
                    y = c.Y + 1;
                    return _cells[x,y];
                }
            }   
            return null;                     
        }            
        public double Distance(Cell c1, Cell c2)
        {
            // straight line distance between two points using pythagoras theorem
            int x1 = c1.X;
            int x2 = c2.X;
            int y1 = c1.Y;
            int y2 = c2.Y;
            return Math.Sqrt(Math.Pow(x1-x2, 2) + Math.Pow(y1 - y2, 2));
        }
        public void Reset()
        {
            for (int i = 0; i < _xBound; i++)
            {
                for (int j = 0; j < _yBound; j++)
                {
                    if (_cells[i,j].Type != Type.WALL)
                    {
                        if (_cells[i,j].Type != Type.ROOT)
                        {
                            if (_cells[i,j].Type != Type.GOAL)
                            {
                                _cells[i,j].Type = Type.ROUTE;
                                _cells[i,j].Parent = null;
                            }       
                        }   
                    }
                }
            }            
        }
        public void Draw()
        {
            for (int i = 0; i < xbound; i++)
            {
                for (int j = 0; j < ybound; j++)
                {                    
                    SplashKit.FillRectangle(Color.Black, i * 50, j * 50, 50, 50); // border          
                    if (_cells[i,j].Type == Type.WALL)
                    {
                        SplashKit.FillRectangle(Color.Black, i * 50 + 1, j * 50 + 1, 49, 49);
                    }
                    if (_cells[i,j].Type == Type.ROUTE)
                    {
                        SplashKit.FillRectangle(Color.White, i * 50 + 1, j * 50 + 1, 49, 49);
                    }
                    if (_cells[i,j].Type == Type.ROOT)
                    {
                        SplashKit.FillRectangle(Color.BrightGreen, i * 50 + 1, j * 50 + 1, 49, 49);
                    }
                    if (_cells[i,j].Type == Type.GOAL)
                    {
                        SplashKit.FillRectangle(Color.Red, i * 50 + 1, j * 50 + 1, 49, 49);
                    }
                    if (_cells[i,j].Type == Type.EXPANDED)
                    {
                        SplashKit.FillRectangle(Color.DarkGray, i * 50 + 1, j * 50 + 1, 49, 49);
                    }
                    if (_cells[i,j].Type == Type.VISITED)
                    {
                        SplashKit.FillRectangle(Color.Yellow, i * 50 + 1, j * 50 + 1, 49, 49);
                    }
                    if (_cells[i,j].Type == Type.PATH)
                    {
                        SplashKit.FillRectangle(Color.DarkBlue, i * 50 + 1, j * 50 + 1, 49, 49);
                    }     
                }
            }          
        }

        public void draw_buttons()
        {
            SplashKit.FillRectangle(Color.LightBlue, 0, 500, 100, 100);
            SplashKit.FillRectangle(Color.LightBlue, 120, 500, 100, 100);
            SplashKit.FillRectangle(Color.LightBlue, 240, 500, 100, 100);
            SplashKit.FillRectangle(Color.LightBlue, 360, 500, 100, 100);
            SplashKit.FillRectangle(Color.LightBlue, 480, 500, 100, 100);
            SplashKit.FillRectangle(Color.LightBlue, 600, 500, 100, 100);

            SplashKit.DrawText("BFS", Color.Black, "italics", 100, 25, 550);
            SplashKit.DrawText("DFS", Color.Black, "italics", 100, 145, 550);
            SplashKit.DrawText("GBFS", Color.Black, "italics", 100, 265, 550);
            SplashKit.DrawText("A*", Color.Black, "italics", 100, 385, 550);
            SplashKit.DrawText("Strat 1", Color.Black, "italics", 100, 505, 550);
            SplashKit.DrawText("Strat 2", Color.Black, "italics", 100, 625, 550);
        }        
    }
}
