using CommandLine;
using Microsoft.VisualStudio.Coverage.Analysis;
using System;
using System.Collections.Generic;
using System.IO;

namespace CoverageConverter
{
    class CoverageConverter
    {
        public class Options
        {
            [Option('c', "coveragefile", Required = true, HelpText = "Location of coverage file.")]
            public string CoverageFileLocation { get; set; }

            [Option('t', "targetdll", Required = true, HelpText = "Location of target dll.")]
            public string TargetDllLocation { get; set; }

            [Option('o', "outputlocation", Required = true, HelpText = "Location for converted coverage file.")]
            public string ConvertedFileLocation { get; set; }

        }
        static void Main(string[] args)
        {
            int checker = 0;
            Parser.Default.ParseArguments<Options>(args)
                   .WithParsed<Options>(o =>
                   {
                       if (o.CoverageFileLocation != string.Empty && File.Exists(o.CoverageFileLocation))
                       {
                           if (Path.GetExtension(o.CoverageFileLocation) != ".coverage")
                           {
                               Console.WriteLine($"Coverage File: -c {o.CoverageFileLocation} isn't a .coverage file");
                           }
                           else
                           {
                               Console.WriteLine("Coverage File: " + o.CoverageFileLocation);
                               checker++;
                           }
                       }
                       else
                       {
                           Console.WriteLine($"Coverage File: -c {o.CoverageFileLocation} doesn't exist");
                       }
                       if (o.TargetDllLocation != string.Empty && File.Exists(o.TargetDllLocation))
                       {
                           if (Path.GetExtension(o.TargetDllLocation) != ".dll")
                           {
                               Console.WriteLine($"Target Dll File: -t {o.TargetDllLocation} isn't a dll file");
                           }
                           else
                           {
                               Console.WriteLine("Target Dll File: " + o.TargetDllLocation);
                               checker++;
                           }                               
                       }
                       else
                       {
                           Console.WriteLine($"Target Dll File: -t {o.TargetDllLocation} doesn't exist");
                       }
                       if (o.ConvertedFileLocation != string.Empty && Directory.Exists(o.ConvertedFileLocation))
                       {
                           Console.WriteLine("Output Location: " + o.ConvertedFileLocation);
                           checker++;
                       }
                       else
                       {
                           Console.WriteLine($"Output Location: -o {o.ConvertedFileLocation} doesn't exist");
                       }
                       if (checker == 3)
                       {
                           try
                           {
                               Console.WriteLine("All is well");
                               List<string> dllpath = new List<string>();
                               dllpath.Add(o.TargetDllLocation);
                               List<string> symbols = new List<string>();

                               FileInfo fi = new FileInfo(o.CoverageFileLocation);
                               string fileName = Path.Combine(o.ConvertedFileLocation, fi.Name + "xml");

                               using (CoverageInfo info = CoverageInfo.CreateFromFile(o.CoverageFileLocation))
                               {
                                   CoverageDS data = info.BuildDataSet();
                                   data.WriteXml(fileName);
                               }
                           }
                           catch (Exception ex)
                           {
                               Console.WriteLine("Badness happened: " + ex.Message);
                           }                           
                       }
                   });
            
        }
    }
}
