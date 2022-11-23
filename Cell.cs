using System;
using System.Collections.Generic;
using System.Text;

namespace PathFinder
{
    public class Cell
    {
        private List<Cell> _neighbours;        
        private Cell _parent;
        private int _x;
        private int _y;
        private Type _type; 
        public Cell(int x, int y)
        {
            _neighbours = new List<Cell>();
            _x = x;
            _y = y;
            _type = Type.ROUTE;
            _parent = null;
        }

        public List<Cell> Neighbours
        {
            get
            {
                return _neighbours;
            }
        }                
        public int X
        {
            get
            {
                return _x;
            }
        }
        public int Y
        {
            get
            {
                return _y;
            }
        }
        public Cell Parent
        {
            get
            {
                return _parent;                
            }
            set
            {
                _parent = value;
            }
        }
        public Type Type
        {
            get 
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }               
        public void AddNeighbour(Cell c)
        {
            _neighbours.Add(c);
        }        
    }
}
