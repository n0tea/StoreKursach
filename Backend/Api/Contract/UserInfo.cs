namespace Backend.Api.Contract
{
    public class UserInfo
    {
        public required Guid Uid { get; init; }
        public required string Email  { get; init; }

        //public restring Password { get; init; }
        //public required int age { get; init; }

    }
}
