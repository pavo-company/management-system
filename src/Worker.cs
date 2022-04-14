namespace management_system
{
    public class Worker : Person
    {
        private static int _instanceId;
        private int Salary { get; set; }
         
        public Worker(string name, string email, int salary)
        {
            Name = name;
            Email = email;
            Salary = salary;
            Id = _instanceId;
            _instanceId++;
        }
        
        public override string ToString()
        {
            return string.Format("User:\n\tName: {0}\n\tEmail: {1}\n\tId: {2}", Name, Email, Id);
        }
    }
}