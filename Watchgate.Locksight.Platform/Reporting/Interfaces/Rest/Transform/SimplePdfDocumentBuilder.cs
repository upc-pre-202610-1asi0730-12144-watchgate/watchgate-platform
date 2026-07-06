using System.Text;

namespace Watchgate.Locksight.Platform.Reporting.Interfaces.Rest.Transform;

public static class SimplePdfDocumentBuilder
{
    public static byte[] Build(string title, IEnumerable<string> lines)
    {
        var content = new StringBuilder();
        content.AppendLine("BT");
        content.AppendLine("/F1 18 Tf");
        content.AppendLine("50 780 Td");
        content.AppendLine($"({Escape(title)}) Tj");
        content.AppendLine("0 -32 Td");
        content.AppendLine("/F1 11 Tf");
        foreach (var line in lines)
        {
            content.AppendLine($"({Escape(line)}) Tj");
            content.AppendLine("0 -18 Td");
        }
        content.AppendLine("ET");

        var contentBytes = Encoding.ASCII.GetBytes(content.ToString());
        var objects = new List<string>
        {
            "1 0 obj\n<< /Type /Catalog /Pages 2 0 R >>\nendobj\n",
            "2 0 obj\n<< /Type /Pages /Kids [3 0 R] /Count 1 >>\nendobj\n",
            "3 0 obj\n<< /Type /Page /Parent 2 0 R /MediaBox [0 0 612 792] /Resources << /Font << /F1 4 0 R >> >> /Contents 5 0 R >>\nendobj\n",
            "4 0 obj\n<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica >>\nendobj\n",
            $"5 0 obj\n<< /Length {contentBytes.Length} >>\nstream\n{content}endstream\nendobj\n"
        };

        using var stream = new MemoryStream();
        WriteAscii(stream, "%PDF-1.4\n");
        var offsets = new List<long> { 0 };
        foreach (var obj in objects)
        {
            offsets.Add(stream.Position);
            WriteAscii(stream, obj);
        }

        var xrefPosition = stream.Position;
        WriteAscii(stream, $"xref\n0 {objects.Count + 1}\n");
        WriteAscii(stream, "0000000000 65535 f \n");
        foreach (var offset in offsets.Skip(1))
            WriteAscii(stream, $"{offset:0000000000} 00000 n \n");

        WriteAscii(stream, $"trailer\n<< /Size {objects.Count + 1} /Root 1 0 R >>\nstartxref\n{xrefPosition}\n%%EOF");
        return stream.ToArray();
    }

    private static void WriteAscii(Stream stream, string value)
    {
        var bytes = Encoding.ASCII.GetBytes(value);
        stream.Write(bytes, 0, bytes.Length);
    }

    private static string Escape(string value) =>
        value.Replace("\\", "\\\\").Replace("(", "\\(").Replace(")", "\\)");
}
