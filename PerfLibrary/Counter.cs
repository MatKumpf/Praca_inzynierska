using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfLibrary
{
    class Counter
    {
        private PerformanceCounter _performanceCounter;
        private string fileName;
        private string id;
        private int[] levels;
        private int maxValue;

        public Counter(string CategoryName, string CounterName, string InstanceName, int[] levels, string FileName)
        {
            _performanceCounter = new PerformanceCounter(CategoryName, CounterName, InstanceName);
            _performanceCounter.NextValue();
            this.levels = levels;
            fileName = FileName + ".data";
            maxValue = 0;
            id = FileName.Split(' ')[1];
        }

        public Counter(string CategoryName, string CounterName, int[] levels, string FileName)
        {
            _performanceCounter = new PerformanceCounter(CategoryName, CounterName);
            _performanceCounter.NextValue();
            this.levels = levels;
            fileName = FileName + ".data";
            maxValue = 0;
            id = FileName.Split(' ')[1];
        }

        public Counter(string CategoryName, string CounterName, string InstanceName, int[] levels, string FileName, int maxvalue)
        {
            _performanceCounter = new PerformanceCounter(CategoryName, CounterName, InstanceName);
            _performanceCounter.NextValue();
            this.levels = levels;
            fileName = FileName + ".data";
            maxValue = maxvalue;
            id = FileName.Split(' ')[1];
        }

        public Counter(string CategoryName, string CounterName, int[] levels, string FileName, int maxvalue)
        {
            _performanceCounter = new PerformanceCounter(CategoryName, CounterName);
            _performanceCounter.NextValue();
            this.levels = levels;
            fileName = FileName + ".data";
            maxValue = maxvalue;
            id = FileName.Split(' ')[1];
        }

        public int MaxValue
        {
            get { return maxValue; }
            set { maxValue = value; }
        }

        public int[] Levels
        {
            get { return levels; }
            set { levels = value; }
        }

        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        public string ID
        {
            get { return id; }
        }

        public int GetLevel()
        {
            int j = 0;

            for (int i = 0; i < levels.Length; i++)
            {
                j++;
                if (maxValue == 0)
                {
                    if (Math.Round(_performanceCounter.NextValue(), 0) < levels[i])
                        break;
                }
                else
                {
                    if ((Math.Round(_performanceCounter.NextValue(), 0) / maxValue) * 100 < levels[i])
                        break;
                }
            }

            return j;
        }
    }
}
