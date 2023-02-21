using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace Midterm_Project.SceneObjects
{
    class Airport
    {
        public GeometryModel3D myModel;
        public ModelVisual3D myVisual;
        public MeshGeometry3D myMesh;

        public Airport(Point3D p1, Point3D p2)
        {
            // Create Image Brush
            ImageBrush myBrush = new ImageBrush();
            myBrush.ImageSource = new BitmapImage(new Uri(@"../../\Assets\AirportRunwayMini.jpg", UriKind.Relative));
            myBrush.Viewport = new Rect(0, 0, 1, 1);
            myBrush.TileMode = TileMode.None;

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
