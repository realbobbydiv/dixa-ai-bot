using DixaBot.Core;

namespace DixaBot.Infrastructure;

public sealed class MockResponseClient : IResponseClient
{
    public Task<EmailAnalysis> AnalyzeAsync(IncomingEmail email, CancellationToken ct)
    {
        // Simple heuristic placeholder (for skeleton + tests)
        var body = (email.Subject + " " + email.Body).ToLowerInvariant();

        var category =
            body.Contains("rtw") || body.Contains("share code") ? EmailCategory.Rtw :
            body.Contains("kyc") || body.Contains("identity") ? EmailCategory.Kyc :
            body.Contains("payment") || body.Contains("earnings") ? EmailCategory.Payments :
            body.Contains("suspend") ? EmailCategory.Suspension :
            body.Contains("login") || body.Contains("otp") ? EmailCategory.Technical :
            EmailCategory.Other;

        var priority = body.Contains("urgent") || body.Contains("today") ? Priority.High : Priority.Medium;

        return Task.FromResult(new EmailAnalysis(
            category,
            priority,
            NeedsEscalation: false,
            Summary: "Skeleton analysis (mock).",
            Entities: new Dictionary<string, string>()
        ));
    }

    public Task<ReplyDraft> CreateReplyAsync(IncomingEmail email, EmailAnalysis analysis, CancellationToken ct)
    {
        var text =
$@"Hi,

Thanks for reaching out. We’ve received your request and we’re reviewing it.

Category detected: {analysis.Category}
Priority: {analysis.Priority}

Best regards,
I Dont wanna tell you my company :D ";

        return Task.FromResult(new ReplyDraft(text, "I Dont wanna tell you my company :D"));
    }
}