﻿using System.ComponentModel.DataAnnotations;
using Shared.Enums;

namespace Shared.UserModels;

public record RegisterRequest(
    [Required, StringLength(50)]
    string FirstName,
    [Required, StringLength(50)]
    string LastName,
    [Required, DataType(DataType.EmailAddress)]
    string EmailAddress,
    string Password,
    string Role);