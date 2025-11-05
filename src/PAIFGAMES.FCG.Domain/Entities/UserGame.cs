namespace PAIFGAMES.FCG.Domain.Entities
{
    public class UserGame
    {
        public long UserId { get; set; }
        public long GameId { get; set; }
        public virtual IdentityUserCustom User { get; set; }
        public virtual Game Game { get; set; }
    }
}
