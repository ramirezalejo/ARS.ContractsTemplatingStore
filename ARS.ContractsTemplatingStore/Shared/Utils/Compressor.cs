using System.IO.Compression;

namespace ARS.ContractsTemplatingStore.Shared.Utils;

public class Compressor
{
    public static async Task<byte[]> CompressAsync(byte[] bytes, CancellationToken cancel = default)
    {
        using var outputStream = new MemoryStream();
        await using (var compressStream = new GZipStream(outputStream, CompressionLevel.Optimal))
        {
            await compressStream.WriteAsync(bytes, 0, bytes.Length, cancel);
        }
        return outputStream.ToArray();
    }

    public static async Task<byte[]> DecompressAsync(byte[] bytes, CancellationToken cancel = default)
    {
        using var inputStream = new MemoryStream(bytes);
        using var outputStream = new MemoryStream();
        await using (var decompressStream = new GZipStream(inputStream, CompressionMode.Decompress))
        {
            await decompressStream.CopyToAsync(outputStream, cancel);
        }
        return outputStream.ToArray();
    }
}