﻿using Net.Chdk.Meta.Model.Camera;
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
                Encoding = EncodingProvider.GetEncoding(tree.Encoding.Version),
                Boot = BootProvider.GetBoot(modelId),
                Card = CardProvider.GetCard(modelId, tree.Card),
            };
        }
    }
}
