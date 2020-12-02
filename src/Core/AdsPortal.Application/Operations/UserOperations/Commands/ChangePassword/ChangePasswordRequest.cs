namespace AdsPortal.Application.Operations.UserOperations.Commands.ChangePassword
{
    using System;
    using AdsPortal.Application.OperationsAbstractions;

    public class ChangePasswordRequest : IOperation
    {
        public Guid UserId { get; set; }
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
    }
}
