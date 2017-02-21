using System.Text;

namespace OneSet.Abstract
{
	public interface IExporter
	{
		string ExportToCsv(StringBuilder csv);
	}
}

