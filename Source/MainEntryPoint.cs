using System.Runtime.InteropServices;
using System.Text;

class Program
{

    #region Structs

    [StructLayout(LayoutKind.Sequential)]
    public struct STARTUPINFO
    {
        public uint cb;
        public string lpReserved;
        public string lpDesktop;
        public string lpTitle;
        public uint dwX;
        public uint dwY;
        public uint dwXSize;
        public uint dwYSize;
        public uint dwXCountChars;
        public uint dwYCountChars;
        public uint dwFillAttribute;
        public uint dwFlags;
        public ushort wShowWindow;
        public ushort cbReserved2;
        public IntPtr lpReserved2;
        public IntPtr hStdInput;
        public IntPtr hStdOutput;
        public IntPtr hStdError;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CPINFO
    {
        public uint MaxCharSize;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] DefaultChar;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        public byte[] LeadByte;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct EXCEPTION_REGISTRATION_RECORD
    {
        public IntPtr Next;
        public IntPtr Handler;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct NT_TIB
    {
        public IntPtr ExceptionList;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct TEB
    {
        public NT_TIB NtTib;
    }

    #endregion

    #region DLLImports

    [DllImport("kernel32.dll")]
    private static extern uint GetVersion();

    [DllImport("kernel32.dll")]
    private static extern IntPtr GetCommandLineA();

    [DllImport("kernel32.dll")]
    private static extern IntPtr GetModuleHandleA(IntPtr lpModuleName);

    [DllImport("kernel32.dll")]
    private static extern void GetStartupInfoA(out STARTUPINFO lpStartupInfo);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
    private static extern IntPtr GetEnvironmentStringsW();

    [DllImport("kernel32.dll")]
    private static extern IntPtr GetEnvironmentStrings();

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
    private static extern int WideCharToMultiByte(
        uint CodePage,
        uint dwFlags,
        IntPtr lpWideCharStr,
        int cchWideChar,
        StringBuilder lpMultiByteStr,
        int cbMultiByte,
        IntPtr lpDefaultChar,
        IntPtr lpUsedDefaultChar);

    [DllImport("kernel32.dll")]
    private static extern bool FreeEnvironmentStringsW(IntPtr lpszEnvironmentBlock);

    [DllImport("kernel32.dll")]
    private static extern bool FreeEnvironmentStringsA(IntPtr lpszEnvironmentBlock);

    [DllImport("kernel32.dll")]
    private static extern IntPtr HeapCreate(uint flOptions, uint dwInitialSize, uint dwMaximumSize);

    [DllImport("kernel32.dll")]
    private static extern bool HeapDestroy(IntPtr hHeap);

    [DllImport("kernel32.dll")]
    private static extern void ExitProcess(uint uExitCode);

    [DllImport("kernel32.dll")]
    private static extern uint TlsAlloc();

    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool TlsSetValue(uint dwTlsIndex, IntPtr lpTlsValue);

    [DllImport("kernel32.dll")]
    private static extern uint GetCurrentThreadId();

    [DllImport("kernel32.dll")]
    private static extern IntPtr GetStdHandle(int nStdHandle);

    [DllImport("kernel32.dll")]
    private static extern int GetFileType(IntPtr hFile);

    [DllImport("kernel32.dll")]
    private static extern uint SetHandleCount(uint uNumber);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetCPInfo(uint CodePage, out CPINFO lpCPInfo);

    [DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
    private static extern uint GetModuleFileNameA(IntPtr hModule, IntPtr lpFilename, uint nSize);

    #endregion

    #region Globals

    private static IntPtr data_56b5e8;
    private static int data_56b5ec;
    private static IntPtr data_56b600;
    private static uint data_56b700;
    private static uint data_50c6e4;
    private static IntPtr data_569994 = IntPtr.Zero;
    private static IntPtr data_569a20 = IntPtr.Zero;
    private static int data_56b704 = 0;
    private static IntPtr data_569cc0 = IntPtr.Zero;
    private static uint data_56b3c0;
    private static int data_56b5e4;
    private static byte[] data_56b4e0 = new byte[256];
    private static int data_56b3d0;
    private static int data_56b3dc;
    private static int data_569cd0;
    private static int data_56b3d4;
    private static IntPtr data_56b708 = IntPtr.Zero; // Beispielinitialisierung, anpassen nach Bedarf
    private static IntPtr data_569bbc = Marshal.AllocHGlobal(0x104); // Puffer für den Modulnamen
    private static IntPtr data_56b714 = IntPtr.Zero; // Beispielinitialisierung, anpassen nach Bedarf
    private static IntPtr data_569a30;
    private static IntPtr data_569a18;
    private static int data_569a14;
    private static readonly uint data_56999c = 1;
    private static IntPtr data_50c230 = IntPtr.Zero;
    private static int data_4fe05c = 0;
    private static int data_4fe000 = 0;
    private static readonly IntPtr data_56b4e1;

    #endregion


    private delegate void FunctionDelegate();


    static int Main()
    {
        int var_4 = 0;
        int var_8 = unchecked((int)0xffffffff);
        int var_c = 0x4fb998;
        int var_10 = 0x4ec734;

        // Windows TEB and Exception Handling cannot be directly translated
        // This part is highly system-specific and is not directly accessible from C#
        // fsbase->NtTib.ExceptionList and similar operations skipped

        int ebx = 0;
        int var_70 = ebx;
        int esi = 0;
        int var_74 = esi;
        int edi = 0;
        int var_78 = edi;
        int[] var_1c = { var_78 };

        uint eax_1 = GetVersion();
        byte edx = (byte)((eax_1 >> 8) & 0xFF);
        int data_569a10 = edx;
        uint ecx_1 = eax_1;
        int data_569a0c = (int)ecx_1;
        int data_569a08 = ((int)(ecx_1 << 8) + edx);
        int data_569a04 = (int)(eax_1 >> 16);

        if (sub_4ec5de(1) == 0)
        {
            sub_4e71cb(0x1c);
            return 0;
        }

        if (sub_4eb167() == 0)
        {
            sub_4e71cb(0x10);
            return 0;
        }

        var_8 = 0;
        sub_4ea927();

        data_56b714 = GetCommandLineA();
        IntPtr eax_6 = sub_4ec337();
        int data_569994 = (int)eax_6.ToInt64();

        sub_4ec0ea(data_569994);
        sub_4ec031();
        sub_4ea109();

        byte var_34 = 0;
        STARTUPINFO lpStartupInfo;
        GetStartupInfoA(out lpStartupInfo);

        IntPtr eax_7 = sub_4ebfd9();
        uint eax_8;

        if ((var_34 & 1) == 0)
        {
            eax_8 = 0xa;
        }
        else
        {
            short var_30 = 0; // var_30 should be appropriately calculated
            eax_8 = (uint)var_30;
        }

        sub_403487(GetModuleHandleA(IntPtr.Zero), 0, eax_7, eax_8);

        int var_64 = 0;
        sub_4ea136(0);

        int[] var_18 = { 0 };
        int ecx_7 = Marshal.ReadInt32(new IntPtr(var_18[0]));
        int var_6c = ecx_7;

        return sub_4ebe61(ecx_7, var_18);
    }

    private static int sub_4ec5de(int arg1)
    {
        uint flOptions = (uint)(arg1 == 0 ? 0 : 1); // Annahme: flOptions basiert auf dem Argument, wenn arg1 0 ist, dann 0, sonst 1
        IntPtr eax = HeapCreate(flOptions, 0x1000, 0);
        data_56b5e8 = eax;

        if (eax != IntPtr.Zero)
        {
            int eax_1 = sub_4ec496();
            data_56b5ec = eax_1;
            IntPtr eax_2 = IntPtr.Zero;

            if (eax_1 != 3)
            {
                if (eax_1 != 2)
                {
                    return 1;
                }
                eax_2 = sub_4edfa1();
            }
            else
            {
                eax_2 = sub_4ed45a(0x3f8);
            }

            if (eax_2 != IntPtr.Zero)
            {
                return 1;
            }

            HeapDestroy(data_56b5e8);
        }

        return 0;
    }

    private static int sub_4ec496()
    {
        // Implementiere die Logik von sub_4ec496
        return 0;
    }

    private static IntPtr sub_4edfa1()
    {
        // Implementiere die Logik von sub_4edfa1
        return IntPtr.Zero;
    }

    private static IntPtr sub_4ed45a(int size)
    {
        // Implementiere die Logik von sub_4ed45a
        return IntPtr.Zero;
    }

    private static void sub_4e71cb(uint arg1)
    {
        if (data_56999c == 1)
        {
            sub_4ec80c();
        }

        sub_4ec845(arg1);
        ExitProcess(0xff);
    }

    private static void sub_4ec80c()
    {
        // Implementiere die Logik von sub_4ec80c
        Console.WriteLine("sub_4ec80c aufgerufen.");
    }

    private static void sub_4ec845(uint arg1)
    {
        // Implementiere die Logik von sub_4ec845
        Console.WriteLine($"sub_4ec845 aufgerufen mit Argument: {arg1}");
    }

    private static int sub_4eb167()
    {
        sub_4ebdc2();
        uint eax = TlsAlloc();
        data_50c6e4 = eax;

        if (eax != 0xffffffff)
        {
            IntPtr lpTlsValue = sub_4ef3ac(1, 0x74);

            if (lpTlsValue != IntPtr.Zero && TlsSetValue(data_50c6e4, lpTlsValue))
            {
                sub_4eb1bb(lpTlsValue);
                uint eax_2 = GetCurrentThreadId();
                Marshal.WriteInt32(lpTlsValue, 4, unchecked((int)0xffffffff));
                Marshal.WriteInt32(lpTlsValue, 0, (int)eax_2);
                return 1;
            }
        }

        return 0;
    }

    private static void sub_4ebdc2()
    {
        // Implementiere die Logik von sub_4ebdc2
        Console.WriteLine("sub_4ebdc2 aufgerufen.");
    }

    private static IntPtr sub_4ef3ac(int arg1, int arg2)
    {
        // Implementiere die Logik von sub_4ef3ac
        // Hier könnte beispielsweise Speicher allokiert werden
        IntPtr allocatedMemory = Marshal.AllocHGlobal(arg2);
        return allocatedMemory;
    }

    private static void sub_4eb1bb(IntPtr lpTlsValue)
    {
        // Implementiere die Logik von sub_4eb1bb
        Console.WriteLine("sub_4eb1bb aufgerufen.");
    }

    private static uint sub_4ea927()
    {
        IntPtr esi = sub_4e85fc(0x480);

        if (esi == IntPtr.Zero)
        {
            sub_4e71a6(0x1b);
        }

        data_56b600 = esi;
        data_56b700 = 0x20;

        IntPtr i = esi + 0x480;
        while (esi.ToInt64() < i.ToInt64())
        {
            Marshal.WriteByte(esi, 4, 0);
            Marshal.WriteInt32(esi, unchecked((int)0xffffffff));
            Marshal.WriteInt32(esi, 8, 0);
            Marshal.WriteByte(esi, 5, 0xa);
            esi += 0x24;
        }

        STARTUPINFO lpStartupInfo;
        GetStartupInfoA(out lpStartupInfo);

        // Die weiteren Schritte sind abhängig von den spezifischen Details von lpStartupInfo
        // In C# müssen diese Variablen und die Logik, die auf sie zugreift, entsprechend implementiert werden
        // Zum Beispiel:
        short var_1a = 0; // Beispielwert, muss aus STARTUPINFO oder einem anderen Kontext ermittelt werden
        IntPtr var_18 = IntPtr.Zero; // Beispielwert, muss ebenfalls ermittelt werden

        if (var_1a != 0 && var_18 != IntPtr.Zero)
        {
            int i_1 = Marshal.ReadInt32(var_18);
            IntPtr ebx_1 = var_18 + 4;
            IntPtr var_8_1 = ebx_1 + i_1;

            if (i_1 >= 0x800)
            {
                i_1 = 0x800;
            }

            if (data_56b700 < i_1)
            {
                IntPtr[] data_56b604 = new IntPtr[32]; // Beispielhafte Initialisierung, abhängig von der Verwendung
                int index = 0;

                do
                {
                    IntPtr eax_4 = sub_4e85fc(0x480);

                    if (eax_4 == IntPtr.Zero)
                    {
                        i_1 = (int)data_56b700;
                        break;
                    }

                    data_56b700 += 0x20;
                    data_56b604[index++] = eax_4;

                    IntPtr j = eax_4 + 0x480;
                    while (eax_4.ToInt64() < j.ToInt64())
                    {
                        Marshal.WriteByte(eax_4, 4, 0);
                        Marshal.WriteInt32(eax_4, unchecked((int)0xffffffff));
                        Marshal.WriteInt32(eax_4, 8, 0);
                        Marshal.WriteByte(eax_4, 5, 0xa);
                        eax_4 += 0x24;
                    }
                } while (data_56b700 < i_1);
            }

            int esi_2 = 0;
            if (i_1 > 0)
            {
                do
                {
                    IntPtr hFile_1 = Marshal.ReadIntPtr(var_8_1);

                    if (hFile_1 != new IntPtr(unchecked((int)0xffffffff)))
                    {
                        byte eax_5 = Marshal.ReadByte(ebx_1);

                        if ((eax_5 & 1) != 0)
                        {
                            int eax_6 = (eax_5 & 8) == 0 ? GetFileType(hFile_1) : 0;
                            if ((eax_5 & 8) != 0 || eax_6 != 0)
                            {
                                int offset = (esi_2 >> 5) * 0x24 + (esi_2 & 0x1f) * 0x24;
                                IntPtr eax_10 = data_56b600 + offset;
                                Marshal.WriteIntPtr(eax_10, hFile_1);
                                Marshal.WriteByte(eax_10, 4, eax_5);
                            }
                        }
                    }

                    var_8_1 += 4;
                    esi_2++;
                    ebx_1 += 1;
                } while (esi_2 < i_1);
            }
        }

        for (int i_2 = 0; i_2 < 3; i_2++)
        {
            int offset = i_2 * 9 * 4;
            IntPtr esi_3 = data_56b600 + offset;

            if (Marshal.ReadIntPtr(esi_3) != new IntPtr(unchecked((int)0xffffffff)))
            {
                Marshal.WriteByte(esi_3, 4, (byte)(Marshal.ReadByte(esi_3, 4) | 0x80));
            }
            else
            {
                Marshal.WriteByte(esi_3, 4, 0x81);
                int nStdHandle = (i_2 == 0) ? -10 : (i_2 == 1) ? -11 : -12;

                IntPtr hFile = GetStdHandle(nStdHandle);
                if (hFile == new IntPtr(unchecked((int)0xffffffff)))
                {
                    Marshal.WriteByte(esi_3, 4, (byte)(Marshal.ReadByte(esi_3, 4) | 0x40));
                }
                else
                {
                    int eax_16 = GetFileType(hFile);
                    if (eax_16 == 0)
                    {
                        Marshal.WriteByte(esi_3, 4, (byte)(Marshal.ReadByte(esi_3, 4) | 0x40));
                    }
                    else
                    {
                        Marshal.WriteIntPtr(esi_3, hFile);
                        if (eax_16 == 2)
                        {
                            Marshal.WriteByte(esi_3, 4, (byte)(Marshal.ReadByte(esi_3, 4) | 0x40));
                        }
                        else if (eax_16 == 3)
                        {
                            Marshal.WriteByte(esi_3, 4, (byte)(Marshal.ReadByte(esi_3, 4) | 8));
                        }
                    }
                }
            }
        }

        return SetHandleCount(data_56b700);
    }

    private static Func<int, int> data_50c240 = sub_4ea147;
    private static int sub_4e71a6(uint arg1)
    {
        if (data_56999c == 1)
        {
            sub_4ec80c();
        }

        sub_4ec845(arg1);
        return data_50c240(0xff);
    }

    private static int sub_4ea147(int arg)
    {
        // Implementiere die Logik für sub_4ea147
        Console.WriteLine($"sub_4ea147 aufgerufen mit Argument: {arg}");
        return arg; // Beispielrückgabewert
    }

    private static IntPtr sub_4ec337()
    {
        IntPtr esi = IntPtr.Zero;
        IntPtr lpMultiByteStr_2 = IntPtr.Zero;

        if (data_569cc0 == IntPtr.Zero)
        {
            esi = GetEnvironmentStringsW();

            if (esi == IntPtr.Zero)
            {
                IntPtr penv = GetEnvironmentStrings();

                if (penv == IntPtr.Zero)
                {
                    return IntPtr.Zero;
                }

                data_569cc0 = new IntPtr(2);
                lpMultiByteStr_2 = ProcessAnsiEnvironmentStrings(penv);
                FreeEnvironmentStringsA(penv);
                return lpMultiByteStr_2;
            }

            data_569cc0 = new IntPtr(1);
        }
        else if (data_569cc0 == new IntPtr(2))
        {
            IntPtr penv = GetEnvironmentStrings();
            if (penv != IntPtr.Zero)
            {
                lpMultiByteStr_2 = ProcessAnsiEnvironmentStrings(penv);
                FreeEnvironmentStringsA(penv);
            }
            return lpMultiByteStr_2;
        }

        if (esi == IntPtr.Zero)
        {
            esi = GetEnvironmentStringsW();
            if (esi == IntPtr.Zero)
                return IntPtr.Zero;
        }

        lpMultiByteStr_2 = ProcessUnicodeEnvironmentStrings(esi);
        FreeEnvironmentStringsW(esi);

        return lpMultiByteStr_2;
    }

    private static IntPtr ProcessAnsiEnvironmentStrings(IntPtr penv)
    {
        IntPtr penv_1 = penv;

        while (Marshal.ReadByte(penv_1) != 0)
        {
            penv_1 += 1;

            if (Marshal.ReadByte(penv_1) == 0)
            {
                penv_1 += 1;
                if (Marshal.ReadByte(penv_1) == 0)
                    break;
            }
        }

        IntPtr esi_1 = sub_4e85fc((int)(penv_1.ToInt64() - penv.ToInt64()) + 1);

        if (esi_1 != IntPtr.Zero)
        {
            sub_4e6170(esi_1, penv, (int)(penv_1.ToInt64() - penv.ToInt64()) + 1);
        }
        else
        {
            esi_1 = IntPtr.Zero;
        }

        return esi_1;
    }

    private static IntPtr ProcessUnicodeEnvironmentStrings(IntPtr esi)
    {
        IntPtr eax_4 = esi;

        while (Marshal.ReadInt16(eax_4) != 0)
        {
            eax_4 += 2; // Increment by 2 bytes for the next WCHAR

            if (Marshal.ReadInt16(eax_4) == 0)
            {
                eax_4 += 2; // Increment by 2 bytes for the next WCHAR
                if (Marshal.ReadInt16(eax_4) == 0)
                    break;
            }
        }

        int cbMultiByte = WideCharToMultiByte(0, 0, esi, (int)((eax_4.ToInt64() - esi.ToInt64()) / 2 + 1), null, 0, IntPtr.Zero, IntPtr.Zero);

        if (cbMultiByte == 0)
        {
            return IntPtr.Zero;
        }

        IntPtr lpMultiByteStr = sub_4e85fc(cbMultiByte);

        if (lpMultiByteStr != IntPtr.Zero)
        {
            StringBuilder sb = new StringBuilder(cbMultiByte);
            if (WideCharToMultiByte(0, 0, esi, (int)((eax_4.ToInt64() - esi.ToInt64()) / 2 + 1), sb, cbMultiByte, IntPtr.Zero, IntPtr.Zero) == 0)
            {
                sub_4e8513(lpMultiByteStr);
                lpMultiByteStr = IntPtr.Zero;
            }
        }

        return lpMultiByteStr;
    }

    private static IntPtr sub_4e85fc(int size)
    {
        return Marshal.AllocHGlobal(size);
    }

    private static IntPtr sub_4e8513(IntPtr arg1)
    {
        int var_8 = -1;
        int var_c = 0x4fb9a8;
        int var_10 = 0x4ec734;

        TEB teb = new TEB();
        IntPtr exceptionListPtr = teb.NtTib.ExceptionList;
        IntPtr exceptionListPtr_1 = exceptionListPtr;
        teb.NtTib.ExceptionList = exceptionListPtr_1;

        if (arg1 != IntPtr.Zero)
        {
            int eax = Marshal.ReadInt32(data_56b5ec);

            if (eax != 3)
            {
                if (eax == 2)
                {
                    sub_4ebdeb(9);
                    int var_8_3 = 1;
                    IntPtr var_2c;
                    int var_24;
                    IntPtr eax_2 = sub_4ee1fd(arg1, out var_2c, out var_24);

                    if (eax_2 != IntPtr.Zero)
                    {
                        sub_4ee254(var_2c, var_24, eax_2);
                    }

                    var_8 = -1;
                    exceptionListPtr = sub_4e85d5();

                    if (eax_2 == IntPtr.Zero)
                    {
                        exceptionListPtr = HeapFree(data_56b5e8, 0, arg1);
                    }
                }
                else
                {
                    exceptionListPtr = HeapFree(data_56b5e8, 0, arg1);
                }
            }
            else
            {
                sub_4ebdeb(9);
                var_8 = 0;
                IntPtr eax_1 = sub_4ed4a2(arg1);

                if (eax_1 != IntPtr.Zero)
                {
                    sub_4ed4cd(eax_1, arg1);
                }

                var_8 = -1;
                exceptionListPtr = sub_4e857d();

                if (eax_1 == IntPtr.Zero)
                {
                    exceptionListPtr = HeapFree(data_56b5e8, 0, arg1);
                }
            }
        }

        teb.NtTib.ExceptionList = exceptionListPtr_1;
        return exceptionListPtr;
    }

    private static void sub_4ebdeb(int arg)
    {
        Console.WriteLine($"sub_4ebdeb aufgerufen mit Argument: {arg}");
    }

    private static IntPtr sub_4ee1fd(IntPtr arg1, out IntPtr var_2c, out int var_24)
    {
        Console.WriteLine("sub_4ee1fd aufgerufen");
        var_2c = IntPtr.Zero;
        var_24 = 0;
        return IntPtr.Zero;
    }

    private static void sub_4ee254(IntPtr var_2c, int var_24, IntPtr eax_2)
    {
        Console.WriteLine("sub_4ee254 aufgerufen");
    }
    private static IntPtr sub_4e85d5()
    {
        Console.WriteLine("sub_4e85d5 aufgerufen");
        return IntPtr.Zero;
    }

    private static IntPtr HeapFree(IntPtr hHeap, uint dwFlags, IntPtr lpMem)
    {
        Console.WriteLine("HeapFree aufgerufen");
        Marshal.FreeHGlobal(lpMem);
        return IntPtr.Zero;
    }

    private static IntPtr sub_4ed4a2(IntPtr arg1)
    {
        Console.WriteLine("sub_4ed4a2 aufgerufen");
        return IntPtr.Zero;
    }

    private static void sub_4ed4cd(IntPtr eax_1, IntPtr arg1)
    {
        Console.WriteLine("sub_4ed4cd aufgerufen");
    }

    private static IntPtr sub_4e857d()
    {
        Console.WriteLine("sub_4e857d aufgerufen");
        return IntPtr.Zero;
    }

    private static void sub_4e6170(IntPtr destination, IntPtr source, int length)
    {
        byte[] buffer = new byte[length];
        Marshal.Copy(source, buffer, 0, length);
        Marshal.Copy(buffer, 0, destination, length);
    }

    private static int sub_4ec0ea(int arg1)
    {
        int var_8 = arg1;
        int var_c = arg1;

        if (data_56b708 == IntPtr.Zero)
        {
            sub_4ecf94();
        }

        GetModuleFileNameA(IntPtr.Zero, data_569bbc, 0x104);
        string moduleFileName = Marshal.PtrToStringAnsi(data_569bbc);
        data_569a30 = data_569bbc;
        string edi = moduleFileName;

        if (Marshal.ReadByte(data_56b714) != 0)
        {
            edi = Marshal.PtrToStringAnsi(data_56b714);
        }

        sub_4ec183(edi, IntPtr.Zero, IntPtr.Zero, ref var_8, ref var_c);
        IntPtr eax_3 = sub_4e85fc(var_c + (var_8 << 2));

        if (eax_3 == IntPtr.Zero)
        {
            sub_4e71a6(8);
        }

        sub_4ec183(edi, eax_3, eax_3 + (var_8 << 2), ref var_8, ref var_c);
        int result = var_8 - 1;
        data_569a18 = eax_3;
        data_569a14 = result;

        return result;
    }

    private static IntPtr sub_4ec183(string arg1, IntPtr arg2, IntPtr arg3, ref int arg4, ref int arg5)
    {
        int localArg5 = arg5;
        int localArg4 = arg4;

        localArg5 = 0;
        IntPtr esi = arg3;
        IntPtr edi = arg2;
        localArg4 = 1;
        IntPtr eax_1 = Marshal.StringToHGlobalAnsi(arg1);

        if (edi != IntPtr.Zero)
        {
            Marshal.WriteIntPtr(edi, esi);
            edi += IntPtr.Size;
            arg2 = edi;
        }

        while (true)
        {
            if (Marshal.ReadByte(eax_1) != 0x22)
            {
                while (true)
                {
                    localArg5 += 1;

                    if (esi != IntPtr.Zero)
                    {
                        byte edx = Marshal.ReadByte(eax_1);
                        Marshal.WriteByte(esi, edx);
                        esi += 1;
                    }

                    byte edx_2 = Marshal.ReadByte(eax_1);
                    eax_1 += 1;

                    if ((Marshal.ReadByte(data_56b4e1 + edx_2) & 4) != 0)
                    {
                        localArg5 += 1;

                        if (esi != IntPtr.Zero)
                        {
                            byte ebx_1 = Marshal.ReadByte(eax_1);
                            Marshal.WriteByte(esi, ebx_1);
                            esi += 1;
                        }

                        eax_1 += 1;
                    }

                    if (edx_2 == 0x20 || edx_2 == 0)
                    {
                        if (edx_2 != 0)
                        {
                            if (esi != IntPtr.Zero)
                                Marshal.WriteByte(esi - 1, 0);

                            break;
                        }
                    }
                    else
                    {
                        if (edx_2 == 9)
                            continue;

                        eax_1 -= 1;
                        break;
                    }
                }
            }
            else
            {
                eax_1 += 1;

                while (true)
                {
                    byte edx = Marshal.ReadByte(eax_1);
                    eax_1 += 1;

                    if (edx == 0x22 || edx == 0)
                        break;

                    if ((Marshal.ReadByte(data_56b4e1 + edx) & 4) != 0)
                    {
                        localArg5 += 1;

                        if (esi != IntPtr.Zero)
                        {
                            edx = Marshal.ReadByte(eax_1);
                            Marshal.WriteByte(esi, edx);
                            esi += 1;
                            eax_1 += 1;
                        }
                    }

                    localArg5 += 1;

                    if (esi != IntPtr.Zero)
                    {
                        Marshal.WriteByte(esi, edx);
                        esi += 1;
                    }
                }

                localArg5 += 1;

                if (esi != IntPtr.Zero)
                {
                    Marshal.WriteByte(esi, 0);
                    esi += 1;
                }

                if (Marshal.ReadByte(eax_1) == 0x22)
                    eax_1 += 1;
            }

            arg5 = 0;

            while (Marshal.ReadByte(eax_1) != 0)
            {
                while (true)
                {
                    byte edx = Marshal.ReadByte(eax_1);

                    if (edx != 0x20 && edx != 9)
                        break;

                    eax_1 += 1;
                }

                if (Marshal.ReadByte(eax_1) == 0)
                    break;

                if (edi != IntPtr.Zero)
                {
                    Marshal.WriteIntPtr(edi, esi);
                    edi += IntPtr.Size;
                    arg2 = edi;
                }

                localArg4 += 1;

                while (true)
                {
                    int i_2 = 0;
                    byte edx = 0;

                    while (Marshal.ReadByte(eax_1) == 0x5c)
                    {
                        eax_1 += 1;
                        i_2 += 1;
                    }

                    if (Marshal.ReadByte(eax_1) == 0x22)
                    {
                        if ((i_2 & 1) == 0)
                        {
                            if (arg5 == 0 || Marshal.ReadByte(eax_1 + 1) != 0x22)
                            {
                                arg1 = null;
                            }
                            else
                            {
                                eax_1 += 1;
                            }

                            edi = arg2;
                            arg5 = 0;
                        }

                        i_2 /= 2;
                    }

                    if (i_2 != 0)
                    {
                        for (int i = 0; i < i_2; i++)
                        {
                            if (esi != IntPtr.Zero)
                            {
                                Marshal.WriteByte(esi, 0x5c);
                                esi += 1;
                            }

                            localArg5 += 1;
                        }
                    }

                    edx = Marshal.ReadByte(eax_1);

                    if (edx == 0)
                        break;

                    if (arg5 == 0 && (edx == 0x20 || edx == 9))
                        break;

                    if (arg1 != null)
                    {
                        if (esi != IntPtr.Zero)
                        {
                            if ((Marshal.ReadByte(data_56b4e1 + edx) & 4) != 0)
                            {
                                Marshal.WriteByte(esi, edx);
                                esi += 1;
                                eax_1 += 1;
                                localArg5 += 1;
                            }

                            edx = Marshal.ReadByte(eax_1);
                            Marshal.WriteByte(esi, edx);
                            esi += 1;
                        }
                        else if ((Marshal.ReadByte(data_56b4e1 + edx) & 4) != 0)
                        {
                            eax_1 += 1;
                            localArg5 += 1;
                        }

                        localArg5 += 1;
                    }

                    eax_1 += 1;
                }

                if (esi != IntPtr.Zero)
                {
                    Marshal.WriteByte(esi, 0);
                    esi += 1;
                }

                localArg5 += 1;
            }

            if (edi != IntPtr.Zero)
                Marshal.WriteIntPtr(edi, IntPtr.Zero);

            arg4 = localArg4 + 1;
            arg5 = localArg5;
            return new IntPtr(arg4);
        }
    }

    private static void sub_4ecf94()
    {
        if (data_56b708 == IntPtr.Zero)
        {
            sub_4ecbbc(0xfffffffd);
            data_56b708 = new IntPtr(1);
        }
    }
   
    private static int sub_4ecbbc(uint arg1)
    {
        sub_4ebdeb(0x19);
        uint CodePage = sub_4ecd69(arg1);
        int result;

        if (CodePage != data_56b3c0)
        {
            if (CodePage == 0)
            {
                sub_4ecde6();
            }
            else
            {
                bool found = false;
                int index = 0;

                // Assume data_50cb58 as an array of uint32_t representing CodePage entries
                uint[] data_50cb58 = new uint[12];  // Beispielinitialisierung, anpassen nach Bedarf

                for (index = 0; index < data_50cb58.Length; index++)
                {
                    if (data_50cb58[index] == CodePage)
                    {
                        found = true;
                        break;
                    }
                }

                if (found)
                {
                    // Example logic, this must be tailored based on actual data
                    int esi_2 = index * 0x30;
                    Array.Clear(data_56b4e0, 0, data_56b4e0.Length);

                    byte[] ebx_1 = new byte[8];  // Beispielhafte Initialisierung, anpassen nach Bedarf

                    for (int i = 0; i < 4; i++)
                    {
                        if (ebx_1[0] != 0)
                        {
                            for (int j = 1; j < ebx_1.Length; j += 2)
                            {
                                if (ebx_1[j] == 0)
                                    break;

                                uint eax_2 = ebx_1[j - 1];
                                uint edi_5 = ebx_1[j];

                                if (eax_2 <= edi_5)
                                {
                                    for (uint k = eax_2; k <= edi_5; k++)
                                    {
                                        data_56b4e0[k] |= ebx_1[i];
                                    }
                                }
                            }
                        }
                    }

                    data_56b3dc = 1;
                    data_56b3c0 = CodePage;
                    data_56b3d0 = index;  // Anpassung je nach Datenstruktur
                    data_56b5e4 = sub_4ecdb3(CodePage);
                }
                else
                {
                    CPINFO lpCPInfo;
                    if (!GetCPInfo(CodePage, out lpCPInfo))
                    {
                        if (data_569cd0 != 0)
                        {
                            sub_4ecde6();
                        }

                        result = -1;
                        sub_4ebe4c(0x19);
                        return result;
                    }

                    data_56b5e4 = 0;
                    Array.Clear(data_56b4e0, 0, data_56b4e0.Length);
                    data_56b3c0 = CodePage;

                    if (lpCPInfo.MaxCharSize <= 1)
                    {
                        data_56b3dc = 0;
                    }
                    else
                    {
                        for (int i = 0; i < lpCPInfo.LeadByte.Length; i += 2)
                        {
                            if (lpCPInfo.LeadByte[i] == 0)
                                break;

                            for (uint j = lpCPInfo.LeadByte[i]; j <= lpCPInfo.LeadByte[i + 1]; j++)
                            {
                                data_56b4e0[j] |= 4;
                            }
                        }

                        for (int i = 1; i < 0xff; i++)
                        {
                            data_56b4e0[i] |= 8;
                        }

                        data_56b5e4 = sub_4ecdb3(CodePage);
                        data_56b3dc = 1;
                    }

                    data_56b3d0 = 0;
                    data_56b3d4 = 0;
                }
            }

            sub_4ece0f();
        }

        result = 0;
        sub_4ebe4c(0x19);
        return result;
    }
    
    private static IntPtr sub_4ec031()
    {
        if (data_56b708 == IntPtr.Zero)
        {
            sub_4ecf94();
        }

        IntPtr esi = data_569994;
        int edi = 0;

        while (true)
        {
            byte eax = Marshal.ReadByte(esi);

            if (eax == 0)
                break;

            if (eax != 0x3d) // '=' Zeichen
            {
                edi += 1;
            }

            esi = IntPtr.Add(esi, sub_4e5d10(esi) + 1);
        }

        IntPtr esi_1 = sub_4e85fc(edi * IntPtr.Size);
        data_569a20 = esi_1;

        if (esi_1 == IntPtr.Zero)
        {
            sub_4e71a6(9);
        }

        IntPtr edi_1 = data_569994;

        while (Marshal.ReadByte(edi_1) != 0)
        {
            int ebp_2 = sub_4e5d10(edi_1) + 1;

            if (Marshal.ReadByte(edi_1) != 0x3d) // '=' Zeichen
            {
                IntPtr eax_4 = sub_4e85fc(ebp_2);
                Marshal.WriteIntPtr(esi_1, eax_4);

                if (eax_4 == IntPtr.Zero)
                {
                    sub_4e71a6(9);
                }

                sub_4e5c20(eax_4, edi_1);
                esi_1 = IntPtr.Add(esi_1, IntPtr.Size);
            }

            edi_1 = IntPtr.Add(edi_1, ebp_2);
        }

        IntPtr result = sub_4e8513(data_569994);
        data_569994 = IntPtr.Zero;
        Marshal.WriteIntPtr(esi_1, IntPtr.Zero);
        data_56b704 = 1;

        return result;
    }

    private static IntPtr sub_4e5c20(IntPtr arg1, IntPtr arg2)
    {
        IntPtr edi = arg1;
        IntPtr ecx = arg2;

        // Erste Schleife: Ausrichten auf 4-Byte-Grenze
        while ((ecx.ToInt64() & 3) != 0)
        {
            byte edx = Marshal.ReadByte(ecx);
            ecx += 1;

            if (edx == 0)
            {
                Marshal.WriteByte(edi, edx);
                return arg1;
            }

            Marshal.WriteByte(edi, edx);
            edi += 1;
        }

        // Zweite Schleife: 4-Byte-Blöcke verarbeiten
        while (true)
        {
            int eax_1 = Marshal.ReadInt32(ecx);
            int edx = eax_1;
            ecx += 4;

            if ((((eax_1 ^ 0xffffffff) ^ (0x7efefeff + eax_1)) & 0x81010100) != 0)
            {
                if ((edx & 0xFF) == 0)
                {
                    Marshal.WriteByte(edi, 0);
                    return arg1;
                }

                if ((edx & 0xFF00) == 0)
                {
                    Marshal.WriteInt16(edi, (short)(edx & 0xFFFF));
                    return arg1;
                }

                if ((edx & 0xFF0000) == 0)
                {
                    Marshal.WriteInt16(edi, (short)(edx & 0xFFFF));
                    Marshal.WriteByte(edi + 2, 0);
                    return arg1;
                }

                if ((edx & 0xFF000000) == 0)
                {
                    Marshal.WriteInt32(edi, edx);
                    return arg1;
                }
            }

            Marshal.WriteInt32(edi, edx);
            edi += 4;
        }
    }

    private static IntPtr sub_4e5c30(IntPtr arg1, IntPtr arg2)
    {
        IntPtr ecx = arg1;
        IntPtr edi = IntPtr.Zero;

        // Erste Schleife: Ausrichten auf 4-Byte-Grenze
        while ((ecx.ToInt64() & 3) != 0)
        {
            byte eax = Marshal.ReadByte(ecx);
            ecx += 1;

            if (eax == 0)
            {
                edi = ecx - 1;
                return CopyString(edi, arg2, arg1);
            }
        }

        // Zweite Schleife: 4-Byte-Blöcke verarbeiten
        while (true)
        {
            int eax_1 = Marshal.ReadInt32(ecx);
            ecx += 4;

            if ((((eax_1 ^ 0xffffffff) ^ (0x7efefeff + eax_1)) & 0x81010100) != 0)
            {
                if ((eax_1 & 0xFF) == 0)
                {
                    edi = ecx - 4;
                    break;
                }
                if ((eax_1 & 0xFF00) == 0)
                {
                    edi = ecx - 3;
                    break;
                }
                if ((eax_1 & 0xFF0000) == 0)
                {
                    edi = ecx - 2;
                    break;
                }
                if ((eax_1 & 0xFF000000) == 0)
                {
                    edi = ecx - 1;
                    break;
                }
            }
        }

        return CopyString(edi, arg2, arg1);
    }

    private static IntPtr CopyString(IntPtr edi, IntPtr ecx_1, IntPtr arg1)
    {
        // Erste Schleife: Ausrichten auf 4-Byte-Grenze
        while ((ecx_1.ToInt64() & 3) != 0)
        {
            byte edx = Marshal.ReadByte(ecx_1);
            ecx_1 += 1;

            if (edx == 0)
            {
                Marshal.WriteByte(edi, edx);
                return arg1;
            }

            Marshal.WriteByte(edi, edx);
            edi += 1;
        }

        // Zweite Schleife: 4-Byte-Blöcke verarbeiten
        while (true)
        {
            int edx = Marshal.ReadInt32(ecx_1);
            ecx_1 += 4;

            if ((((edx ^ 0xffffffff) ^ (0x7efefeff + edx)) & 0x81010100) != 0)
            {
                if ((edx & 0xFF) == 0)
                {
                    Marshal.WriteByte(edi, 0);
                    return arg1;
                }
                if ((edx & 0xFF00) == 0)
                {
                    Marshal.WriteInt16(edi, (short)(edx & 0xFFFF));
                    return arg1;
                }
                if ((edx & 0xFF0000) == 0)
                {
                    Marshal.WriteInt16(edi, (short)(edx & 0xFFFF));
                    Marshal.WriteByte(edi + 2, 0);
                    return arg1;
                }
                if ((edx & 0xFF000000) == 0)
                {
                    Marshal.WriteInt32(edi, edx);
                    return arg1;
                }
            }

            Marshal.WriteInt32(edi, edx);
            edi += 4;
        }
    }

    private static int sub_4e5d10(IntPtr arg1)
    {
        IntPtr ecx = arg1;

        // Loop 1: Align ecx to a 4-byte boundary
        while ((ecx.ToInt64() & 3) != 0)
        {
            byte eax = Marshal.ReadByte(ecx);
            ecx += 1;

            if (eax == 0)
            {
                return (int)(ecx.ToInt64() - arg1.ToInt64()) - 1;
            }
        }

        // Loop 2: Check 4 bytes at a time
        while (true)
        {
            int eax_2 = Marshal.ReadInt32(ecx);
            ecx += 4;

            if ((((eax_2 ^ 0xffffffff) ^ (0x7efefeff + eax_2)) & 0x81010100) != 0)
            {
                int eax_5 = Marshal.ReadInt32(ecx - 4);

                if ((eax_5 & 0xFF) == 0)
                {
                    return (int)(ecx.ToInt64() - arg1.ToInt64()) - 4;
                }

                if ((eax_5 & 0xFF00) == 0)
                {
                    return (int)(ecx.ToInt64() - arg1.ToInt64()) - 3;
                }

                if ((eax_5 & 0xFF0000) == 0)
                {
                    return (int)(ecx.ToInt64() - arg1.ToInt64()) - 2;
                }

                if ((eax_5 & 0xFF000000) == 0)
                {
                    return (int)(ecx.ToInt64() - arg1.ToInt64()) - 1;
                }
            }
        }
    }

    private static int sub_4ea109()
    {
        IntPtr eax_1 = data_50c230;

        if (eax_1 != IntPtr.Zero)
        {
            // Ruft die Funktion auf, auf die data_50c230 zeigt
            FunctionDelegate function = (FunctionDelegate)Marshal.GetDelegateForFunctionPointer(eax_1, typeof(FunctionDelegate));
            function();
        }

        // Ruft sub_4ea20f auf mit entsprechenden Argumenten
        sub_4ea20f(data_4fe05c, 0x4fe06c);

        // Hier könnte ein anderer Rückgabewert bestimmt werden, zum Beispiel:
        // Wenn sub_4ea20f nichts zurückgibt, könnte der Rückgabewert `0` oder ein anderer Wert sein.
        sub_4ea20f(data_4fe000, 0x4fe058);

        // Da sub_4ea20f void ist, geben wir hier z.B. einen Standardwert zurück.
        return 0;  // oder ein anderer Wert, der in diesem Kontext sinnvoll ist
    }

    private static void sub_4ea20f(IntPtr arg1, int arg2)
    {
        // Schleife über die Speicheradressen von arg1 bis arg2
        while (arg1.ToInt32() < arg2)
        {
            // Liest die Adresse der Funktion
            IntPtr eax = Marshal.ReadIntPtr(arg1);

            if (eax != IntPtr.Zero)
            {
                // Konvertiere den Funktionszeiger in einen Delegate und rufe die Funktion auf
                FunctionDelegate function = (FunctionDelegate)Marshal.GetDelegateForFunctionPointer(eax, typeof(FunctionDelegate));
                function();
            }

            arg1 = IntPtr.Add(arg1, IntPtr.Size);
        }
    }

    private static IntPtr sub_4ebfd9() { return IntPtr.Zero; }
    private static void sub_403487(IntPtr hModule, int arg1, IntPtr arg2, uint arg3) { }
    private static void sub_4ea136(int value) { }
    private static int sub_4ebe61(int value1, int[] value2) { return 0; }
    private static uint sub_4ecd69(uint arg)
    {
        Console.WriteLine($"sub_4ecd69 aufgerufen mit Argument: {arg}");
        return arg;  // Beispielrückgabe
    }
    private static void sub_4ecde6()
    {
        Console.WriteLine("sub_4ecde6 aufgerufen");
    }
    private static int sub_4ecdb3(uint CodePage)
    {
        Console.WriteLine($"sub_4ecdb3 aufgerufen mit CodePage: {CodePage}");
        return 0;  // Beispielrückgabe
    }
    private static void sub_4ece0f()
    {
        Console.WriteLine("sub_4ece0f aufgerufen");
    }
    private static void sub_4ebe4c(int arg)
    {
        Console.WriteLine($"sub_4ebe4c aufgerufen mit Argument: {arg}");
    }
}