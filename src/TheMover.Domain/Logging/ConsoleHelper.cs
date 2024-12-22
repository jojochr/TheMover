using System.Runtime.InteropServices;

namespace TheMover.Domain.Logging {
    internal static class ConsoleHelper {
        public static void CreateConsole() {
            AllocConsole();

            // This Code is from: https://stackoverflow.com/questions/15604014/no-console-output-when-using-allocconsole-and-target-architecture-x86
            IntPtr defaultStdout = new(7);
            IntPtr currentStdout = GetStdHandle(STD_OUTPUT_HANDLE);

            if(currentStdout != defaultStdout) {
                // reset stdout
                SetStdHandle(STD_OUTPUT_HANDLE, defaultStdout);
            }

            // reopen stdout
            TextWriter writer = new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true, };
            Console.SetOut(writer);
        }

        // P/Invoke required:
        private const uint STD_OUTPUT_HANDLE = 0xFFFFFFF5;

        [DllImport(dllName: "kernel32.dll")]
        private static extern IntPtr GetStdHandle(uint nStdHandle);

        [DllImport(dllName: "kernel32.dll")]
        private static extern void SetStdHandle(uint nStdHandle, IntPtr handle);

        [DllImport(dllName: "kernel32")]
        private static extern bool AllocConsole();
    }
}
