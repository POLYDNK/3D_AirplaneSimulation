using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace Midterm_Project.SceneObjects
{
    class ControlTower
    {
        public ModelVisual3D myVisual;
        public Model3DGroup myModel;

        public ControlTower(Point3D p1, double h)
        {
            myVisual = new ModelVisual3D();
            myModel = new Model3DGroup();

            // Create concrete brush
            ImageBrush concreteBrush = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri(@"../../\Assets\facadeTexture.jpg", UriKind.Relative)),
                Viewport = new Rect(0, 0, 1, 1),
                TileMode = TileMode.None
            };

            // Create glass brush
            ImageBrush glassBrush = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri(@"../../\Assets\glass-panel.jpg", UriKind.Relative)),
                Viewport = new Rect(0, 0, 1, 1),
                TileMode = TileMode.None
            };

            double r = 15;
            double rh = r / 2;

            // Hexagon Vertices
            Point3D v1 = new Point3D(p1.X + r, p1.Y, p1.Z);
            Point3D v2 = new Point3D(p1.X + rh, p1.Y + h, p1.Z + r);
            Point3D v3 = new Point3D(p1.X - rh, p1.Y, p1.Z + r);
            Point3D v4 = new Point3D(p1.X - r, p1.Y + h, p1.Z);
            Point3D v5 = new Point3D(p1.X - rh, p1.Y, p1.Z - r);
            Point3D v6 = new Point3D(p1.X + rh, p1.Y + h, p1.Z - r);

            // Hexagon Sides
            CubeSide wall1 = new CubeSide(v1, v2, concreteBrush);
            CubeSide wall2 = new CubeSide(v2, v3, concreteBrush);
            CubeSide wall3 = new CubeSide(v3, v4, concreteBrush);
            CubeSide wall4 = new CubeSide(v4, v5, concreteBrush);
            CubeSide wall5 = new CubeSide(v5, v6, concreteBrush);
            CubeSide wall6 = new CubeSide(v6, v1, concreteBrush);

            // Glass tower top
            Cube glassTop = new Cube(new Point3D(p1.X - r, p1.Y + h, p1.Z - r), new Point3D(p1.X + r, p1.Y + (h*1.6), p1.Z + r), glassBrush);

            // Combine all sides to hexagon
            myModel.Children.Add(wall1.GetModel());
            myModel.Children.Add(wall2.GetModel());
            myModel.Children.Add(wall3.GetModel());
            myModel.Children.Add(wall4.GetModel());
            myModel.Children.Add(wall5.GetModel());
            myModel.Children.Add(wall6.GetModel());
            myModel.Children.Add(glassTop.myModel);


            // Set visual to cube
            myVisual.Content = myModel;
        }
    }
}
