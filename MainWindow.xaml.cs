/// @name: MainWindow.xaml.cs
/// @author: Bryson Squibb, 5003954275
/// @date: 11/28/2021
/// @description: This cs file initializes a 3D environemt
/// where the user can control an airplane.

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using System.IO;

namespace Midterm_Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // World camera
        private PerspectiveCamera worldCam;
        private enum CAMERAMODE
        {
            AIRPLANE,
            UNLOCKED,
            TOWERVIEW
        }

        private CAMERAMODE cameraMode = CAMERAMODE.AIRPLANE;

        // Fps
        public static int framecap = 1000 / 140; // milliseconds / fps

        // Recording
        private FileStream fs;
        private bool recording = false;
        private bool fileOpen = false;
        private readonly List<byte> recordingData = new List<byte>();
        private readonly string path = @"../../\Assets\Replay.dat";

        // Airplane
        private readonly Airplane airplane;

        /// <summary>
        /// Main entry point of this program
        /// </summary>
        public MainWindow()
        {
            // Initialize Main Window
            InitializeComponent();

            // Create Airplane
            airplane = new Airplane(new Point3D(-622, 2, -243), Color.FromRgb(255, 0, 0));
            myViewport.Children.Add(airplane.myVisual);

            // Load Map
            GreenCity map = new GreenCity();
            myViewport.Children.Add(map.myVisual);

            // Setup DispatcherTimer for step event (occurs every frame)
            DispatcherTimer gameStep = new DispatcherTimer(DispatcherPriority.Render);
            gameStep.Tick += new EventHandler(dispatcherTimer_Tick);
            gameStep.Interval = new TimeSpan(0, 0, 0, 0, framecap);
            gameStep.Start();
        }

        /// <summary>
        /// Event that occurs every step (frame)
        /// </summary>
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            // Run airplane logic every frame
            airplane.AirplaneStep();

            // Move camera
            repositionCamera();

            // Update text in window
            Time.Text = DateTime.Now.ToString();
            VerticalSpeed.Text = "Vertical Speed: " + airplane.ySpeed.ToString("0.00");
            ForwardSpeed.Text = "Forward Speed: " + airplane.forwardSpeed.ToString("0.00");
            AngleX.Text = "Angle X: " + airplane.angX.ToString("0.00");
            AngleY.Text = "Angle Y: " + airplane.angY.ToString("0.00");
            AngleZ.Text = "Angle Z: " + airplane.angZ.ToString("0.00");
            X.Text = "X: " + airplane.x.ToString("0.00");
            Y.Text = "Y: " + airplane.y.ToString("0.00");
            Z.Text = "Z: " + airplane.z.ToString("0.00");

            OnScreenKeyboard();

            if (recording)
            {
                recordInputs();
            }
        }

        /// <summary>
        /// Do things like camera setup when the viewport is loaded
        /// </summary>
        private void myViewport_Loaded(object sender, RoutedEventArgs e)
        {
            // Camera setup
            worldCam = new PerspectiveCamera();
            worldCam.FarPlaneDistance = 6000.0;
            worldCam.LookDirection = new Vector3D(0.0, 0.0, 1.0);
            worldCam.UpDirection = new Vector3D(0.0, 1.0, 0.0);
            worldCam.NearPlaneDistance = 1.0;
            worldCam.Position = new Point3D(0.0, 5.0, -40.0);
            worldCam.FieldOfView = 60.0;
            myViewport.Camera = worldCam;
        }

        /// <summary>
        /// Adjust camera position and look direction based on the position and orientation of the airplane
        /// </summary>
        private void repositionCamera()
        {
            switch (cameraMode)
            {
                case CAMERAMODE.AIRPLANE:
                    // Adjust look direction and position
                    worldCam.LookDirection = new Vector3D(0, 0, 1);
                    worldCam.Position = new Point3D(0.0, 5.0, -40.0);

                    // Apply same transformation as the airplane to the camera
                    worldCam.Transform = airplane.GetHullTransformation();
                    break;

                case CAMERAMODE.UNLOCKED:
                    worldCam.Transform = new Transform3DGroup();
                    worldCam.LookDirection = new Vector3D(airplane.x - worldCam.Position.X, airplane.y - worldCam.Position.Y, airplane.z - worldCam.Position.Z);
                    break;

                case CAMERAMODE.TOWERVIEW:
                    worldCam.Transform = new Transform3DGroup();
                    worldCam.LookDirection = new Vector3D(airplane.x - worldCam.Position.X, airplane.y - worldCam.Position.Y, airplane.z - worldCam.Position.Z);
                    break;

                default:
                    break;
            }

        }

        /// <summary>
        /// Do things when keys are pressed
        /// </summary>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            // if locked then unlock; if unlocked then lock
            if (Keyboard.IsKeyDown(Key.F))
            {
                switch(cameraMode)
                {
                    case CAMERAMODE.TOWERVIEW:
                        worldCam.Position = new Point3D(airplane.x, airplane.y + 5, airplane.z - 40); // save position of plane when unlocking
                        cameraMode = CAMERAMODE.AIRPLANE;
                        CameraMode.Text = "Camera Mode: Airplane";
                        break;

                    case CAMERAMODE.AIRPLANE:
                        worldCam.Position = new Point3D(airplane.x, airplane.y + 5, airplane.z - 40); // save position of plane when unlocking
                        cameraMode = CAMERAMODE.UNLOCKED;
                        CameraMode.Text = "Camera Mode: Unlocked";
                        break;

                    case CAMERAMODE.UNLOCKED:
                        worldCam.Position = new Point3D(-1000, 60, 0); // lazily set position to control tower
                        cameraMode = CAMERAMODE.TOWERVIEW;
                        CameraMode.Text = "Camera Mode: Tower View";
                        break;

                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Record keys associated from controlling the airplane
        /// </summary>
        private void recordInputs()
        {
            byte b = 0;

            // Get keys
            if (Keyboard.IsKeyDown(Key.W)) { b += 1; }
            if (Keyboard.IsKeyDown(Key.A)) { b += 2; }
            if (Keyboard.IsKeyDown(Key.S)) { b += 4; }
            if (Keyboard.IsKeyDown(Key.D)) { b += 8; }
            if (Keyboard.IsKeyDown(Key.Q)) { b += 16; }
            if (Keyboard.IsKeyDown(Key.E)) { b += 32; }
            if (Keyboard.IsKeyDown(Key.LeftShift)) { b += 64; }
            if (Keyboard.IsKeyDown(Key.LeftCtrl)) { b += 128; }

            input.Text = b.ToString();

            // Add keys to file
            recordingData.Add(b);
        }

        /// <summary>
        /// Show what keys are being pressed from either the user or the recording
        /// </summary>
        private void OnScreenKeyboard()
        {
            SolidColorBrush greenBrush = new SolidColorBrush(Color.FromRgb(50, 255, 50));
            SolidColorBrush whiteBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));

            // On Screen Keyboard
            if (airplane.replayMode == false)
            {
                W.Background = Keyboard.IsKeyDown(Key.W) ? greenBrush : whiteBrush;
                A.Background = Keyboard.IsKeyDown(Key.A) ? greenBrush : whiteBrush;
                S.Background = Keyboard.IsKeyDown(Key.S) ? greenBrush : whiteBrush;
                D.Background = Keyboard.IsKeyDown(Key.D) ? greenBrush : whiteBrush;
                Q.Background = Keyboard.IsKeyDown(Key.Q) ? greenBrush : whiteBrush;
                E.Background = Keyboard.IsKeyDown(Key.E) ? greenBrush : whiteBrush;
                Shift.Background = Keyboard.IsKeyDown(Key.LeftShift) ? greenBrush : whiteBrush;
                Control.Background = Keyboard.IsKeyDown(Key.LeftCtrl) ? greenBrush : whiteBrush;
            }
            else
            {
                // Show keys pressed from the recording
                int i = airplane.inputs;
                int t;
                input.Text = i.ToString();

                if (i != -1)
                {
                    t = i & 0x0000_0000_0000_0001;
                    W.Background = t == 1 ? greenBrush : whiteBrush;

                    t = i & 0x0000_0000_0000_0002;
                    A.Background = t == 2 ? greenBrush : whiteBrush;

                    t = i & 0x0000_0000_0000_0004;
                    S.Background = t == 4 ? greenBrush : whiteBrush;

                    t = i & 0x0000_0000_0000_0008;
                    D.Background = t == 8 ? greenBrush : whiteBrush;

                    t = i & 0x0000_0000_0000_0010;
                    Q.Background = t == 16 ? greenBrush : whiteBrush;

                    t = i & 0x0000_0000_0000_0020;
                    E.Background = t == 32 ? greenBrush : whiteBrush;

                    t = i & 0x0000_0000_0000_0040;
                    Shift.Background = t == 64 ? greenBrush : whiteBrush;

                    t = i & 0x0000_0000_0000_0080;
                    Control.Background = t == 128 ? greenBrush : whiteBrush;
                }
            }
        }

        /// <summary>
        /// Start the replay
        /// </summary>
        private void ReplayButton_Click(object sender, RoutedEventArgs e)
        {
            // test if airplane isn't already playing a replay
            if (airplane.replayMode == false)
            {
                if (fileOpen == true)
                {
                    fs.Close();
                    fileOpen = false;

                }
                recording = false;
                airplane.playReplay();
            }
        }

        /// <summary>
        /// Start recording a replay
        /// </summary>
        private void RecordButton_Click(object sender, RoutedEventArgs e)
        {
            if (recording == false)
            {
                airplane.ResetPlane();
                recording = true;
                RecordButton.Content = "Recording...";
            }
            else
            {
                RecordButton.Content = "Record Animation";

                if (airplane.replayMode == false)
                {
                    if (fileOpen == false)
                    {
                        File.Delete(path);
                        fs = File.OpenWrite(path);
                        fileOpen = true;
                    }

                    // Write the data to the file, byte by byte.
                    for (int i = 0; i < recordingData.Count; i++)
                    {
                        fs.WriteByte(recordingData[i]);
                    }
                }
            }
        }

        private void windSound_MediaEnded(object sender, RoutedEventArgs e)
        {
            windSound.Position = new TimeSpan(0);
        }
    }
}
