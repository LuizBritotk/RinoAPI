    using System;

    namespace Rino.Domain.Entities
    {
        public class User
        {
            public Guid Id { get; set; }
            public string Email { get; set; }
            public string Login { get; set; }
            public string PasswordHash { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string PhoneNumber { get; set; }
            public DateTime? DateOfBirth { get; set; }
            public string? TokenJWT { get; set; }
    }
    }
