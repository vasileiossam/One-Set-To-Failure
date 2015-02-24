using System;

namespace Set
{
	public class Settings
	{
        [PrimaryKey, AutoIncrement]
        public int SettingID { get; set; }

        public string Key {get; set;}
        public string Value {get; set;}

        IsMetric <------------- ugly when in db, do in json
            ismetric best practice? what happens if json gone and user selects antoher setting -- existing data

		public Setting ()
		{

		}
	}
}

