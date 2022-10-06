using System.Runtime.InteropServices;
using System.Text;

namespace Miscellaneous.Tasks {
    internal class NatStarTask : TaskBase {
        private readonly DirectoryInfo _binNatStar = new("PATH_TO_NATSTAR_BIN_FOLDER");
        private readonly DirectoryInfo _glob = new(@"PATH_TO_GLOB_FOLDER");

        protected override void ExecuteTask() {
            if (IntPtr.Size != 4) {
                Logger.Error($"Application must be launch in x86 mode");
                return;
            }

            if (!_binNatStar.Exists) {
                Logger.Error($"Binaries folder {_binNatStar} doesn't exist");
                return;
            }

            if (!_glob.Exists) {
                Logger.Error($"GLOB folder {_glob} doesn't exist");
                return;
            }

            Environment.SetEnvironmentVariable("PATH", $"{_binNatStar.FullName};{Environment.GetEnvironmentVariable("PATH")}");
            NatStar.Initialize(_glob.FullName);
            NatStar.StartTransaction();

            int id = 0;
            short type = 0;
            short sizeData = 0;
            IntPtr pData = IntPtr.Zero;
            StringBuilder name = new(256);
            StringBuilder description = new(256);

            IntPtr entity = NatStar.FindEntity(1, "CLASS_NAME");
            NatStar.InfosEntity(entity, ref id, ref type, name, description, ref sizeData, ref pData);

            byte[] datas = new byte[sizeData];
            Marshal.Copy(pData, datas, 0, sizeData);

            Logger.Log($"Id          : {id:X8}");
            Logger.Log($"Type        : {type}");
            Logger.Log($"Name        : {name} ({NatStar.Normalize(name.ToString(), 6)})");
            Logger.Log($"Description : {description}");
            Logger.Log($"Datas size  : {sizeData}");
            Logger.Log($"Datas       : {Encoding.Default.GetString(datas)}");

            NatStar.StopTransaction();
            NatStar.Terminate();
        }

        private static class NatStar {
            #region Initialize & Terminate
            [DllImport(@"nsw2ldct.dll", EntryPoint = "DICTINITIALIZE")]
            private static extern void Initialize(IntPtr window, short eventId, string globDirectory);
            internal static void Initialize(string globDirectory) => Initialize(IntPtr.Zero, 0, globDirectory);

            [DllImport(@"nsw2ldct.dll", EntryPoint = "DICTTERMINATE")]
            internal static extern void Terminate();
            #endregion

            #region Transactions
            [DllImport(@"nsw2ldct.dll", EntryPoint = "DICTSTARTTRANSACTION")]
            private static extern short StartTransaction(byte typeTransaction);
            internal static short StartTransaction() => StartTransaction(0);

            [DllImport(@"nsw2ldct.dll", EntryPoint = "DICTSTOPTRANSACTION")]
            internal static extern void StopTransaction();
            #endregion

            #region Entities
            [DllImport(@"nsw2ldct.dll", EntryPoint = "DICTFINDENTITY")]
            internal static extern IntPtr FindEntity(short type, string name);

            [DllImport(@"nsw2ldct.dll", EntryPoint = "DICTGETENTITY")]
            internal static extern IntPtr GetEntity(int id);

            [DllImport(@"nsw2ldct.dll", EntryPoint = "DICTFIRSTENTITY")]
            internal static extern IntPtr FirstEntity(short type, ref IntPtr entity);

            [DllImport(@"nsw2ldct.dll", EntryPoint = "DICTNEXTENTITY")]
            internal static extern IntPtr NextEntity(short type, ref IntPtr entity, IntPtr search);

            [DllImport(@"nsw2ldct.dll", EntryPoint = "DICTINFOSENTITY")]
            internal static extern void InfosEntity(IntPtr entty, ref int id, ref short type, StringBuilder name, StringBuilder description, ref short size, ref IntPtr data);
            #endregion

            #region Links
            [DllImport(@"nsw2ldct.dll", EntryPoint = "DICTFIRSTLINK")]
            internal static extern short FirstLink(short type, IntPtr entity);

            [DllImport(@"nsw2ldct.dll", EntryPoint = "DICTNEXTLINK")]
            internal static extern short NextLink(short type, IntPtr entity, short index);

            [DllImport(@"nsw2ldct.dll", EntryPoint = "DICTINFOSLINK")]
            internal static extern IntPtr InfosLink(IntPtr entity, short index);
            #endregion

            #region Miscellaneous
            [DllImport(@"nsw2thfr.dll", EntryPoint = "szNORMALIZE")]
            private static extern void Normalize(StringBuilder normalizeName, string name, int size);
            internal static string Normalize(string name, int size) {
                if (name.Length <= size)
                    return name;

                StringBuilder normalizedName = new(size);
                Normalize(normalizedName, $"?{name}", size);
                return normalizedName.ToString();
            }
            #endregion
        }
    }
}
