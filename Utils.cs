using System;
using System.IO;

namespace Mono8;

// thanks https://thecodebuzz.com/how-to-get-file-size-in-csharp/ 

public static class Utils {

    public static long GetFileSize(string FilePath)
    {
        if (File.Exists(FilePath))
        {
            return new FileInfo(FilePath).Length;
        }

        return 0;
    }

    public static string ToHex(this int value) {
        return String.Format("0x{0:X}", value);
    }

}