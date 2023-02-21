using Midterm_Project.SceneObjects;
using Midterm_Project.SceneObjects.Plants;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Midterm_Project
{
    class GreenCity
    {
        public ModelVisual3D myVisual;
        public Model3DGroup myModel;

        public GreenCity()
        {
            myVisual = new ModelVisual3D();
            myModel = new Model3DGroup();

            // Scene building
            Model3DGroup lightingModels = new Model3DGroup();
            ModelVisual3D lightingVisuals = new ModelVisual3D();

            DirectionalLight sunlight = new DirectionalLight(Color.FromRgb(201, 255, 229), new Vector3D(0, -1, 0));
            sunlight.Transform = new TranslateTransform3D(0, 5100, 0);

            DirectionalLight ambient = new DirectionalLight(Color.FromRgb(201, 255, 229), new Vector3D(0, 1, 0));
            ambient.Transform = new TranslateTransform3D(0, 0, 0);

            GrassField ground3 = new GrassField(new Point3D(-1800, 0, -1000), new Point3D(-2500, 0, 1500));
            GrassField ground1 = new GrassField(new Point3D(-1790, 0, -1000), new Point3D(-5, 0, 1500));
            GrassField ground2 = new GrassField(new Point3D(5, 0, -1000), new Point3D(500, 0, 1500));

            BushField bushfield1 = new BushField(new Point3D(10, 0, -1000), new Point3D(500, 0, 1500), 200);
            BushField bushfield2 = new BushField(new Point3D(-10, 0, -1000), new Point3D(-600, 0, 1500), 100);
            BushField bushfield3 = new BushField(new Point3D(-600, 0, 300), new Point3D(-900, 0, 1500), 100);
            BushField bushfield4 = new BushField(new Point3D(-900, 0, -1000), new Point3D(-1780, 0, 1500), 100);

            Runway runway1 = new Runway(new Point3D(-5, 0, -1000), new Point3D(5, 0, 1500));
            Runway runway2 = new Runway(new Point3D(-1800, 0, -1000), new Point3D(-1790, 0, 1500));

            ControlTower tower = new ControlTower(new Point3D(-1000, 0, 0), 30);

            Airport airport = new Airport(new Point3D(-600, 0.1, -300), new Point3D(-900, 0.1, 300));

            Building b1 = new Building(new Point3D(200, 200, 500), new Point3D(100, 0, 400));
            Building b2 = new Building(new Point3D(200, 100, 1000), new Point3D(100, 0, 900));
            Building b3 = new Building(new Point3D(-300, 200, 900), new Point3D(-200, 0, 800));

            Building s1 = new Building(new Point3D(-2000, 400, 1200), new Point3D(-1900, 0, 1300));
            Building s2 = new Building(new Point3D(-2000, 500, 800), new Point3D(-1900, 0, 900));
            Building s3 = new Building(new Point3D(-2000, 600, 500), new Point3D(-1900, 0, 400));
            Building s4 = new Building(new Point3D(-2000, 100, 0), new Point3D(-1900, 0, 100));
            Building s5 = new Building(new Point3D(-2000, 200, -400), new Point3D(-1900, 0, -500));

            Building s6 = new Building(new Point3D(-1690, 100, 1400), new Point3D(-1590, 0, 1300));
            Building s7 = new Building(new Point3D(-1690, 250, 400), new Point3D(-1590, 0, 300));
            Building s8 = new Building(new Point3D(-1690, 350, -600), new Point3D(-1590, 0, -700));
            Building s9 = new Building(new Point3D(-1690, 100, -900), new Point3D(-1590, 0, -1000));

            Bush1 bsh1 = new Bush1(new Point3D(20, 0, 20));
            Bush2 bsh2 = new Bush2(new Point3D(20, 0, 25));

            Tree1 tree1 = new Tree1(new Point3D(20, 0, 30));
            Tree2 tree2 = new Tree2(new Point3D(-20, 0, 35));

            Skybox skyBox = new Skybox(new Point3D(-2500, -2500, -2500), new Point3D(2500, 2500, 2500));

            // Add objects to viewport
            lightingModels.Children.Add(sunlight);
            lightingModels.Children.Add(ambient);
            lightingVisuals.Content = lightingModels;
            myVisual.Children.Add(lightingVisuals);
            myVisual.Children.Add(ground1.myVisual);
            myVisual.Children.Add(ground2.myVisual);
            myVisual.Children.Add(ground3.myVisual);
            myVisual.Children.Add(runway1.myVisual);
            myVisual.Children.Add(runway2.myVisual);
            myVisual.Children.Add(tower.myVisual);
            myVisual.Children.Add(airport.myVisual);
            myVisual.Children.Add(b1.myVisual);
            myVisual.Children.Add(b2.myVisual);
            myVisual.Children.Add(b3.myVisual);
            myVisual.Children.Add(s1.myVisual);
            myVisual.Children.Add(s2.myVisual);
            myVisual.Children.Add(s3.myVisual);
            myVisual.Children.Add(s4.myVisual);
            myVisual.Children.Add(s5.myVisual);
            myVisual.Children.Add(s6.myVisual);
            myVisual.Children.Add(s7.myVisual);
            myVisual.Children.Add(s8.myVisual);
            myVisual.Children.Add(s9.myVisual);
            myVisual.Children.Add(bsh1.myVisual);
            myVisual.Children.Add(bsh2.myVisual);
            myVisual.Children.Add(tree1.myVisual);
            myVisual.Children.Add(tree2.myVisual);
            myVisual.Children.Add(bushfield1.myVisual);
            myVisual.Children.Add(bushfield2.myVisual);
            myVisual.Children.Add(bushfield3.myVisual);
            myVisual.Children.Add(bushfield4.myVisual);
            myVisual.Children.Add(skyBox.myVisual);
        }
    }
}
