using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;
namespace PathFinder
{    
    public class menu
    {
        private bool searched;
        private string path;

        public menu()
        {
            searched = false;
            path = "";
        }

        public void Action(Map map, Algorithms a, string method)
        {
            // user input from terminal
            if (!searched) // continue with search only if havent searched
            {
                switch (method)
                {
                    case "BFS":
                        path = "> " + a.BFS() + "\n";
                        searched = true;
                        break;
                    case "DFS":
                        path = "> " + a.DFS() + "\n";
                        searched = true;
                        break;
                    case "GBFS":
                        path = "> " + a.GBFS() + "\n";
                        searched = true;
                        break;
                    case "A*":
                        path += "> " + a.ASTAR() + "\n";
                        searched = true;
                        break;
                    case "DLS":
                        Console.WriteLine("Enter depth limit for Depth limit search: ");
                        string depth = Console.ReadLine();
                        int i = 0;
                        if (int.TryParse(depth, out i))
                        {
                            path += "> " + a.DLS(i) + "\n";
                            searched = true;
                        }
                        else
                        {
                            Console.WriteLine("Invalid limit for depth limited search");
                        }
                        break;
                    case "DJKS":
                        path += "> " + a.Dijkstra() + "\n";
                        searched = true;
                        break;
                    default:
                        Console.WriteLine("Invalid Command");
                        break;
                }
                map.Draw();
                SplashKit.RefreshScreen();

                // display result
                if (path == null)
                {
                    Console.WriteLine("no solution");
                }
                else
                {
                    Console.WriteLine(path);
                }                ;
            }                                
        }
    }
}
