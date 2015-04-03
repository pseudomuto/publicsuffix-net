//
// ListParserTest.cs
//
// Author:
//       PseudoMuto <david.muto@gmail.com>
//
// Copyright (c) 2015 PseudoMuto
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using NUnit.Framework;
using System.IO;

namespace PublicSuffix.Test
{
	[TestFixture]
	public class ListParserTest
	{
		private readonly ListParser _subject = new ListParser();

		[TestCase]
		public void ParseIgnoresBlankLines()
		{
			var lines = new string[] {
				"  ", "ac", "", "com.ac", "     ", "edu.ac", "", " ", ""
			};

			using (var stream = StringStream(lines))
			{
				var list = _subject.Parse(stream);
				Assert.AreEqual(3, list.Count);
			}
		}

		[TestCase]
		public void ParseIgnoresComments()
		{
			var lines = new string[] {
				"// ac : http://en.wikipedia.org/wiki/.ac",
				"ac",
				"   // com.ac : Some indented comment",
				"com.ac"
			};

			using (var stream = StringStream(lines))
			{
				var list = _subject.Parse(stream);
				Assert.AreEqual(2, list.Count);
			}
		}

		private Stream StringStream(params string[] lines)
		{
			var stream = new MemoryStream();
			var writer = new StreamWriter(stream);

			foreach (var line in lines)
			{
				writer.WriteLine(line);
			}

			writer.Flush();
			stream.Flush();
			stream.Position = 0;
			return stream;
		}
	}
}

