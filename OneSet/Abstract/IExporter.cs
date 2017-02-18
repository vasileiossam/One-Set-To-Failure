using System.Text;

namespace OneSet
{
	public interface IExporter
	{
		string ExportToCsv(StringBuilder csv);
	}
}

