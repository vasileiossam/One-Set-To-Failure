using System;
using SQLite.Net.Attributes;
using System.Collections.Generic;
using SQLiteNetExtensions.Attributes;
using System.Linq;
using AutoMapper;

namespace Set.Models
{
	public class ImagePack
	{
		public int ImagePackId { get; set; }
        public string Title { get; set; }

        public ImagePack()
		{
		}
	}
}

