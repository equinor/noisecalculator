namespace NoiseCalculator.Domain.Entities
{
    public class Role
    {
        public virtual int Id { get; private set; }
        public virtual string Title { get; set; }
        public virtual RoleTypeEnum RoleType { get; set; }
        public virtual string SystemTitle { get; set; }
    }
}
