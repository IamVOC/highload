namespace SpaceBattle.Lib;

using Npgsql;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

public class SaveToOutboxCommand : SpaceBattle.Lib.ICommand
{
	private string _body;
	private IHeaderDictionary _headers;
	private readonly string ?_connectionString;

	public SaveToOutboxCommand(string body, IHeaderDictionary headers)
	{
		_body = body;
		_headers = headers;
		string ?host = Environment.GetEnvironmentVariable("DB_HOST");
        string ?database = Environment.GetEnvironmentVariable("DB_NAME");
        string ?userId = Environment.GetEnvironmentVariable("DB_USER");
        string ?password = Environment.GetEnvironmentVariable("DB_PASS");
        string ?port = Environment.GetEnvironmentVariable("DB_PORT");

        NpgsqlConnectionStringBuilder builder = new NpgsqlConnectionStringBuilder
        {
            Host = host,
            Database = database,
            Username = userId,
            Password = password,
            Port = Convert.ToInt32(port)
        };

		_connectionString = builder.ConnectionString;
	}

	public void Execute()
	{
		DateTime created_at = DateTime.Now;
		JsonDocument message = JsonDocument.Parse(this._body);
		string path = message.RootElement.GetProperty("path").ToString();
		string body = message.RootElement.GetProperty("body").ToString();

		StringBuilder headersBuilder = new StringBuilder();
        foreach (var header in _headers)
        {
            headersBuilder.AppendLine($"{header.Key}: {header.Value}");
        }
		string headersString = headersBuilder.ToString();


		using (NpgsqlConnection connection = new NpgsqlConnection(this._connectionString))
        {
            connection.Open();

            string query = "INSERT INTO outbox_messages (body, headers, path, status, created_at, updated_at) VALUES (@body, @headers, @path, @status, @created_at, @updated_at)";

            using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
            {
				command.Parameters.AddWithValue("@body", body);
                command.Parameters.AddWithValue("@headers", headersString);
				command.Parameters.AddWithValue("@path", path);
				command.Parameters.AddWithValue("@status", "pending");
				command.Parameters.AddWithValue("@created_at", created_at);
				command.Parameters.AddWithValue("@updated_at", created_at);

                command.ExecuteNonQuery();
            }
        }
	}
}
