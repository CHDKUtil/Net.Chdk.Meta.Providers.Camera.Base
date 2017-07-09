using Net.Chdk.Meta.Model.Camera;
using Net.Chdk.Meta.Model.CameraList;
using Net.Chdk.Meta.Model.CameraTree;
using Net.Chdk.Meta.Model.Platform;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Net.Chdk.Meta.Providers.Camera
{
    public abstract class BuildProvider<TCamera, TModel, TCard>
        where TCamera : CameraData<TCamera, TModel, TCard>
        where TModel : CameraModelData
        where TCard : CardData
    {
        private ICameraProvider<TCamera, TModel, TCard> CameraProvider { get; }
        private ICameraModelProvider<TModel> ModelProvider { get; }
        private ICameraPlatformProvider PlatformProvider { get; }
        private ICameraValidator CameraValidator { get; }

        protected BuildProvider(ICameraProvider<TCamera, TModel, TCard> cameraProvider, ICameraModelProvider<TModel> modelProvider,
            ICameraPlatformProvider platformProvider, ICameraValidator cameraValidator)
        {
            CameraProvider = cameraProvider;
            ModelProvider = modelProvider;
            PlatformProvider = platformProvider;
            CameraValidator = cameraValidator;
        }

        public IDictionary<string, TCamera> GetCameras(IDictionary<string, PlatformData> platforms, IDictionary<string, ListPlatformData> list, IDictionary<string, TreePlatformData> tree)
        {
            CameraValidator.Validate(list, tree);
            var cameras = new SortedDictionary<uint, TCamera>();
            foreach (var kvp in list)
                AddModel(cameras, platforms, tree, kvp.Key, kvp.Value);
            return cameras.ToDictionary(kvp => $"0x{kvp.Key:x}", kvp => kvp.Value);
        }

        private void AddModel(SortedDictionary<uint, TCamera> cameras, IDictionary<string, PlatformData> platforms, IDictionary<string, TreePlatformData> treeCameras, string key, ListPlatformData list)
        {
            var platform = PlatformProvider.GetPlatform(key, platforms);
            var tree = PlatformProvider.GetTree(key, treeCameras);
            var modelId = Convert.ToUInt32(platform.ModelId, 16);
            var camera = GetOrAddCamera(modelId, key, list, tree, cameras);
            var model = ModelProvider.GetModel(key, platform.Names, list, tree);
            camera.Models = camera.Models.Concat(new[] { model }).ToArray();
        }

        private TCamera GetOrAddCamera(uint modelId, string platform, ListPlatformData list, TreePlatformData tree, SortedDictionary<uint, TCamera> cameras)
        {
            if (!cameras.TryGetValue(modelId, out TCamera camera))
            {
                camera = CameraProvider.GetCamera(modelId, platform, list, tree);
                cameras.Add(modelId, camera);
            }
            return camera;
        }
    }
}
