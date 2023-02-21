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

namespace Midterm_Project.SceneObjects
{
    class Skybox
    {
        public ModelVisual3D myVisual;
        public Model3DGroup myModel;

        public Skybox(Point3D p1, Point3D p2)
        {
            myVisual = new ModelVisual3D();
            myModel = new Model3DGroup();
            BitmapImage skyboxTexture = new BitmapImage(new Uri(@"../../\Assets\sky-with-clouds-texture2000.jpg", UriKind.Relative));

            // Cube Vertices (all six faces can be defined w/ four vertices)
            Point3D v1 = p1;
            Point3D v2 = new Point3D(p2.X, p2.Y, p1.Z);
            Point3D v3 = new Point3D(p2.X, p1.Y, p2.Z);
            Point3D v4 = new Point3D(p1.X, p2.Y, p2.Z);

            // Cube Sides
            CubeSide side1 = new CubeSide(v1, v2, skyboxTexture);
            CubeSide side2 = new CubeSide(v2, v3, skyboxTexture);
            CubeSide side3 = new CubeSide(v3, v4, skyboxTexture);
            CubeSide side4 = new CubeSide(v4, v1, skyboxTexture);
            CubeTop top = new CubeTop(v2, v4, skyboxTexture);
            CubeTop bottom = new CubeTop(v1, v3, skyboxTexture);

            // Combine all sides to cube
            myModel.Children.Add(side1.GetModel());
            myModel.Children.Add(side2.GetModel());
            myModel.Children.Add(side3.GetModel());
            myModel.Children.Add(side4.GetModel());
            myModel.Children.Add(top.GetModel());
            myModel.Children.Add(bottom.GetModel());

            // Set visual to cube
            myVisual.Content = myModel;
        }
    }
}
