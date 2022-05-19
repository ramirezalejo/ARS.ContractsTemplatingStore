using System.Text.Json.Serialization;

namespace ARS.ContractsTemplatingStore.Shared.Models;

public class FileObject
{
    public string Base64data { get; set; }
    public string? ContentType { get; set; }
    public string FileName { get; set; }
    public byte State { get; set; }

    [JsonIgnore]
    public byte[]? Buffer { get; set; }
    [JsonIgnore]
    public int OriginalSize { get; set; }
    [JsonIgnore]
    public int CompressedSize { get; set; }

    public bool IsOkToCompress()
    {
        if (ContentType != null)
        {
            var low = ContentType.ToLower();
            if (low.StartsWith("image/"))
                return low.EndsWith("/svg+xml") || low.EndsWith("/bmp");
            if (low.StartsWith("audio/") || low.StartsWith("video/"))
                return low.EndsWith("/wav");
            if (low.StartsWith("text/"))
                return true;
            if(low.StartsWith("application/"))
            {
                switch(low.Split('/')[1])
                {
                    case "x-abiword":
                    case "octet-stream": // assume it can be compressed
                    case "x-csh":
                    case "x-msword":
                    case "vnd.openxmlformats-officedocument.wordprocessingml.document":
                    case "json":
                    case "ld+json":
                    case "vnd.apple.installer+xml":
                    case "vnd.oasis.opendocument.presentation":
                    case "vnd.oasis.opendocument.spreadsheet":
                    case "vnd.oasis.opendocument.text":
                    case "x-httpd-php":
                    case "vnd.ms-powerpoint":
                    case "vnd.openxmlformats-officedocument.presentationml.presentation":
                    case "rtf":
                    case "x-sh":
                    case "vnd.visio":
                    case "xhtml+xml":
                    case "vnd.ms-excel":
                    case "vnd.openxmlformats-officedocument.spreadsheetml.sheet":
                    case "xml":
                    case "vnd.mozilla.xul+xml":
                        return true;
                }
            }
        }
        return false;
    }
}