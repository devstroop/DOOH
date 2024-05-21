using DOOH.Server.Models.DOOHDB;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DOOH.Adboard.Services
{
    public class InterloopService
    {
        private readonly IConfiguration _configuration;

        public InterloopService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<string> ExecuteAsync(string command, CancellationToken cancellationToken)
        {
            var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            var isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
            var isMacOS = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

            var cmdBase = isWindows ? "cmd" : isLinux ? "bash" : isMacOS ? "bash" : throw new PlatformNotSupportedException();

            var cmdArgs = isWindows ? $"/c {command}" : isLinux || isMacOS ? $"-c \"{command}\"" : throw new PlatformNotSupportedException();

            var psi = new ProcessStartInfo(cmdBase, cmdArgs)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            var process = Process.Start(psi);
            if (process == null)
            {
                throw new InvalidOperationException("Failed to start process");
            }
            return process.StandardOutput.ReadToEndAsync(cancellationToken);
        }

        public async Task Run(string command, CancellationToken cancellationToken)
        {
            var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            var isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
            var isMacOS = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

            var cmdBase = isWindows ? "cmd" : isLinux ? "bash" : isMacOS ? "bash" : throw new PlatformNotSupportedException();

            var cmdArgs = isWindows ? $"/c {command}" : isLinux || isMacOS ? $"-c \"{command}\"" : throw new PlatformNotSupportedException();

            var psi = new ProcessStartInfo(cmdBase, cmdArgs)
            {
                RedirectStandardOutput = false,
                RedirectStandardError = false,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            var process = Process.Start(psi);
            if (process == null)
            {
                throw new InvalidOperationException("Failed to start process");
            }
            await process.WaitForExitAsync(cancellationToken);
        }

        public async Task<Tuple<DateTime, DateTime>> Play(Advertisement advertisement, CancellationToken cancellationToken)
        {
            var command = $"cvlc -vvv https://cdn.hallads.com/{advertisement.AttachmentKey} vlc://quit";
            var startedAt = DateTime.Now;
            await Run(command, cancellationToken);
            await Clear(cancellationToken);
            var stoppedAt = DateTime.Now;
            return new Tuple<DateTime, DateTime>(startedAt, stoppedAt);
        }

        public async Task Clear(CancellationToken cancellationToken)
        {
            await Run("setterm -clear >/dev/tty1;", cancellationToken);
        }
    }
}
