namespace management_system
{
    public class Person
    {
        protected int Id { get; set; }
        protected string Name { get; set; }
        protected string Surname { get; set; }
        protected string Email { get; set; }
        public override string ToString()
        {
            return string.Format("User:\n\tName: {0}\n\tEmail: {1}\n\tId: {2}", Name, Email, Id);
        }
    }
}