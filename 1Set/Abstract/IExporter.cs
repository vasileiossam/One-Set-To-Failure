using System.Text;

namespace Set
{
	public interface IExporter
	{
		string ExportToCsv(StringBuilder csv);
	}
}

