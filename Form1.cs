using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MathNet.Numerics.Distributions;
using ZedGraph;
using System.IO;
using C5;

namespace OS
{
    public partial class Form1 : Form
    {
        int N;   //Number of processes
        double ATmean;
        double ATstdDev;
        double BTmean;
        double BTstdDev;
        double PriGamma;

        public Form1()
        {
            InitializeComponent();
            this.Text = "CPU";
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        //Read Parameters for distributions for the Process Generator
        private void button1_Click(object sender, EventArgs e)
        {
            string s = textBoxIn.Text;
            if (s.Length == 0)
            {
                MessageBox.Show("Invalid File Name", "Error",
                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int i = 0;
            try
            {
                foreach (string line in File.ReadLines(s, Encoding.UTF8))
                {
                    string[] parts = line.Split(new char[0]);
                    if (i == 0)
                    {
                        N = Int32.Parse(parts[0]);
                    }
                    else if (i == 1)
                    {
                        ATmean = Convert.ToDouble(parts[0]);
                        ATstdDev = Convert.ToDouble(parts[1]);
                    }
                    else if (i == 2)
                    {
                        BTmean = Convert.ToDouble(parts[0]);
                        BTstdDev = Convert.ToDouble(parts[1]);
                    }
                    else
                    {
                        PriGamma = Convert.ToDouble(parts[0]);
                    }
                    if (N < 0 || ATmean < 0.0 || ATstdDev < 0.0 || BTmean < 0.0 || BTstdDev < 0.0 || PriGamma < 0)
                    {
                        MessageBox.Show("Negative Values Deteced !!", "Error",
                           MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    ++i;
                }
            }

            catch (FileNotFoundException)
            {
                MessageBox.Show("Invalid File Name", "Error",
                 MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            ProcessGenerator();
        }

        //Generate Processes
        private void ProcessGenerator()
        {
            Normal ATNormal = new Normal(ATmean, ATstdDev);
            Normal BTNormal = new Normal(BTmean, BTstdDev);
            Poisson PriPoisson = new Poisson(PriGamma);

            using (StreamWriter sw = new StreamWriter("Processes.txt"))
            {
                sw.WriteLine(N);
                double randomValue;

                for (int i = 1; i <= N; ++i)
                {
                    string line = "";

                    line += i.ToString();
                    line += " ";

                    randomValue = ATNormal.Sample();
                    if (randomValue < 0)      // no negative AT
                    {
                        --i;
                        continue;
                    }
                    line += randomValue.ToString();
                    line += " ";

                    randomValue = BTNormal.Sample();
                    if (randomValue <= 0)     // no negative or zero BT
                    {
                        --i;
                        continue;
                    }
                    line += randomValue.ToString();
                    line += " ";

                    randomValue = PriPoisson.Sample();
                    if (randomValue < 0)      // no negative priority
                    {
                        --i;
                        continue;
                    }
                    line += randomValue.ToString();

                    sw.WriteLine(line);
                }
            }
            MessageBox.Show("Processes Generated Successfully", "Notification",
                 MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //Read Processes from file
        private Process[] ReadProcess()
        {
            Process []parr = null;
            int i = 0;
            string s = "Processes.txt";
            foreach (string line in File.ReadLines(s, Encoding.UTF8))
            {
                string[] parts = line.Split(new char[0]);

                if (parts.Length == 1)  // this is Number of process which i do have it already
                {
                    N = Int32.Parse(parts[0]);
                    parr = new Process[N];
                    continue;
                }
                int ID = Int32.Parse(parts[0]);
                double AT = Convert.ToDouble(parts[1]);
                double BT = Convert.ToDouble(parts[2]);
                int Priority = Int32.Parse(parts[3]);

                parr[i] = new Process(ID, AT, BT, Priority);
                ++i;
            }
            return parr;
        }

        //Plot ID/Time Graph for Scheduling
        private void PlotGraph(List<Tuple<int, Tuple<double, double>>> ProcessQueue)
        {
            GraphPane myPane = zedGraphControl1.GraphPane;

            // Set the Titles
            zedGraphControl1.GraphPane.CurveList.Clear();
            zedGraphControl1.GraphPane.GraphObjList.Clear();
            myPane.Title.Text = "Scheduling Processes";
            myPane.XAxis.Title.Text = "Time";
            myPane.YAxis.Title.Text = "ID";
            myPane.XAxis.Scale.Min = 0.0;
            myPane.YAxis.Scale.Min = 0.0;
            myPane.XAxis.Scale.MajorStep = 1;
            myPane.YAxis.Scale.MajorStep = 1;

            double xMax = 0, yMax = 0;
            for (int i = 0; i < ProcessQueue.Count; ++i)
            {
                int id = ProcessQueue[i].Item1;
                double from = ProcessQueue[i].Item2.Item1;
                double to = ProcessQueue[i].Item2.Item2;

                PointPairList PL = new PointPairList();
                PL.Add(from, id);
                PL.Add(to, id);

                xMax = Math.Max(xMax, to);
                yMax = Math.Max(yMax, id);

                LineItem L = myPane.AddCurve(null, PL, Color.Blue, SymbolType.None);
                L.Line.Fill = new Fill(Color.White, Color.Blue, 45F);
                
            }
            myPane.XAxis.Scale.Max = Math.Ceiling(xMax) + 5;
            myPane.YAxis.Scale.Max = yMax + 1;

            zedGraphControl1.AxisChange();
            zedGraphControl1.Refresh();

        }

        //Print Final statistics to a file
        private void Statistics(int n, double[] arrival, double[] burst, double[] finish)
        {
            double[] WT = new double[n + 1];        // Waiting Time
            double[] TAT = new double[n + 1];       // Turnaround Time
            double[] WTAT = new double[n + 1];      // Weighted TAT

            double SumTAT = 0, SumWTAT = 0;
            for (int i = 1; i <= n; ++i)
            {
                TAT[i] = finish[i] - arrival[i];
                WT[i] = TAT[i] - burst[i];
                WTAT[i] = TAT[i] / burst[i];

                SumTAT += TAT[i];
                SumWTAT += WTAT[i];
            }
            SumTAT /= n;                           // Average TAT
            SumWTAT /= n;                          // Average WTAT

            using (StreamWriter sw = new StreamWriter("Statistics.txt"))
            {
                sw.WriteLine(n);

                // id  TAT  WT  WTAT
                for (int i = 1; i <= n; ++i)
                {
                    string line = "";

                    line += i.ToString();
                    line += " ";

                    line += TAT[i].ToString();
                    line += " ";

                    line += WT[i].ToString();
                    line += " ";
                
                    line += WTAT[i].ToString();

                    sw.WriteLine(line);
                }

                string s = "Average Turnaround time of the schedule = " + SumTAT.ToString();
                sw.WriteLine(s);
                s = "Average Weighted turnaround time of the schedule = " + SumWTAT.ToString();
                sw.WriteLine(s);
            }
        }

        //Validate the Q and S time
        private bool CheckQSBox(int x)
        {
            string s = textBoxS.Text;

            try
            {
                if (s.Length == 0)
                    throw new Exception("Empty");

                double S = Convert.ToDouble(s);

                if (S < 0)
                {
                    MessageBox.Show("Invalid Switch Time", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            catch (Exception e)
            {
                if (e.Message == "Empty")
                {
                    MessageBox.Show("Empty Swtich Time Field Detected", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                }
                else
                {
                    MessageBox.Show("Invalid Switch Time", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return false;
            }
            if (x == 1) return true;
            string q = textBoxQ.Text;
            try
            {
                if (q.Length == 0)
                    throw new Exception("Empty");

                double Q = Convert.ToDouble(q);

                if (Q <= 0)
                {
                    MessageBox.Show("Invalid Quantum Time", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            catch (Exception e)
            {
                if (e.Message == "Empty")
                {
                    MessageBox.Show("Empty Quantum Time Field Detected", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
                else
                {
                    MessageBox.Show("Invalid Quantum Time", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return false;
            }
            return true;
        }

        //HPF Algorithm
        private void HPFButton_Click(object sender, EventArgs e)
        {
            Process[] parr = ReadProcess();
            if (parr.Length == 0) return;

            double[] Arrival = new double[parr.Length + 1];
            double[] Burst = new double[parr.Length + 1];
            double[] Finish = new double[parr.Length + 1];

            //Store AT , BT of each process
            for (int idx = 0; idx < parr.Length; ++idx)
            {
                Arrival[idx + 1] = parr[idx].AT;
                Burst[idx + 1] = parr[idx].BT;
            }

            //Sort the Processes according to their AT in ascending order
            Array.Sort(parr,
                    delegate (Process x, Process y) {return (x.AT.CompareTo(y.AT));});

            //This PQ represents the processes in ready state
            var heap = new IntervalHeap<Tuple<int, Tuple<int,double>>>();

            //This list represents process queue (id,(from time , to time))  for Graph
            List<Tuple<int, Tuple<double, double>>> ProcessQueue = new List<Tuple<int, Tuple<double, double>>>();

            double CurrentTime = parr[0].AT;

            int lft = parr.Length;

            while (lft > 0)
            {
                for (int i = 0; i < parr.Length; ++i)
                {
                    if (parr[i] != null && parr[i].AT <= CurrentTime)
                    {
                        heap.Add(new Tuple<int, Tuple<int, double>>(parr[i].Priority, new Tuple<int, double>(-1 * parr[i].ID, parr[i].BT)));
                        parr[i] = null;
                    }
                    else if (parr[i] != null)
                        break;             
                }

                //if there is such a large gap between process that all no ready process left 
                if (heap.IsEmpty)
                {
                    //get the AT of the process after the gap
                    for (int i = 0; i < parr.Length; ++i)
                    {
                        if (parr[i] != null)
                        {
                            CurrentTime = parr[i].AT;
                            break;
                        }
                    }
                    //Go Up and Fill the Heap
                    continue;
                }
         
                //(priority, (id,burst time))
                //Get the process that will be executed
                var CurrentProcess = heap.Max();
                heap.DeleteMax();
                --lft;

                int id = CurrentProcess.Item2.Item1 * -1;
                //execution duration start
                double from = CurrentTime;
                //execution duration end
                double to = CurrentTime + CurrentProcess.Item2.Item2;
                //Update Current Time
                CurrentTime += CurrentProcess.Item2.Item2;

                //Time when process finished
                Finish[id] = to;

                ProcessQueue.Add(new Tuple<int, Tuple<double, double>>(id, new Tuple<double, double>(from, to)));
            }
            PlotGraph(ProcessQueue);
            Statistics(parr.Length, Arrival, Burst, Finish);
        }

        //FCFS Algorithm
        private void FCFSButton_Click(object sender, EventArgs e)
        {
            Process[] parr = ReadProcess();
            if (parr.Length == 0) return;

            double[] Arrival = new double[parr.Length + 1];
            double[] Burst = new double[parr.Length + 1];
            double[] Finish = new double[parr.Length + 1];

            //store AT , BT of each Process
            for (int idx = 0; idx < parr.Length; ++idx)
            {
                Arrival[idx + 1] = parr[idx].AT;
                Burst[idx + 1] = parr[idx].BT;
            }

            //Sort the Processes according to their AT in ascending order
            Array.Sort(parr,
                    delegate (Process x, Process y) { return (x.AT.CompareTo(y.AT)); });

            //This list represents process queue (id,(from time , to time))  for Graph
            List<Tuple<int, Tuple<double, double>>> ProcessQueue = new List<Tuple<int, Tuple<double, double>>>();

            double CurrentTime = parr[0].AT;

            for (int i = 0; i < parr.Length; ++i)
            {
                int id = parr[i].ID;

                //if there is a gap between the last process and the next process
                if (parr[i].AT > CurrentTime)
                    CurrentTime = parr[i].AT;

                double from = CurrentTime;
                double to = CurrentTime + parr[i].BT;

                Finish[id] = CurrentTime + parr[i].BT;     
                CurrentTime += parr[i].BT;

                ProcessQueue.Add(new Tuple<int, Tuple<double, double>>(id, new Tuple<double, double>(from, to)));
            }
            PlotGraph(ProcessQueue);
            Statistics(parr.Length, Arrival, Burst, Finish);
        }

        //RR Algorithm
        private void RRButton_Click(object sender, EventArgs e)
        {
            //Check Quantum and Switch Time
            bool c = CheckQSBox(0);
            if (c == false) return;

            double Q = Convert.ToDouble(textBoxQ.Text);
            double S = Convert.ToDouble(textBoxS.Text);

            Process[] parr = ReadProcess();
            if (parr.Length == 0) return;

            double[] Arrival = new double[parr.Length + 1];
            double[] Burst = new double[parr.Length + 1];
            double[] Finish = new double[parr.Length + 1];

            //store AT , BT of each Process
            for (int idx = 0; idx < parr.Length; ++idx)
            {
                Arrival[idx + 1] = parr[idx].AT;
                Burst[idx + 1] = parr[idx].BT;
            }

            //Sort the Processes according to their AT in ascending order
            Array.Sort(parr,
                    delegate (Process x, Process y) { return (x.AT.CompareTo(y.AT)); });

            //This list represents process queue (id,(from time , to time))  for Graph
            List<Tuple<int, Tuple<double, double>>> ProcessQueue = new List<Tuple<int, Tuple<double, double>>>();

            double CurrentTime = parr[0].AT;

            //Ready Queue
            Queue<Tuple<int, double>> ReadyQueue = new Queue<Tuple<int, double>>();

            int lft = parr.Length;
            Tuple<int, double> lastP = new Tuple<int, double>(0, 0);

            while (lft > 0)
            {
                //Push all processes that have arrived in the queue
                for (int i = 0; i < parr.Length; ++i)
                {
                    if (parr[i] != null && parr[i].AT <= CurrentTime)
                    {
                        ReadyQueue.Enqueue(new Tuple<int, double>(parr[i].ID, parr[i].BT));
                        parr[i] = null;
                    }
                    else if (parr[i] != null)
                        break;
                }
                //After adding the new processes
                //Adding the last process after executing it Q time
                if (lastP.Item1 != 0) ReadyQueue.Enqueue(lastP);

                //Empty Queue = finished all ready processes but there are still some to come later
                if (ReadyQueue.Count == 0)
                {
                    for (int i = 0; i < parr.Length; ++i)
                    {
                        if (parr[i] != null)
                        {
                            //Advance the time to the next process
                            CurrentTime = parr[i].AT;
                            break;
                        }
                    }

                    //Go Up and Fill the Queue
                    continue;
                }

                Tuple<int, double> p = ReadyQueue.Dequeue();

                int id = p.Item1;
                double bt = p.Item2;
                double ExecutionTime = Math.Min(bt, Q);

                double from = CurrentTime;
                double to = CurrentTime + ExecutionTime;
                ProcessQueue.Add(new Tuple<int, Tuple<double, double>>(id, new Tuple<double, double>(from, to)));

                //Determine whether the process will go back to the queue or not
                //Assumption Switch Time is only used when a process is pushed again to the queue
                //Q > bt
                if (bt - ExecutionTime == 0)
                {
                    lastP = new Tuple<int, double>(0, 0);
                    Finish[id] = to;
                    --lft;
                    CurrentTime = CurrentTime + ExecutionTime;
                }
                //Q < bt
                else
                {
                    lastP = new Tuple<int, double>(id, bt - ExecutionTime);
                    CurrentTime = CurrentTime + ExecutionTime + S;
                }
               
            }
            PlotGraph(ProcessQueue);
            Statistics(parr.Length, Arrival, Burst, Finish);
        }

        //SRTN Algorithm
        private void SRTNButton_Click(object sender, EventArgs e)
        {
            //Check Switch Time
            bool c = CheckQSBox(1);
            if (c == false) return;

            double S = Convert.ToDouble(textBoxS.Text);

            Process[] parr = ReadProcess();
            if (parr.Length == 0) return;

            double[] Arrival = new double[parr.Length + 1];
            double[] Burst = new double[parr.Length + 1];
            double[] Finish = new double[parr.Length + 1];

            //Store AT , BT of each process
            for (int idx = 0; idx < parr.Length; ++idx)
            {
                Arrival[idx + 1] = parr[idx].AT;
                Burst[idx + 1] = parr[idx].BT;
            }

            //Sort the Processes according to their AT in ascending order
            Array.Sort(parr, delegate (Process x, Process y) { return (x.AT.CompareTo(y.AT)); });

            //This PQ represents the processes in ready state
            var heap = new IntervalHeap<Tuple<double,int> >();  //(Burst,id)

            //This list represents process queue (id,(from time , to time))  for Graph
            List<Tuple<int, Tuple<double, double>>> ProcessQueue = new List<Tuple<int, Tuple<double, double>>>();
            double current_time = parr[0].AT;
            for (int idx = 0; idx < parr.Length; ++idx)
            {
                if (heap.IsEmpty)  // If Heap is empty, then push the current process.
                {
                    heap.Add(new Tuple<double, int>(parr[idx].BT,parr[idx].ID));
                    continue;
                }
                while (idx < parr.Length && parr[idx].AT <= current_time)
                {
                    heap.Add(new Tuple<double, int>(parr[idx].BT, parr[idx].ID));
                    idx++;
                }
                var cur = heap.Min();
                if (idx >= parr.Length || cur.Item1 + current_time <= parr[idx].AT)
                {                   // If the top process of the heap will be done before the current process begin
                                    // Then don't push the current process and finish the top one.
                    Finish[cur.Item2] = cur.Item1 + current_time;
                    ProcessQueue.Add(new Tuple<int, Tuple<double, double>>(cur.Item2, new Tuple<double, double>(current_time, Finish[cur.Item2])));
                    current_time = Finish[cur.Item2];
                    heap.DeleteMin(); idx--;
                    continue;
                }
               
                // If I came here, Then there is an overlapping in intervals.
                heap.DeleteMin(); // pop the top process.
                // push the top process, with the time before overlapping.
                ProcessQueue.Add(new Tuple<int, Tuple<double, double>>(cur.Item2, new Tuple<double, double>(current_time,parr[idx].AT)));
                //Update the top process data.
                cur = new Tuple<double, int>(cur.Item1 - (parr[idx].AT - current_time), cur.Item2);

                //Push both in the heap.
                var tmp = new Tuple<double, int>(parr[idx].BT, parr[idx].ID);
                heap.Add(new Tuple<double, int>(parr[idx].BT, parr[idx].ID));
                heap.Add(cur);
                current_time = parr[idx].AT;
                if (cur.Item2 != heap.Min().Item2) current_time += S;
            }
            //Now we have no problem to do SJF.
            while (!heap.IsEmpty)
            {
                var ccur = heap.Min();
                heap.DeleteMin();
                Finish[ccur.Item2] = current_time + ccur.Item1; 
                
                ProcessQueue.Add(new Tuple<int, Tuple<double, double>>(ccur.Item2, new Tuple<double, double>(current_time , Finish[ccur.Item2])));
                current_time = Finish[ccur.Item2];
            }
            PlotGraph(ProcessQueue);
            Statistics(parr.Length, Arrival, Burst, Finish);
        }     
    }

}
