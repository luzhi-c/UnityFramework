
using GameFramework;
using GameFramework.Event;
using YooAsset;

namespace GamePlay.Boot
{
    /// <summary>
    /// 补丁流程步骤改变
    /// </summary>
    public class PatchStcepsChangeArgs : GameFramework.Event.GameEventArgs
    {
        public static readonly int EventId = typeof(PatchStcepsChangeArgs).GetHashCode();
        public override int Id
        {
            get
            {
                return EventId;
            }
        }
        public string Tips;

        public static PatchStcepsChangeArgs Create(string tips)
        {
            var msg = ReferencePool.Acquire<PatchStcepsChangeArgs>();
            msg.Tips = tips;
            return msg;
        }

        public override void Clear()
        {
            Tips = null;
        }

    }

    /// <summary>
    /// 发现更新文件
    /// </summary>
    public class FoundUpdateFilesArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(FoundUpdateFilesArgs).GetHashCode();
        public override int Id
        {
            get
            {
                return EventId;
            }
        }
        public int TotalCount;
        public long TotalSizeBytes;

        public static FoundUpdateFilesArgs Create(int totalCount, long totalSizeBytes)
        {
            var msg = ReferencePool.Acquire<FoundUpdateFilesArgs>();
            msg.TotalCount = totalCount;
            msg.TotalSizeBytes = totalSizeBytes;
            return msg;
        }

        public override void Clear()
        {
            TotalCount = 0;
            TotalSizeBytes = 0;
        }

    }
    /// <summary>
    /// 下载进度更新
    /// </summary>
    public class DownloadUpdateArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(DownloadUpdateArgs).GetHashCode();
        public override int Id
        {
            get
            {
                return EventId;
            }
        }
        public int TotalDownloadCount;
        public int CurrentDownloadCount;
        public long TotalDownloadSizeBytes;
        public long CurrentDownloadSizeBytes;


        public static DownloadUpdateArgs Create(DownloadUpdateData updateData)
        {
            var msg = ReferencePool.Acquire<DownloadUpdateArgs>();
            msg.TotalDownloadCount = updateData.TotalDownloadCount;
            msg.CurrentDownloadCount = updateData.CurrentDownloadCount;
            msg.TotalDownloadSizeBytes = updateData.TotalDownloadBytes;
            msg.CurrentDownloadSizeBytes = updateData.CurrentDownloadBytes;
            return msg;
        }

        public override void Clear()
        {
            TotalDownloadCount = 0;
            CurrentDownloadCount = 0;
            TotalDownloadSizeBytes = 0;
            CurrentDownloadSizeBytes = 0;
        }
    }

    /// <summary>
    /// 网络文件下载失败
    /// </summary>
    public class WebFileDownloadFailedArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(WebFileDownloadFailedArgs).GetHashCode();
        public override int Id
        {
            get
            {
                return EventId;
            }
        }
        public string FileName;
        public string Error;

        public static WebFileDownloadFailedArgs Create(DownloadErrorData errorData)
        {
            var msg = ReferencePool.Acquire<WebFileDownloadFailedArgs>();
            msg.FileName = errorData.FileName;
            msg.Error = errorData.ErrorInfo;
            return msg;
        }

        public override void Clear()
        {
            FileName = null;
            Error = null;
        }
    }

    public class UserBeginDownloadWebFilesArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(UserBeginDownloadWebFilesArgs).GetHashCode();
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public static UserBeginDownloadWebFilesArgs Create()
        {
            var msg = ReferencePool.Acquire<UserBeginDownloadWebFilesArgs>();
            return msg;
        }

        public override void Clear()
        {

        }
    }
    public class PackageVersionRequestFailedArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(PackageVersionRequestFailedArgs).GetHashCode();
        public override int Id
        {
            get
            {
                return EventId;
            }
        }
        public static PackageVersionRequestFailedArgs Create()
        {
            var msg = ReferencePool.Acquire<PackageVersionRequestFailedArgs>();
            return msg;
        }

        public override void Clear()
        {

        }
    }

    public class PackageManifestUpdateFailedArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(PackageManifestUpdateFailedArgs).GetHashCode();
        public override int Id
        {
            get
            {
                return EventId;
            }
        }
        public static PackageManifestUpdateFailedArgs Create()
        {
            var msg = ReferencePool.Acquire<PackageManifestUpdateFailedArgs>();
            return msg;
        }

        public override void Clear()
        {

        }
    }

    public class InitializeFailedArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(InitializeFailedArgs).GetHashCode();
        public override int Id
        {
            get
            {
                return EventId;
            }
        }
        public static InitializeFailedArgs Create()
        {
            var msg = ReferencePool.Acquire<InitializeFailedArgs>();
            return msg;
        }

        public override void Clear()
        {

        }
    }

    public class UserTryInitializeArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(UserTryInitializeArgs).GetHashCode();
        public override int Id
        {
            get
            {
                return EventId;
            }
        }
        public static UserTryInitializeArgs Create()
        {
            var msg = ReferencePool.Acquire<UserTryInitializeArgs>();
            return msg;
        }

        public override void Clear()
        {

        }
    }

    public class UserTryRequestPackageVersionArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(UserTryRequestPackageVersionArgs).GetHashCode();
        public override int Id
        {
            get
            {
                return EventId;
            }
        }
        public static UserTryRequestPackageVersionArgs Create()
        {
            var msg = ReferencePool.Acquire<UserTryRequestPackageVersionArgs>();
            return msg;
        }

        public override void Clear()
        {

        }
    }

    public class UserTryUpdatePackageManifestArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(UserTryUpdatePackageManifestArgs).GetHashCode();
        public override int Id
        {
            get
            {
                return EventId;
            }
        }
        public static UserTryUpdatePackageManifestArgs Create()
        {
            var msg = ReferencePool.Acquire<UserTryUpdatePackageManifestArgs>();
            return msg;
        }

        public override void Clear()
        {

        }
    }

    public class UserTryDownloadWebFilesArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(UserTryDownloadWebFilesArgs).GetHashCode();
        public override int Id
        {
            get
            {
                return EventId;
            }
        }
        public static UserTryDownloadWebFilesArgs Create()
        {
            var msg = ReferencePool.Acquire<UserTryDownloadWebFilesArgs>();
            return msg;
        }

        public override void Clear()
        {

        }
    }




}
