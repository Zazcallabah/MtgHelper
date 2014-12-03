using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using Image = System.Windows.Controls.Image;

namespace MtgHelper
{
	/// <summary>
	/// Interaction logic for ShowImage.xaml
	/// </summary>
	public partial class ShowImage : Window
	{
		public ShowImage( List<Bitmap> oldBitmap )
		{
			InitializeComponent();
			var w = 0;
			foreach( var b in oldBitmap )
			{
				w += b.Width + 6;
				var c = new Canvas { Margin = new Thickness( 3 ), Width = b.Width, Height = b.Height };
				c.Children.Add( new Image()
				{
					Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
						b.GetHbitmap( System.Drawing.Color.Transparent ),
						IntPtr.Zero,
						new Int32Rect( 0, 0, b.Width, b.Height ),
						null ),
					Width = b.Width,
					Height = b.Height
				} );
				wrp.Children.Add( c );

				//	this.Width = cnvImage.Width = imgPreview.Width = oldBitmap.Width + 100;
				//	Height = cnvImage.Height = imgPreview.Height = oldBitmap.Height + 100;
			}
			wrp.Width = 800;
			//			wrp.Width = w;
		}
	}
}
