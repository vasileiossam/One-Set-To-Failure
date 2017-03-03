using System.Threading.Tasks;
using OneSet.Models;

namespace OneSet.Abstract
{
	public interface IBackupRestoreService
	{
		Task Backup();
		Task Restore();
		Task<BackupInfo> GetBackupInfo();
	}
}

