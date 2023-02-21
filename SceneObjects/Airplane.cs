/// @name: Airplane.cs
/// @author: Bryson Squibb, 5003954275
/// @date: 11/28/2021
/// @description: This cs file initializes an airplane object
/// to be used in a 3D environment.

using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.IO;

namespace Midterm_Project
{
    public class Airplane
    {
        public ModelVisual3D myVisual;
        public Model3DGroup myModel;

        // Universal constants
        private const double AIRDENSITY = 1.17; // air density isn't really constant, but it will do for now
        private const double WINGAREA = 27 * 2; // wing area

        // Parts of the airplane
        Model3DGroup hull;
        Model3DGroup propeller;
        CubeTop aileron1;
        CubeTop aileron2;
        CubeTop elevator;
        CubeSide rudder;

        // x,y,z vectors
        Vector3D vx = new Vector3D(5, 0, 0);
        Vector3D vy = new Vector3D(0, 5, 0);
        Vector3D vz = new Vector3D(0, 0, 5);

        // Movement Vectors
        readonly static Vector3D orgForwardDir = new Vector3D(0, 0, 1);
        readonly static Vector3D orgUpwardDir = new Vector3D(0, 1, 0);
        public Vector3D forwardDirection = orgForwardDir;
        public Vector3D upwardDirection;
        public Vector3D movementVector = new Vector3D(0, 0, 0);
        public Vector3D movementDirection = new Vector3D(0, 0, 0);

        // Position and Orientation
        private double orgX = 0;
        private double orgY = 5;
        private double orgZ = 0;
        public double x = 0;
        public double y = 0;
        public double z = 0;
        public double angX = 0;
        public double angY = 0;
        public double angZ = 0;

        // Movement
        public double maxSpeed = 2.5;
        public double linearSpeed = 0;
        public double forwardSpeed = 0;
        public double upwardSpeed = 0;
        public double ySpeed = 0;
        public double yDelta = 0;
        public double gravityAccel = 0.153125;
        public double wingSpeed;

        // Physics Properties
        readonly double mass = 1100; // kg
        public double propellerSpeed = 0;

        // Control Surface Angles
        private double rudderAngle = 0;
        private double aileronAngle = 0;
        private double elevatorAngle = 0;
        private double maxRudderAngle = 45;
        private double maxAileronAngle = 25;
        private double maxElevatorAngle = 25;

        // Translation
        public TranslateTransform3D translateTransform3D = new TranslateTransform3D();

        // Rotation
        Quaternion q3d = new Quaternion();
        Quaternion rudderQ = new Quaternion();
        Quaternion propellerQ = new Quaternion();
        Quaternion aileron1Q = new Quaternion();
        Quaternion aileron2Q = new Quaternion();
        Quaternion elevatorQ = new Quaternion();
        public QuaternionRotation3D qRotation;
        QuaternionRotation3D rRotation;
        QuaternionRotation3D pRotation;
        QuaternionRotation3D a1Rotation;
        QuaternionRotation3D a2Rotation;
        QuaternionRotation3D eRotation;
        public RotateTransform3D bodyRotation = new RotateTransform3D();
        RotateTransform3D rudderRotation = new RotateTransform3D();
        RotateTransform3D propellerRotation = new RotateTransform3D();
        RotateTransform3D aileronRotation1 = new RotateTransform3D();
        RotateTransform3D aileronRotation2 = new RotateTransform3D();
        RotateTransform3D elevatorRotation = new RotateTransform3D();  

        // Other vars
        public bool replayMode = false;
        FileStream fs;
        public int inputs = 0;
        List<int> recordingData = new List<int>();
        int playMarker = 0;

        /// <summary>
        /// Airplane constructor
        /// </summary>
        /// <param name="spawn">Spawn point in 3D space</param>
        /// <param name="color">Color of airplane</param>
        public Airplane(Point3D spawn, Color color)
        {
            // Save original spawn point
            orgX = spawn.X;
            orgY = spawn.Y;
            orgZ = spawn.Z;

            // Move airplane to spawn
            x = orgX;
            y = orgY;
            z = orgZ;

            CreateAirplaneModel(color);
            initializeTransformations();
        }

        /// <summary>
        /// Create both the airplane model and visual out of primitive shapes.
        /// </summary>
        /// <param name="color">Color to paint the airplane.</param>
        private void CreateAirplaneModel(Color color)
        {
            myVisual = new ModelVisual3D();
            myModel = new Model3DGroup();
            hull = new Model3DGroup();
            propeller = new Model3DGroup();

            // FuseLage
            Point3D fp1 = new Point3D(-1, +1, +4);
            Point3D fp2 = new Point3D(+1, 0, -4);
            Cube fuseLage = new Cube(fp1, fp2, color);

            // Cockpit
            Point3D cp1 = new Point3D(-0.5, 1.6, 3);
            Point3D cp2 = new Point3D(+0.5, 1, 0);
            Cube cockpit = new Cube(cp1, cp2, Color.FromRgb(255, 255, 255));

            // Tail
            Point3D tp1 = new Point3D(-0.5, +1, -4);
            Point3D tp2 = new Point3D(+0.5, 0.4, -7);
            Cube tail = new Cube(tp1, tp2, color);

            // WingL
            Point3D wrp1 = new Point3D(+8, +0.5, +3);
            Point3D wrp2 = new Point3D(+1, +0.5, 0);
            CubeTop wingR = new CubeTop(wrp1, wrp2, color);

            // WingR
            Point3D wlp1 = new Point3D(-8, +0.5, +3);
            Point3D wlp2 = new Point3D(-1, +0.5, 0);
            CubeTop wingL = new CubeTop(wlp1, wlp2, color);

            // Rudder
            Point3D rp1 = new Point3D(0, 3, -7);
            Point3D rp2 = new Point3D(0, 0.4, -7.6);
            rudder = new CubeSide(rp1, rp2, Color.FromRgb(255, 145, 0));

            // Propeller
            Point3D pp1 = new Point3D(-0.1, +0.6, +5.5);
            Point3D pp2 = new Point3D(+0.1, +0.4, +4);
            Cube propellerPost = new Cube(pp1, pp2, color);
            Point3D b1p1 = new Point3D(-2, +0.7, +5.5);
            Point3D b1p2 = new Point3D(0, +0.3, +5.5);
            CubeSide propellerBlade1 = new CubeSide(b1p1, b1p2, color);
            Point3D b2p1 = new Point3D(0, +0.7, +5.5);
            Point3D b2p2 = new Point3D(2, +0.3, +5.5);
            CubeSide propellerBlade2 = new CubeSide(b2p1, b2p2, color);

            // Aileron
            Point3D a1p1 = new Point3D(+8, +0.5, 0);
            Point3D a1p2 = new Point3D(+1, +0.5, -1);
            aileron1 = new CubeTop(a1p1, a1p2, Color.FromRgb(0, 0, 255));
            Point3D a2p1 = new Point3D(-8, +0.5, 0);
            Point3D a2p2 = new Point3D(-1, +0.5, -1);
            aileron2 = new CubeTop(a2p1, a2p2, Color.FromRgb(0, 0, 255));

            // Elevator
            Point3D e1p1 = new Point3D(-3, +0.5, -7);
            Point3D e1p2 = new Point3D(3, +0.5, -7.6);
            elevator = new CubeTop(e1p1, e1p2, Color.FromRgb(0, 255, 0));

            // Hull
            hull.Children.Add(fuseLage.myModel);
            hull.Children.Add(cockpit.myModel);
            hull.Children.Add(tail.myModel);
            hull.Children.Add(wingL.myModel);
            hull.Children.Add(wingR.myModel);

            // Propeller
            propeller.Children.Add(propellerPost.myModel);
            propeller.Children.Add(propellerBlade1.myModel);
            propeller.Children.Add(propellerBlade2.myModel);

            // Add all models to visual group
            myModel.Children.Add(hull);
            myModel.Children.Add(rudder.myModel);
            myModel.Children.Add(propeller);
            myModel.Children.Add(aileron1.myModel);
            myModel.Children.Add(aileron2.myModel);
            myModel.Children.Add(elevator.myModel);

            // Set visual to cube
            myVisual.Content = myModel;
        }

        /// <summary>
        /// Initialize the transforms of the body and control surfaces of the airplane.
        /// </summary>
        private void initializeTransformations()
        {
            // Translation transformation setup
            translateTransform3D.OffsetX = x;
            translateTransform3D.OffsetY = y;
            translateTransform3D.OffsetZ = z;

            // Rotation transformation setup
            qRotation = new QuaternionRotation3D(q3d);
            bodyRotation = new RotateTransform3D(qRotation);

            rRotation = new QuaternionRotation3D(rudderQ);
            rudderRotation = new RotateTransform3D(rRotation);

            eRotation = new QuaternionRotation3D(elevatorQ);
            elevatorRotation = new RotateTransform3D(eRotation);

            pRotation = new QuaternionRotation3D(propellerQ);
            propellerRotation = new RotateTransform3D(pRotation);

            a1Rotation = new QuaternionRotation3D(aileron1Q);
            aileronRotation1 = new RotateTransform3D(a1Rotation);

            a2Rotation = new QuaternionRotation3D(aileron2Q);
            aileronRotation2 = new RotateTransform3D(a2Rotation);

            // Body Center
            bodyRotation.CenterX = 0;
            bodyRotation.CenterY = 0.5;
            bodyRotation.CenterZ = 0.5;

            // Rudder Center
            rudderRotation.CenterX = 0;
            rudderRotation.CenterY = 3;
            rudderRotation.CenterZ = -7;

            // Propeller Center
            propellerRotation.CenterX = 0;
            propellerRotation.CenterY = 0.5;
            propellerRotation.CenterZ = 0;

            // Aileron Centers
            aileronRotation1.CenterX = 0;
            aileronRotation1.CenterY = 0.5;
            aileronRotation1.CenterZ = 0;

            aileronRotation2.CenterX = 0;
            aileronRotation2.CenterY = 0.5;
            aileronRotation2.CenterZ = 0;

            // Elevator Centers
            elevatorRotation.CenterX = 0;
            elevatorRotation.CenterY = 0.5;
            elevatorRotation.CenterZ = -7;
        }

        /// <summary>
        /// Read controls from either user or replay, then control the airplane based on what's read
        /// </summary>
        private void controls()
        {
            double rotateX = 0;
            double rotateY = 0;
            double rotateZ = 0;
            double leftTurnMin = 0;
            double rightTurnMin = 0;
            double elevatorRotate = 0;
            double aileronRotate = 0;
            double rudderRotate = 0;

            // Plane Controls
            if (replayMode)
            {
                int i = recordingData[playMarker]; // Read byte from recording data
                int t;                             // temp var
                playMarker++;                      // Set marker to next byte
                inputs = i;                        // Save data read to var

                if (i == -1)
                {
                    replayMode = false; // stop replay
                    fs.Close();         // close file
                }
                else
                {
                    // Control airplane with recording data
                    t = i & 0x0000_0000_0000_0001;
                    if (t == 1) { elevatorRotate += 5; }

                    t = i & 0x0000_0000_0000_0002;
                    if (t == 2) { rudderRotate += -5; leftTurnMin = 0.2; }

                    t = i & 0x0000_0000_0000_0004;
                    if (t == 4) { elevatorRotate += -5; }

                    t = i & 0x0000_0000_0000_0008;
                    if (t == 8) { rudderRotate += 5; rightTurnMin = -0.2; }

                    t = i & 0x0000_0000_0000_0010;
                    if (t == 16) { aileronRotate += 5; }

                    t = i & 0x0000_0000_0000_0020;
                    if (t == 32) { aileronRotate += -5; }

                    t = i & 0x0000_0000_0000_0040;
                    if (t == 64) { changeSpeed(0.005); }

                    t = i & 0x0000_0000_0000_0080;
                    if (t == 128) { changeSpeed(-0.008); }
                }
            }
            else
            {
                // Control airplane with keyboard
                if (Keyboard.IsKeyDown(Key.W)) { elevatorRotate += 5; }
                if (Keyboard.IsKeyDown(Key.A)) { rudderRotate += -5; leftTurnMin = 0.2; }
                if (Keyboard.IsKeyDown(Key.S)) { elevatorRotate += -5; }
                if (Keyboard.IsKeyDown(Key.D)) { rudderRotate += 5; rightTurnMin = -0.2; }
                if (Keyboard.IsKeyDown(Key.Q)) { aileronRotate += 5; }
                if (Keyboard.IsKeyDown(Key.E)) { aileronRotate += -5; }
                if (Keyboard.IsKeyDown(Key.LeftShift)) { changeSpeed(0.005); }
                if (Keyboard.IsKeyDown(Key.LeftCtrl)) { changeSpeed(-0.008); }
            }

            if (elevatorRotate != 0)
            {
                rotateControlSurface(ref elevatorAngle, elevatorRotate, maxElevatorAngle);
            }
            else
            {
                rotateControlSurface(ref elevatorAngle, 5, 0);
            }

            if (rudderRotate != 0)
            {
                rotateControlSurface(ref rudderAngle, rudderRotate, maxRudderAngle);
            }
            else
            {
                rotateControlSurface(ref rudderAngle, 5, 0);
            }

            if (aileronRotate != 0)
            {
                rotateControlSurface(ref aileronAngle, aileronRotate, maxAileronAngle);
            }
            else
            {
                rotateControlSurface(ref aileronAngle, 5, 0);
            }

            // Turning vars
            rotateX = ParabolicFunction(elevatorAngle * (0.8 / maxElevatorAngle));
            rotateY = ParabolicFunction(rudderAngle * (0.1 / maxRudderAngle)) + leftTurnMin + rightTurnMin;
            rotateZ = ParabolicFunction(aileronAngle * (3 / maxAileronAngle));

            transformModel(rotateX, rotateY, rotateZ);
            CalculateMovement();
        }

        /// <summary>
        /// Rotate a control suruface to a target angle
        /// </summary>
        /// <param name="surface">Surface that's being rotated</param>
        /// <param name="amount">Amount to rotate</param>
        /// <param name="target">Target angle</param>
        private void rotateControlSurface(ref double surface, double amount, double target)
        {
            if (surface + amount > target)
            {
                surface = target;
            }
            else if (surface + amount < -target)
            {
                surface = -target;
            }
            else
            {
                surface += amount;
            }
        }

        /// <summary>
        /// Handle the transformations of the airplane and it's control surfaces
        /// </summary>
        /// <param name="xRotate">Change in X rotation</param>
        /// <param name="yRotate">Change in Y rotation</param>
        /// <param name="zRotate">Change in Z rotation</param>
        private void transformModel(double xRotate, double yRotate, double zRotate)
        {
            // Rotation
            if (xRotate != 0)
            {
                qRotation.Quaternion *= new Quaternion(vx, xRotate);
                angX += xRotate;
            }
            if (yRotate != 0)
            {
                qRotation.Quaternion *= new Quaternion(vy, yRotate);
                angY += yRotate;
            }
            if (zRotate != 0)
            {
                qRotation.Quaternion *= new Quaternion(vz, zRotate);
                angZ += zRotate;
            }

            // Control surfaces rotations
            rRotation.Quaternion = new Quaternion(vy, rudderAngle);
            a1Rotation.Quaternion = new Quaternion(vx, -aileronAngle);
            a2Rotation.Quaternion = new Quaternion(vx, aileronAngle);
            eRotation.Quaternion = new Quaternion(vx, -elevatorAngle);
            pRotation.Quaternion *= new Quaternion(vz, propellerSpeed);

            UpdateMovementVectors();

            // Get Transformations
            Transform3DGroup rudderTransforms = ControlSurfaceTransformation(rudderRotation);
            Transform3DGroup propellerTransforms = ControlSurfaceTransformation(propellerRotation);
            Transform3DGroup aileron1Transforms = ControlSurfaceTransformation(aileronRotation1);
            Transform3DGroup aileron2Transforms = ControlSurfaceTransformation(aileronRotation2);
            Transform3DGroup elevatorTransforms = ControlSurfaceTransformation(elevatorRotation);

            // Apply Transformations
            hull.Transform = GetHullTransformation();
            rudder.myModel.Transform = rudderTransforms;
            propeller.Transform = propellerTransforms;
            aileron1.myModel.Transform = aileron1Transforms;
            aileron2.myModel.Transform = aileron2Transforms;
            elevator.myModel.Transform = elevatorTransforms;
        }

        /// <summary>
        /// Find the vectors for forward and upward movement, set forward speed, and calculate upward speed from lift
        /// </summary>
        private void UpdateMovementVectors()
        {
            Matrix3D identity = Matrix3D.Identity; // Set forward direction to identity matrix
            identity.Rotate(qRotation.Quaternion); // Rotate identity matrix based on the plane's quaternion rotation

            forwardDirection = identity.Transform(orgForwardDir); // Forward direction is now the vector of the direction the plane is facing

            // Set the magnitude of forwardDirection to forwardSpeed
            if (forwardSpeed == 0 & propellerSpeed == 0)
            {
                forwardDirection = Vector3D.Multiply(forwardDirection, 0.001);
            }
            else
            {
                if (forwardSpeed != 0)
                {
                    forwardDirection = Vector3D.Multiply(forwardDirection, forwardSpeed);
                }
            }

            // Calculate Wing Speed
            double vx2 = Math.Pow(forwardDirection.X, 2);
            double vy2 = Math.Pow(forwardDirection.Y, 2);
            double vz2 = Math.Pow(forwardDirection.Z, 2);
            wingSpeed = Math.Sqrt(vx2 + vy2 + vz2);

            // Determine upward speed from lift and drag
            double dragForce = Math.Pow(upwardSpeed, 2) * 84 * AIRDENSITY;
            double wingAcceleration = (wingSpeed - dragForce) / mass;
            upwardSpeed += wingAcceleration; // apply acceleration

            // Apply gravity
            upwardDirection = identity.Transform(orgUpwardDir);
            upwardDirection = Vector3D.Multiply(upwardDirection, upwardSpeed) + new Vector3D(0, -gravityAccel, 0);

            // Get upward direction vector
            Vector3D verticalLift = upwardDirection;
            verticalLift.Normalize();
        }

        private void changeSpeed(double d)
        {
            double x = linearSpeed;
            double newSpeed = linearSpeed + d;

            // Case 1: 0 < newSpeed <= maxSpeed
            if (newSpeed > 0 & newSpeed < maxSpeed)
            {
                linearSpeed = newSpeed;
                forwardSpeed = ((x - 5 - ((x - 5) * (x - 5))) / 12) + 2.5;
            }

            // Case 2: 0 > newSpeed > -maxSpeed
            if (newSpeed < 0 & newSpeed > -maxSpeed / 2)
            {
                linearSpeed = newSpeed;
                forwardSpeed = ((x - 5 - ((x - 5) * (x - 5))) / 12) + 2.5;
                forwardSpeed /= 2; // speed penalty
            }

            propellerSpeed = forwardSpeed * 1000;
        }

        /// <summary>
        /// Move airplane based on forward and upward vectors
        /// </summary>
        private void CalculateMovement()
        {
            double yPrev = y;

            // Apply Forces
            x = translateTransform3D.OffsetX += forwardDirection.X + upwardDirection.X;
            y = translateTransform3D.OffsetY += forwardDirection.Y + upwardDirection.Y;
            z = translateTransform3D.OffsetZ += forwardDirection.Z + upwardDirection.Z;

            if (y < 2)
            {
                y = translateTransform3D.OffsetY = 2;
            }

            ySpeed = y - yPrev;
        }

        /// <summary>
        /// Do this every frame
        /// </summary>
        public void AirplaneStep()
        {
            controls();
        }

        private Transform3DGroup ControlSurfaceTransformation(RotateTransform3D surfaceRotationTransformation)
        {
            Transform3DGroup surfaceTransformGroup = new Transform3DGroup();
            surfaceTransformGroup.Children.Add(surfaceRotationTransformation);
            surfaceTransformGroup.Children.Add(bodyRotation);
            surfaceTransformGroup.Children.Add(translateTransform3D);

            return surfaceTransformGroup;
        }

        public Transform3DGroup GetHullTransformation()
        {
            Transform3DGroup hullTransforms = new Transform3DGroup();
            hullTransforms.Children.Add(bodyRotation);
            hullTransforms.Children.Add(translateTransform3D);
            return hullTransforms;
        }

        private double ParabolicFunction(double n)
        {
            double a = Math.Pow(0.7 * Math.Abs(wingSpeed), 1.2);
            double b = Math.Abs(n);
            double c;

            if (n <= 0)
            {
                c = (wingSpeed - a) * b;
            }
            else
            {
                c = -(wingSpeed - a) * b;
            }

            return c;
        }

        public void playReplay()
        {
            ResetPlane();

            // Enable replay mode
            replayMode = true;

            // Open replay file
            string path = @"../../\Assets\Replay.dat";
            fs = File.OpenRead(path);

            // Load replay into memory
            for (int i = 0; i != -1; i = fs.ReadByte())
            {
                recordingData.Add(i);
            }

            recordingData.Add(-1);
        }

        public void ResetPlane()
        {
            // Reset everything
            forwardDirection = orgForwardDir;
            upwardDirection = orgUpwardDir;
            x = orgX;
            y = orgY;
            z = orgZ;
            angX = 0;
            angY = 0;
            angZ = 0;
            forwardSpeed = 0.001;
            linearSpeed = 0;
            upwardSpeed = 0;
            ySpeed = 0;
            yDelta = 0;
            gravityAccel = 0.153125;
            initializeTransformations();
        }
    }
}