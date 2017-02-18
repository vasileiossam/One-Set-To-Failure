using System.Threading.Tasks;
using OneSet.Models;

namespace OneSet.Abstract
{
	public interface IBackupRestore
	{
		Task Backup();
		Task Restore();
		Task<BackupInfo> GetBackupInfo();
	}
}

