using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Oocx.Acme.Tests.FakeHttp
{
    public class PreventDisposeContentWrapper : HttpContent
    {
        public byte[] Bytes { get; }
        public Type ContentType { get; }

        private PreventDisposeContentWrapper(byte[] bytes, Type contentType)
        {
            Bytes = bytes;
            ContentType = contentType;
        }

        public static async Task<PreventDisposeContentWrapper> CreateWrapperAsync(HttpContent wrappedContent)
        {
            if (wrappedContent == null)
            {
                return new PreventDisposeContentWrapper(new byte[0], null);
            }            
            var bytes = await wrappedContent.ReadAsByteArrayAsync();
            var wrapper = new PreventDisposeContentWrapper(bytes, wrappedContent.GetType());
            foreach (var header in wrappedContent.Headers)
            {
                wrapper.Headers.Add(header.Key, header.Value);
            }
            return wrapper;
        }

        protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            await stream.WriteAsync(Bytes, 0, Bytes.Length);
        }

        protected override bool TryComputeLength(out long length)
        {
            length = Bytes.Length;
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            
        }
    }
}