using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace Midterm_Project
{
    public class Runway
    {
        public GeometryModel3D myModel;
        public ModelVisual3D myVisual;
        public MeshGeometry3D myMesh;

        public Runway(Point3D p1, Point3D p2)
        {
            // Create Image Brush
            ImageBrush myBrush = new ImageBrush();
            myBrush.ImageSource = new BitmapImage(new Uri(@"../../\Assets\roadTexture.jpg", UriKind.Relative));
            myBrush.Viewport = new Rect(0, 0, 0.5, 0.001);
            myBrush.TileMode = TileMode.Tile;

            // Use a CubeTop as a runway w/ created brush
            CubeTop runway = new CubeTop(p1, p2, myBrush);
            myModel = runway.myModel;
            myVisual = runway.myVisual;
            myMesh = runway.myMesh;
        }

        public ModelVisual3D GetVisual()
        {
            return myVisual;
        }
    }
}
