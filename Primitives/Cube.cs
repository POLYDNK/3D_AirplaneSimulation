using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Midterm_Project
{
    public class Cube
    {
        public ModelVisual3D myVisual;
        public Model3DGroup myModel;

        public Cube(Point3D p1, Point3D p2, Color color)
        {
            myVisual = new ModelVisual3D();
            myModel = new Model3DGroup();

            // Cube Vertices (all six faces can be defined w/ four vertices)
            Point3D v1 = p1;
            Point3D v2 = new Point3D(p2.X, p2.Y, p1.Z);
            Point3D v3 = new Point3D(p2.X, p1.Y, p2.Z);
            Point3D v4 = new Point3D(p1.X, p2.Y, p2.Z);

            // Cube Sides
            CubeSide wall1 = new CubeSide(v1, v2, color);
            CubeSide wall2 = new CubeSide(v2, v3, color);
            CubeSide wall3 = new CubeSide(v3, v4, color);
            CubeSide wall4 = new CubeSide(v4, v1, color);
            CubeTop ceiling = new CubeTop(v1, v3, color);
            CubeTop floor = new CubeTop(v2, v4, color);

            // Combine all sides to cube
            myModel.Children.Add(wall1.GetModel());
            myModel.Children.Add(wall2.GetModel());
            myModel.Children.Add(wall3.GetModel());
            myModel.Children.Add(wall4.GetModel());
            myModel.Children.Add(ceiling.GetModel());
            myModel.Children.Add(floor.GetModel());

            // Set visual to cube
            myVisual.Content = myModel;
        }

        public Cube(Point3D p1, Point3D p2, ImageBrush image)
        {
            myVisual = new ModelVisual3D();
            myModel = new Model3DGroup();

            // Cube Vertices (all six faces can be defined w/ four vertices)
            Point3D v1 = p1;
            Point3D v2 = new Point3D(p2.X, p2.Y, p1.Z);
            Point3D v3 = new Point3D(p2.X, p1.Y, p2.Z);
            Point3D v4 = new Point3D(p1.X, p2.Y, p2.Z);

            // Cube Sides
            CubeSide wall1 = new CubeSide(v1, v2, image);
            CubeSide wall2 = new CubeSide(v2, v3, image);
            CubeSide wall3 = new CubeSide(v3, v4, image);
            CubeSide wall4 = new CubeSide(v4, v1, image);
            CubeTop ceiling = new CubeTop(v1, v3, image);
            CubeTop floor = new CubeTop(v2, v4, image);

            // Combine all sides to cube
            myModel.Children.Add(wall1.GetModel());
            myModel.Children.Add(wall2.GetModel());
            myModel.Children.Add(wall3.GetModel());
            myModel.Children.Add(wall4.GetModel());
            myModel.Children.Add(ceiling.GetModel());
            myModel.Children.Add(floor.GetModel());

            // Set visual to cube
            myVisual.Content = myModel;
        }
    }
}
