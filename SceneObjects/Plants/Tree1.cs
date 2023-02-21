using System;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace Midterm_Project.SceneObjects.Plants
{
    class Tree1
    {
        public ModelVisual3D myVisual;
        public Model3DGroup myModel;

        public Tree1(Point3D position)
        {
            // Create image brush
            BitmapImage plantTexture = new BitmapImage(new Uri(@"../../\Assets\Plants\tree1.png", UriKind.Relative));

            Plant newBush = new Plant(position, 10, 10, plantTexture);

            myVisual = newBush.myVisual;
            myModel = newBush.myModel;
        }
    }
}