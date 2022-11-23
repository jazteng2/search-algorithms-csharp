using System;
using SplashKitSDK;
using System.IO;
using System.Text.RegularExpressions;  
using System.Collections.Generic; 

namespace PathFinder
{
    class Program
    {        
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter Map name");
            Console.WriteLine("Maps:\n" +
                "Data.txt");
            string file_name = Console.ReadLine();
            Console.WriteLine("Please enter Search Type");
            Console.WriteLine("Search Algorithms:\n " +
                "BFS (Breath first search) \n" +
                "DFS (Depth first search) \n " +
                "GBFS (Greedy Best first search) \n" +
                "A* (A-Star search) \n" +
                "DLS (Depth Limited Search) \n" +
                "DJKS (Dijkstra's search)");            
            string method = Console.ReadLine();

            // Set up environment
            StreamReader filePath = new StreamReader(@"C:\Assignments\PartB\" + file_name);
            Map map = new Map(filePath);   
            Algorithms a = new Algorithms(map);
            menu menu = new menu();
            
            int width = map.xbound * 50;
            int height = map.ybound * 50;
            bool display_map = false;
            new Window("pathfinder", width, height);  
            List<string> window_text = new List<string>();

            Console.WriteLine(file_name + " " + method + " " + map.xbound * map.ybound);            
            do
            {
                SplashKit.ProcessEvents();
                SplashKit.ClearScreen();                
                map.Draw();
                if (display_map == true)
                {
                    menu.Action(map, a, method);
                }
                display_map = true;
                SplashKit.RefreshScreen();                
            } while (!SplashKit.WindowCloseRequested("pathfinder"));                 
        }        
    }
}
