# DVD26

DVD26 is a Windows-only .NET 8 WPF application for archiving **unprotected,
legally copyable, or self-authored DVDs**. This first milestone provides the
project structure, prerequisite validation, disc/title and track selection UI,
and a reviewable FFmpeg command plan. It intentionally does not decrypt discs,
load decryption libraries, or bypass copy protection.

## Prerequisites

1. Windows 10 or 11 (x64) and the [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0).
2. A current Windows build of `ffmpeg.exe` and `ffprobe.exe` from the
   [FFmpeg download page](https://ffmpeg.org/download.html). Keep both tools in
   the extracted build's `bin` directory so you can select them on first run.
3. A DVD drive and a disc that you are legally permitted to copy. Commercial
   encrypted discs are outside this application's scope.

Do **not** upload the FFmpeg download or its executables to GitHub. The files are
large external dependencies and are intentionally ignored by this repository.
On first run, DVD26 asks you to select `ffmpeg.exe` and `ffprobe.exe` from the
`bin` directory of your extracted FFmpeg download. It stores only their paths in
`%LOCALAPPDATA%\DVD26\settings.json`; it does not copy or upload the tools. Adding
the FFmpeg `bin` directory to `PATH` remains useful for command-line verification.

Verify the tools from PowerShell:

```powershell
dotnet --version
ffmpeg -version
ffprobe -version
```

## Build and run

```powershell
dotnet restore DVD26.sln
dotnet build DVD26.sln
dotnet run --project src/DVD26/DVD26.csproj
```

The project targets `net8.0-windows` and cannot run on Linux or macOS.

## Planned workflow

1. Select a DVD drive or an existing `VIDEO_TS` folder and scan it with
   `ffprobe` metadata.
2. DVD26 proposes the longest title as the main feature while retaining manual
   title selection.
3. Choose audio and soft-subtitle tracks. MKV mode stream-copies video, audio,
   and subtitles without re-encoding. MP4 mode encodes video with H.264 and
   preserves compatible selectable subtitle streams (or reports an incompatible
   track instead of burning it in).
4. Review and run the generated command. Copy mode can reproduce unprotected
   media as an ISO or `VIDEO_TS` folder without transcoding.

The initial milestone does not execute archive jobs yet; the **Start archive**
button remains disabled until a disc scan and command-review implementation is
connected. FFmpeg/ffprobe remain external processes and are not redistributed.

## Legal and technical scope

DVD26 will stop and explain the limitation when a source cannot be read by the
operating system and FFmpeg. It will not include CSS/AACS decryption, key
databases, or instructions for circumventing access controls. Laws vary by
jurisdiction; users are responsible for ensuring they have permission to copy
their media.

### About `libdvdcss.dll`

Do not add `libdvdcss.dll` to this repository or the application directory.
DVD26 deliberately does not redistribute, load, or invoke it. The filename is
ignored by Git, and the application reports an unsupported-component warning if
it finds the DLL beside the executable or in its working directory. Remove the
file to continue using DVD26 with readable, unprotected sources.
