using System;
using System.Collections.Generic;

namespace SlackMVCBack;

public partial class Message
{
    public int Id { get; set; }

    public int AuthorId { get; set; }

    public int ThreadId { get; set; }

    public string Content { get; set; } = null!;

    public DateTime Date { get; set; }

    public virtual User Author { get; set; } = null!;

    public virtual Thread Thread { get; set; } = null!;
}
