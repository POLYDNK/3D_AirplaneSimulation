using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace Midterm_Project
{
    class Building
    {
        public ModelVisual3D myVisual;
        public Model3DGroup myModel;
        private Point3D p1;
        private Point3D p2;

        public Building(Point3D orgP1, Point3D orgP2)
        {
            myVisual = new ModelVisual3D();
            myModel = new Model3DGroup();
            BitmapImage buildingSideTexture = new BitmapImage(new Uri(@"../../\Assets\BuildingTexture.jpg", UriKind.Relative));

            // Save points and ensure p1 < p2 for x and z, and, p1 > p2 for y
            p1 = orgP1;
            p2 = orgP2;
            double temp;
            if (p1.X > p2.X) { temp = p1.X; p1.X = p2.X; p2.X = temp; }
            if (p1.Y < p2.Y) { temp = p1.Y; p1.Y = p2.Y; p2.Y = temp; }
            if (p1.Z > p2.Z) { temp = p1.Z; p1.Z = p2.Z; p2.Z = temp; }

            // Cube Vertices (all six faces can be defined w/ four vertices)
            Point3D v1 = p1;
            Point3D v2 = new Point3D(p2.X, p2.Y, p1.Z);
            Point3D v3 = new Point3D(p2.X, p1.Y, p2.Z);
            Point3D v4 = new Point3D(p1.X, p2.Y, p2.Z);

            // Cube Sides
            CubeSide wall1 = new CubeSide(v1, v2, buildingSideTexture);
            CubeSide wall2 = new CubeSide(v2, v3, buildingSideTexture);
            CubeSide wall3 = new CubeSide(v3, v4, buildingSideTexture);
            CubeSide wall4 = new CubeSide(v4, v1, buildingSideTexture);
            CubeTop ceiling = new CubeTop(v1, v3, Color.FromRgb(100, 100, 100));

            // Combine all sides to cube
            myModel.Children.Add(wall1.GetModel());
            myModel.Children.Add(wall2.GetModel());
            myModel.Children.Add(wall3.GetModel());
            myModel.Children.Add(wall4.GetModel());
            myModel.Children.Add(ceiling.GetModel());

            // Set visual to cube
            myVisual.Content = myModel;
        }

        public bool IfCollision(Point3D collisionPoint)
        {
            bool collision = false;

            if ((collisionPoint.X >= p1.X) & (collisionPoint.X <= p2.X))
            {
                if ((collisionPoint.Y >= p2.Y) & (collisionPoint.Y <= p1.Y))
                {
                    if ((collisionPoint.Z >= p1.Z) & (collisionPoint.Z <= p2.Z))
                    {
                        collision = true;
                    }
                }
            }

            return collision;
        }
    }
}
