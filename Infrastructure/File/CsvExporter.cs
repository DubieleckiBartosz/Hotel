using Application.Contracts;
using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Infrastructure.File
{
    public class CsvExporter: ICsvExporter
    {
        public byte[] GetToCsvExport<T>(IEnumerable<T> toExport)
        {
            using var memoryStream = new MemoryStream();
            using (var streamWriter = new StreamWriter(memoryStream))
            {
                using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);
                csvWriter.WriteRecords(toExport);
            }
            return memoryStream.ToArray();
        }
    }
}
