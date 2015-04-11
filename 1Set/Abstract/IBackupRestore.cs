using System;
using System.Threading.Tasks;
using Set.Models;

namespace Set.Abstract
{
	public interface IBackupRestore
	{
		Task Backup();
		Task Restore();
		Task<BackupInfo> GetBackupInfo();
	}
}

