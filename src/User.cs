namespace management_system
{
    public class User
    {
        private string Name { get; set; }
        private string Email { get; set; }

        public User(string name, string email)
        {
            Name = name;
            Email = email;
        }
        public override string ToString()
        {
            return string.Format("User:\n" +
                                 "\tName: {0}\n" +
                                 "\tEmail: {1}", Name, Email);
        }

    }
}