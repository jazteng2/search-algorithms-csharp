using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using SplashKitSDK;
namespace PathFinder
{
    public class Algorithms
    {
        private List<Cell> closedSet; // explored cells
        private List<Cell> goals; // list of goals
        private Cell root; // intitial state/cell
        private Map Map;        
        public Algorithms(Map m)
        {
            Map = m;
            closedSet = new List<Cell>();
            goals = Map.Goals;            
            root = Map.Root;
        }

        // search algorithms
        public string BFS()
        {                                    
            Queue<Cell> openSet = new Queue<Cell>();
            openSet.Enqueue(root);
            while (openSet.Count() > 0)
            {          
                SplashKit.ClearScreen();                    
                Map.Draw();  

                // Dequeue node and add to closed set
                Cell current = openSet.Dequeue();    
                closedSet.Add(current);            
                                             
                // visual purposes
                if (current.Type != Type.GOAL && current.Type != Type.ROOT)
                {
                    current.Type = Type.EXPANDED;                    
                }

                // check goal
                foreach (Cell goal in goals)
                {
                    if (current == goal)
                    {
                        Console.WriteLine("Expanded: " + closedSet.Count());
                        closedSet.Clear();
                        return Create_Path(current);                        
                    }                    
                }                   

                // expand node
                foreach (Cell neighbour in current.Neighbours)
                {       
                    if (neighbour != null)
                    {
                        if (!openSet.Contains(neighbour) && !closedSet.Contains(neighbour) && neighbour.Type != Type.WALL)
                        {                    
                            if (neighbour.Type != Type.GOAL && neighbour.Type != Type.ROOT)
                            {
                                neighbour.Type = Type.VISITED;
                            }
                            neighbour.Parent = current;
                            openSet.Enqueue(neighbour);     
                        }
                    }
                }
                SplashKit.RefreshScreen();       
                SplashKit.Delay(200);
            }                        
            return null;
        }   
        public string DFS()
        {
            Stack<Cell> openSet = new Stack<Cell>();
            openSet.Push(root);
            while (openSet.Count() > 0)
            {
                SplashKit.ClearScreen();
                Map.Draw();
                Cell current = openSet.Pop();                
                closedSet.Add(current);
                // visual purposes
                if (current.Type != Type.ROOT && current.Type != Type.GOAL)
                {
                    current.Type = Type.EXPANDED;
                }

                // check goal
                foreach (Cell goal in goals)
                {
                    if (current == goal)
                    {
                        Console.WriteLine("Expanded: " + closedSet.Count());
                        closedSet.Clear();
                        return Create_Path(current);                        
                    }                    
                }
                // expand node                               
                for (int i = current.Neighbours.Count() - 1; i >= 0; i--)
                {
                    Cell neighbour = current.Neighbours[i];
                    if (!closedSet.Contains(neighbour) && neighbour.Type != Type.WALL)
                    {
                        if (neighbour.Type != Type.ROOT && neighbour.Type != Type.GOAL)
                        {
                            neighbour.Type = Type.VISITED;
                        }
                        openSet.Push(neighbour);
                        neighbour.Parent = current;
                    }
                }                            
                SplashKit.RefreshScreen();
                SplashKit.Delay(200);
            }
            return null;
        }         
        public string ASTAR()
        {
            List<Cell> openSet = new List<Cell>();
            Cell goal = Get_goal("MIN");         
            openSet.Add(root);            
            while (openSet.Count() > 0)
            {
                SplashKit.ClearScreen();
                Cell current = min_cell(openSet, goal);
                openSet.Remove(current);
                closedSet.Add(current);
                
                // visual purposes
                if (current.Type != Type.GOAL && current.Type != Type.ROOT)
                {
                    current.Type = Type.EXPANDED;                    
                }

                // check goal
                if (current == goal)
                {
                    Console.WriteLine("Expanded: " + closedSet.Count());
                    closedSet.Clear();
                    return Create_Path(current);
                }            

                // expand node   
                foreach (Cell neighbour in current.Neighbours)
                {                    
                    if (!openSet.Contains(neighbour) && !closedSet.Contains(neighbour) && neighbour.Type != Type.WALL)
                    {
                        if (neighbour.Type != Type.GOAL && neighbour.Type != Type.ROOT)
                        {
                            neighbour.Type = Type.VISITED;
                        }
                        openSet.Add(neighbour);
                        neighbour.Parent = current;
                    }
                }
                 
                Map.Draw();    
                SplashKit.RefreshScreen();
                SplashKit.Delay(200);
            }
            return null;        
        }
        public string GBFS()
        {            
            Cell goal = Get_goal("MIN");
            List<Cell> openSet = new List<Cell>();
            openSet.Add(root);
            while (openSet.Count() > 0)
            {
                SplashKit.ClearScreen();
                Cell current = min_cell(openSet,goal); 
                openSet.Remove(current);               
                closedSet.Add(current);
                
                // visual purposes
                if (current.Type != Type.GOAL && current.Type != Type.ROOT)
                {
                    current.Type = Type.EXPANDED;                    
                }

                // check goal
                if (current == goal)
                {
                    Console.WriteLine("Expanded: " + closedSet.Count());
                    closedSet.Clear();
                    return Create_Path(current);                    
                }

                // expand node
                foreach (Cell neighbour in current.Neighbours)
                {                          
                    if (!openSet.Contains(neighbour) && !closedSet.Contains(neighbour) && neighbour.Type != Type.WALL)
                    {                        
                        if (neighbour.Type != Type.GOAL && neighbour.Type != Type.ROOT)
                        {
                            neighbour.Type = Type.VISITED;
                        }
                        openSet.Add(neighbour);
                        neighbour.Parent = current;
                    }
                }
                Map.Draw();
                SplashKit.RefreshScreen();
                SplashKit.Delay(200);
            }
            return null;
        }
        public string DLS(int limit)
        {            
            Cell goal = Get_goal("MIN"); // asume closest goal is only goal
            Stack<Cell> openSet = new Stack<Cell>();
            openSet.Push(root);
            int depth = 1;
            while (openSet.Count() > 0)
            {                
                if (depth <= limit)
                {
                    SplashKit.ClearScreen();
                    Cell current = openSet.Pop();
                    closedSet.Add(current);

                    // visual purposes
                    if (current.Type != Type.ROOT && current.Type != Type.GOAL)
                    {
                        current.Type = Type.EXPANDED;
                    }     

                    // check goal                                   
                    if (current == goal)
                    {
                        Console.WriteLine("Expanded: " + closedSet.Count());
                        closedSet.Clear();                        
                        return Create_Path(current);
                    }

                    // expand node
                    for (int i = current.Neighbours.Count() - 1; i >= 0; i--)
                    {
                        Cell neighbour = current.Neighbours[i];
                        if (!closedSet.Contains(neighbour) && neighbour.Type != Type.WALL)
                        {
                            if (neighbour.Type != Type.ROOT && neighbour.Type != Type.GOAL)
                            {
                                neighbour.Type = Type.VISITED;
                            }
                            neighbour.Parent = current;
                            openSet.Push(neighbour);
                        }
                    }
                    depth++;
                    Map.Draw();
                    SplashKit.RefreshScreen();
                    SplashKit.Delay(200);
                } 
                else
                {
                    closedSet.Clear();
                    return "Goal not found within depth limit of ";
                }
            }
            return null;
        }         
        public string Dijkstra()
        {
            // order of expansion not fixed
            Dictionary<Cell, int> dist = new Dictionary<Cell, int>();
            List<Cell> cells = Map.List_cells();
            List<Cell> openSet = new List<Cell>();
            Cell goal = Get_goal("MIN");
            foreach (Cell c in cells)
            {
                dist.Add(c, int.MaxValue);
                openSet.Add(c);
            }
            dist[root] = 0;
            
            while (openSet.Count() > 0)
            {
                SplashKit.ClearScreen();

                // find node with lowest node (start root is 0) and assign as current node
                Cell min = openSet.First();                
                foreach (Cell c in openSet)
                {
                    if (dist[c] < dist[min])
                    {
                        min = c;
                    }    
                }
                Cell current = min;

                //  Remove from unexplored Set
                openSet.Remove(current);    
                closedSet.Add(current);            

                // visual purposes
                if (current.Type != Type.ROOT && current.Type != Type.GOAL)
                {
                    current.Type = Type.EXPANDED;
                }

                // check goal
                if (current == goal)
                {
                    Console.WriteLine("Expanded: " + closedSet.Count());
                    closedSet.Clear();
                    return Create_Path(current);                    
                }

                // expand node
                foreach (Cell neighbour in current.Neighbours)
                {
                    if (!closedSet.Contains(neighbour) && neighbour.Type != Type.WALL)
                    {
                        int dist_neighbour = dist[current] + Dist_cells(current,neighbour); // dist of current + dist of neighbour = 2 for all cells in grid
                        if (dist_neighbour < dist[neighbour])
                        {
                            // visual purposes
                            if (neighbour.Type != Type.ROOT && neighbour.Type != Type.GOAL)
                            {
                                neighbour.Type = Type.VISITED;
                            }
                            dist[neighbour] = dist_neighbour;
                            neighbour.Parent = current;
                        }
                    }
                }

                Map.Draw();
                SplashKit.RefreshScreen();
                SplashKit.Delay(100);
            }
            return null;            
        }  
        
        // methods
        private Cell Get_goal(string type)
        {            
            // get goal from sequence of 2 goals in the state space            
            Cell result = goals.First();
            foreach (Cell g in goals)
            {
                if (type == "MIN")
                {
                    if (Map.Distance(root, g) < Map.Distance(root, result))
                    {
                        result = g;
                    }
                }
                if (type == "MAX")
                {
                    if (Map.Distance(root, g) > Map.Distance(root, result))
                    {
                        result = g;
                    }
                }                
            }
            return result;
        }
        private Cell min_cell(List<Cell> cells, Cell goal)
        {
            // get cell with minimum distance from cell to goal
            Cell min = cells.First();
            foreach (Cell c in cells)
            {
                int f1 = Dist_cells(c, goal);
                int f2 = Dist_cells(min, goal);
                if (Math.Min(f1, f2) == f1 && !Math.Equals(f1,f2))
                {
                    min = c;
                }                
            }            
            return min;
        }               
        private int Dist_cells(Cell c1, Cell c2)
        {
            // using manhattan distance from node to goal
            int x = Math.Abs(c2.X - c1.X);
            int y = Math.Abs(c2.Y - c1.Y);
            return x + y;
        }        
        private string Create_Path(Cell current)
        {
            // using cell.Parent to backtrack and return string path
            string str_path = "";
            Cell next = current.Parent;
            List<string> action_list = new List<string>();
            action_list.Add(movement(current, next) + " ");
            while (next != root)
            {
                next.Type = Type.PATH;
                action_list.Add(movement(next, next.Parent) + " ");                
                next = next.Parent;                 
            }   
            int index = action_list.Count() - 1;
            while (index >= 0)
            {
                str_path += action_list[index];
                index--;
            }
            return str_path;
        }        
        private string movement(Cell current, Cell previous)
        {
            // return action from previous to current
            if (current.X - 1 == previous.X && current.Y == previous.Y) // right
            {
                return "right";
            }
            if (current.X + 1 == previous.X && current.Y == previous.Y) // right
            {
                return "left";
            }
            if (current.X == previous.X && current.Y + 1 == previous.Y) // right
            {
                return "up";
            }
            if (current.X == previous.X && current.Y - 1 == previous.Y) // right
            {
                return "down";
            }
            return null;
        }
       
    }
}
