namespace DixaBot.Core;

public record IncomingEmail(string Subject, string Body);

public enum EmailCategory
{
    Unknown,
    Payments,
    Kyc,
    Rtw,
    Suspension,
    Technical,
    Other
}

public enum Priority
{
    Low,
    Medium,
    High
}

public record EmailAnalysis(
    EmailCategory Category,
    Priority Priority,
    bool NeedsEscalation,
    string Summary,
    Dictionary<string, string> Entities
);

public record ReplyDraft(
    string ReplyText,
    string Signature
);