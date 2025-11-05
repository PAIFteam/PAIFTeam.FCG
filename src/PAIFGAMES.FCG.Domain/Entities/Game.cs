namespace PAIFGAMES.FCG.Domain.Entities
{
    public class Game
    { 
    public Game(string name, decimal value)
    {
        UId = Guid.NewGuid();
        Name = name;
        Value = value;
    }
    public long Id { get; private set; }
    public Guid UId { get; private set; }
    public string Name { get; set; }
    public decimal Value { get; set; }
    public virtual ICollection<UserGame> UserGames { get; set; }
    }
}
