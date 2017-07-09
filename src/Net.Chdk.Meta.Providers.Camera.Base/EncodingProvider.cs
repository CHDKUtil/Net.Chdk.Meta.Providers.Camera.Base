using Net.Chdk.Meta.Model.Camera;
using System;

namespace Net.Chdk.Meta.Providers.Camera
{
    public abstract class EncodingProvider : IEncodingProvider
    {
        #region Fields

        protected static readonly EncodingData EmptyEncoding = new EncodingData
        {
            Name = string.Empty
        };

        #endregion

        #region Constructor

        protected EncodingProvider()
        {
            _encodings = new Lazy<EncodingData[]>(GetEncodings);
        }

        #endregion

        #region IEncodingProvider Members

        public EncodingData GetEncoding(uint version)
        {
            return Encodings[version];
        }

        #endregion

        #region Encodings

        private readonly Lazy<EncodingData[]> _encodings;

        private EncodingData[] Encodings => _encodings.Value;

        protected abstract EncodingData[] GetEncodings();

        #endregion
    }
}
