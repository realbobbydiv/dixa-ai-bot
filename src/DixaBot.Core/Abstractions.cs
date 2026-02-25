namespace DixaBot.Core;

public interface IResponseClient
{
    Task<EmailAnalysis> AnalyzeAsync(IncomingEmail email, CancellationToken ct);
    Task<ReplyDraft> CreateReplyAsync(IncomingEmail email, EmailAnalysis analysis, CancellationToken ct);
}

public interface IDixaClient
{
    Task SaveAuthStateAsync(CancellationToken ct);
    Task<IReadOnlyList<string>> ListClaimableConversationIdsAsync(int max, CancellationToken ct);
    Task ClaimConversationAsync(string conversationId, CancellationToken ct);
}