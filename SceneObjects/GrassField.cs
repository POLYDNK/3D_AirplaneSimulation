using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace Midterm_Project
{
    public class GrassField
    {
        public GeometryModel3D myModel;
        public ModelVisual3D myVisual;
        public MeshGeometry3D myMesh;

        public GrassField(Point3D p1, Point3D p2)
        {
            // Create Image Brush
            ImageBrush myBrush = new ImageBrush();
            myBrush.ImageSource = new BitmapImage(new Uri(@"../../\Assets\grass-lawn-texture.jpg", UriKind.Relative));
            myBrush.Viewport = new Rect(0, 0, 0.01, 0.01);
            myBrush.TileMode = TileMode.Tile;

            // Use a CubeTop as a grass field w/ created brush
            CubeTop grassField = new CubeTop(p1, p2, myBrush);
            myModel = grassField.myModel;
            myVisual = grassField.myVisual;
            myMesh = grassField.myMesh;
        }

        public ModelVisual3D GetVisual()
        {
            return myVisual;
        }
    }
}
