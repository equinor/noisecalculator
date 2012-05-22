using NoiseCalculator.Domain.Enums;

namespace NoiseCalculator.Domain.Entities
{
    public class Role
    {
        public virtual int Id { get; set; }
        public virtual RoleDefinition RoleDefinition { get; set; }
        public virtual string Title { get; set; }
        public virtual RoleTypeEnum RoleType { get; set; }
        public virtual string SystemTitle { get; set; }
        public virtual string CultureName { get; set; }
    }
}
