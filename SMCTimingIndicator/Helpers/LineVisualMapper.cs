using cAlgo.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMCTimingIndicator.Enums;

namespace SMCTimingIndicator.Helpers
{
	public static class LineVisualMapper
	{
		public static LineStyle ToLineStyle(MyLineStyle style) => style switch
		{
			MyLineStyle.Solid => LineStyle.Solid,
			MyLineStyle.Dotted => LineStyle.Dots,
			MyLineStyle.Dashed => LineStyle.Lines,
			_ => LineStyle.Lines
		};

		public static Color ToColor(MyLineColor color) => color switch
		{
			MyLineColor.White => Color.White,
			MyLineColor.Red => Color.Red,
			MyLineColor.Blue => Color.Blue,
			MyLineColor.Yellow => Color.Yellow,
			MyLineColor.Green => Color.Green,
			MyLineColor.Purple => Color.Purple,
			MyLineColor.Grey => Color.Gray,
			MyLineColor.Orange => Color.Orange,
			_ => Color.White
		};

		public static int ToThickness(MyLineThickness thickness) => (int)thickness;
	}
}
