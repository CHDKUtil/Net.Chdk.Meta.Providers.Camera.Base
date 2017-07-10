using Net.Chdk.Meta.Model.Camera;
using Net.Chdk.Meta.Model.CameraList;
using Net.Chdk.Meta.Model.CameraTree;

namespace Net.Chdk.Meta.Providers.Camera
{
    public abstract class CameraProvider<TCamera, TModel, TCard>
        where TCamera : CameraData<TCamera, TModel, TCard>, new()
        where TModel : CameraModelData
        where TCard : CardData
    {
        private IEncodingProvider EncodingProvider { get; }
        private IBootProvider BootProvider { get; }
        private ICardProvider<TCard> CardProvider { get; }

        protected CameraProvider(IEncodingProvider encodingProvider, IBootProvider bootProvider, ICardProvider<TCard> cardProvider)
        {
            EncodingProvider = encodingProvider;
            BootProvider = bootProvider;
            CardProvider = cardProvider;
        }

        public virtual TCamera GetCamera(uint modelId, string platform, ListPlatformData list, TreePlatformData tree)
        {
            return new TCamera
            {
                Models = new TModel[0],
                Encoding = GetEncoding(tree.Encoding),
                Boot = GetBoot(modelId),
                Card = GetCard(modelId, tree.Card),
            };
        }

        private EncodingData GetEncoding(TreeEncodingData encoding)
        {
            return encoding != null
                ? EncodingProvider.GetEncoding(encoding.Version)
                : null;
        }

        private BootData GetBoot(uint modelId)
        {
            return BootProvider.GetBoot(modelId);
        }

        private TCard GetCard(uint modelId, TreeCardData card)
        {
            return card != null
                ? CardProvider.GetCard(modelId, card)
                : null;
        }
    }
}
