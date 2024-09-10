using System;

namespace Aurigma.DirectMail.Sample.WebHost.Interfaces;

/// <summary>
/// Logger used in the application startup process.
/// </summary>
public interface IStartupLogger : IDisposable
{
    /// <summary>
    /// Writes the diagnostic message at the <c>Debug</c> level.
    /// </summary>
    /// <param name="msg">Log message.</param>
    void Debug(string msg);

    /// <summary>
    /// Writes the diagnostic message at the <c>Warn</c> level.
    /// </summary>
    /// <param name="msg">Log message.</param>
    void Warn(string msg);

    /// <summary>
    /// Writes the diagnostic message and exception at the <c>Error</c> level.
    /// </summary>
    /// <param name="msg">A <see langword="string" /> to be written.</param>
    /// <param name="e">An exception to be logged.</param>
    void Error(Exception e, string msg);
}
