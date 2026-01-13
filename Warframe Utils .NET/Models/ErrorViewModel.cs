namespace Warframe_Utils_.NET.Models
{
    /// <summary>
    /// ErrorViewModel - Passes error information to the error page view.
    /// 
    /// When an unhandled exception occurs during request processing,
    /// the exception handler redirects to /Home/Error with this viewmodel.
    /// 
    /// The ErrorViewModel provides debugging information via the RequestId,
    /// which correlates the error in the UI with logs on the server.
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// Unique identifier for the failed request.
        /// Used to correlate the error page display with server logs.
        /// 
        /// Can be either:
        /// - Activity.Current.Id (in distributed tracing scenarios)
        /// - HttpContext.TraceIdentifier (standard request ID)
        /// 
        /// Displayed at the bottom of the error page for support purposes.
        /// Users can provide this ID when reporting issues.
        /// </summary>
        public string? RequestId { get; set; }

        /// <summary>
        /// Computed property - should the RequestId be displayed?
        /// Returns true only if RequestId has a value.
        /// 
        /// Used in the view to conditionally show request details:
        /// @if (Model.ShowRequestId) { /* display RequestId */ }
        /// 
        /// Hides the debug info from users if no ID is available.
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
