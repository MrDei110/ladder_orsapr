using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using NickStrupat;
using LadderPlugin;
using BuilderClass;
using System.Security.Cryptography;

namespace StressTesting
{
    /// <summary>
    /// Класс нагрузочного тестирования.
    /// </summary>
    public class StressTester
    {
        /// <summary>
        /// Метод для нагрузочного тестирования.
        /// </summary>
        public void StressTesting()
        {
            var builder = new Builder();
            var stopWatch = new Stopwatch();
            var parameters = new Parameters();
            parameters.SetParameter(ParameterType.TotalHeight, 970);
            parameters.SetParameter(ParameterType.StepsAmount, 2);
            parameters.SetParameter(ParameterType.MaterialThickness, 35);
            parameters.SetParameter(ParameterType.StepsSpacing, 300);
            parameters.SetParameter(ParameterType.StepsWidth, 500);
            Process currentProcess = System.Diagnostics.Process.GetCurrentProcess();
            var count = 0;
            var streamWriter = new StreamWriter("log.txt");
            const double gigabyteInByte = 0.000000000931322574615478515625;
            while (true)
            {
                stopWatch.Start();
                builder.Build(parameters, 0);
                stopWatch.Stop();
                var computerInfo = new ComputerInfo();
                var usedMemory = (computerInfo.TotalPhysicalMemory
                                  - computerInfo.AvailablePhysicalMemory)
                                  * gigabyteInByte;
                streamWriter.WriteLine(
                    $"{++count}\t{stopWatch.Elapsed:hh\\:mm\\:ss}\t{usedMemory}");
                streamWriter.Flush();
                stopWatch.Reset();
            }
        }
    }
}
