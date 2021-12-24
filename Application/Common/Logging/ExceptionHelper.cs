namespace Application.Common.Logging
{
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public static class ExceptionHelper
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string GetCurrentMethod()
        {
            var st = new StackTrace();
            var sf = st.GetFrame(1);

            return sf?.GetMethod()?.Name;
        }
    }
}
