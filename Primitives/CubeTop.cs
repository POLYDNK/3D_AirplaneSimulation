using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace Midterm_Project
{
    class CubeTop
    {
        public GeometryModel3D myModel;
        public ModelVisual3D myVisual;
        public MeshGeometry3D myMesh;
        public DiffuseMaterial myMaterial;

        /// <summary>
        /// Define a top or bottom of a cube in 3D space. This cube side is drawn from point p1 to point p2.
        /// </summary>
        /// <param name="p1">Point to start drawing from</param>
        /// <param name="p2">Point to draw to</param>
        /// <param name="color">Color of myMesh</param>
        public CubeTop(Point3D p1, Point3D p2, Color color)
        {
            // Create model
            CreateModel(p1, p2);
            myModel.Geometry = myMesh;

            // Create solid color brush
            SolidColorBrush myBrush = new SolidColorBrush(color);

            // Create new material from brush then apply it to the model
            myMaterial = new DiffuseMaterial(myBrush);
            myModel.Material = myMaterial;
            myModel.BackMaterial = myMaterial;
            myVisual.Content = myModel;
        }

        /// <summary>
        /// Define a top or bottom of a cube in 3D space. This cube side is drawn from point p1 to point p2.
        /// </summary>
        /// <param name="p1">Point to start drawing from</param>
        /// <param name="p2">Point to draw to</param>
        /// <param name="image">Image texture of myMesh</param>
        public CubeTop(Point3D p1, Point3D p2, BitmapImage image)
        {
            // Create Model
            CreateModel(p1, p2);
            myModel.Geometry = myMesh;

            // Create image brush
            ImageBrush myBrush = new ImageBrush();
            myBrush.ImageSource = image;
            myBrush.Viewport = new Rect(0, 0, 0.2, 0.1);
            myBrush.TileMode = TileMode.Tile;

            // Create new material from brush then apply it to the model
            myMaterial = new DiffuseMaterial(myBrush);
            myModel.Material = myMaterial;
            myModel.BackMaterial = myMaterial;
            myVisual.Content = myModel;
        }

        /// <summary>
        /// Define a top or bottom of a cube in 3D space. This cube side is drawn from point p1 to point p2.
        /// </summary>
        /// <param name="p1">Point to start drawing from</param>
        /// <param name="p2">Point to draw to</param>
        /// <param name="myBrush">Brush to paint myMesh</param>
        public CubeTop(Point3D p1, Point3D p2, ImageBrush myBrush)
        {
            // Create Model
            CreateModel(p1, p2);
            myModel.Geometry = myMesh;

            // Create new material from brush then apply it to the model
            myMaterial = new DiffuseMaterial(myBrush);
            myModel.Material = myMaterial;
            myModel.BackMaterial = myMaterial;
            myVisual.Content = myModel;
        }

        public void CreateModel(Point3D p1, Point3D p2)
        {
            // Create new Model, Visual, and Meash
            myModel = new GeometryModel3D();
            myVisual = new ModelVisual3D();
            myMesh = new MeshGeometry3D();

            // Add positions based on p1 and p2
            Point3D v1 = new Point3D(p1.X, p1.Y, p1.Z);
            Point3D v2 = new Point3D(p2.X, p2.Y, p1.Z);
            Point3D v3 = new Point3D(p1.X, p1.Y, p2.Z);
            Point3D v4 = new Point3D(p2.X, p2.Y, p2.Z);

            myMesh.Positions.Add(v1); // Triangle 1, v1
            myMesh.Positions.Add(v2); // Triangle 1, v2
            myMesh.Positions.Add(v3); // Triangle 1, v3

            myMesh.Positions.Add(v2); // Triangle 2, v1
            myMesh.Positions.Add(v3); // Triangle 2, v2
            myMesh.Positions.Add(v4); // Triangle 2, v3

            myMesh.TriangleIndices.Add(0);
            myMesh.TriangleIndices.Add(1);
            myMesh.TriangleIndices.Add(2);
            myMesh.TriangleIndices.Add(3);
            myMesh.TriangleIndices.Add(4);
            myMesh.TriangleIndices.Add(5);

            myMesh.Normals.Add(new Vector3D(0, 1, 0));
            myMesh.Normals.Add(new Vector3D(0, 1, 0));
            myMesh.Normals.Add(new Vector3D(0, 1, 0));
            myMesh.Normals.Add(new Vector3D(0, 1, 0));
            myMesh.Normals.Add(new Vector3D(0, 1, 0));
            myMesh.Normals.Add(new Vector3D(0, 1, 0));

            myMesh.TextureCoordinates.Add(new Point(0, 0));
            myMesh.TextureCoordinates.Add(new Point(1, 0));
            myMesh.TextureCoordinates.Add(new Point(0, 1));
            myMesh.TextureCoordinates.Add(new Point(1, 0));
            myMesh.TextureCoordinates.Add(new Point(0, 1));
            myMesh.TextureCoordinates.Add(new Point(1, 1));
        }

        public ModelVisual3D GetVisual()
        {
            return myVisual;
        }

        public MeshGeometry3D GetGeometry()
        {
            return myMesh;
        }

        public GeometryModel3D GetModel()
        {
            return myModel;
        }
    }
}
