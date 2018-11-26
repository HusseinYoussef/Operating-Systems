using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS
{
    class Process
    {    
        public int ID, Priority;
        public double AT, BT;
        
        public Process(int id = 1, double arrive = 0 , double burst = 1, int p = 0)
        {
            ID = id;
            AT = arrive;
            BT = burst;
            Priority = p; 
        }
    }
    public interface IComparer
    {
        int Compare(Object x, Object y);
    }
    public class test : IComparer<Tuple<int,int>>
    {
        public int Compare(Tuple<int, int> x, Tuple<int, int> y)
        {
            if (x.Item1 < y.Item1)
                return -1;
            else if (x.Item1 == y.Item1)
            {
                if (x.Item2 < y.Item2)
                    return 0;
                else
                    return 1;
            }
            return 1;
        }
    }
}
