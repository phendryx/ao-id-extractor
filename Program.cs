﻿using ao_id_extractor.Extractors;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace ao_id_extractor
{
    class Program
    {
        static void PrintCmdHelp()
        {
            Console.WriteLine("How to use:\nao-id-extractor.exe modeID outFormat [outFolder]\n" +
                "modeID\t\t#Extraction 0=Item, 1=Location, 2=Resource, 3=All\n" +
                "outFormat\t#l=Text List, j=JSON b=Both\n" +
                "[outFolder]\t#OPTIONAL: Output folder path. Default: current directory\n" +
                "[gameFolder]\t#OPTIONAL: Location of the main AlbionOnline folder");
        }

        static List<string> ParseCommandline(string[] args)
        {
            string modeID;
            string outFormat;
            string gameFolder = "";
            string outFolder = "";

            if (args.Length >= 2)
            {

                if (int.Parse(args[0]) > -1 && int.Parse(args[0]) <= 3)
                {
                    modeID = args[0];
                }
                else
                {
                    PrintCmdHelp();
                    return null;
                }


                if (args[1] == "l" || args[1] == "j" || args[1] == "b")
                {
                    outFormat = args[1];
                }
                else
                {
                    PrintCmdHelp();
                    return null;
                }

                if (args.Length == 3)
                {
                    outFolder = args[2];
                }

                if (args.Length == 4)
                {
                    gameFolder = args[3];
                }
            }
            else
            {
                PrintCmdHelp();
                return null;
            }

            return new List<string>() { modeID, outFormat, outFolder, gameFolder };
        }

        static void Main(string[] args)
        {
            List<string> cmds = ParseCommandline(args);

            if (cmds == null)
            {
                Console.Read();
                return;
            }

            if (cmds[2] == "")
                cmds[2] = Directory.GetCurrentDirectory();

            var exportType = ExportType.Both;
            switch (cmds[1])
            {
                case "l":
                    exportType = ExportType.TextList;
                    break;
                case "j":
                    exportType = ExportType.Json;
                    break;
            }

            if (cmds[0] == "0" || cmds[0] == "3")
            {
                new ItemExtractor(cmds[2], exportType, cmds[3]).Extract();
            }

            if (cmds[0] == "1" || cmds[0] == "3")
            {
                new LocationExtractor(cmds[2], exportType, cmds[3]).Extract();
            }

            if (cmds[0] == "2" || cmds[0] == "3")
            {
                new ResourceExtractor(cmds[2], exportType, cmds[3]).Extract();
            }
        }
    }
}
