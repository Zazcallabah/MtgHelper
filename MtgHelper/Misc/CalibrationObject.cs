using System;
using System.Drawing;

namespace MtgHelper.Misc
{
	public class CalibrationObject
	{
		public Point Topleft { get; private set; }
		public Point Bottomright { get; private set; }

		public CalibrationObject( CalibrationController controller )
		{
			var u = controller.Up;
			var d = controller.Down;
			Topleft = new Point( (int) Math.Min( u.X, d.X ), (int) Math.Min( u.Y, d.Y ) );
			Bottomright = new Point( (int) Math.Max( u.X, d.X ), (int) Math.Max( u.Y, d.Y ) );
		}
	}
}