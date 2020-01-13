using System;
using System.Diagnostics;
using Acr.UserDialogs;
using Microsoft.AppCenter.Crashes;
using Urho;

namespace Spitzer.Models
{
    public class SpitzerModel : Application
    {
        Scene scene;
        Camera camera;
        private Octree octree;
        private Node telescope;
        private bool movementsEnabled;
        private float touchSensitivity = 2f;
        private Node cameraNode;
        private int numTaps;
        private DateTime tapTimeStep = DateTime.Now;
        private const float CameraMinDist = 1.0f;
        private const float CameraInitialDist = 4.0f;
        private const float CameraMaxDist = 50.0f;

        [Preserve]
        public SpitzerModel(ApplicationOptions options) : base(options)
        {
        }

        static SpitzerModel()
        {
            UnhandledException += (s, e) =>
            {
                if (Debugger.IsAttached)
                    Debugger.Break();
                e.Handled = true;
                Crashes.TrackError(e.Exception);
            };
        }

        protected override void Start()
        {
            base.Start();
            CreateScene();
            SetupViewport();
            Input.TouchBegin += args =>
            {
                // System.Console.WriteLine($"TouchBegin numTaps: {numTaps}");
                TimeSpan timeStep = (DateTime.Now - tapTimeStep);
                // System.Console.WriteLine($"TouchBegin timeStep: {timeStep.TotalMilliseconds}");
                if(timeStep.TotalMilliseconds > 2000)
                {
                    numTaps = 0;
                    tapTimeStep = DateTime.Now;
                    return;
                }
                if (numTaps == 0)
                {
                    tapTimeStep = DateTime.Now;
                    numTaps++;
                }
                else if (numTaps == 2)
                {
                    if (timeStep.TotalMilliseconds < 1600)
                    {
                        ResetModelView();
                        tapTimeStep = default;
                        numTaps = 0;
                        return;
                    }
                    else
                    {
                        numTaps = 0;
                    }

                }
                if (timeStep.TotalMilliseconds < 800)
                {
                    numTaps++;
                }
            };

            Input.TouchMove += args =>
            {
                // System.Console.WriteLine($"TouchMove DX/DY: {args.DX}/{args.DY}");
                if (Math.Abs(args.DX) > 1 || Math.Abs(args.DY) > 1)
                {
                    numTaps = 0;
                }
                // System.Console.WriteLine($"TouchMove numTaps: {numTaps}");
            };
        }

        protected override void Stop()
        {
            base.Stop();
            Input.UnsubscribeFromAllEvents();
        }

        private void SetupViewport()
        {
            var renderer = Renderer;
            var vp = new Viewport(Context, scene, camera, null);
            renderer.SetViewport(0, vp);
        }

        private async void CreateScene()
        {
            scene = new Scene();
            octree = scene.CreateComponent<Octree>();

            telescope = scene.InstantiateXml(source: ResourceCache.GetFile("Scene.xml"),
                position: new Vector3(x: 0, y: -1f, z: 1f),
                rotation: new Quaternion(180, 90, 180));
            telescope.SetScale(CameraInitialDist);
            CameraDistance = CameraInitialDist;
            cameraNode = scene.CreateChild();
            camera = cameraNode.CreateComponent<Camera>();
            cameraNode.Position = new Vector3(0, 0, -35);

            Node lightNode = cameraNode.CreateChild();
            var light = lightNode.CreateComponent<Light>();
            light.LightType = LightType.Point;
            light.Range = 100;
            light.Brightness = 1.3f;

            try
            {
                await telescope.RunActionsAsync();
            }
            catch (OperationCanceledException)
            {
                if (Debugger.IsAttached)
                    Debugger.Break();
            }

            movementsEnabled = true;
        }

        protected override void OnUpdate(float timeStep)
        {
            base.OnUpdate(timeStep);

            if (Input.NumTouches >= 1 && movementsEnabled)
            {
                if (Input.NumTouches == 1)
                {
                    var touch = Input.GetTouch(0);
                    telescope.Rotate(new Quaternion(-touch.Delta.Y, -touch.Delta.X, 0), TransformSpace.Local);
                }
                else if (Input.NumTouches == 2)
                {
                    var touch1 = Input.GetTouch(0);
                    var touch2 = Input.GetTouch(1);

                    int sens = 0;
                    if (Math.Abs(touch1.Position.Y - touch2.Position.Y) >
                        Math.Abs(touch1.LastPosition.Y - touch2.LastPosition.Y))
                        sens = 1;
                    else
                        sens = -1;
                    CameraDistance += Math.Abs(touch1.Delta.Y - touch2.Delta.Y) * sens * touchSensitivity / 250.0f;
                    CameraDistance =
                        MathHelper.Clamp(CameraDistance, CameraMinDist, CameraMaxDist);
                    telescope.SetScale(CameraDistance);
                }
            }
        }

        public float CameraDistance { get; set; }

        public void ResetModelView()
        {
            telescope.Position = new Vector3(x: 0, y: -1f, z: 1f);
            telescope.Rotation = new Quaternion(180, 90, 180);
            telescope.SetScale(CameraInitialDist);
        }
    }
}