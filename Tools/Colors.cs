using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Tools
{
    public static class Colors
    {
		private static Random _random = new Random();
		public static Color GetRandomColor()
		{
			Color.FromArgb(_random.Next(250), _random.Next(250), _random.Next(250));
		}
    }
}
