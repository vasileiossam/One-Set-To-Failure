using System.Text;

namespace OneSet.Abstract
{
	public interface ITextStorage
	{
	    string Save(StringBuilder text, string fileName);
	}
}

