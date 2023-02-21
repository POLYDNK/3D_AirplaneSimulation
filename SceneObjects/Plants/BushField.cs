using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace Midterm_Project.SceneObjects.Plants
{
    class BushField
    {
        public ModelVisual3D myVisual;
        public Model3DGroup myModel;

        public BushField(Point3D p1, Point3D p2, int count)
        {
            myVisual = new ModelVisual3D();
            myModel = new Model3DGroup();

            Bush1 tempB1;
            Bush2 tempB2;
            Tree1 tempT1;
            Tree2 tempT2;

            Random r = new Random();
            double xRange = p2.X - p1.X;
            double zRange = p2.Z - p1.Z;
            int rType;
            double rX, rZ;

            for (int i = 0; i < count; i++)
            {
                // Randomize Bush Type
                rType = r.Next(0, 4);

                // Randomize Bush Location
                rX = r.NextDouble() * xRange;
                rZ = r.NextDouble() * zRange;

                switch(rType)
                {
                    case 0:
                        tempB1 = new Bush1(new Point3D(p1.X + rX, p1.Y, p1.Z + rZ));
                        myModel.Children.Add(tempB1.myModel);
                        break;

                    case 1:
                        tempB2 = new Bush2(new Point3D(p1.X + rX, p1.Y, p1.Z + rZ));
                        myModel.Children.Add(tempB2.myModel);
                        break;

                    case 2:
                        tempT1 = new Tree1(new Point3D(p1.X + rX, p1.Y, p1.Z + rZ));
                        myModel.Children.Add(tempT1.myModel);
                        break;

                    case 3:
                        tempT2 = new Tree2(new Point3D(p1.X + rX, p1.Y, p1.Z + rZ));
                        myModel.Children.Add(tempT2.myModel);
                        break;

                    default:
                        break;
                }
            }

            // Set visual to bushes
            myVisual.Content = myModel;
        }
    }
}
