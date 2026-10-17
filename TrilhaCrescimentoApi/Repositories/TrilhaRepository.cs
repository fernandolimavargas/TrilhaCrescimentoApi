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
            connection.Open(); 
            var transaction = connection.BeginTransaction();
            connection.Execute(sql, new { IdTime = idTime, IdUsuario = idUsuario , IdPasso = idPasso }, transaction : transaction);
            transaction.Commit();
        }
    }

    public bool VerificarCheckin(int idUsuario, int idPasso)
    {
        var sql = @"
            SELECT count(*)
                FROM checkin
                where data_checkin = CURRENT_DATE
                and id_usuario = @IdUsuario
                and id_passo = @IdPasso;";

        using (var connection = CreateConnection())
        {
            var dados =  connection.ExecuteScalar<int>(
                sql,
                new
                {
                    IdUsuario = idUsuario,
                    IdPasso = idPasso
                }
            );

            if (dados > 0)
            {
                return true;
            } else
            {
                return false; 
            }
        }
    }
}
