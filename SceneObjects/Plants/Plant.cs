using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace Midterm_Project.SceneObjects.Plants
{
    class Plant
    {
        public ModelVisual3D myVisual;
        public Model3DGroup myModel;

        public Plant(Point3D position, double width, double height, BitmapImage image)
        {
            myVisual = new ModelVisual3D();
            myModel = new Model3DGroup();

            // Create image brush
            ImageBrush myBrush = new ImageBrush
            {
                ImageSource = image,
                Viewport = new Rect(0, 0, 1, 1),
                TileMode = TileMode.None
            };

            // Plant Vertices
            Point3D v1 = new Point3D(position.X - width / 2, position.Y + height, position.Z);
            Point3D v2 = new Point3D(position.X + width / 2, position.Y, position.Z);

            Point3D v3 = new Point3D(position.X, position.Y + height, position.Z - width / 2);
            Point3D v4 = new Point3D(position.X, position.Y, position.Z + width / 2);

            // Plant images
            CubeSide image1 = new CubeSide(v1, v2, myBrush);
            CubeSide image2 = new CubeSide(v3, v4, myBrush);

            // Combine all images
            myModel.Children.Add(image1.GetModel());
            myModel.Children.Add(image2.GetModel());

            // Set visual to plant
            myVisual.Content = myModel;
        }
    }
}
