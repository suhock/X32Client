using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Suhock.X32.Nodes;
using Suhock.X32.Routing.Config;
using Suhock.X32.Types;
using Suhock.X32.Types.Enums;
using Suhock.X32.Types.Floats;

namespace Suhock.X32.Routing;

internal sealed class X32RoutingApplication : IDisposable
{
    private readonly IX32Client _client;
    private readonly IEnumerable<SendConfig> _sends;
    private readonly IEnumerable<TemplateConfig> _templates;
    private readonly IEnumerable<ChannelConfig> _channels;

    public ILogger<X32RoutingApplication>? Logger { get; init; }

    public X32RoutingApplication(IX32Client client, IEnumerable<SendConfig> sends, 
        IEnumerable<TemplateConfig> templates, IEnumerable<ChannelConfig> channels)
    {
        _sends = sends;
        _templates = templates;
        _client = client;
        _channels = channels;
    }

    public void Dispose()
    {
        // _client.Dispose();
    }

    public async Task Run()
    {
        var channels = _channels.ToArray();

        _client.Run();

        var channelHeadAmpStates = await QueryChannelHeadAmpStates(channels).ConfigureAwait(false);

        foreach (var channel in channels)
        {
            LogUpdateChannel(channel, channelHeadAmpStates[channel]);
            await UpdateUserInRouting(channel).ConfigureAwait(false);

            var channelNode = _client.Root.Channel(channel.Id);

            await channelNode.Config.Source(SourceExtensions.In(channel.Id)).ConfigureAwait(false);
            await channelNode.Config.Name(channel.Name).ConfigureAwait(false);

            if (channel.Id % 2 == 1)
            {
                await _client.Root.Config.ChannelLink(channel.Id, channel.Link).ConfigureAwait(false);
            }

            var template = _templates.FirstOrDefault(template => channel.Template.Equals(template.Name));

            if (template is null)
            {
                Logger?.LogWarning("Template '{template}' not found", channel.Template);
                continue;
            }

            await ApplyTemplateToChannel(template, channelNode).ConfigureAwait(false);
            await UpdateHeadampState(GetHeadAmpIndex(channel), channelHeadAmpStates[channel]).ConfigureAwait(false);
        }
    }

    private async Task<Dictionary<ChannelConfig, HeadAmpState?>> QueryChannelHeadAmpStates(
        IReadOnlyCollection<ChannelConfig> channels)
    {
        var tasks = new Dictionary<ChannelConfig, Task<HeadAmpState?>>(channels.Count);
        var channelHeadAmpStates = new Dictionary<ChannelConfig, HeadAmpState?>(channels.Count);

        foreach (var channel in channels)
        {
            tasks[channel] = LoadHeadampState(channel);
        }

        foreach (var (channel, task) in tasks)
        {
            channelHeadAmpStates[channel] = await task.ConfigureAwait(false);
        }

        return channelHeadAmpStates;
    }

    private async Task ApplyTemplateToChannel(TemplateConfig template, ChannelNode channelNode)
    {
        foreach (var send in _sends)
        {
            var mixOn = template.Sends.Contains(send.Id);
            await channelNode.Mix.Send(send.Id).On(mixOn).ConfigureAwait(false);
            await channelNode.Mix.Send(send.Id).TapType(send.TapType).ConfigureAwait(false);
        }

        await channelNode.Mix.On(template.On).ConfigureAwait(false);
        await channelNode.Mix.StereoSendOn(template.StereoSendOn).ConfigureAwait(false);
        await channelNode.Group.DcaGroups(template.DcaGroups).ConfigureAwait(false);
        await channelNode.Config.Color(template.Color).ConfigureAwait(false);
        await channelNode.Group.MuteGroups(template.MuteGroups).ConfigureAwait(false);

        if (!template.On)
        {
            await channelNode.Mix.Fader(FaderFineLevel.MinValue).ConfigureAwait(false);
        }
    }

    private async Task UpdateUserInRouting(ChannelConfig channel)
    {
        await _client.Root.Config.UserRouting.In(channel.Id,
            UserRoutingInputSource.FromInt(X32Util.ConvertStringToUserInIndex(channel.Source))).ConfigureAwait(false);
    }

    private static int GetHeadAmpIndex(ChannelConfig channel)
    {
        var userInIndex = X32Util.ConvertStringToUserInIndex(channel.Source);

        return X32Util.ConvertUserInIndexToHeadampIndex(userInIndex);
    }

    private void LogUpdateChannel(ChannelConfig channel, HeadAmpState? headAmpState)
    {
        Logger?.LogInformation("Channel Id={channelId}, Source={channelSource}", channel.Id, channel.Source);

        if (headAmpState is not null)
        {
            Logger?.LogInformation("Gain={gain:+0.0;-0.0}dB, 48V={phantom}",
                headAmpState.Gain.ToString(),
                headAmpState.Phantom ? "On" : "Off");
        }
    }

    private async Task UpdateHeadampState(int targetHeadampIndex, HeadAmpState? headampState)
    {
        if (headampState is null || targetHeadampIndex < 0)
        {
            return;
        }

        var node = _client.Root.HeadAmp(targetHeadampIndex);

        await Task.WhenAll(
            node.Gain(headampState.Gain),
            node.Phantom(headampState.Phantom)
        ).ConfigureAwait(false);
    }

    private readonly IDictionary<int, HeadAmpState> _headAmpStateCache = new Dictionary<int, HeadAmpState>();

    private async Task<HeadAmpState?> LoadHeadampState(ChannelConfig channel)
    {
        var haIndex = await GetHeadampIndex(channel).ConfigureAwait(false);

        if (haIndex <= -1)
        {
            return null;
        }

        if (!_headAmpStateCache.ContainsKey(haIndex))
        {
            _headAmpStateCache[haIndex] = await RetrieveHeadampState(haIndex).ConfigureAwait(false);
        }

        return _headAmpStateCache[haIndex];
    }

    private async Task<int> GetHeadampIndex(ChannelConfig channel)
    {
        return await _client.Root.HeadAmpIndex(SourceExtensions.In(channel.Id)).ConfigureAwait(false);
    }

    private async Task<HeadAmpState> RetrieveHeadampState(int index)
    {
        var node = _client.Root.HeadAmp(index);
        var gainTask = node.Gain();
        var phantomTask = node.Phantom();

        return new HeadAmpState(
            await gainTask.ConfigureAwait(false),
            await phantomTask.ConfigureAwait(false)
        );
    }
}