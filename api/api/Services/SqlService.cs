using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using api.Models;
using api.Models.Parameters;

public class SqlService
{
    private readonly VolejbalContext _context;

    public SqlService(VolejbalContext context)
    {
        _context = context;
    }

    public async Task VlozSestavuTsqlAsync(int idZapas, int idTymDomaci, int idTymHost, List<SestavaInput> sestava)
    {
        var connection = _context.Database.GetDbConnection();
        await using var command = connection.CreateCommand();

        command.CommandText = "VlozSestavu";
        command.CommandType = CommandType.StoredProcedure;

        // Parametry
        command.Parameters.Add(new SqlParameter("@IdZapas", idZapas));
        command.Parameters.Add(new SqlParameter("@IdTymDomaci", idTymDomaci));
        command.Parameters.Add(new SqlParameter("@IdTymHost", idTymHost));

        // Table-Valued Parameter
        var table = new DataTable();
        table.Columns.Add("IdHrac", typeof(int));
        table.Columns.Add("IdTym", typeof(int));
        table.Columns.Add("JeKapitan", typeof(bool));
        table.Columns.Add("JeLibero", typeof(bool));
        table.Columns.Add("Hraje", typeof(bool));

        foreach (var hrac in sestava)
        {
            table.Rows.Add(hrac.IdHrac, hrac.IdTym, hrac.JeKapitan, hrac.JeLibero, hrac.Hraje);
        }

        var sestavaParam = new SqlParameter("@Sestava", table)
        {
            SqlDbType = SqlDbType.Structured,
            TypeName = "dbo.SestavaHraceTyp"
        };
        command.Parameters.Add(sestavaParam);

        if (connection.State != ConnectionState.Open)
            await connection.OpenAsync();

        await command.ExecuteNonQueryAsync();
    }
}

