using System;
using System.Diagnostics;
using Microsoft.AppCenter.Crashes;
using Urho;
using Urho.Actions;
using Urho.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Application = Urho.Application;

namespace Spitzer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SpitzerModelPage : ContentPage
    {
        private SpitzerModel urhoApp;

        public SpitzerModelPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            // base.OnAppearing();
            urhoApp = await UrhoSurface.Show<SpitzerModel>(new ApplicationOptions(assetsFolder: null)
                {Orientation = ApplicationOptions.OrientationType.LandscapeAndPortrait});
        }

        protected override void OnDisappearing()
        {
            UrhoSurface.OnDestroy();
            base.OnDisappearing();
        }
    }

    public class SpitzerModel : Application
    {
        Scene scene;
        Camera camera;
        private Octree octree;
        private Node telescope;
        private bool movementsEnabled;
        private float touchSensitivity = 2f;
        private Node cameraNode;
        private const float CameraMinDist = 1.0f;
        private const float CameraInitialDist = 3.0f;
        private const float CameraMaxDist = 50.0f;

        [Preserve]
        public SpitzerModel(ApplicationOptions options) : base(options)
        {
        }

        static SpitzerModel()
        {
            UnhandledException += (s, e) =>
            {
                //if (Debugger.IsAttached)
                //    Debugger.Break();
                e.Handled = true;
                Crashes.TrackError(e.Exception);
            };
        }

        protected override void Start()
        {
            base.Start();
            CreateScene();
            SetupViewport();
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

            ResourceCache.AddResourceDir("SpitzerModel", 1);
            telescope = scene.InstantiateXml(source: ResourceCache.GetFile("Scene.xml"),
                position: new Vector3(x: 0, y: -1f, z: 1f),
                rotation: new Quaternion(180, 90, 180));
            telescope.SetScale(CameraInitialDist);

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
            if (Input.NumTouches >= 1 && movementsEnabled)
            {
                if (Input.NumTouches == 1)
                {
                    var touch = Input.GetTouch(0);
                    telescope.Rotate(new Quaternion(-touch.Delta.X, -touch.Delta.Y, 0), TransformSpace.Local);
                }
                else if (Input.NumTouches == 2)
                {
                    var touch1 = Input.GetTouch(0);
                    var touch2 = Input.GetTouch(1);

                    int sens = 0;
                    // Check for zoom direction (in/out)
                    if (Math.Abs(touch1.Position.Y - touch2.Position.Y) >
                        Math.Abs(touch1.LastPosition.Y - touch2.LastPosition.Y))
                        sens = 1;
                    else
                        sens = -1;
                    CameraDistance += Math.Abs(touch1.Delta.Y - touch2.Delta.Y) * sens * touchSensitivity / 500.0f;
                    CameraDistance =
                        MathHelper.Clamp(CameraDistance, CameraMinDist, CameraMaxDist); 
                    telescope.SetScale(CameraDistance);
                }
            }
            base.OnUpdate(timeStep);
        }

        public float CameraDistance { get; set; }
    }
}