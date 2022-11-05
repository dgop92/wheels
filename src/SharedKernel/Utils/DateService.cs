using System;
using System.Globalization;

namespace SharedKernel.Utils;

public static class DateService
{
    public static string NowAsString => DateTime.UtcNow.ToString(
        "yyyy-MM-ddTHH\\:mm\\:ss.fffffffzzz",
        CultureInfo.InvariantCulture
    );
}
