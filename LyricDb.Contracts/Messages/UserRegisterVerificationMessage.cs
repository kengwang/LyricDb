using System;
using LyricDb.Contracts.Models;

namespace LyricDb.Contracts.Messages;

public class UserRegisterVerificationMessage
{
    public required UserInfo User { get; set; }
    public required string ActivationToken { get; set; }
}