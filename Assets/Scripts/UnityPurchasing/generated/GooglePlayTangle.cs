// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("1VZYV2fVVl1V1VZWV+gz388gEvrXX+/y7CTFWIEstxt5KXvENzfO1d4txBUoh69RvzK6y5jrETpCUQhlbvVb0IaoXiHEOBcDUZ+sxegXFihtgtV35oqnh3v5HH5j9Cw2XIGwSVSTOKBDdJp3Hov9cLhpXZP1nzXS1RGAEnGbKus+7U61p5MXWKigh1fWOqOXDZJ5V/Mjc8Z6CoCdnY0fHgl33dAlp9SOUzVixfDwwX4ChoRgxTVO/8mfHFlxZ2EpvEGP8Wc1ONkQagsDJvQH1PsIaCjvOCNK6Qeg9mfVVnVnWlFefdEf0aBaVlZWUldUyTVFDn4cE9MbZW+5gbpfJD/KDx7xjsUyRBr02VR45HAMLe0xdfmhfzo1hfvc6VSoQlVUVldW");
        private static int[] order = new int[] { 1,3,7,13,4,7,6,12,12,9,10,13,13,13,14 };
        private static int key = 87;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
