namespace NoiseCalculator.Domain.Entities
{
    public class Administrator
    {
        private string _username;

        protected Administrator() {  }
        public Administrator(string username)
        {
            _username = username;
        }

        public virtual string Username
        {
            get { return _username; }
        }
    }
}
