#if USE_SQLITE_NET
using System;
using System.IO;
using SQLite;

namespace SQLCipherApp
{
	public class Message
	{
		[PrimaryKey]
		public int Id { get; set; }

		public string Content { get; set; }
	}

	public class MessageDb
	{
		public const string MIME_TYPE = "application/x-net-zetetic-messagedb";
	
		public string FilePath {get; set;}

		public string Password {private get; set;}
		
		public MessageDb (string filePath)
		{
			FilePath = filePath;
		}

		public SQLiteConnection GetConnection() 
		{
			return new SQLiteConnection(FilePath);
		}

		public string LoadMessage() 
		{
			if(File.Exists(FilePath)) 
			{
				using(var connection = GetConnection())
				{
					var message = connection.Query<Message> ("SELECT * FROM Message WHERE Id = ?", 0);
					return message[0].Content;
				}
			}
			return null;
		}

		public void SaveMessage(string content) 
		{
			File.Delete(FilePath);
			using(var connection = GetConnection())
			{
				connection.CreateTable<Message>();
				connection.InsertOrReplace(new Message() {Id = 0, Content = content});
			}

		}
	}
}
#endif

