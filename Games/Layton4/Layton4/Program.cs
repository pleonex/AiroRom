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
using System.IO;
using System.Linq;
using System.Reflection;
using Libgame;

namespace Layton4
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            if (args.Length < 1)
                return;

            string executableDir = args [0];
            var root = GameFolderFactory.FromPath(executableDir);
            FileManager.Initialize(root, new FileInfoCollection());

            foreach (var file in root.Files) {
                var container = file as GameFile;
                if (container == null || !container.Name.EndsWith(".fa"))
                    continue;

                container.SetFormat<Gfsa>();
            }

            ReadAll(root);
            ExtractFolder(args[0], root);
        }

        private static void ReadAll(FileContainer container)
        {
            foreach (GameFile subfile in container.Files) {
                if (subfile.Format == null)
                    FileManager.AssignBestFormat(subfile);

                if (subfile.Format != null)
                    subfile.Format.Read();

                if (subfile.Files.Count > 0 || subfile.Folders.Count > 0)
                    ReadAll(subfile);
            }

            foreach (GameFolder subfolder in container.Folders)
                ReadAll(subfolder);
        }

        private static void ExtractFolder(string outputDir, FileContainer folder)
        {
            string folderDir = Path.Combine(outputDir, folder.Name);
            Directory.CreateDirectory(folderDir);

            foreach (GameFile subfile in folder.Files) {
                if (subfile.Files.Count > 0 || subfile.Folders.Count > 0)
                    ExtractFolder(folderDir, subfile);
                else
                    subfile.Stream.WriteTo(Path.Combine(folderDir, subfile.Name));
            }

            foreach (GameFolder subfolder in folder.Folders)
                ExtractFolder(folderDir, subfolder);
        }
    }
}
