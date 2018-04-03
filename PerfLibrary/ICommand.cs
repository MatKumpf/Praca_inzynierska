using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfLibrary
{
    interface ICommand
    {
        string[] GetAllParam();
        int GetLengthEnum();
        bool IsAvailable(string parameter);
        string Name { get; }
        char[] PrepareDataCharArray(string parameter, string data);
        string PrepareDataString(string parameter, string data);
        bool ValidationAndPrepareData(string parameter, ref string data);
        bool ValidationAndPrepareData(string parameter, ref string data, string counter);
    }
}
