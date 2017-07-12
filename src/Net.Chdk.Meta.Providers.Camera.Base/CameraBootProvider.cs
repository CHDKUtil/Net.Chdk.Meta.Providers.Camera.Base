using Net.Chdk.Meta.Model.Camera;

namespace Net.Chdk.Meta.Providers.Camera
{
    public abstract class CameraBootProvider : ICameraBootProvider
    {
        public BootData GetBoot(uint modelId)
        {
            return new BootData
            {
                Fs = GetBootFileSystem(modelId),
            };
        }

        protected abstract string GetBootFileSystem(uint modelId);
    }
}
