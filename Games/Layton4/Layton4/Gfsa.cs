//
//  Gfsa.cs
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
using Libgame;
using Libgame.IO;
using Mono.Addins;

namespace Layton4
{
    [Extension]
    public class Gfsa : Format
    {
        const string MagicStamp = "GFSA";
        const int NumHeaders = 4;

        public override string FormatName {
            get { return "Layton4.Gfsa"; }
        }

        public override void Read(DataStream strIn)
        {
            var reader = new DataReader(strIn);
            if (reader.ReadString(4) != MagicStamp)
                throw new FormatException("Invalid " + FormatName + " format");

            for (int i = 0; i < NumHeaders; i++) {
                uint offset = reader.ReadUInt32();

                // Peek next offset to calculate block size
                uint nextOffset = reader.ReadUInt32(); strIn.Seek(-4, SeekMode.Current);
                uint size = nextOffset - offset;

                var subStream = new DataStream(strIn, offset, size);
                var subFile = new GameFile("Block" + i.ToString() + ".bin", subStream);
                subFile.SetFormat<GfsaBlock>();
                subFile.Format.Read();
                this.File.AddFile(subFile);
            }
        }

        DataStream GetBlock(DataStream strIn, uint offset, uint size)
        {
            return null;
        }

        public override void Write(DataStream strOut)
        {
            throw new NotImplementedException();
        }

        public override void Import(params DataStream[] strIn)
        {
            throw new NotImplementedException();
        }

        public override void Export(params DataStream[] strOut)
        {
            throw new NotImplementedException();
        }

        protected override void Dispose(bool freeManagedResourcesAlso)
        {
        }
    }
}
