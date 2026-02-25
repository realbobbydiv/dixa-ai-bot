using DixaBot.Core;
using DixaBot.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;

var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true)
    .Build();

var services = new ServiceCollection();

services.AddLogging(b => b.AddConsole());

var dixaOpt = config.GetSection("Dixa").Get<DixaOptions>() ?? new DixaOptions();
services.AddSingleton(dixaOpt);

services.AddSingleton<IDixaClient, PlaywrightDixaClient>();
services.AddSingleton<IResponseClient, MockResponseClient>();

await using var sp = services.BuildServiceProvider();
var log = sp.GetRequiredService<ILoggerFactory>().CreateLogger("DixaBot");

if (args.Length == 0)
{
    PrintHelp();
    return;
}

var cmd = args[0].Trim().ToLowerInvariant();

switch (cmd)
{
    case "auth":
        await AuthAsync(sp, log);
        break;

    case "dry-run":
        await DryRunAsync(sp, log);
        break;

    case "run":
        await RunAsync(sp, log);
        break;

    case "classify":
        await ClassifyOnceAsync(sp, log);
        break;

    case "draft":
        await DraftOnceAsync(sp, log);
        break;

    default:
        PrintHelp();
        break;
}

static void PrintHelp()
{
        Console.WriteLine(@"
Dixa Bot (skeleton)

Commands:
    auth       Opens browser to login via Google and saves storageState.json
    dry-run    Lists claimable conversations (placeholder until selectors are added)
    run        Claims conversations (placeholder until selectors are added)
    classify   Demo: analyze a pasted email
    draft      Demo: analyze + create a reply

Examples:
    dotnet run --project src/DixaBot.Cli auth
    dotnet run --project src/DixaBot.Cli classify
");
}

static async Task AuthAsync(IServiceProvider sp, ILogger log)
{
    var dixa = sp.GetRequiredService<IDixaClient>();
    log.LogInformation("Starting AUTH flow (Google login)...");
    await dixa.SaveAuthStateAsync(CancellationToken.None);
    log.LogInformation("Saved storageState.json (local file).");
}

static async Task DryRunAsync(IServiceProvider sp, ILogger log)
{
    var dixa = sp.GetRequiredService<IDixaClient>();
    log.LogInformation("Dry-run: listing claimable conversations (placeholder)...");
    var ids = await dixa.ListClaimableConversationIdsAsync(
        max: sp.GetRequiredService<DixaOptions>().MaxClaimsPerRun,
        ct: CancellationToken.None);

    if (ids.Count == 0)
        log.LogWarning("No claimable conversations found (or selectors not implemented yet).");
    else
        foreach (var id in ids) log.LogInformation("Would claim: {Id}", id);
}

static async Task RunAsync(IServiceProvider sp, ILogger log)
{
    var dixa = sp.GetRequiredService<IDixaClient>();
    var max = sp.GetRequiredService<DixaOptions>().MaxClaimsPerRun;

    log.LogInformation("Run: claiming up to {Max} conversations (placeholder)...", max);

    var ids = await dixa.ListClaimableConversationIdsAsync(max, CancellationToken.None);

    foreach (var id in ids.Take(max))
    {
        log.LogInformation("Claiming: {Id}", id);
        await dixa.ClaimConversationAsync(id, CancellationToken.None);
    }

    log.LogInformation("Done.");
}

static async Task ClassifyOnceAsync(IServiceProvider sp, ILogger log)
{
    var client = sp.GetRequiredService<IResponseClient>();

    Console.Write("Subject: ");
    var subject = Console.ReadLine() ?? "";

    Console.WriteLine("Body (end with an empty line):");
    var lines = new List<string>();
    while (true)
    {
        var line = Console.ReadLine();
        if (string.IsNullOrEmpty(line)) break;
        lines.Add(line);
    }

    var email = new IncomingEmail(subject, string.Join('\n', lines));
    var analysis = await client.AnalyzeAsync(email, CancellationToken.None);

    log.LogInformation("Category: {Category}, Priority: {Priority}", analysis.Category, analysis.Priority);
    log.LogInformation("Summary: {Summary}", analysis.Summary);
}

static async Task DraftOnceAsync(IServiceProvider sp, ILogger log)
{
    var client = sp.GetRequiredService<IResponseClient>();

    Console.Write("Subject: ");
    var subject = Console.ReadLine() ?? "";

    Console.WriteLine("Body (end with an empty line):");
    var lines = new List<string>();
    while (true)
    {
        var line = Console.ReadLine();
        if (string.IsNullOrEmpty(line)) break;
        lines.Add(line);
    }

    var email = new IncomingEmail(subject, string.Join('\n', lines));
    var analysis = await client.AnalyzeAsync(email, CancellationToken.None);
    var reply = await client.CreateReplyAsync(email, analysis, CancellationToken.None);

    Console.WriteLine("\n--- REPLY DRAFT ---\n");
    Console.WriteLine(reply.ReplyText);
}