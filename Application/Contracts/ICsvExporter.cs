using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface ICsvExporter
    {
        byte[] GetToCsvExport<T>(IEnumerable<T> toExport);
    }
}
