//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="none">
// Copyright (C) 2015
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program. If not, see "http://www.gnu.org/licenses/".
// </copyright>
// <author>pleoNeX</author>
// <email>benito356@gmail.com</email>
//-----------------------------------------------------------------------
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Libgame;
using Libgame.IO;
using Nitro.Rom;
using Common;

namespace RC4Finder
{
    public static class MainClass
    {
        private static readonly byte[] Rc4Function = {
            //0x70, 0x40, 0x2D, 0xE9, 0x00, 0x00, 0x52, 0xE3, 0x02, 0x40, 0x80, 0xE2, 0x00, 0xC0, 0xD0, 0xE5, 
            0x01, 0xE0, 0xD0, 0xE5, 0x00, 0x30, 0xA0, 0xE3, 0x10, 0x00, 0x00, 0xDA, 0x01, 0x50, 0x8C, 0xE2, 
            0xFF, 0xC0, 0x05, 0xE2, 0x0C, 0x60, 0xD4, 0xE7, 0x06, 0x50, 0x8E, 0xE0, 0xFF, 0xE0, 0x05, 0xE2, 
            0x0E, 0x50, 0xD4, 0xE7, 0x0C, 0x50, 0xC4, 0xE7, 0x05, 0x50, 0x86, 0xE0, 0x0E, 0x60, 0xC4, 0xE7, 
            0xFF, 0x50, 0x05, 0xE2, 0x03, 0x60, 0xD1, 0xE7, 0x05, 0x50, 0xD4, 0xE7, 0x05, 0x50, 0x26, 0xE0, 
            0x03, 0x50, 0xC1, 0xE7, 0x01, 0x30, 0x83, 0xE2, 0x02, 0x00, 0x53, 0xE1, 0xEE, 0xFF, 0xFF, 0xBA,
            //0x00, 0xC0, 0xC0, 0xE5, 0x01, 0xE0, 0xC0, 0xE5
        };

        public static void Main(string[] args)
        {
            if (args.Length != 1) {
                ShowHelp();
                return;
            }

            // Initialize the system
            Initialize();

            // If it's a file
            string[] roms = new string[] { args[0] };

            // But if it's a folder
            if (File.GetAttributes(args[0]).HasFlag(FileAttributes.Directory))
                roms = Directory.GetFiles(args[0], "*.nds");

            // For each file
            foreach (string rom in roms) {
                SearchGame(rom);
                Console.WriteLine();
                Console.WriteLine("--------------------------------");
                Console.WriteLine();
            }
        }

        private static void ShowHelp()
        {
            Console.WriteLine( "USAGE: RC4Finder game" );
        }

        private static void Initialize()
        {
            // This is totally unnecessary in this case. 
            // I must change it in the libgame project.
            XElement root = new XElement("GameChanges");
            root.Add(new XElement("RelativePaths"));
            root.Add(new XElement("CharTables"));

            XElement specialChars = new XElement("SpecialChars");
            specialChars.Add(new XElement("Ellipsis"));
            specialChars.Add(new XElement("QuoteOpen", "\""));
            specialChars.Add(new XElement("QuoteClose", "\""));
            specialChars.Add(new XElement("FuriganaOpen", "["));
            specialChars.Add(new XElement("FuriganaClose", "]"));
            root.Add(specialChars);

            Configuration.Initialize(new XDocument(root));
        }

        private static void SearchGame(string romPath)
        {
            // Create file of the ROM
            DataStream romStream = new DataStream(romPath, FileMode.Open, FileAccess.Read);
            GameFile rom = new GameFile("Game.nds", romStream);
            Format romFormat = new Rom();
            romFormat.Initialize(rom);

            // Read the ROM
            Console.WriteLine("Reading: {0}", romPath);
            rom.Format.Read();

            // For each ARM and Overlay, search the string.
            foreach (GameFile systemFile in rom.Folders[1].GetFilesRecursive(false))
                SearchAndShow(systemFile);
        }

        private static void SearchAndShow(GameFile file) {
            // It is better not to try to modify it.
            if (file.Name == "ARM7.bin")
                return;

            // 1.A Check if it's encoded
            bool isEncoded;

            // Take ARM as compressed always, if the file finally
            // it's not compressed the tool will do nothing
            ArmFile arm9 = file as ArmFile;
            bool isArm9  = arm9 != null;
            isEncoded    = isArm9;

            // If it's an overlay is easy to know when it's compressed
            OverlayFile overlay = file as OverlayFile;
            if (overlay != null)
                isEncoded = overlay.IsEncoded;

            // 1.B Decode if so
            if (isEncoded) {
                SetEncodingFormat(file, true);
                file.Format.Read();                 // Get data from file
                file.Format.Import(new string[0]);  // Call to the decoding program
                file.Format.Write();                // Overwrite data file
                file.Format.Dispose();
            }

            // 2 Search.
            //  If we have found something, let's examine its data
            long pos = SearchText(file.Stream, 0);
            if (pos != -1) {
                // Get RAM address
                if (isArm9)
                    pos += arm9.RamAddress;
                else
                    pos += overlay.RamAddress;

                Console.WriteLine("Found in {0} at 0x{1:X8}", file.Name, pos);
                Console.Write("Press Enter to continue.");
                Console.ReadKey(true);
            }
        }

        private static void SetEncodingFormat(GameFile file, bool toDecode) {
            bool isArm = ( file is ArmFile ) && ( (ArmFile)file ).IsArm9;
            string arg0 = toDecode ? "-d" : (isArm ? "-en9" : "-en");

            XElement formatNode = new XElement("Parameters");

            XElement importNode = new XElement("Import");
            importNode.Add(new XElement("Path", "./blz.exe" ));
            importNode.Add(new XElement("Arguments", arg0 + " \"{$PATH:./temp.bin}\"" ));
            importNode.Add(new XElement("InUnixRunOn", "wine" ));

            XElement copyTo = new XElement("CopyTo", "./temp.bin");
            copyTo.SetAttributeValue("autoremove", "true");
            importNode.Add(copyTo);

            XElement outputFile = new XElement("OutputFile", "./temp.bin");
            outputFile.SetAttributeValue("autoremove", "true");
            importNode.Add(outputFile);

            formatNode.Add(importNode);

            Format externalProgram = new ExternalProgram();
            externalProgram.Initialize(file, formatNode);
        }

        private static long SearchText(DataStream stream, long pos) {
            stream.Seek(pos, SeekMode.Origin);

            // Search the text
            bool found = false;
            while (!stream.EOF && !found) {
                if (stream.ReadByte() != Rc4Function[0])
                    continue;

                // Found first char, let's suppuse we have found it
                found = true;
                for (int i = 1; i < Rc4Function.Length && found; i++ ) {
                    // Oh, no, they are different, try again from this position
                    if (stream.ReadByte() != Rc4Function[i]) {
                        found = false;
                        stream.Seek(-i, SeekMode.Current);
                    }
                }
            }

            // EOF
            if (!found)
                return -1;

            // Go to the start of the match
            stream.Seek(-Rc4Function.Length, SeekMode.Current);
            return stream.RelativePosition;
        }
    }
}