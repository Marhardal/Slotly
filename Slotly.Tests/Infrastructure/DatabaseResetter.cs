using Respawn;
using Npgsql;

namespace Tests;

public class DatabaseResetter
{
    private Respawner? _respawner;

    public async Task ResetAsync(string connectionString)
    {
        await using var conn = new NpgsqlConnection(connectionString);
        await conn.OpenAsync();

        _respawner ??= await Respawner.CreateAsync(conn, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = new[] { "public" },
            TablesToIgnore = new[] { new Respawn.Graph.Table("__EFMigrationsHistory") }
        });

        await _respawner.ResetAsync(conn);
    }
}
