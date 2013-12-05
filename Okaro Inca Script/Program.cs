using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace Okaro_Inca_Script
{
    class IncaStrings
    {
        public string Flow {get;set;}
        public string NO3 { get; set; }
        public string NH4 { get; set; }
        public string Vol { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Reading in data...");
            List<IncaStrings> data = new List<IncaStrings>();
            string file1 = File.ReadLines("setup.txt").Skip(2).Take(1).First();// Reads in config data
            string file2 = File.ReadLines("setup.txt").Skip(3).Take(1).First();
            string inflowFile = File.ReadLines("setup.txt").Skip(6).Take(1).First();
            string batFile = File.ReadLines("setup.txt").Skip(7).Take(1).First();
            string[] splitFile1 = file1.Split(',');
            string[] splitFile2 = file2.Split(',');
            string[] stream1Lines = new string [1460];
            string[] stream2Lines = new string[1460];
            int skip = int.Parse(splitFile1[1]);// How many lines to skip
            // Reads in stream 1 data
            for (int i = 0; i < int.Parse(splitFile1[2]); i++)
            {
                stream1Lines[i] = File.ReadLines(splitFile1[0]).Skip(skip).Take(1).First();
                skip++;
            }
            skip = int.Parse(splitFile2[1]); // How many lines to skip
            // Reads in stream 2 data
            for (int i = 0; i < int.Parse(splitFile2[2]); i++)
            {
                stream2Lines[i] = File.ReadLines(splitFile2[0]).Skip(skip).Take(1).First();
                skip++;
            }
            
            //Puts the data read in into incastring objects and then finally into a list
             for (int i = 0; i < 1460; i++)
              {
                 int itemCount = 0;
                 IncaStrings newIncaStream1 = new IncaStrings();
                 IncaStrings newIncaStream2 = new IncaStrings();
                string[] splitStream1 = stream1Lines[i].Trim().Split(' ');
                string[] splitStream2 = stream2Lines[i].Trim().Split(' ');
                for (int j = 0; j < splitStream1.Count(); j++)
                {
                    if (splitStream1[j] != "")
                    {
                        switch (itemCount)
                        {
                            case 0:
                                newIncaStream1.Flow = splitStream1[j];
                                break;
                            case 1:
                                newIncaStream1.NO3 = splitStream1[j];
                                break;

                            case 2:
                                newIncaStream1.NH4 = splitStream1[j];
                                break;
                            case 3:
                                newIncaStream1.Vol = splitStream1[j];
                                break;

                        }
                        itemCount++;
                    }
                }
                itemCount = 0;
                for (int j = 0; j < splitStream2.Count(); j++)
                {
                    if (splitStream2[j] != "")
                    {
                        switch (itemCount)
                        {
                            case 0:
                                newIncaStream2.Flow = splitStream2[j];
                                break;
                            case 1:
                                newIncaStream2.NO3 = splitStream2[j];
                                break;

                            case 2:
                                newIncaStream2.NH4 = splitStream2[j];
                                break;
                            case 3:
                                newIncaStream2.Vol = splitStream2[j];
                                break;

                        }
                        itemCount++;
                    }
                }
                 data.Add(newIncaStream1);
                 data.Add(newIncaStream2);
             }
            
            //Writes data to the new file
             string seperator = "\t";
             StreamWriter writer = new StreamWriter("output.txt");
             StreamReader reader = new StreamReader(inflowFile);
             writer.WriteLine(reader.ReadLine());
             writer.WriteLine(reader.ReadLine());
             writer.WriteLine(reader.ReadLine());
             writer.WriteLine(reader.ReadLine());
             writer.WriteLine(reader.ReadLine());
             writer.WriteLine(reader.ReadLine());
             int counter = 6;
             foreach (IncaStrings inca in data)
             {
                 if (counter > 756) break;
                 counter++;

                 string line = reader.ReadLine();
                 string[] splitLine = line.Split('\t');
                 writer.WriteLine(splitLine[0] + seperator + splitLine[1] + seperator + inca.Vol + seperator + splitLine[3] + seperator + splitLine[4] + seperator + splitLine[5] + seperator + inca.NO3 + seperator + inca.NH4 + seperator + splitLine[8] + seperator + splitLine[9] + seperator +splitLine[10] + seperator +splitLine[11] + seperator +splitLine[12] + seperator + splitLine[13]); // needs to go to 13
             }
             writer.Write(reader.ReadToEnd());

            //Clean up
             writer.Close();
             reader.Close();
             Console.WriteLine("Copying File...");
            //TODO: fix up the copy files process
             System.IO.File.Copy("output.txt", inflowFile, true);
             // Start the child process.
             Process p = new Process();
             
             Console.WriteLine("Starting Model Run...");
           p.StartInfo.UseShellExecute = false;
             p.StartInfo.RedirectStandardOutput = true;
             p.StartInfo.FileName = batFile;
             p.Start();
             Console.WriteLine(p.StandardOutput.ReadToEnd());
             p.WaitForExit(); 
             Console.WriteLine("done");
             Console.ReadLine();

        }
    }
}
