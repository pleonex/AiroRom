//
//  Program.cs
//
//  Author:
//       Benito Palacios Sánchez <benito356@gmail.com>
//
//  Copyright (c) 2015 Benito Palacios Sánchez
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Libgame;
using Libgame.IO;
using Nitro.Rom;

namespace NitroFilcher
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			args = new string[] {
				"/home/benito/Ninokuni [PATCHED].nds",
				"/home/benito/nino.txt",
				"/home/benito/output.txt"
			};

			DataStream romStream = new DataStream(args[0], FileMode.Open, FileAccess.Read);
			GameFile rom = new GameFile("Game.nds", romStream);
			Format romFormat = new Rom();
			romFormat.Initialize(rom);
			romFormat.Read();

			ExportFiles(rom, args[1], args[2]);
		}

		private static void ExportFiles(GameFile rom, string listAddress, string outputFile)
		{
			string[] entries = File.ReadAllLines(listAddress);
			List<string> files = new List<string>();

			int x = Console.CursorLeft;
			int y = Console.CursorTop;
			for (int i = 0; i < entries.Length; i++) {
				Console.SetCursorPosition(x, y);
				Console.WriteLine("Analyzing {0:06} of {1:06} ({2:F2})",
					i, entries.Length, i * 100.0 / entries.Length);

				string f = ExportFile(rom, entries[i]);
				if (!string.IsNullOrEmpty(f) && !files.Contains(f))
					files.Add(f);
			}

			File.WriteAllLines(outputFile, files);
		}

		private static string ExportFile(GameFile rom, string entry)
		{
			string[] fields = entry.Split(',');
			uint offset = Convert.ToUInt32(fields[0], 16);
			int size = Convert.ToInt32(fields[1], 16);

			return SearchFile(rom, offset, size);
		}

		private static string SearchFile(FileContainer folder, long offset, int size)
		{
			foreach (var file in folder.Files.Cast<GameFile>())
				if (IsContained(file, offset, size))
					return file.Path;

			foreach (var subfolder in folder.Folders) {
				var result = SearchFile(subfolder, offset, size);
				if (!string.IsNullOrEmpty(result))
					return result;
			}

			return string.Empty;
		}

		private static bool IsContained(GameFile file, long offset, int size)
		{
			long endOffset = file.Stream.Position + file.Stream.Length;
			return (offset >= file.Stream.Position) && (offset + size <= endOffset);
		}
	}
}
