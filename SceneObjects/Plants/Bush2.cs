using System;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace Midterm_Project.SceneObjects.Plants
{
    class Bush2
    {
        public ModelVisual3D myVisual;
        public Model3DGroup myModel;

        public Bush2(Point3D position)
        {
            // Create image brush
            BitmapImage plantTexture = new BitmapImage(new Uri(@"../../\Assets\Plants\plant2.png", UriKind.Relative));

            Plant newBush = new Plant(position, 2, 3, plantTexture);

            myVisual = newBush.myVisual;
            myModel = newBush.myModel;
        }
    }
}
