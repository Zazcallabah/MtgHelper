using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using Brushes = System.Windows.Media.Brushes;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace MtgHelper
{
	/// <summary>
	/// Interaction logic for Calibrator.xaml
	/// </summary>
	public partial class Calibrator : Window
	{
		public Calibrator()
		{
			InitializeComponent();
		}

		Rectangle rectSelectArea = new Rectangle
		{
			Stroke = Brushes.LightBlue,
			StrokeThickness = 2
		};
		object _lock = new object();
		int state = 0;

		public CalibrationController Controller
		{
			get
			{
				return (CalibrationController) DataContext;
			}
			set
			{
				DataContext = value;
			}
		}

		void MouseDown( object sender, MouseButtonEventArgs e )
		{
			lock( _lock )
			{
				state = 0;
				Controller.Down = e.GetPosition( cnvImage );

				cnvImage.Children.Remove( rectSelectArea );


				Canvas.SetLeft( rectSelectArea, Controller.Down.X );
				Canvas.SetTop( rectSelectArea, Controller.Down.X );
				cnvImage.Children.Add( rectSelectArea );
				state = 1;
			}
		}

		void MouseMove( object sender, MouseEventArgs e )
		{
			lock( _lock )
			{
				if( state != 1 )
					return;

				Controller.Up = e.GetPosition( cnvImage );

				RedrawRectangle();
			}
		}

		void RedrawRectangle()
		{
			var x = Math.Min( Controller.Up.X, Controller.Down.X );
			var y = Math.Min( Controller.Up.Y, Controller.Down.Y );

			var w = Math.Max( Controller.Up.X, Controller.Down.X ) - x;
			var h = Math.Max( Controller.Up.Y, Controller.Down.Y ) - y;

			rectSelectArea.Width = Math.Abs( w - 2 );
			rectSelectArea.Height = Math.Abs( h - 2 );

			Canvas.SetLeft( rectSelectArea, x );
			Canvas.SetTop( rectSelectArea, y );
		}


		void ButtonBase_OnClick( object sender, RoutedEventArgs e )
		{
			Close();
		}

		void ImgPreview_OnPreviewMouseUp( object sender, MouseButtonEventArgs e )
		{
			lock( _lock )
			{
				if( state != 1 )
					return;

				Controller.Up = e.GetPosition( cnvImage );
				Controller.Adjust();
				RedrawRectangle();
				state = 2;
			}
		}

		void ToggleShots( object sender, RoutedEventArgs e )
		{
			throw new NotImplementedException();
		}
	}
}
