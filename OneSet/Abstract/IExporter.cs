﻿using System.Threading.Tasks;

namespace OneSet.Abstract
{
    public interface IExporter
    {
        Task<string> ExportToCsv();
    }
}
