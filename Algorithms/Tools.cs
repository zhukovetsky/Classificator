using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OxyPlot;

namespace Algorithms
{
	public static class Tools
	{
		private static readonly Random _random = new Random();

		public static OxyColor GetRandomColor()
		{
			return OxyColor.FromArgb(250, (byte) _random.Next(250), (byte) _random.Next(250), (byte) _random.Next(250));
		}
	}
}
