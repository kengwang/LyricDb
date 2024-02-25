using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LyricDb.Contracts.Models;

public class Message
{
    [Key]
    public Guid Id { get; set; }
    public int TargetRole { get; set; } = -1;
    public List<Guid> TargetUserIds { get; set; } = [];
    public string Content { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public List<Guid> ReadUsers { get; set; } = [];
}