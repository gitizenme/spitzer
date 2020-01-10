using System;
using System.Diagnostics;
using Microsoft.AppCenter.Crashes;
using Urho;
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
                position: new Vector3(x: 0, y: -1f, z:1f),
                rotation: new Quaternion(180, 90, 180));
            telescope.SetScale(3);
            
            var cameraNode = scene.CreateChild();
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
        }
    }
}