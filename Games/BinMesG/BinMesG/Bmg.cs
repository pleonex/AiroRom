//
//  Bmg.cs
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
using System.Xml.Linq;
using Libgame;
using Libgame.IO;
using System.Text;

namespace BinMesG
{
	public class Bmg : XmlExportable
	{
		Entry[] entries;
		Entry[] extraEntries;
		uint unknown;

		public override string FormatName {
			get { return "Text.bmg"; }
		}

		static string MagicStamp {
			get { return "MESGbmg1"; }
		}

		static string InfoStamp {
			get { return "INF1"; }
		}

		static string DataStamp {
			get { return "DAT1"; }
		}

		static string StringStamp {
			get { return "STR1"; }
		}

		static string FlwStamp {
			get { return "FLW1"; }
		}

		public override void Read(DataStream strIn)
		{
			var reader = new DataReader(strIn);

			if (reader.ReadString(8) != MagicStamp)
				throw new FormatException();

			reader.ReadUInt32();	// File size
			reader.ReadUInt32();	// Number of sections
			unknown = reader.ReadUInt32();

			if (!GotoSection(reader, InfoStamp))
				throw new FormatException();
			long infoPos = strIn.RelativePosition;

			if (!GotoSection(reader, DataStamp))
				throw new FormatException();
			long dataPos = strIn.RelativePosition;

			long stringPos = -1;
			if (GotoSection(reader, StringStamp))
				stringPos = strIn.RelativePosition;

			strIn.Seek(infoPos, SeekMode.Origin);
			int numEntries = reader.ReadUInt16();

			entries = new Entry[numEntries];
			for (int i = 0; i < numEntries; i++)
				entries[i] = ReadEntry(strIn, i, infoPos, dataPos, stringPos);

			extraEntries = new Entry[0];
			if (GotoSection(reader, FlwStamp)) {
				long flwPos = strIn.RelativePosition;
				uint numEntriesFlw = reader.ReadUInt32();
				reader.ReadUInt32();	// Unknown

				extraEntries = new Entry[numEntriesFlw];
				for (int i = 0; i < numEntriesFlw; i++) {
					strIn.Seek(flwPos + 8 + (8 * i) + 2, SeekMode.Origin);
					int id = reader.ReadUInt16();

					extraEntries[i] = entries[id];
				}
			}
		}

		bool GotoSection(DataReader reader, string stamp)
		{
			reader.Stream.Seek(0x0C, SeekMode.Origin);
			uint numSections = reader.ReadUInt32();

			bool found = false;
			reader.Stream.Seek(0x20, SeekMode.Origin);
			for (int i = 0; i < numSections && !found; i++) {
				found = (reader.ReadString(4) == stamp);
				uint size = reader.ReadUInt32();

				if (!found)
					reader.Stream.Seek(size - 0x8, SeekMode.Current);
			}

			return found;
		}

		Entry ReadEntry(DataStream strIn, int id, long infoPos, long dataPos, long stringPos)
		{
			var reader = new DataReader(strIn, EndiannessMode.LittleEndian, Encoding.Unicode);
			strIn.Seek(infoPos + 2, SeekMode.Origin);
			int entrySize = reader.ReadUInt16();

			strIn.Seek(infoPos + 8 + (id * entrySize), SeekMode.Origin);
			uint offset = reader.ReadUInt32();
			int meta1 = (entrySize > 4) ? reader.ReadUInt16() : -1;
			int meta2 = (entrySize > 4) ? reader.ReadUInt16() : -1;

			strIn.Seek(dataPos + offset, SeekMode.Origin);
			var entry = new Entry();
			entry.Text = reader.ReadString();

			if (stringPos != -1) {
				strIn.Seek(stringPos + meta1, SeekMode.Origin);
				entry.Metadata1 = reader.ReadString();

				strIn.Seek(stringPos + meta2, SeekMode.Origin);
				entry.Metadata2 = reader.ReadString();
			}

			return entry;
		}

		public override void Write(DataStream strOut)
		{
			throw new NotImplementedException();
		}

		protected override void Import(XElement root)
		{
			throw new NotImplementedException();
		}

		protected override void Export(XElement root)
		{
			root.Add(new XElement("Unknown", unknown));

			var xelements = new XElement("Entries");
			root.Add(xelements);
			foreach (var e in entries)
				xelements.Add(ExportEntry(e));

			var extraElements = new XElement("Extras");
			root.Add(extraElements);
			foreach (var e in extraEntries)
				extraElements.Add(ExportEntry(e));
		}

		XElement ExportEntry(Entry entry)
		{
			var el = new XElement("Entry");
			el.Add(new XElement("Text", entry.Text.ToXmlString(4, '[', ']')));
			el.Add(new XElement("Metadata1", entry.Metadata1.ToXmlString(4, '[', ']')));
			el.Add(new XElement("Metadata2", entry.Metadata2.ToXmlString(4, '[', ']')));
			return el;
		}

		protected override void Dispose(bool freeManagedResourcesAlso)
		{
		}

		private class Entry
		{
			public string Text { get; set; }
			public string Metadata1 { get; set; }
			public string Metadata2 { get; set; }
		}
	}
}

