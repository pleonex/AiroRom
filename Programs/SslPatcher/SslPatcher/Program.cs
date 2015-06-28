//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="none">
// Copyright (C) 2014
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

namespace SslPatcher
{
    public static class MainClass
	{
        private const string SearchText  = "https://";
        private const string ReplaceText = "http://";

		public static void Main(string[] args)
		{
 			Console.WriteLine("No SSL autopatcher ~~ by pleonex ~~");
            Console.WriteLine("           Version 1.4             ");
			Console.WriteLine();

            if (args.Length == 0) {
				ShowHelp();
				return;
			}

            Stopwatch watch = new Stopwatch();
            watch.Start();

            // Initialize the system
            Initialize();

            foreach (string romPath in args)
                PatchGame(romPath);

            watch.Stop();
            Console.WriteLine("It took: {0}", watch.Elapsed);
            Console.WriteLine();
            Console.WriteLine("Press Enter to quit. . .");
            Console.ReadKey(true);
		}

		private static void ShowHelp() {
			Console.WriteLine("USAGE: SslPatcher.exe Game.nds");
			Console.WriteLine();
            Console.WriteLine("Output file will be written in the game dir");
			Console.WriteLine("with name \"Game [NOSSL].nds\"");
			Console.WriteLine();
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

        private static void PatchGame(string romPath)
        {
            // Create file of the ROM
            DataStream romStream = new DataStream(romPath, FileMode.Open, FileAccess.Read);
            GameFile rom = new GameFile("Game.nds", romStream);
            Format romFormat = new Rom();
            romFormat.Initialize(rom);

            // Read the ROM
            Console.WriteLine("Reading ROM: {0}", romPath);
            rom.Format.Read();

            // For each ARM and Overlay, search the string and change it.
            bool found = false;
            foreach (GameFile systemFile in rom.Folders[1].GetFilesRecursive(false)) {
                if (SearchAndModify(systemFile))
                    found = true;
            }

            // If no match, close without writing new ROM
            if (!found) {
                Console.WriteLine("Not found");
            } else {
                // Save the new ROM
                Console.WriteLine("Writing new ROM");
                string outPath = Path.Combine(Path.GetDirectoryName(romPath), Path.GetFileNameWithoutExtension(romPath));
                outPath += " [NOSSL].nds";
                rom.Format.Write(outPath);

                // Close
                rom.Format.Dispose();
            }
        }

        private static bool SearchAndModify(GameFile file)
		{
            // It is better not to try to modify it.
            if (file.Name == "ARM7.bin")
                return false;

			Console.WriteLine("Searching in {0}", file.Name);
			bool found = false;
            string[] noParams = new string[0];

            // 1.A Check if it's encoded
            bool isEncoded;

            // Take ARM as compressed always, if the file finally
            // it's not compressed the tool will do nothing
            bool isArm9 = file is ArmFile;
            isEncoded = isArm9;

            // If it's an overlay is easy to know when it's compressed
            OverlayFile overlay = file as OverlayFile;
            if (overlay != null)
                isEncoded = overlay.IsEncoded;

            // 1.B Decode if so
            if (isEncoded) {
                // Get the original size
                long encodedSize = file.Length;

                SetEncodingFormat(file, true);
                file.Format.Read();             // Get data from file
                file.Format.Import(noParams);   // Call to the decoding program
                file.Format.Write();            // Overwrite data file
                file.Format.Dispose();

                // Check if the file was decoded
                // else, it is not encoded and must not be
                // encoded again
                // This can happen only with ARM9 file
                isEncoded = encodedSize != file.Length;
                if (!isEncoded)
                    Console.WriteLine("\tFile {0} is not encoded.", file.Name);
            } else {
                // Get a stream to work with and set to the file
                // This allow read & write operations
                // In the case of decoding this is already done
                DataStream stream = new DataStream(new MemoryStream(), 0, 0);
                file.Stream.WriteTo(stream);
                file.ChangeStream(stream);
            }

			// 2 While get the end of the file
            //	2.A Search next value & Edit the value
            int pos = 0;
            while (pos != -1) {
                pos = Search(file.Stream, pos);
                if (pos != -1) {
                    found = true;
                    pos = Replace(file.Stream, pos);
                }
            }

            // Write if it was decoded return to original state
            if (isEncoded) {
                SetEncodingFormat(file, false);
                file.Format.Read();             // Get data from file
                file.Format.Import(noParams);   // Call to encoding program
                file.Format.Write();            // Overwrite data file
                file.Format.Dispose();
            }

            return found;
		}

        private static void SetEncodingFormat(GameFile file, bool toDecode)
        {
            bool isArm = (file is ArmFile) && ((ArmFile)file).IsArm9;
            string arg0 = toDecode ? "-d" : (isArm ? "-en9" : "-en");

            XElement formatNode = new XElement("Parameters");

            XElement importNode = new XElement("Import");
            importNode.Add(new XElement("Path", "./blz.exe"));
            importNode.Add(new XElement("Arguments", arg0 + " \"{$PATH:./arm9.bin}\""));
            importNode.Add(new XElement("InUnixRunOn", "wine"));

            XElement copyTo = new XElement("CopyTo", "./arm9.bin");
            copyTo.SetAttributeValue("autoremove", "true");
            importNode.Add(copyTo);

            XElement outputFile = new XElement("OutputFile", "./arm9.bin");
            outputFile.SetAttributeValue("autoremove", "true");
            importNode.Add(outputFile);

            formatNode.Add(importNode);

            Format externalProgram = new ExternalProgram();
            externalProgram.Initialize(file, formatNode);
        }
	
        private static int Search(DataStream stream, int pos)
        {
            stream.Seek(pos, SeekMode.Origin);

            // Search
            bool found = false;
            while (!stream.EOF && !found) {
                if (stream.ReadByte() != SearchText[0])
                    continue;

                found = true;
                for (int i = 1; i < SearchText.Length && found; i++) {
                    if (stream.ReadByte() != SearchText[i]) {
                        found = false;
                        stream.Seek(-i, SeekMode.Current);
                    }
                }
            }
                
            if (!found)
                return -1;

            // Go to the start of the match
            stream.Seek(-SearchText.Length, SeekMode.Current);
            return (int)stream.RelativePosition;
        }

        private static int Replace(DataStream stream, int pos)
        {
            stream.Seek(pos, SeekMode.Origin);

            // Get full string, including last null char '\0'
            StringBuilder sb = new StringBuilder();
            char ch;
            do {
                ch = (char)stream.ReadByte();
                sb.Append(ch);
            } while (ch != '\0');

            Console.WriteLine("\tFound at: {0:X8} -> {1}", stream.Position, sb);

            // Replace string
            sb.Replace(SearchText, ReplaceText);

            // Write new string
            stream.Seek(pos, SeekMode.Origin);
            foreach (char c in sb.ToString())
                stream.WriteByte(Convert.ToByte(c));

            return pos + sb.Length + 1;
        }
    }
}
