using Dapper;
using TrilhaCrescimentoApi.Data;

public class TrilhaRepository : ConexaoDapper
{
    public TrilhaRepository(IConfiguration configuration) : base(configuration) { }

    public List<Times> BuscarTimes()
    {
        var sql = @"SELECT id, nome FROM times";

        using (var connection = CreateConnection())
        {
            var times = connection.Query<Times>(sql).ToList();
            return times;
        }
    }

    public List<Passos> BuscarPassos()
    {
        var passos = @"SELECT id, nome FROM passos";
        using (var connection = CreateConnection())
        {
            var passosList = connection.Query<Passos>(passos).ToList();
            return passosList;
        }
    }

    public void Checkin(int idTime, int idUsuario, int idPasso)
    {
        var sql = @"INSERT INTO checkin (id_time, id_usuario, id_passo) VALUES (@IdTime, @IdUsuario, @IdPasso)";    

        using (var connection = CreateConnection())
        {
            connection.Execute(sql, new { IdTime = idTime, IdUsuario = idUsuario , IdPasso = idPasso });
        }
    }

    public bool VerificarCheckin(int idUsuario)
    {
        var sql = @" SELECT COUNT(*)
                FROM checkin
                WHERE id_usuario = @IdUsuario
                AND DATE(data_checkin) = CURRENT_DATE;";

        using (var connection = CreateConnection())
        {
            var count = connection.ExecuteScalar<int>(sql, new { IdUsuario = idUsuario });
            return count > 0;
        }
    }
}
