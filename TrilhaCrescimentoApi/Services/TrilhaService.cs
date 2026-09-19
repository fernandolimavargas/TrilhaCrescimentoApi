public class TrilhaService
{
    private readonly TrilhaRepository _trilhaRepository;
    public TrilhaService(TrilhaRepository trilhaRepository)
    {
        _trilhaRepository = trilhaRepository;
    }

    public List<Times> BuscarTimes()
    {
        return _trilhaRepository.BuscarTimes();
    }

    public List<Passos> BuscarPassos()
    {
        return _trilhaRepository.BuscarPassos();
    }
    
    public bool Checkin(int idTime, int idUsuario, int idPasso)
    {
        var jaFezCheckin = _trilhaRepository.VerificarCheckin(idUsuario);
        if (jaFezCheckin)
            return false ;

        _trilhaRepository.Checkin(idTime, idUsuario, idPasso);
        return true; 
    }

    public bool VerificarCheckin(int idUsuario)
    {
        return _trilhaRepository.VerificarCheckin(idUsuario);
    }
}