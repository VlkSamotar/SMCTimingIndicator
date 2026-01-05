/*
  SMC Timing Indicator for cTrader
  --------------------------------
  Author: Jakub Březa
  Date: December 2025
  License: MIT (see LICENSE file for details)

  Description:
  This indicator draws up to eight fully configurable vertical timing lines
  to highlight key intraday sessions, liquidity windows, or market events.
  Each line supports custom time, color, style, thickness, and optional
  historical rendering. A global UTC offset ensures correct alignment across
  different brokers and server timezones.

  Features:
  - Up to 8 independent timing lines
  - Customizable time, color, style, and thickness
  - Optional historical mode for backtesting
  - Global UTC offset for consistent timing
  - Clean, modular architecture for easy maintenance
*/

using cAlgo.API;
using SMCTimingIndicator.Enums;
using SMCTimingIndicator.Helpers;
using SMCTimingIndicator.Models;
using SMCTimingIndicator.Services;
using System;

namespace SMCTimingIndicator
{
	[Indicator(IsOverlay = true, AccessRights = AccessRights.None)]
	public class SMCTimingIndicator : Indicator
	{
		[Parameter("Show Historical Lines?", DefaultValue = false, Group = "Global Settings")]
		public bool ShowHistory { get; set; }

		[Parameter("UTC Offset (minutes)", DefaultValue = 60, Group = "Global Settings")]
		public int TimezoneOffsetMinutes { get; set; }

		[Parameter("Show at interval below", DefaultValue = PossibleTimeFrames.Hour1, Group = "Global Settings")]
		public PossibleTimeFrames ShowBelowTimeFrame { get; set; }

		
		[Parameter("Line 1 Time (HH:mm)", DefaultValue = "08:00", Group = "Line 1 Settings")]
		public string Line1Time { get; set; }
		[Parameter("Line 1 Color", DefaultValue = MyLineColor.Blue, Group = "Line 1 Settings")]
		public MyLineColor Line1Color { get; set; }
		[Parameter("Line 1 Style", DefaultValue = MyLineStyle.Dashed, Group = "Line 1 Settings")]
		public MyLineStyle Line1Style { get; set; }
		[Parameter("Line 1 Thickness", DefaultValue = MyLineThickness.One, Group = "Line 1 Settings")]
		public MyLineThickness Line1Thickness { get; set; }


		[Parameter("Line 2 Time (HH:mm)", DefaultValue = "09:00", Group = "Line 2 Settings")]
		public string Line2Time { get; set; }
		[Parameter("Line 2 Color", DefaultValue = MyLineColor.Orange, Group = "Line 2 Settings")]
		public MyLineColor Line2Color { get; set; }
		[Parameter("Line 2 Style", DefaultValue = MyLineStyle.Dashed, Group = "Line 2 Settings")]
		public MyLineStyle Line2Style { get; set; }
		[Parameter("Line 2 Thickness", DefaultValue = MyLineThickness.One, Group = "Line 2 Settings")]
		public MyLineThickness Line2Thickness { get; set; }


		[Parameter("Line 3 Time (HH:mm)", DefaultValue = "10:30", Group = "Line 3 Settings")]
		public string Line3Time { get; set; }
		[Parameter("Line 3 Color", DefaultValue = MyLineColor.Grey, Group = "Line 3 Settings")]
		public MyLineColor Line3Color { get; set; }
		[Parameter("Line 3 Style", DefaultValue = MyLineStyle.Dashed, Group = "Line 3 Settings")]
		public MyLineStyle Line3Style { get; set; }
		[Parameter("Line 3 Thickness", DefaultValue = MyLineThickness.One, Group = "Line 3 Settings")]
		public MyLineThickness Line3Thickness { get; set; }


		[Parameter("Line 4 Time (HH:mm)", DefaultValue = "12:30", Group = "Line 4 Settings")]
		public string Line4Time { get; set; }
		[Parameter("Line 4 Color", DefaultValue = MyLineColor.Grey, Group = "Line 4 Settings")]
		public MyLineColor Line4Color { get; set; }
		[Parameter("Line 4 Style", DefaultValue = MyLineStyle.Dashed, Group = "Line 4 Settings")]
		public MyLineStyle Line4Style { get; set; }
		[Parameter("Line 4 Thickness", DefaultValue = MyLineThickness.One, Group = "Line 4 Settings")]
		public MyLineThickness Line4Thickness { get; set; }


		[Parameter("Line 5 Time (HH:mm)", DefaultValue = "14:00", Group = "Line 5 Settings")]
		public string Line5Time { get; set; }
		[Parameter("Line 5 Color", DefaultValue = MyLineColor.Red, Group = "Line 5 Settings")]
		public MyLineColor Line5Color { get; set; }
		[Parameter("Line 5 Style", DefaultValue = MyLineStyle.Dashed, Group = "Line 5 Settings")]
		public MyLineStyle Line5Style { get; set; }
		[Parameter("Line 5 Thickness", DefaultValue = MyLineThickness.One, Group = "Line 5 Settings")]
		public MyLineThickness Line5Thickness { get; set; }


		[Parameter("Line 6 Time (HH:mm)", DefaultValue = "15:00", Group = "Line 6 Settings")]
		public string Line6Time { get; set; }
		[Parameter("Line 6 Color", DefaultValue = MyLineColor.Red, Group = "Line 6 Settings")]
		public MyLineColor Line6Color { get; set; }
		[Parameter("Line 6 Style", DefaultValue = MyLineStyle.Dashed, Group = "Line 6 Settings")]
		public MyLineStyle Line6Style { get; set; }
		[Parameter("Line 6 Thickness", DefaultValue = MyLineThickness.One, Group = "Line 6 Settings")]
		public MyLineThickness Line6Thickness { get; set; }


		[Parameter("Line 7 Time (HH:mm)", DefaultValue = "15:30", Group = "Line 7 Settings")]
		public string Line7Time { get; set; }
		[Parameter("Line 7 Color", DefaultValue = MyLineColor.Red, Group = "Line 7 Settings")]
		public MyLineColor Line7Color { get; set; }
		[Parameter("Line 7 Style", DefaultValue = MyLineStyle.Dashed, Group = "Line 7 Settings")]
		public MyLineStyle Line7Style { get; set; }
		[Parameter("Line 7 Thickness", DefaultValue = MyLineThickness.One, Group = "Line 7 Settings")]
		public MyLineThickness Line7Thickness { get; set; }


		[Parameter("Line 8 Time (HH:mm)", DefaultValue = "17:00", Group = "Line 8 Settings")]
		public string Line8Time { get; set; }
		[Parameter("Line 8 Color", DefaultValue = MyLineColor.Purple, Group = "Line 8 Settings")]
		public MyLineColor Line8Color { get; set; }
		[Parameter("Line 8 Style", DefaultValue = MyLineStyle.Dashed, Group = "Line 8 Settings")]
		public MyLineStyle Line8Style { get; set; }
		[Parameter("Line 8 Thickness", DefaultValue = MyLineThickness.One, Group = "Line 8 Settings")]
		public MyLineThickness Line8Thickness { get; set; }

		private DateTime lastDrawnDay;
		private LineDrawer drawer;
		private List<LineDefinition> lines;

		protected override void Initialize()
		{
			if (TimeFrame > TimeFrameMapper.FromPossibleTimeFrames(ShowBelowTimeFrame)) return;

			lastDrawnDay = DateTime.MinValue;
			drawer = new LineDrawer(Chart, TimezoneOffsetMinutes);

			lines = new List<LineDefinition>
			{
				new LineDefinition(1, Line1Time, Line1Color, Line1Style, Line1Thickness),
				new LineDefinition(2, Line2Time, Line2Color, Line2Style, Line2Thickness),
				new LineDefinition(3, Line3Time, Line3Color, Line3Style, Line3Thickness),
				new LineDefinition(4, Line4Time, Line4Color, Line4Style, Line4Thickness),	
				new LineDefinition(5, Line5Time, Line5Color, Line5Style, Line5Thickness),
				new LineDefinition(6, Line6Time, Line6Color, Line6Style, Line6Thickness),
				new LineDefinition(7, Line7Time, Line7Color, Line7Style, Line7Thickness),
				new LineDefinition(8, Line8Time, Line8Color, Line8Style, Line8Thickness)
			};

			var todayUtc = Bars.OpenTimes.LastValue.Date;
			
			if (ShowHistory)
				DrawHistoricalLines();
			else
			{
				DrawLinesForDay(todayUtc, overwrite: true);
				lastDrawnDay = todayUtc;
			}					
		}

		public override void Calculate(int index)
		{
			if (index != Bars.Count - 1 || TimeFrame > TimeFrameMapper.FromPossibleTimeFrames(ShowBelowTimeFrame)) return;

			var todayUtc = Bars.OpenTimes[index].Date;

			if (todayUtc > lastDrawnDay &&
				todayUtc.DayOfWeek != DayOfWeek.Saturday &&
				todayUtc.DayOfWeek != DayOfWeek.Sunday)
			{
				DrawLinesForDay(todayUtc, overwrite: !ShowHistory);
				lastDrawnDay = todayUtc;
			}
		}

		private void DrawLinesForDay(DateTime dayUtc, bool overwrite)
		{
			foreach (var line in lines)
				drawer.Draw(line, dayUtc, overwrite);
		}

		private void DrawHistoricalLines()
		{
			DateTime lastDay = DateTime.MinValue;

			foreach (var barTime in Bars.OpenTimes)
			{
				var dayUtc = barTime.Date;
				if (dayUtc == lastDay) continue;

				if (dayUtc.DayOfWeek == DayOfWeek.Saturday || dayUtc.DayOfWeek == DayOfWeek.Sunday)
					continue;

				DrawLinesForDay(dayUtc, overwrite: false);
				lastDay = dayUtc;
			}
		}
	}
}
